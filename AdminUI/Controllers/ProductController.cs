﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminUI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.Domains;
using Service.Interfaces;

namespace AdminUI.Controllers
{
    public class ProductController : Controller
    {
        IMapper mapper;
        IProductService productService;
        IProductCategoryService productCategoryService;
        public ProductController(IMapper mapper, IProductService productService, IProductCategoryService productCategoryService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.productCategoryService = productCategoryService;
        }
        public IActionResult Index()
        {
            List<ProductCategoryViewModel> productCategoryViewModels = mapper.Map<List<ProductCategoryViewModel>>(productCategoryService.ListCategories());
            return View(productCategoryViewModels);
        }

        public IActionResult ListProducts(int id)
        {
            IEnumerable<ProductViewModel> productViewModels = mapper.Map<IEnumerable<ProductViewModel>>(productCategoryService.GetCategory(id).Products);
            return View(productViewModels);
        }

        public IActionResult CreateProduct(int id)
        {
            ViewData["categoryId"] = id;
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductViewModel productViewModel)
        {
            productViewModel.Category = mapper.Map<ProductCategoryViewModel>(productCategoryService.GetCategory(productViewModel.CategoryId));
            productService.AddProduct(mapper.Map<ProductDomain>(productViewModel));
            return RedirectToAction("ListProducts", new { id = productViewModel.Category.Id });
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(ProductCategoryViewModel productCategoryViewModel)
        {
            productCategoryService.AddCategory(mapper.Map<ProductCategoryDomain>(productCategoryViewModel));
            return RedirectToAction("Index");
        }
    }
}