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
        public string UserId { get; set; }
        [Required]
        public int Specialization { get; set; }
        [Required]
        public string DoctorId { get; set; }
        [Required]
        public string OrderPlacedDate { get; set; }

        [Required]
        public List<ProductListViewModel> Services { get; set; }

        public string Description { get; set; }
        public string OrderStatus { get; set; }
        public int TrackingId { get; set; }
 
        public string Etag { get; set; }
        public string DeliveryDate {get; set;}
        public double OrderTotal { get; set; }
        public string PaymentMode { get; set; }
    }
}