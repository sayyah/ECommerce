﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModel
{
   public class WishListViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string ImagePath { get; set; }
        public string Alt { get; set; }
        public int Price { get; set; }
        public double Exist { get; set; }
        public string StoreStatus { get; set; }

    }
}