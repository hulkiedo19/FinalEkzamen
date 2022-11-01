using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DemEkzVariant3.Pustovoy.Domain.Entities
{
    public partial class Product
    {
        [NotMapped]
        private string? _image;
        [NotMapped]
        private decimal _minCostForAgent;

        public Product()
        {
            ProductCostHistories = new HashSet<ProductCostHistory>();
            ProductMaterials = new HashSet<ProductMaterial>();
            ProductSales = new HashSet<ProductSale>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int? ProductTypeId { get; set; }
        public string ArticleNumber { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image
        {
            get => _image;
            set => _image = value;
        }
        public int? ProductionPersonCount { get; set; }
        public int? ProductionWorkshopNumber { get; set; }
        public decimal MinCostForAgent
        {
            get => _minCostForAgent;
            set => _minCostForAgent = value;
        }

        [NotMapped]
        public string Name
        {
            get
            {
                string productTypeTitle = (ProductType == null) ? "Тип" : ProductType.Title;
                return $"{productTypeTitle} | {Title}";
            }
        }

        [NotMapped]
        public string ImagePath
        {
            get
            {
                if ((_image == null) || (_image == string.Empty))
                    return "..\\Resources\\picture.png";
                else
                    return $"..\\Resources{_image}";
            }
        }

        [NotMapped]
        public string Materials
        {
            get
            {
                if (ProductMaterials.Count == 0)
                    return "Нет материалов";

                StringBuilder materials = new StringBuilder();
                materials.Append("Материалы: ");
                foreach (var pm in ProductMaterials)
                    materials.Append($"{pm.Material.Title}, ");
                materials.Remove(materials.Length - 2, 2);

                return materials.ToString();
            }
        }

        [NotMapped]
        public decimal Cost
        {
            get
            {
                if (ProductMaterials.Count == 0)
                    return _minCostForAgent;

                decimal cost = 0;
                foreach (var pm in ProductMaterials)
                    cost += Math.Ceiling((decimal)pm.Count) * pm.Material.Cost;

                return cost;
            }
        }

        public virtual ProductType? ProductType { get; set; }
        public virtual ICollection<ProductCostHistory> ProductCostHistories { get; set; }
        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
    }
}
