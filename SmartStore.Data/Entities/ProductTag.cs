using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStore.Data.Entities
{
    public class ProductTag: IJoinEntity<Product>, IJoinEntity<Tag>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        Product IJoinEntity<Product>.Navigation
        {
            get => Product;
            set => Product = value;
        }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        Tag IJoinEntity<Tag>.Navigation
        {
            get => Tag;
            set => Tag = value;
        }
    }
}
