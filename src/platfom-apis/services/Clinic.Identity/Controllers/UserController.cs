using AutoMapper;
using Clinic.Caching.Interfaces;
using Clinic.Common.Options;
using Clinic.Data.Models;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Clinic.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        [HttpGet]
        public async Task<IActionResult> GetListDoctor()
        {
            /*var doctors = await this.cacheService.GetCacheAsync<IEnumerable<ApplicationUser>>("listdoctors").ConfigureAwait(false);
            if (doctors == null)
            {
                 doctors =
                await this.cacheService.AddOrUpdateCacheAsync<IEnumerable<ApplicationUser>>("listdoctors", doctors).ConfigureAwait(false);
            }*/
            var doctors = await _userManager.GetUsersInRoleAsync(SD.DOCTOR);
            var usersDto = autoMapper.Map<List<ApplicationUsersDto>>(doctors);
            var userModels = autoMapper.Map<List<ApplicationUserModel>>(usersDto);
            return Ok(autoMapper.Map<List<DoctorListViewModel>>(userModels));
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
            return Ok(model);
        }
    }
}
