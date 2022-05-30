using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project_dc_system.Models;

namespace project_dc_system.Controllers
{
    public class TelefonesController : Controller
    {
        private readonly VendasContext _context = new VendasContext();

        // GET: Telefones
        public async Task<IActionResult> Index()
        {
            var vendasContext = _context.Telefones.Include(t => t.Cliente);
            var telefones = await vendasContext.ToListAsync();
            telefones.ForEach(t => t.Fone = FormataTelefone(t.Fone));
            return View(telefones);
        }

        // GET: Telefones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Telefones == null)
            {
                return NotFound();
            }

            var telefone = await _context.Telefones
                .Include(t => t.Cliente)
                .FirstOrDefaultAsync(m => m.TelefoneId == id);
            if (telefone == null)
            {
                return NotFound();
            }
            telefone.Fone = FormataTelefone(telefone.Fone);
            return View(telefone);
        }

        // GET: Telefones/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "ClienteName");
            return View();
        }

        // POST: Telefones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TelefoneId,Fone,ClienteId")] Telefone telefone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(telefone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "ClienteName", telefone.ClienteId);
            return View(telefone);
        }

        // GET: Telefones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Telefones == null)
            {
                return NotFound();
            }

            var telefone = await _context.Telefones.FindAsync(id);
            if (telefone == null)
            {
                return NotFound();
            }
            telefone.Fone = FormataTelefone(telefone.Fone);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "ClienteName", telefone.ClienteId);
            return View(telefone);
        }

        // POST: Telefones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TelefoneId,Fone,ClienteId")] Telefone telefone)
        {
            if (id != telefone.TelefoneId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telefone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelefoneExists(telefone.TelefoneId))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "ClienteName", telefone.ClienteId);
            return View(telefone);
        }

        // GET: Telefones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Telefones == null)
            {
                return NotFound();
            }

            var telefone = await _context.Telefones
                .Include(t => t.Cliente)
                .FirstOrDefaultAsync(m => m.TelefoneId == id);
            if (telefone == null)
            {
                return NotFound();
            }

            telefone.Fone = FormataTelefone(telefone.Fone);

            return View(telefone);
        }

        // POST: Telefones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Telefones == null)
            {
                return Problem("Entity set 'VendasContext.Telefones'  is null.");
            }
            var telefone = await _context.Telefones.FindAsync(id);
            if (telefone != null)
            {
                _context.Telefones.Remove(telefone);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelefoneExists(int id)
        {
          return (_context.Telefones?.Any(e => e.TelefoneId == id)).GetValueOrDefault();
        }

        // Formantando a exibição do Telefone
        public static string FormataTelefone(string valor)
        {
            if (valor.Length == 11)
            {
                var cpf = "(" + valor[0] + valor[1] + ") " + valor[2] + valor[3] + valor[4] + valor[5] +
                    valor[6] + "-" + valor[7] + valor[8] + valor[9] + valor[10];
                return cpf;
            }
            if (valor.Length == 10)
            {
                var cnpj = "(" + valor[0] + valor[1] + ") " + valor[2] + valor[3] + valor[4] + valor[5] +
                    "-" + valor[6] + valor[7] + valor[8] + valor[9];
                return cnpj;
            }
            return valor;
        }
    }
}
