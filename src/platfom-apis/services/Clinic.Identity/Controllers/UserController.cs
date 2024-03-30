using AutoMapper;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Clinic.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Identity.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper autoMapper;
        public UserController(UserManager<ApplicationUser> userManager,
            IMapper autoMapper)
        {
            _userManager = userManager;
            this.autoMapper = autoMapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetListDoctor()
        {
<<<<<<< Updated upstream
            var items = await _userManager.GetUsersInRoleAsync(SD.DOCTOR);
            var usersDto = autoMapper.Map<List<ApplicationUsersDto>>(items);
            var userModels = autoMapper.Map<List<ApplicationUserModel>>(usersDto);
            return  Ok(autoMapper.Map<List<DoctorListViewModel>>(userModels));
=======
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
                PhoneNumber = doctorDto.PhoneNumber,
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
        [HttpGet]
        public async Task<IActionResult> GetSchedule(string doctorId)
        {
            var items = await _userManager.Users.Include(x => x.ScheduleTimes).FirstOrDefaultAsync(u => u.Id == doctorId);
            ResponseDto result = new();
            result.Result = items.ScheduleTimes.Select(x => x.Time).ToList();
            return Ok(result);
>>>>>>> Stashed changes
        }
    }
}
