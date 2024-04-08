namespace Clinic.DTO.Models.Model;

public class HealthPackageModel
{
    public string Id { get; set; }


    public string Name { get; set; }
    
    public double Price { get; set; }
        
    public string Description { get; set; }

    public List<ProductListViewModel> Products { get; set; }
    
    public string Etag { get; set; }
}