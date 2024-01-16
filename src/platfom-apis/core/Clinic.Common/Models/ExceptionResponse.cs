using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Common.Models
{
    public class ExceptionResponse
    {
        public string ErrorMessage { get; set; }
        public string CorrelationIdentifer { get; set; }
        public string InnerException { get; set; }
    }
}
