using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project_dc_system.Models;

namespace project_dc_system.Controllers
{
    public class VendasController : Controller
    {
        private readonly VendasContext _context = new VendasContext();

        // GET: Vendas
        public async Task<IActionResult> Index(string sortOrder)
        {
            var vendasContext = _context.Vendas.Include(t => t.Cliente);
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Date_desc" : "";
            ViewData["ValueSortParm"] = sortOrder == "Value" ? "Value_desc" : "Value";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            var vendas = from s in vendasContext
                         select s;

            switch (sortOrder)
            {
                case "Date_desc":
                    vendas = vendas.OrderByDescending(s => s.DataVenda);
                    break;
                case "Value":
                    vendas = vendas.OrderBy(s => s.ValorVenda);
                    break;
                case "Value_desc":
                    vendas = vendas.OrderByDescending(s => s.ValorVenda);
                    break;
                case "Name":
                    vendas = vendas.OrderBy(s => s.Cliente.ClienteName);
                    break;
                case "Name_desc":
                    vendas = vendas.OrderByDescending(s => s.Cliente.ClienteName);
                    break;
                default:
                    vendas = vendas.OrderBy(s => s.DataVenda);
                    break;
            }

            return View(await vendas.AsNoTracking().ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vendas == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.VendaId == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "ClienteName");
            return View();
        }

        // POST: Vendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendaId,DataVenda,ValorVenda,ClienteId")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                venda.DataVenda = venda.DataVenda.ToUniversalTime();
                _context.Add(venda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "ClienteName", venda.ClienteId);
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vendas == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "ClienteName", venda.ClienteId);
            return View(venda);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendaId,DataVenda,ValorVenda,ClienteId")] Venda venda)
        {
            if (id != venda.VendaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.VendaId))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "ClienteName", venda.ClienteId);
            return View(venda);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vendas == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.VendaId == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vendas == null)
            {
                return Problem("Entity set 'VendasContext.Vendas'  is null.");
            }
            var venda = await _context.Vendas.FindAsync(id);
            if (venda != null)
            {
                _context.Vendas.Remove(venda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return (_context.Vendas?.Any(e => e.VendaId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Relatorio(string nameString, string dateString, string valueString, string sortOrder)
        {
            ViewData["NameFilter"] = nameString;
            ViewData["DateFilter"] = dateString;
            ViewData["ValueFilter"] = valueString;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["ValueSortParm"] = sortOrder == "value" ? "value_desc" : "value";
            ViewData["NamSortParm"] = sortOrder == "name" ? "name_desc" : "name";

            var vendas = from v in _context.Vendas
                         select v;

            switch (sortOrder)
            {
                case "date_desc":
                    vendas = vendas.OrderByDescending(s => s.DataVenda);
                    break;
                case "value":
                    vendas = vendas.OrderBy(s => s.ValorVenda);
                    break;
                case "value_desc":
                    vendas = vendas.OrderByDescending(s => s.ValorVenda);
                    break;
                case "name":
                    vendas = vendas.OrderBy(s => s.Cliente.ClienteName);
                    break;
                case "name_desc":
                    vendas = vendas.OrderByDescending(s => s.Cliente.ClienteName);
                    break;
                default:
                    vendas = vendas.OrderBy(s => s.DataVenda);
                    break;
            }

            if (!String.IsNullOrEmpty(nameString))
            {
                vendas = _context.Vendas.Include(v => v.Cliente).Where(s => s.Cliente.ClienteName.Contains(nameString));
            }
            if (!String.IsNullOrEmpty(dateString))
            {
                vendas = vendas.Where(s => s.DataVenda.ToLocalTime().CompareTo(Convert.ToDateTime(dateString).Date) == 0);
            }
            if (!String.IsNullOrEmpty(valueString))
            {
                vendas = vendas.Where(s => s.ValorVenda == Decimal.Parse(valueString));
            }

            vendas = vendas.Include(v => v.Cliente);
            return View(await vendas.ToListAsync());
        }
    }
}
