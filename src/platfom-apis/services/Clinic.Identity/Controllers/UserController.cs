using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using Clinic.Caching.Interfaces;
using Clinic.Common.Options;
using Clinic.Data.Models;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Clinic.DTO.Models.Model;
using Clinic.Identity.Models;
using Duende.IdentityServer.Extensions;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Clinic.Identity.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper autoMapper;
        private readonly HttpClient httpClient;
        private readonly IDistributedCacheService cacheService;
        private readonly IOptions<ApplicationSettings> applicationSettings;
        public UserController(UserManager<ApplicationUser> userManager,
            IMapper autoMapper,
            IHttpClientFactory httpClientFactory,
            IDistributedCacheService cacheService,
             IOptions<ApplicationSettings> applicationSettings)
        {
            _userManager = userManager;
            this.autoMapper = autoMapper;
            httpClient = httpClientFactory.CreateClient();
            this.cacheService = cacheService;
            this.applicationSettings = applicationSettings;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetListDoctor([FromBody] FilterDto dto)
        {
            List<ApplicationUserModel> userModels;
            ResponseDto result = new();
            var doctors = await _userManager.GetUsersInRoleAsync(SD.DOCTOR);
            var usersDto = autoMapper.Map<List<ApplicationUsersDto>>(doctors.ToList());
            userModels = autoMapper.Map<List<ApplicationUserModel>>(usersDto);
            var listDoctors = autoMapper.Map<List<DoctorListViewModel>>(userModels);

                Console.WriteLine("Specialization : " + dto.Specialization);
                if (dto.Specialization != 0)
                {
                    
                    listDoctors = listDoctors.Where(x => x.Specialization == dto.Specialization).ToList();
                }
                if (!dto.Title.IsNullOrEmpty())
                {
                    listDoctors = listDoctors.Where(x => x.Title.ContainsCaseInsensitive(dto.Title)).ToList();
                }
                if (!dto.DoctorName.IsNullOrEmpty())
                {
                    listDoctors = listDoctors.Where(x => x.Name.ContainsCaseInsensitive(dto.DoctorName)).ToList();
                }  
                result.Result = listDoctors;
                return Ok(result);                                
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSchedule([FromBody] UpdateSchedule dto)
        {
            var doctor = await _userManager.Users.Include(x => x.ScheduleTimes).FirstOrDefaultAsync(u => u.Id == dto.DoctorId);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId);
            if (doctor != null)
            {
                doctor.ScheduleTimes.Add(new ScheduleTime
                {
                    UserId = dto.UserId,
                    Time = dto.OrderTime
                });
                await _userManager.UpdateAsync(doctor);
            }
            if(user != null)
            {
                user.Detail = dto.Detail != null && dto.Detail.UserName != null ? JsonConvert.SerializeObject(dto.Detail) : null;
                await _userManager.UpdateAsync(user);
            }
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetDoctorSchedule(string userId)
        {
            BookingViewModel model = new();
            var items = await _userManager.Users.Include(x => x.ScheduleTimes).FirstOrDefaultAsync(u => u.Id == userId);
            var usersDto = autoMapper.Map<ApplicationUsersDto>(items);
            var userModels = autoMapper.Map<ApplicationUserModel>(usersDto);
            string filterCriteria = $"e.Specialization = {userModels.Specialization}";
            using var productsRequest = new HttpRequestMessage(HttpMethod.Get, $"{this.applicationSettings.Value.ProductsApiEndpoint}/getproducts?filterCriteria={filterCriteria}");
            var productResponse = await httpClient.SendAsync(productsRequest).ConfigureAwait(false);
            if (productResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var productResult = await productResponse.Content.ReadFromJsonAsync<ResponseDto>().ConfigureAwait(false);
                var products = JsonConvert.DeserializeObject<List<ProductListViewModel>>(Convert.ToString((productResult.Result)));
                model.Name = userModels.Name;
                model.Specialization = userModels.Specialization;
                model.ProductListViewModels = products;
                model.OrderedTime = userModels.OrderedTimes;
                model.DoctorId = userModels.Id;
                model.ClinicNum = int.Parse(userModels.ClinicNum);
            }
            ResponseDto result = new();
            result.Result = model;
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetDetail(string userId)
        {
            BriefViewModel model = new();
            var items = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            model = !items.Detail.IsNullOrEmpty() ? JsonConvert.DeserializeObject<BriefViewModel>(items.Detail) : new();
            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateDoctor([FromBody] DoctorDto doctorDto)
        {
            ResponseDto result = new();
            if (doctorDto == null)
            {
                return BadRequest();
            }
            Detail detail = new()
            {
                ClinicNum = doctorDto.ClinicNum,
                Specialization = doctorDto.Specialization,
                Title = doctorDto.Title
            };
            ApplicationUser customerUser = new()
            {
                UserName = doctorDto.UserName,
                Email = doctorDto.UserName,
                EmailConfirmed = true,
                PhoneNumber = doctorDto.PhoneNumber,
                Name = doctorDto.Name,
                DateOfBirth = doctorDto.DateOfBirth.Ticks,
                Detail = JsonConvert.SerializeObject(detail),
                AverageRating = doctorDto.Rating,
                ImageUrl = doctorDto.ImageUrl,
                Introduction = doctorDto.Introduction
            };
            var item = await _userManager.Users.FirstOrDefaultAsync(u => u.Id ==doctorDto.Id);
            if (item != null)
            {
                item.UserName = doctorDto.UserName;
                item.Email = doctorDto.UserName;
                item.PhoneNumber = doctorDto.PhoneNumber;
                item.Name = doctorDto.Name;
                item.DateOfBirth = doctorDto.DateOfBirth.Ticks;
                item.Detail = JsonConvert.SerializeObject(detail);
                item.AverageRating = doctorDto.Rating;
                item.Introduction = doctorDto.Introduction;
                await _userManager.UpdateAsync(item);
            }
            else
            {
                await _userManager.CreateAsync(customerUser, doctorDto.Password);
                await _userManager.AddToRoleAsync(customerUser, SD.DOCTOR);

                var temp2 = _userManager.AddClaimsAsync(customerUser, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, customerUser.Name),
                    new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                }).Result;
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSchedule(string doctorId)
        {
            var items = await _userManager.Users.Include(x => x.ScheduleTimes).FirstOrDefaultAsync(u => u.Id == doctorId);
            ResponseDto result = new();
            result.Result = items.ScheduleTimes.Select(x => x.Time).ToList();
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDoctor(string id)
        {
            var item = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
           
            if (item != null)
            {
                ResponseDto result = new();
                await _userManager.DeleteAsync(item);
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetDoctor(string id)
        {
            var item = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (item != null)
            {
                ResponseDto result = new();
                Detail detail = JsonConvert.DeserializeObject<Detail>(item.Detail);
                DoctorDto doctor = new()
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    PhoneNumber = item.PhoneNumber,
                    Name = item.Name,
                    DateOfBirth = new DateTime(item.DateOfBirth),
                    ClinicNum = detail.ClinicNum,
                    Title = detail.Title,
                    Specialization = detail.Specialization,
                    ImageUrl = item.ImageUrl,
                    Rating = (int)item.AverageRating,
                    Introduction = item.Introduction
                };
                result.Result = doctor;
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
