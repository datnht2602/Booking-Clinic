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

        public string DoctorName { get; set; }
        [Required]
        public long OrderPlacedDate { get; set; }
        public string Description { get; set; }
        public string CouponCode { get; set; }
        public double DiscountTotal { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CartTotalItems { get; set; }
        [Required]
        public List<ProductListViewModel> Products { get; set; }
        
        public double OrderTotal { get; set; }
        public int PaymentMode { get; set; }
        public string OrderStatus { get; set; }
 
        public string Etag { get; set; }

    }
}