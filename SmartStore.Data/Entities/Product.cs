using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartStore.Data.Entities
{
    public class Product
    {
        public Product()
           => Tags = new JoinCollectionFacade<Tag, Product, ProductTag>(this, ProductTags);

        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        public decimal SellingPrice { get; set; }
        public byte[] RowVersion { get; set; }
        private ICollection<ProductTag> ProductTags { get; } = new List<ProductTag>();
        [NotMapped]
        public IEnumerable<Tag> Tags { get; }

        public void AddTag(string tag)
        {
            if (!ProductTags.Any(p => p.Tag.Name == tag))
                ProductTags.Add(new ProductTag
                {
                    Product = this,
                    Tag = new Tag() { Name = tag }
                });
        }

        internal void FillTags(List<Tag> existingTags)
        {
            foreach (var prodTag in ProductTags)
            {
                var tag = existingTags.FirstOrDefault(t => t.Name.Equals(prodTag.Tag.Name));
                if (tag != null)
                    prodTag.Tag = tag;
            }
        }

        public void UpdateTags(string[] tags)
        {
            var tagsToRemove = ProductTags.Where(p => !tags.Any(t => t == p.Tag.Name)).ToList();
            foreach (var productTag in tagsToRemove)
            {
                ProductTags.Remove(productTag);
            }
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
        }
    }
}
