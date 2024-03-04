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
            var items = await _userManager.GetUsersInRoleAsync(SD.DOCTOR);
            var usersDto = autoMapper.Map<List<ApplicationUsersDto>>(items);
            var userModels = autoMapper.Map<List<ApplicationUserModel>>(usersDto);
            return  Ok(autoMapper.Map<List<DoctorListViewModel>>(userModels));
        }
        [HttpGet]
        public async Task<IActionResult> GetDoctorSchedule([FromRoute]string userId)
        {
            List<long> result = new();
            var items = await _userManager.Users.Include(x=> x.ScheduleTimes).FirstOrDefaultAsync(u => u.Id == userId);
            if (items != null && items.ScheduleTimes != null)
            {
                result = items.ScheduleTimes.Select(x => x.Time).ToList();
            }
            return  Ok(result);
        }
    }
}
