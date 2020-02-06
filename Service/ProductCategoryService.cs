﻿using AutoMapper;
using DAL.Interfaces;
using Entity;
using Service.Domains;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class ProductCategoryService : IProductCategoryService
    {
        private IProductCategoryRepository productCategoryRepository { get; set; }
        private IMapper mapper { get; set; }
        private IUnitOfWork unitOfWork;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.productCategoryRepository = productCategoryRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public bool Add(ProductCategoryDomain productCategoryDomain)
        {
            productCategoryRepository.Add(mapper.Map<ProductCategory>(productCategoryDomain));
            unitOfWork.Commit();
            return true;
        }

        public bool Delete(ProductCategoryDomain productCategoryDomain)
        {
            return productCategoryRepository.Delete(mapper.Map<ProductCategory>(productCategoryDomain));
        }

        public ProductCategoryDomain GetById(int id)
        {
            return mapper.Map<ProductCategoryDomain>(productCategoryRepository.GetByIdEagerly(id));
        }

        public IEnumerable<ProductCategoryDomain> GetAll()
        {
            return mapper.Map<IEnumerable<ProductCategoryDomain>>(productCategoryRepository.GetAll());
        }

        public void Update(ProductCategoryDomain productCategoryDomain)
        {
            throw new NotImplementedException();
        }
    }
}
