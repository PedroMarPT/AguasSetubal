using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AguasSetubal.Data;
using AguasSetubal.Models;

namespace AguasSetubal.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsController(IClientsRepository clientsRepository)
        {
            this._clientsRepository = clientsRepository;
        }

        // GET: Clientes
        public IActionResult Index()
        {
            return View(_clientsRepository.GetAll());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _clientsRepository.GetByIdAsync(id.Value);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _clientsRepository.CreateAsync(cliente);

                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _clientsRepository.GetByIdAsync(id.Value);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _clientsRepository.UpdateAsync(cliente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _clientsRepository.ExistsAsync(cliente.Id))
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
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _clientsRepository.GetByIdAsync(id.Value);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _clientsRepository.GetByIdAsync(id);
            await _clientsRepository.DeleteAsync(cliente);

            return RedirectToAction(nameof(Index));
        }

    }
}




