using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.DataAccess.Repository;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using BookWeb.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork context, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> ProductList = _unitOfWork.Product.GetAll(includeProperty:"Category").ToList();
            return View(ProductList);
       
        }

        [HttpGet]
        public IActionResult UpSert(int? id)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(
              c => new SelectListItem
              {
                  Text = c.Name,
                  Value = c.Id.ToString()
              }
                );
          
            ProductVM productVM = new()
            {
                CategoryList = categoryList,
                Product = new Product()
            };
            if(id == null || id == 0)
            {
                // Create
                return View(productVM);
            }
            else
            {
                // Update
                productVM.Product = _unitOfWork.Product.Get(p => p.Id == id);
                return View(productVM);
            }




        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ProductVM productVM, IFormFile? file)
        {
            
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(file != null)
                {
                    string fileName =  Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(webRootPath, @"images\products");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImgPath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                            if(System.IO.File.Exists(oldImgPath))
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                    
                    }
                    using ( var filestream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    productVM.Product.ImageUrl = @"\images\products\" + fileName;
                } 


            if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
         

               
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");

            
            //else
            //{
            //    productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });
            //    return View(productVM);
            //}
            //if (ModelState.IsValid)
            //{

            // }
            //return View();

        }


        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    Product product = _unitOfWork.Product.Get(p=>p.Id == id);
        //    return View(product);   

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product updated Successfully";
        //        return RedirectToAction("Index");

        //    }
        //    return View();

        //}
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id == null || _unitOfWork == null)
            {
                return NotFound();
            }

             Product product = _unitOfWork.Product.Get(p=>p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
           

          
        }

        [HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || _unitOfWork == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = _unitOfWork.Product.Get(p => p.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirm(int? id)
        //{
        //    if (id == null || _unitOfWork == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = _unitOfWork.Product.Get(p => p.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }


        //    _unitOfWork.Product.Remove(product);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product Deleted Successfully";
        //    return RedirectToAction("Index");


        //}

        // API 
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> ProductList = _unitOfWork.Product.GetAll(includeProperty: "Category").ToList();
            return Json(new { data = ProductList} );

        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productsFromDB = _unitOfWork.Product.Get(p=>p.Id == id);

            if(productsFromDB == null)
            {
                return Json(new { success = false , message = "Failed"});
            }
             var oldImgPath = Path.Combine(_webHostEnvironment.WebRootPath, productsFromDB.ImageUrl.TrimStart('\\'));

              if(System.IO.File.Exists(oldImgPath))
                 {
                    System.IO.File.Delete(oldImgPath);
                 }

            _unitOfWork.Product.Remove(productsFromDB);
            _unitOfWork.Save();

            return Json(new { success = true, message = "success" });
        }




    }
}
