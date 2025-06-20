using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Application.Dto;
using EShop.Domain.Models.Products;

namespace EShop.Application.AutoMappers
{
    public class ProductUpdateMappingProfile : Profile
    {
        public ProductUpdateMappingProfile()
        {
            CreateMap<ProductDto, Product>().ForAllMembers(options =>
                options.Condition((source, destination, sourceMember) =>
                    sourceMember != null));
        }
    }
}
