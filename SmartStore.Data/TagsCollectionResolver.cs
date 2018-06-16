using System.Collections.Generic;
using AutoMapper;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;

namespace SmartStore.Data
{
    public class TagsCollectionResolver : IValueResolver<ProductModel, Product, IEnumerable<Tag>>
    {
        public TagsCollectionResolver()
        {

        }

        public IEnumerable<Tag> Resolve(ProductModel source, 
                                        Product destination, 
                                        IEnumerable<Tag> destMember, 
                                        ResolutionContext context)
        {
            foreach (var tag in source.Tags)
            {
                destination.AddTag(tag);
            }
            return destination.Tags;
        }
    }
}
