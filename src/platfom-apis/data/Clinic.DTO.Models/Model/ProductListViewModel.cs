using System;
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


        public Specialization Specialization { get; set; }


        public int Price { get; set; }
    }
}
