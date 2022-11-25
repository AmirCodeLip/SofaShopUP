using AutoMapper;
using DataLayer.Domin.Models;
using DataLayer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccessToRole, WebAccessToRole>();
        }
    }
}
