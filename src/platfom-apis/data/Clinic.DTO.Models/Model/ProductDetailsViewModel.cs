using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.DTO.Models
{
    public class ProductDetailsViewModel
    {
        [Required]
        public string Id { get; init; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Specialization { get; set; }
        [Required]
        public int Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> ImageUrls { get; set; }
        
        public double Rating { get; set; }
        
        public string Authors { get; set; }
        
        public string Description { get; set; }
        
        public bool IsMainCombo { get; set; }
        
        public string Combo { get; set; }
        public string Etag { get; set; }
    }
}