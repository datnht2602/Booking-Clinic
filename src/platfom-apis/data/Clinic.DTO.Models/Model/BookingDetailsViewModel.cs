using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.DTO.Models
{
    public class BookingDetailsViewModel
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }

        public string UserId { get; set; }
        [Required]
        public int Specialization { get; set; }
        [Required]
        public string DoctorId { get; set; }
        [Required]
        public string OrderPlacedDate { get; set; }
        public string Description { get; set; }

        [Required]
        public List<ProductListViewModel> Products { get; set; }
        
        public double OrderTotal { get; set; }
        public string PaymentMode { get; set; }
        public string OrderStatus { get; set; }
        public int TrackingId { get; set; }
 
        public string Etag { get; set; }

    }
}