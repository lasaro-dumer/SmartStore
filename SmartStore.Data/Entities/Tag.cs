using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartStore.Data.Entities
{
    public class Tag
    {
        public Tag()
            => Products = new JoinCollectionFacade<Product, Tag, ProductTag>(this, ProductTags);

        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        private ICollection<ProductTag> ProductTags { get; } = new List<ProductTag>();
        [NotMapped]
        public IEnumerable<Product> Products { get; }
    }
}
