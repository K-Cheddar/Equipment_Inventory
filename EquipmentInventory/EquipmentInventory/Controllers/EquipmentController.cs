using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EquipmentInventory.Data;
using EquipmentInventory.Models;

namespace EquipmentInventory.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly InventoryContext _context;

        public EquipmentController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Equipment.ToListAsync());
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .SingleOrDefaultAsync(m => m.ID == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Quantity,Status,Description,Link,Location,Category")] Equipment equipment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(equipment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                //Log error
                ModelState.AddModelError("", "Unable to save changes. " +
                    " Try again, and see if the problem persists " +
                    "see your system administrator.");
            }

            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.SingleOrDefaultAsync(m => m.ID == id);
            if (equipment == null)
            {
                return NotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentToUpdate = await _context.Equipment.SingleOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Equipment>(
                equipmentToUpdate, "",
                s => s.Quantity, s => s.Description, s => s.Name,
                s => s.Location, s => s.Status, s => s.Link,
                s => s.Category
                ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    //Log error
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
                }
            }

           
            return View(equipmentToUpdate);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (equipment == null)
            {
                return NotFound();
            }
            
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipment
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (equipment == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                _context.Equipment.Remove(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                //Log the error
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.ID == id);
        }
    }
}
