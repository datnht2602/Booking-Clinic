using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.DTO.Models
{
    public enum OrderStatus
    {
        /// <summary>
        /// Order status while in cart.
        /// </summary>
        Cart,

        /// <summary>
        /// Order status after purchase.s
        /// </summary>
        Submitted,
    }
}