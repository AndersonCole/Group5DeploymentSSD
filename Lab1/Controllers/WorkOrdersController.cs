using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1.Data;
using Lab1.Models;
using Microsoft.AspNetCore.Authorization;

namespace Lab1.Controllers
{
    [Authorize(Roles = "Manager,Employee")]
    public class WorkOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkOrders
        public async Task<IActionResult> Index()
        {
              return _context.WorkOrders != null ? 
                          View(await _context.WorkOrders.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.WorkOrders'  is null.");
        }

        // GET: WorkOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WorkOrders == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workOrder == null)
            {
                return NotFound();
            }

            return View(workOrder);
        }

        // GET: WorkOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeName,Customer,Description,Date")] WorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workOrder);
        }

        [Authorize(Roles = "Manager")]
        // GET: WorkOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkOrders == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return NotFound();
            }
            return View(workOrder);
        }

        // POST: WorkOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeName,Customer,Description,Date")] WorkOrder workOrder)
        {
            if (id != workOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkOrderExists(workOrder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workOrder);
        }

        // GET: WorkOrders/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WorkOrders == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workOrder == null)
            {
                return NotFound();
            }

            return View(workOrder);
        }

        // POST: WorkOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WorkOrders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.WorkOrders'  is null.");
            }
            var workOrder = await _context.WorkOrders.FindAsync(id);
            if (workOrder != null)
            {
                _context.WorkOrders.Remove(workOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkOrderExists(int id)
        {
          return (_context.WorkOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
