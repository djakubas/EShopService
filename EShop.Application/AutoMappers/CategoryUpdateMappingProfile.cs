using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Application.Dto;
using EShop.Application.DTO;
using EShop.Domain.Models.Products;

namespace EShop.Application.AutoMappers
{
    public class CategoryUpdateMappingProfile : Profile
    {
        public CategoryUpdateMappingProfile()
        {
            CreateMap<CategoryDto, Category>();
        }
    }
}
