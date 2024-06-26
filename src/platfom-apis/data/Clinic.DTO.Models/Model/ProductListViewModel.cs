﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.DTO.Models
{
    public class ProductListViewModel
    {
        public string Id { get; set; }


        public string Name { get; set; }
        public string Description { get; set; }

        public Specialization Specialization { get; set; }

        public int Price { get; set; }
        
        public string FormattedTotal => $"{Price:N0}";
        public string FormattedName => $"Service Name: {Name}, Price: {Price.ToString("C")}";
    }
}
