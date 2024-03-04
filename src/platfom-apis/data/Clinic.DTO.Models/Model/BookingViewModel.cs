namespace Clinic.DTO.Models;

public class BookingViewModel
{
    public List<long> OrderedTime { get; set; }
    public string Name { get; set; }
    public int Specialization { get; set; }
    public List<ProductListViewModel> ProductListViewModels { get; set; }
}