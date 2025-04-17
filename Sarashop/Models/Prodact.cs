﻿namespace Sarashop.Models
{
    public class Prodact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public double Quntity { get; set; }
        public bool State { get; set; }
        public double Rate { get; set; }
        public int CategoryId { get; set; }
        public int BrandID { get; set; }
        public string mainImg { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }


    }
}
