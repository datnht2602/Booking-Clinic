using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Clinic.DTO.Models;

namespace InvoiceSamurai.Shared;
public record InvoiceModel
{


    private int _collectionHaschanged = -1;
    public InvoiceModel()
    {
        Itens = new();
    }
    
    public CustomerModel Customer { get; set; }

    public int Number { get; set; } = 58656;
    public string BusinessRegistrationNumber { get; set; } = "25.888.666/0000-88";
    public string CompanyName { get; set; } = "Young Blood Clinic";
    public string Coupon { get; set; } = "Young Blood Clinic";

    public List<ProductListViewModel> Itens { get; set; }



    public string FormattedPrice => $"$ {Itens.Sum(c => c.Price):n2}";


}

