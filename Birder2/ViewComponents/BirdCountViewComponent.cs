﻿using Birder2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder2.ViewComponents
{

    public class BirdCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public BirdCountViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bir = await _context.Birds.CountAsync(); // ToListAsync();
            //var items = await GetItemsAsync(maxPriority, isDone);
            return View("Default", bir.ToString());
        }

        //private Task<List<TodoItem>> GetItemsAsync(int maxPriority, bool isDone)
        //{
        //    return db.ToDo.Where(x => x.IsDone == isDone &&
        //                         x.Priority <= maxPriority).ToListAsync();
        //}
    }
}