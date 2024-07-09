using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using BookWeb.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork context)
        {
            _unitOfWork = context;
        }

        public IActionResult Index()
        {
            List<Category> CategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(CategoryList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category category = _unitOfWork.Category.Get(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null || _unitOfWork == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category.Get(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || _unitOfWork == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category.Get(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int? id)
        {
            if (id == null || _unitOfWork == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category.Get(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }


            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");


        }


        //public async Task<IActionResult> Index()
        //{
        //      return View(await _context.Category.ToListAsync());
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,DisplayOrder")] Category category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(category);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(category);
        //}


        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Category == null)
        //    {
        //        return NotFound();
        //    }

        //    var category = await _context.Category
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Category == null)
        //    {
        //        return NotFound();
        //    }

        //    var category = await _context.Category.FindAsync(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(category);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DisplayOrder")] Category category)
        //{
        //    if (id != category.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(category);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CategoryExists(category.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(category);
        //}


        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Category == null)
        //    {
        //        return NotFound();
        //    }

        //    var category = await _context.Category
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Category == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Category'  is null.");
        //    }
        //    var category = await _context.Category.FindAsync(id);
        //    if (category != null)
        //    {
        //        _context.Category.Remove(category);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CategoryExists(int id)
        //{
        //  return _context.Category.Any(e => e.Id == id);
        //}
    }
}
