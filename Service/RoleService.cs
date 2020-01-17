﻿using AutoMapper;
using DAL.Repositories;
using Entity;
using Service.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class RoleService
    {
        public RoleRepository roleRepository { get; set; }
        public IMapper mapper { get; set; }
        public RoleService(RoleRepository roleRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.roleRepository = roleRepository;
        }

        public IEnumerable<RoleDomain> GetAllRoles()
        {
            IEnumerable<Role> roles = roleRepository.GetAll();

            return mapper.Map<IEnumerable<Role>, IEnumerable<RoleDomain>>(roles);
        }
    }
}
