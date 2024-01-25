using Clinic.Identity.Data;
using Clinic.Identity.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Clinic.Identity.IDBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DBInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;          
        }
        public void Initialize()
        {
            if(_roleManager.FindByNameAsync(SD.ADMIN).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(SD.ADMIN)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.PATIENT)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.DOCTOR)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.MANAGER)).GetAwaiter().GetResult();
            }else
            {
                return;
            }
            ApplicationUser adminUser = new()
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "1111111111",
                Name = "Admin"
            };

            _userManager.CreateAsync(adminUser, "Admin123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, SD.ADMIN).GetAwaiter().GetResult();

            var temp1 = _userManager.AddClaimsAsync(adminUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, adminUser.Name),
                new Claim(JwtClaimTypes.Role, SD.ADMIN),
                new Claim("dob", adminUser.DateOfBirth.ToString())
            }).Result;

            ApplicationUser customerUser = new()
            {
                UserName = "customer@gmail.com",
                Email = "customer@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "1111111111",
                Name = "Customer"
            };

            _userManager.CreateAsync(customerUser, "Customer123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, SD.PATIENT).GetAwaiter().GetResult();

            var temp2 = _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, customerUser.Name),
                new Claim(JwtClaimTypes.Role, SD.PATIENT),
                new Claim("dob", customerUser.DateOfBirth.ToString())
            }).Result;

            ApplicationUser doctorUser = new()
            {
                UserName = "doctor@gmail.com",
                Email = "doctor@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "1111111111",
                Name = "Doctor"
            };

            _userManager.CreateAsync(doctorUser, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser, SD.DOCTOR).GetAwaiter().GetResult();

            var temp3 = _userManager.AddClaimsAsync(doctorUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser.DateOfBirth.ToString())
            }).Result;

            ApplicationUser managerUser = new()
            {
                UserName = "manager@gmail.com",
                Email = "manager@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "1111111111",
                Name = "Manager"
            };

            _userManager.CreateAsync(managerUser, "Manager123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(managerUser, SD.MANAGER).GetAwaiter().GetResult();

            var temp4 = _userManager.AddClaimsAsync(managerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, managerUser.Name),
                new Claim(JwtClaimTypes.Role, SD.PATIENT),
                new Claim("dob", managerUser.DateOfBirth.ToString())
            }).Result;
        }
    }
}
