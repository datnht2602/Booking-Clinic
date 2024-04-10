using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.DTO.Models.Dto
{
    public class ChangeDoctorDto
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string ClinicNum { get; set; }
        public string ImageUrl { get; set; }
    }
}
