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
        public string Specialization { get; set; }
        [Required]
        [Range(0, 9999)]
        public int Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<RatingViewModel> Rating { get; set; }
        public List<string> Authors { get; set; }
        public string Etag { get; set; }
    }
}