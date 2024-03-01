using Clinic.Identity.Data;
using Clinic.Identity.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Clinic.Data.Models;
using Newtonsoft.Json;

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

            ApplicationUser doctorUser123123 = new()
            {
                UserName = "doctor@gmail.com",
                Email = "doctor@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "1111111111",
                Name = "Doctor"
            };

            _userManager.CreateAsync(doctorUser123123, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser123123, SD.DOCTOR).GetAwaiter().GetResult();

            var temp = _userManager.AddClaimsAsync(doctorUser123123, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser123123.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser123123.DateOfBirth.ToString())
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
            ApplicationUser doctorUser1 = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 4,
                     Title = "Doctor",
                     Specialization = 1,
                     ClinicNum  = "101",
                }),
                DateOfBirth = 0,
                ImageUrl = "",
                Introduction = "introduction",
                AverageRating = 4,
                UserName = "doctor101@gmail.com",
                Email = "doctor101@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "1111111111",
                Name = "Doctor"
            };

            _userManager.CreateAsync(doctorUser1, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser1, SD.DOCTOR).GetAwaiter().GetResult();

            var temp5 = _userManager.AddClaimsAsync(doctorUser1, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser1.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser1.DateOfBirth.ToString())
            }).Result;
            ApplicationUser doctorUser2 = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail 
                {
               ExperienceYear = 20,
               Title = "Chuyên gia",
               Specialization = 1,
               ClinicNum = "101",
            }),
       DateOfBirth = new DateTime(1977-02-19).Ticks,
       ImageUrl = "",
       Introduction = "PGS. TS. BS Trần Minh Hoàng là chuyên gia hàng đầu về mạch máu, được đào tạo chuyên sâu tại các trường đại học y khoa danh tiếng ở Pháp. PGS Minh Hoàng dành gần 10 năm học tập và làm việc tại Pháp tập trung vào điều trị, phẫu thuật tim mạch, mạch máu. Năm 2011, ông trở về TP. HCM tiếp tục cống hiến và phát triển y học nước nhà. Với hơn 20 năm kinh nghiệm trong lĩnh vực điều trị các bệnh lý về tĩnh mạch, động mạch; PGS Minh Hoàng đã thực hiện thành công nhiều ca phẫu thuật khó giúp nhiều đôi chân đau nhức của người bệnh có thể tự do sải bước, nâng cao chất lượng cuộc sống.",
       AverageRating = 4.7,
       UserName = "minhhoang101@gmail.com",
       Email = "minhhoang101@gmail.com",
       EmailConfirmed = true,
       PhoneNumber = "0975636921",
       Name = "Trần Minh Hoàng"
       
            };

        _userManager.CreateAsync(doctorUser2, "Doctor123!").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(doctorUser2, SD.DOCTOR).GetAwaiter().GetResult();

        var temp6 = _userManager.AddClaimsAsync(doctorUser2, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, doctorUser2.Name),
            new Claim(JwtClaimTypes.Role, SD.DOCTOR),
            new Claim("dob", doctorUser2.DateOfBirth.ToString())
        }).Result;

ApplicationUser doctorUser7 = new()
    {
      Detail = JsonConvert.SerializeObject(new Detail
      {
          ExperienceYear = 25,
          Title = "Phó giám đốc",
          Specialization = 2,
          ClinicNum = "102",
      }),
       DateOfBirth = new DateTime(1970-10-28).Ticks,
       ImageUrl = "",
       Introduction = "Hơn 25 năm kinh nghiệm trong phẫu thuật và điều trị các bệnh lý Tim Mạch - Tim Mạch Can Thiệp - Can Thiệp Mạch Máu, từng công tác tại nhiều bệnh viện lớn tại TP. HCM. Trưởng khoa tại nhiều bệnh viện lớn. Chủ nhiệm nhiều công trình nghiên cứu khoa học lớn về can thiệp nội mạch điều trị phình động mạch, hẹp-tắc động mạch, chấn thương mạch máu, can thiệp tiêu sợi huyết… Chủ tọa và tham gia báo cáo tại nhiều hội nghị tim mạch trong nước và quốc tế. Chuyên môn giỏi, tận tình với mọi người bệnh và được rất nhiều người bệnh kính phục và yêu mến.",
       AverageRating = 4.9,
       UserName = "duytrang102@gmail.com",
       Email = "duytrang102@gmail.com",
       EmailConfirmed = true,
       PhoneNumber = "0864762801",
       Name = "Dương Duy Trang"
    };

        _userManager.CreateAsync(doctorUser7, "Doctor123!").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(doctorUser7, SD.DOCTOR).GetAwaiter().GetResult();

        var temp7 = _userManager.AddClaimsAsync(doctorUser7, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, doctorUser7.Name),
            new Claim(JwtClaimTypes.Role, SD.DOCTOR),
            new Claim("dob", doctorUser7.DateOfBirth.ToString())
        }).Result;

ApplicationUser doctorUser8 = new()
    {
      Detail = JsonConvert.SerializeObject(new Detail
      {
          ExperienceYear = 30,
          Title = "Trưởng khoa",
          Specialization = 3,
          ClinicNum = "103",
      }),
       DateOfBirth = new DateTime(1969-03-12).Ticks,
       ImageUrl = "",
       Introduction = "BS.CKII. Đặng Đình Hoan là một bác sĩ có chuyên môn cao và dày dặn kinh nghiệm về chẩn đoán hình ảnh, X-quang can thiệp. Trong gần 40 năm gắn bó với lĩnh vực Chẩn đoán hình ảnh, BS. Đặng Đình Hoan đã có hơn 30 năm đảm nhiệm chức vụ Trưởng khoa Chẩn đoán hình ảnh, Bệnh viện Bình Dân.",
       AverageRating = 4.8,
       UserName = "dinhhoan103@gmail.com",
       Email = "dinhhoan103@gmail.com",
       EmailConfirmed = true,
       PhoneNumber = "0386391032",
       Name = "Đặng Đình Hoan"
    };

        _userManager.CreateAsync(doctorUser8, "Doctor123!").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(doctorUser8, SD.DOCTOR).GetAwaiter().GetResult();

        var temp8 = _userManager.AddClaimsAsync(doctorUser8, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, doctorUser8.Name),
            new Claim(JwtClaimTypes.Role, SD.DOCTOR),
            new Claim("dob", doctorUser8.DateOfBirth.ToString())
        }).Result;

ApplicationUser doctorUser9 = new()
    {
      Detail = JsonConvert.SerializeObject(new Detail
      {
          ExperienceYear = 29,
          Title = "Trưởng khoa",
          Specialization = 4,
          ClinicNum = "104",
      }),
       DateOfBirth = new DateTime(1970-10-30).Ticks,
       ImageUrl = "",
       Introduction = "BS. CKII. Trương Thiện Niềm là bác sĩ đa khoa, được đào tạo chuyên khoa I Nội lão khoa, chuyên khoa II Nội chung, nhiều năm kinh nghiệm khám Nội tổng quát và đã vinh dự nhận được nhiều bằng khen cấp tỉnh và Trung ương.",
       AverageRating = 4.7,
       UserName = "thienniem104@gmail.com",
       Email = "thienniem104@gmail.com",
       EmailConfirmed = true,
       PhoneNumber = "0905637828",
       Name = "Trương Thiện Niềm"
    };

        _userManager.CreateAsync(doctorUser9, "Doctor123!").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(doctorUser9, SD.DOCTOR).GetAwaiter().GetResult();

        var temp9 = _userManager.AddClaimsAsync(doctorUser9, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, doctorUser9.Name),
            new Claim(JwtClaimTypes.Role, SD.DOCTOR),
            new Claim("dob", doctorUser9.DateOfBirth.ToString())
        }).Result;

ApplicationUser doctorUser10 = new()
    {
      Detail = JsonConvert.SerializeObject(new Detail
      {
          ExperienceYear = 22,
          Title = "Trưởng khoa ngoại",
          Specialization = 5,
          ClinicNum = "105",
      }),
       DateOfBirth = new DateTime(1977-04-18).Ticks,
       ImageUrl = "",
       Introduction = "Lĩnh vực lâm sàng chuyên sâu: Chuyên gia Can thiệp xâm lấn tối thiểu các bệnh lý đường mật - tụy (lành tính lẫn ác tính – ERCP và PTBD)",
       AverageRating = 4.7,
       UserName = "thetoan105@gmail.com",
       Email = "thetoan105@gmail.com",
       EmailConfirmed = true,
       PhoneNumber = "0914762937",
       Name = "Nguyễn Thế Toàn"
    };

        _userManager.CreateAsync(doctorUser10, "Doctor123!").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(doctorUser10, SD.DOCTOR).GetAwaiter().GetResult();

        var temp10 = _userManager.AddClaimsAsync(doctorUser10, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, doctorUser10.Name),
            new Claim(JwtClaimTypes.Role, SD.DOCTOR),
            new Claim("dob", doctorUser10.DateOfBirth.ToString())
        }).Result;

ApplicationUser doctorUser11 = new()
    {
      Detail = JsonConvert.SerializeObject(new Detail
      {
          ExperienceYear = 22,
          Title = "Bác sĩ Ngoại Thần Kinh",
          Specialization = 5,
          ClinicNum = "106",
      }),
       DateOfBirth = new DateTime(1977-04-18).Ticks,
       ImageUrl = "",
       Introduction = "Một số thành tích cá nhân đặc biệt: Bằng khen của Bộ Trưởng Bộ Y Tế khen thưởng phẫu thuật thành công ca bệnh nhi bị dao đâm xuyên hộp sọ tại BV Nhi Đồng I; Tham gia nghiên cứu khoa học và có các bài báo cáo tại hội nghị Ngoại Thần Kinh toàn quốc 2008, 2009 và 2013.",
       AverageRating = 4.7,
       UserName = "dangkhoa106@gmail.com",
       Email = "dangkhoa106@gmail.com",
       EmailConfirmed = true,
       PhoneNumber = "0367299302",
       Name = "Phùng Đăng Khoa"
    };

        _userManager.CreateAsync(doctorUser11, "Doctor123!").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(doctorUser11, SD.DOCTOR).GetAwaiter().GetResult();

        var temp11 = _userManager.AddClaimsAsync(doctorUser11, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, doctorUser11.Name),
            new Claim(JwtClaimTypes.Role, SD.DOCTOR),
            new Claim("dob", doctorUser11.DateOfBirth.ToString())
        }).Result;

ApplicationUser doctorUser12 = new()
    {
      Detail = JsonConvert.SerializeObject(new Detail
      {
          ExperienceYear = 22,
          Title = "Trưởng Đơn vị",
          Specialization = 2,
          ClinicNum = "107",
      }),
       DateOfBirth = new DateTime(1965-03-31).Ticks,
       ImageUrl = "",
       Introduction = "Quá trình đào tạo và Bằng cấp chuyên môn: 2007 – 2009: Thạc sĩ Y khoa - Trường Đại học Y Dược TP. Hồ Chí Minh.; 1984 – 1990: Bác sĩ đa khoa - Trường Đại học Y Dược TP. Hồ Chí Minh.",
       AverageRating = 4.7,
       UserName = "thanhtam107@gmail.com",
       Email = "thanhtam107@gmail.com",
       EmailConfirmed = true,
       PhoneNumber = "0867286379",
       Name = "Dương Thị Thanh Tâm"
    };

        _userManager.CreateAsync(doctorUser12, "Doctor123!").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(doctorUser12, SD.DOCTOR).GetAwaiter().GetResult();

        var temp12 = _userManager.AddClaimsAsync(doctorUser12, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, doctorUser12.Name),
            new Claim(JwtClaimTypes.Role, SD.DOCTOR),
            new Claim("dob", doctorUser12.DateOfBirth.ToString())
        }).Result;

ApplicationUser doctorUser13 = new()
    {
        Detail = JsonConvert.SerializeObject(new Detail
        {
        ExperienceYear = 33,
        Title = "Trưởng khoa",
        Specialization = 6,
        ClinicNum  = "108",
        }),
        DateOfBirth = new DateTime(1976-06-23).Ticks,
        ImageUrl = "",
        Introduction = "Với gần 33 năm là bác sĩ tại các bệnh viện nhi lớn của TP.HCM, BS CKII NGUYỄN THỊ THU HÀ có nhiều kinh nghiệm trong chẩn đoán và điều trị bệnh lý nhi khoa, đặc biệt có trên 20 năm làm việc trong lãnh vực tiêu hóa nhi.",
        AverageRating = 5,
        UserName = "Ha108@gmail.com",
        Email = "Ha108@gmail.com",
        EmailConfirmed = true,
        PhoneNumber = "0992649244",
        Name = "Nguyễn Thị Thu Hà"
    };

        _userManager.CreateAsync(doctorUser13, "Doctor123!").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(doctorUser13, SD.DOCTOR).GetAwaiter().GetResult();

        var temp13 = _userManager.AddClaimsAsync(doctorUser13, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, doctorUser13.Name),
            new Claim(JwtClaimTypes.Role, SD.DOCTOR),
            new Claim("dob", doctorUser13.DateOfBirth.ToString())
        }).Result;

ApplicationUser doctorUser14 = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 25,
                     Title = "Trưởng khoa",
                     Specialization = 7,
                     ClinicNum  = "109",
                }),
                DateOfBirth = new DateTime(1978-02-13).Ticks,
                ImageUrl = "",
                Introduction = "Bác sĩ Võ Đức Hiếu có rất nhiều kinh nghiệm về tư vấn phòng ngừa, tầm soát phát hiện sớm, chẩn đoán chính xác và hướng dẫn điều trị ung thư, nhất là trong điều trị ung thư vú, ung thư đường tiêu hóa, bệnh lý tuyến giáp,...",
                AverageRating = 5,
                UserName = "Hieu109@gmail.com",
                Email = "Hieu109@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0948264925",
                Name = "Võ Đức Hiếu"
            };

            _userManager.CreateAsync(doctorUser14, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser14, SD.DOCTOR).GetAwaiter().GetResult();

            var temp14 = _userManager.AddClaimsAsync(doctorUser14, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser14.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser14.DateOfBirth.ToString())
            }).Result;

ApplicationUser doctorUser15 = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 28,
                     Title = "Trưởng khoa",
                     Specialization = 8,
                     ClinicNum  = "110",
                }),
                DateOfBirth = new DateTime(1983-08-03).Ticks,
                ImageUrl = "",
                Introduction = "Phòng mạch của Tiến sĩ, Bác sĩ Nguyễn Bá Thắng chuyên khám và điều trị các bệnh về nội khoa - tâm thần kinh như: trầm cảm, rối loạn tâm thần, ác mộng, đái dầm, lo âu, nhớ kém, ám ảnh, suy nhược thần kinh, mất ngủ, khó tập trung, rối loạn thần kinh thực vật, rối loạn tiêu hóa, đau thần kinh tọa, động kinh, đau nhức đầu, chóng mặt,…",
                AverageRating = 5,
                UserName = "Thang110@gmail.com",
                Email = "Thang110@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0972046204",
                Name = "Nguyễn Bá Thắng"
            };

            _userManager.CreateAsync(doctorUser15, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser15, SD.DOCTOR).GetAwaiter().GetResult();

            var temp15 = _userManager.AddClaimsAsync(doctorUser15, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser15.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser15.DateOfBirth.ToString())
            }).Result;
ApplicationUser doctorUser16 = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 10,
                     Title = "Trưởng khoa",
                     Specialization = 9,
                     ClinicNum  = "111",
                }),
                DateOfBirth = new DateTime(1988-01-01).Ticks,
                ImageUrl = "",
                Introduction = "BS.CKI. Nguyễn Thị Quỳnh Đan đã có gần 10 năm kinh nghiệm điều trị y học cổ truyền, được đào tạo Chuyên khoa I Y học cổ truyền tại Đại học Y Dược Huế.",
                AverageRating = 5,
                UserName = "dan111@gmail.com",
                Email = "dan111@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0909538382",
                Name = "Nguyễn Thị Quỳnh Đan"
            };

            _userManager.CreateAsync(doctorUser16, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser16, SD.DOCTOR).GetAwaiter().GetResult();

            var temp16 = _userManager.AddClaimsAsync(doctorUser16, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser16.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser16.DateOfBirth.ToString())
            }).Result;

            ApplicationUser doctorUser17 = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 10,
                     Title = "Trưởng khoa",
                     Specialization = 10,
                     ClinicNum  = "112",
                }),
                DateOfBirth = new DateTime(1989-04-20).Ticks,
                ImageUrl = "",
                Introduction = "BS Hoàng Đức với 10 năm kinh nghiệm, từng công tác tại các bệnh viện lớn tại TP.HCM, hiện đang công tác tại khoa Nội Thần kinh - Đột quỵ, Bệnh viện Gia An 115. Một bác sĩ luôn tích cực phấn đấu, chủ động nâng cao tay nghề và được rất nhiều bệnh nhân yêu quý bởi sự tận tình, nhiệt tâm.",
                AverageRating = 5,
                UserName = "Duc112@gmail.com",
                Email = "Duc112@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0975391540",
                Name = "Đinh Hoàng Đức"
            };

            _userManager.CreateAsync(doctorUser17, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser17, SD.DOCTOR).GetAwaiter().GetResult();

            var temp17 = _userManager.AddClaimsAsync(doctorUser17, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser17.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser17.DateOfBirth.ToString())
            }).Result;

ApplicationUser doctorUser18 = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 8,
                     Title = "Trưởng khoa",
                     Specialization = 11,
                     ClinicNum  = "113",
                }),
                DateOfBirth = new DateTime(1984-08-27).Ticks,
                ImageUrl = "",
                Introduction = "Hơn 8 năm kinh nghiệm trong lĩnh vực Răng - Hàm - Mặt, BS. CKI. Nguyễn Kim Thanh có chuyên môn sâu về khám và điều trị các bệnh lý: nha khoa tổng quát, nha khoa thẩm mỹ, nha khoa dự phòng, đặc biệt ứng dụng nha khoa kỹ thuật cao như: Phẫu thuật cấy ghép Implant…",
                AverageRating = 5,
                UserName = "Thanh113@gmail.com",
                Email = "Thanh113@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0969264793",
                Name = "Nguyễn Kim Thanh"
            };

            _userManager.CreateAsync(doctorUser18, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser18, SD.DOCTOR).GetAwaiter().GetResult();

            var temp18 = _userManager.AddClaimsAsync(doctorUser18, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser18.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser18.DateOfBirth.ToString())
            }).Result;

/*
ApplicationUser doctorUser = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 10,
                     Title = "Trưởng khoa",
                     Specialization = "Khoa Vật lý trị liệu và Phục hồi chức năng",
                     ClinicNum  = "114",
                }),
                DateOfBirth = "1990-10-30",
                ImageUrl = "",
                Introduction = "Phục hồi chức năng các bệnh lý, tổn thương hoặc sau phẫu thuật thần kinh, chấn thương chỉnh hình và cơ xương khớp, hô hấp-tim mạch, ung thư, rối loạn nuốt, rối loạn giọng nói, liệt dây thanh, mất ngôn ngữ, các rối loạn giao tiếp do tổn thương thần kinh, nói lắp ở người lớn qua các giai đoạn cấp tính đến mạn tính, tại viện (khoa ICU, phòng hậu phẫu, nội trú) và cộng đồng (bệnh nhân ngoại trú và tư vấn môi trường sinh hoạt tại nhà) theo mô hình ICF.",
                AverageRating = 5,
                UserName = "Thao114@gmail.com",
                Email = "Thao114@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0952846294",
                Name = "Huỳnh Ngọc Thảo"
            };

            _userManager.CreateAsync(doctorUser, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser, SD.DOCTOR).GetAwaiter().GetResult();

            var temp3 = _userManager.AddClaimsAsync(doctorUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser.DateOfBirth.ToString())
            }).Result;

ApplicationUser doctorUser = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 15,
                     Title = "Trưởng khoa",
                     Specialization = "Khoa Nội tiết",
                     ClinicNum  = "115",
                }),
                DateOfBirth = "1983-08-27",
                ImageUrl = "",
                Introduction = "Hơn 15 năm khoác áo blouse cũng là chừng ấy năm ông gắn bó trong lĩnh vực hồi sức tích cực chống độc. Trước khi về công tác tại Bệnh viện Gia An 115, ông đã có thời gian dài với 16 năm làm việc tại Bệnh viện Nhân dân 115. Tại đây, ông đã nhận được rất nhiều giấy khen của Bệnh viện, Giấy khen của Sở Y tế TP.HCM và vinh dự nhận Bằng khen hoàn thành xuất sắc nhiệm vụ do UBND Thành phố HCM trao tặng năm 2019.",
                AverageRating = 5,
                UserName = "Sang115@gmail.com",
                Email = "Sang115@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0963529564",
                Name = "Trần Thanh Sang"
            };

            _userManager.CreateAsync(doctorUser, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser, SD.DOCTOR).GetAwaiter().GetResult();

            var temp3 = _userManager.AddClaimsAsync(doctorUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser.DateOfBirth.ToString())
            }).Result;

ApplicationUser doctorUser = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 14,
                     Title = "Chuyên gia",
                     Specialization = "Khoa Nội tổng hợp",
                     ClinicNum  = "116",
                }),
                DateOfBirth = "1989-05-20",
                ImageUrl = "",
                Introduction = "BS. CKI. Phạm Công Doanh chuyên khoa Hồi Sức Cấp Cứu - Chống Độc và đã có gần 15 năm kinh nghiệm, trong đó có hơn 10 năm công tác tại khoa Hồi Sức Tích Cực - Chống Độc, BV Nhân dân 115. Luôn nỗ lực nâng cao tay nghề, BS. Phạm Công Doanh đã tham gia nhiều khóa đào tạo ở trong nước cũng như quốc tế như tại Hàn Quốc, Đài Loan…",
                AverageRating = 5,
                UserName = "Doanh116@gmail.com",
                Email = "Doanh116@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0992659253",
                Name = "Phạm Công Doanh"
            };

            _userManager.CreateAsync(doctorUser, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser, SD.DOCTOR).GetAwaiter().GetResult();

            var temp3 = _userManager.AddClaimsAsync(doctorUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser.DateOfBirth.ToString())
            }).Result;

ApplicationUser doctorUser = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 20,
                     Title = "Chuyên gia",
                     Specialization = "Khoa Nhi",
                     ClinicNum  = "117",
                }),
                DateOfBirth = "1982-11-18",
                ImageUrl = "",
                Introduction = "Hơn 20 năm kinh nghiệm, BS.CKI. Trần Thị Mai Uyên đã khám, điều trị hiệu quả cho hàng nghìn bệnh nhân mắc các bệnh liên quan đến thần kinh – đột quỵ. Với chuyên môn cao và sự tận tâm, dịu dàng, BS. Mai Uyên đã được rất nhiều bệnh nhân và thân nhân cảm phục, yêu mến.",
                AverageRating = 5,
                UserName = "Uyen117@gmail.com",
                Email = "Uyen117@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0929462945",
                Name = "Trần Thị Mai Uyên"
            };

            _userManager.CreateAsync(doctorUser, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser, SD.DOCTOR).GetAwaiter().GetResult();

            var temp3 = _userManager.AddClaimsAsync(doctorUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser.DateOfBirth.ToString())
            }).Result;

ApplicationUser doctorUser = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 20,
                     Title = "Chuyên gia",
                     Specialization = "Khoa Nội tiết",
                     ClinicNum  = "118",
                }),
                DateOfBirth = "1981-07-03",
                ImageUrl = "",
                Introduction = "Tiến sĩ, Bác sĩ Trần Quang Nam hiện đang là Trưởng khoa Nội Tiết bệnh viện Đại học Y Dược TP. HCM, Phó Trưởng Bộ môn Nội tiết tại Đại học Y dược TP. HCM. Bác sĩ có nhiều năm kinh nghiệm trong việc chuyên khám và điều trị các bệnh như đái tháo đường, bệnh bướu cổ, bệnh nội tiết và các bệnh nội khoa.",
                AverageRating = 5,
                UserName = "Nam118@gmail.com",
                Email = "Nam118@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0919462945",
                Name = "Trần Quang Nam"
            };

            _userManager.CreateAsync(doctorUser, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser, SD.DOCTOR).GetAwaiter().GetResult();

            var temp3 = _userManager.AddClaimsAsync(doctorUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser.DateOfBirth.ToString())
            }).Result;

ApplicationUser doctorUser = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 17,
                     Title = "Chuyên gia",
                     Specialization = "Khoa Nhi",
                     ClinicNum  = "119",
                }),
                DateOfBirth = "1984-12-19",
                ImageUrl = "",
                Introduction = "Bác sĩ Chuyên khoa II Lê Thị Minh Hồng hiện đang là Phó Giám đốc Bệnh viện Nhi Đồng 2. Bác sĩ trực tiếp khám bệnh theo yêu cầu chất lượng cao tại Bệnh Viện Nhi Đồng 2 và phòng khám Nhi khoa.",
                AverageRating = 5,
                UserName = "Hong119@gmail.com",
                Email = "Hong119@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0903659254",
                Name = "Lê Thị Minh Hồng"
            };

            _userManager.CreateAsync(doctorUser, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser, SD.DOCTOR).GetAwaiter().GetResult();

            var temp3 = _userManager.AddClaimsAsync(doctorUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser.DateOfBirth.ToString())
            }).Result;

ApplicationUser doctorUser = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 20,
                     Title = "Trưởng khoa",
                     Specialization = "Khoa Tiêu Hóa",
                     ClinicNum  = "120",
                }),
                DateOfBirth = "1980-03-10",
                ImageUrl = "",
                Introduction = "Phó Giáo sư, Tiến sĩ, Bác sĩ Lâm Việt Trung đã có hơn 20 năm kinh nghiệm trong lĩnh vực Tiêu hóa. Là một bác sĩ giỏi, có bề dày kinh nghiệm cũng như chuyên môn cao, PGS.TS.BS Lâm Việt Trung hiện là Trưởng khoa Ngoại tiêu hóa - Bệnh viện Chợ Rẫy và hiện đang giữ chức vụ Phó Giám Đốc Bệnh Viện Chợ Rẫy.",
                AverageRating = 5,
                UserName = "Trung120@gmail.com",
                Email = "Trung120@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0962946204",
                Name = "Lâm Việt Trung"
            };

            _userManager.CreateAsync(doctorUser, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser, SD.DOCTOR).GetAwaiter().GetResult();

            var temp3 = _userManager.AddClaimsAsync(doctorUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser.DateOfBirth.ToString())
            }).Result;        

ApplicationUser doctorUser = new()
            {
                Detail = JsonConvert.SerializeObject(new Detail
                {
                     ExperienceYear = 23,
                     Title = "Trưởng khoa",
                     Specialization = "Khoa Tai-Mũi-Họng",
                     ClinicNum  = "121",
                }),
                DateOfBirth = "1978-07-10",
                ImageUrl = "",
                Introduction = "Phòng khám trang bị đầy đủ các thiết bị cần thiết cho việc khám và chữa bệnh về tai mũi họng như đèn soi tai, đèn clar,... Phòng khám tai mũi họng của bác sĩ Hoàn chuyên điều trị viêm xoang, viêm họng hạt, viêm tai giữa mãn tính, viêm mũi dị ứng, viêm VA và các dịch vụ khám chữa bệnh khác.",
                AverageRating = 5,
                UserName = "syhoan121@gmail.com",
                Email = "syhoan121@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0384672639",
                Name = "Phạm Sỹ Hoàn"
            };

            _userManager.CreateAsync(doctorUser, "Doctor123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(doctorUser, SD.DOCTOR).GetAwaiter().GetResult();

            var temp3 = _userManager.AddClaimsAsync(doctorUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, doctorUser.Name),
                new Claim(JwtClaimTypes.Role, SD.DOCTOR),
                new Claim("dob", doctorUser.DateOfBirth.ToString())
            }).Result; 
            */

        }
    }
}
