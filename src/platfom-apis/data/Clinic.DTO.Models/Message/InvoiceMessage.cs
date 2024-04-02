using Clinic.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.DTO.Models.Message
{
    public class InvoiceMessage : BaseMessage
    {
        public string InvoiceId { get; set; }
    }
}
