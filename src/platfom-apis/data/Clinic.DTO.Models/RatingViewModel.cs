using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.DTO.Models
{
    public class RatingViewModel
    {
        /// <summary>
        /// Gets or sets the stars.
        /// </summary>
        /// <value>
        /// The stars.
        /// </value>
        public int Stars { get; set; }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public int Percentage { get; set; }
    }
}