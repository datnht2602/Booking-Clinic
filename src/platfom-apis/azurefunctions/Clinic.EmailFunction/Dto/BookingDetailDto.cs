
using System;
using System.Collections.Generic;

namespace Clinic.DTO.Models.Message
{
    public class BookingDetailDto 
    {
        public string Id { get; set; }
        public DateTime MessageCreated { get; set; }
        public string BookingId { get; set; }
        public string UserName { get; set; }

        public string UserId { get; set; }
        public int Specialization { get; set; }
        public string DoctorId { get; set; }

        public string DoctorName { get; set; }
        public string Description { get; set; }
        public string CouponCode { get; set; }
        public double DiscountTotal { get; set; }
        public List<ProductListViewModel> Products { get; set; }
        public string OrderStatus { get; set; }
        public int CartTotalItems { get; set; }
        public long OrderPlacedDate { get; set; }

        public BriefViewModel BriefViewModel { get; set; }

        public double OrderTotal { get; set; }
        public int PaymentMode { get; set; }
    }
}
