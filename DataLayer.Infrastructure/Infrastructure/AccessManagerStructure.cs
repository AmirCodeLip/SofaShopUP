using AutoMapper;
using DataLayer.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Infrastructure.Models;
using DataLayer.Domin.Models.Web;
using DataLayer.Access.Services.Identity;

namespace DataLayer.Infrastructure.Infrastructure
{
    public class AccessManagerStructure
    {
        private readonly IMapper _mapper;
        private readonly IAccessToRoleRepository _accessToRoleRepository;

        public AccessManagerStructure(IAccessToRoleRepository accessToRoleRepository,
             IMapper mapper)
        {
            _accessToRoleRepository = accessToRoleRepository;
            _mapper = mapper;
        }

        public async Task<AccessToRole> GetEditModel(int? id)
        {
            WebAccessToRole webAccessToRole = id.HasValue ?
                await _accessToRoleRepository.FindAsync(id) : null;
            AccessToRole accessToRole = null;
            if (webAccessToRole == null)
            {
                accessToRole = new AccessToRole
                {
                    Name = "دسترسی جدید"
                };
            }
            else
            {
                accessToRole = _mapper.Map<WebAccessToRole, AccessToRole>(webAccessToRole);
            }
            return accessToRole;
        }

        public List<AccessToRole> GetAllAccessToRoles()
        {
            List<AccessToRole> result =
                _mapper.Map<List<WebAccessToRole>, List<AccessToRole>>(_accessToRoleRepository.GetList());
            return result;
        }

    }
}
