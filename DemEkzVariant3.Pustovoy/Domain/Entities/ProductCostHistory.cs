﻿using System;
using System.Collections.Generic;

namespace DemEkzVariant3.Pustovoy.Domain.Entities
{
    public partial class ProductCostHistory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime ChangeDate { get; set; }
        public decimal CostValue { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}