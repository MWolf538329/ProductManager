﻿namespace ProductManager.MVC.Models
{
    public class CustomerViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
