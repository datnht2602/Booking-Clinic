using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.DTO.Models
{
    public class ProductListViewModel
    {
         /// <summary>
        /// Gets or sets the image url.
        /// </summary>
        /// <value>
        /// The image url.
        /// </value>
        public List<Uri> ImageUrls { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the average rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        public double AverageRating { get; set; }
    }
}