using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.DTO.Models.Model
{
    public class UpdateSchedule
    {
        public string DoctorId { get; set; }
        public string UserId { get; set; }
        public long OrderTime { get; set; }
        public BriefViewModel Detail { get; set; }
        public int ServiceCount { get; set; }
    }
}
