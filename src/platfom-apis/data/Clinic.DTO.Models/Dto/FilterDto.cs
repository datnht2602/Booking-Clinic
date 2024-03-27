using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.DTO.Models.Dto
{
    public class FilterDto
    {
        public int Specialization { get; set; }
        public string Title { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
    }
}
