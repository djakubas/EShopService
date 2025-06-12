using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Application.Dto;
using EShop.Domain.Models;

namespace EShop.Application.AutoMappers
{
    public class ProducMappingProfile : Profile
    {
        public ProducMappingProfile()
        {
            CreateMap<ProductDto, Product>();
        }
    }
}
