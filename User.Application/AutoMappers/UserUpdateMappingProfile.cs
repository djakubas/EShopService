using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using User.Application.DTO;
using User.Domain.Models;

namespace User.Application.AutoMappers
{
    public class UserUpdateMappingProfile : Profile
    {
        public UserUpdateMappingProfile()
        {
            CreateMap<UserUpdateDto, UserModel>().ForAllMembers(options => 
                options.Condition((source,destination,sourceMember) => 
                    sourceMember != null));
        }
    }
}
