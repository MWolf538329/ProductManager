﻿namespace ProductManager.MVC.Models
{
    public class AssortmentProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category {  get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Contents { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
