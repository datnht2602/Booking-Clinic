using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using Clinic.Caching.Interfaces;
using Clinic.Common.Options;
using Clinic.Data.Models;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
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
                if (!dto.DoctorName.IsNullOrEmpty())
                {
                    listDoctors = listDoctors.Where(x => x.Title.Contains(dto.Title)).ToList();
                }
                if (!dto.DoctorName.IsNullOrEmpty())
                {
                    listDoctors = listDoctors.Where(x => x.Name.Contains(dto.DoctorName)).ToList();
                }  
                result.Result = listDoctors;
                return Ok(result);                                
        }
        [HttpGet]
        public async Task<IActionResult> GetDoctorSchedule(string userId)
        {
            BookingViewModel model = new();
            var items = await _userManager.Users.Include(x => x.ScheduleTimes).FirstOrDefaultAsync(u => u.Id == userId);
            var usersDto = autoMapper.Map<ApplicationUsersDto>(items);
            var userModels = autoMapper.Map<ApplicationUserModel>(usersDto);
            using var productsRequest = new HttpRequestMessage(HttpMethod.Get, $"{this.applicationSettings.Value.ProductsApiEndpoint}/getproducts");
            var productResponse = await httpClient.SendAsync(productsRequest).ConfigureAwait(false);
            if (productResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var products = await productResponse.Content.ReadFromJsonAsync<List<ProductListViewModel>>().ConfigureAwait(false);
                model.Name = userModels.Name;
                model.Specialization = userModels.Specialization;
                model.ProductListViewModels =
                    products.Where(x => (int)x.Specialization == userModels.Specialization).ToList();
                model.OrderedTime = userModels.OrderedTimes;
                model.DoctorId = userModels.Id;
                model.ClinicNum = int.Parse(userModels.ClinicNum);
            }
            ResponseDto result = new();
            result.Result = model;
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateDoctor([FromBody] DoctorDto doctorDto)
        {
            if (doctorDto == null)
            {
                return BadRequest();
            }

            Detail detail = new()
            {
                ClinicNum = doctorDto.ClinicNum,
                ExperienceYear = doctorDto.ExperienceYear,
                Specialization = doctorDto.Specialization,
                Title = doctorDto.Title
            };
            ApplicationUser customerUser = new()
            {
                UserName = doctorDto.UserName,
                Email = doctorDto.UserName,
                EmailConfirmed = true,
                PhoneNumber = "1111111111",
                Name = doctorDto.Name,
                DateOfBirth = doctorDto.DateOfBirth.Ticks,
                Detail = JsonConvert.SerializeObject(detail)
            };

            await _userManager.CreateAsync(customerUser, doctorDto.Password);
            await _userManager.AddToRoleAsync(customerUser, SD.DOCTOR);

            var temp2 = _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, customerUser.Name),
                new Claim(JwtClaimTypes.Role, SD.PATIENT),
            }).Result;
            return Ok();
        }
    }
}
