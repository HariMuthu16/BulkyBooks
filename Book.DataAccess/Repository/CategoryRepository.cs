﻿using Book.DataAccess.Repository.IRepository;
using Book.Models;
using BookWeb.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
          
        }

        public void Update(Category obj)
        {
           _db.Category.Update(obj);
        }


    }
}
