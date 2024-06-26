namespace Clinic.DTO.Models;

public class BookingViewModel
{
    public string DoctorId { get; set; }
    public List<long> OrderedTime { get; set; }
    public string Name { get; set; }
    public int Specialization { get; set; }
    public List<ProductListViewModel> ProductListViewModels { get; set; }
    public double OrderTotal { get; set; }
    public int ClinicNum { get; set; }
    public string Image { get; set; }
}