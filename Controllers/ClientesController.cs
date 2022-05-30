using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project_dc_system.Models;
using project_dc_system.Controllers;

namespace project_dc_system.Controllers
{
    public class ClientesController : Controller
    {
        private readonly VendasContext _context = new VendasContext();

        // GET: Clientes
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "ClienteName_desc" : "";
            var clientes = from s in _context.Clientes
                           select s;

            switch(sortOrder)
            {
                case "ClienteName_desc":
                    clientes = clientes.OrderByDescending(s => s.ClienteName);
                    break;
                default:
                    clientes = clientes.OrderBy(s => s.ClienteName);
                    break;
            }

            var dbclientes = await clientes.AsNoTracking().ToListAsync(); // Retornando a lista de clientes a serem exibidos
            dbclientes.ForEach(s => s.CpfOrCnpj = FormataCpfCnpj(s.CpfOrCnpj)); // Formatando os cpf e cnpj para exibição
            return _context.Clientes != null ? View(dbclientes) :
                Problem("Entity set 'VendasContext.Clientes'  is null.");
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }
            cliente.CpfOrCnpj = FormataCpfCnpj(cliente.CpfOrCnpj); // Formatando cpf ou cnpj para exibição
            cliente.Telefones = _context.Telefones.Where(s => s.ClienteId == cliente.ClienteId).ToList(); // Retornando a lista de telefones do cliente
            cliente.Telefones.ForEach(t => t.Fone = TelefonesController.FormataTelefone(t.Fone)); // Formatando os telefones para exibição
            cliente.Emails = _context.Emails.Where(s => s.ClienteId == cliente.ClienteId).ToList(); // Retornando a lista de emails do cliente

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,ClienteName,CpfOrCnpj")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.CpfOrCnpj = String.Join("", System.Text.RegularExpressions.Regex.Split(cliente.CpfOrCnpj, @"[^\d]"));
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            cliente.CpfOrCnpj = FormataCpfCnpj(cliente.CpfOrCnpj);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,ClienteName,CpfOrCnpj")] Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cliente.CpfOrCnpj = String.Join("", System.Text.RegularExpressions.Regex.Split(cliente.CpfOrCnpj, @"[^\d]"));
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteId))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            cliente.CpfOrCnpj = FormataCpfCnpj(cliente.CpfOrCnpj);
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'VendasContext.Clientes'  is null.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return (_context.Clientes?.Any(e => e.ClienteId == id)).GetValueOrDefault();
        }

        // Formantando a exibição do CPF e CNPJ
        public static string FormataCpfCnpj(string valor)
        {
            if(valor.Length == 11)
            {
                var cpf = "" + valor[0] + valor[1] + valor[2] + "." + valor[3] + valor[4] + valor[5] + "." +
                    valor[6] + valor[7] + valor[8] + "-" + valor[9] + valor[10];
                return cpf;
            }
            if(valor.Length == 14)
            {
                var cnpj = "" + valor[0] + valor[1] + "." + valor[2]+ valor[3] + valor[4] + "." + valor[5]+
                    valor[6] + valor[7]+ "/" + valor[8] + valor[9] + valor[10] + valor[11] + "-" +
                    valor[12] + valor[13];
                return cnpj;
            }
            return valor;
        }
    }
}
