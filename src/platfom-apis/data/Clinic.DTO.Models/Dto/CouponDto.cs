namespace Clinic.DTO.Models.Dto;

public class CouponDto
{
    public string Id { get; set; }
    public string CouponCode { get; set; }
    public double DiscountAmount { get; set; }
    public bool IsEnable { get; set; }
}