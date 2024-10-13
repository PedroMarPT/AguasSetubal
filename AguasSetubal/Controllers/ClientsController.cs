using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AguasSetubal.Data;
using AguasSetubal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasSetubal.Helpers;
using Syncfusion.XPS;

namespace AguasSetubal.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IUserHelper _userHelper;

        public ClientsController(IClientsRepository clientsRepository, IUserHelper userHelper)
        {
            this._clientsRepository = clientsRepository;
            _userHelper = userHelper;
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        // GET: Clientes
        public IActionResult Index()
        {
            return View(_clientsRepository.GetAll());
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
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

        [Authorize]
        [Authorize(Roles = "Admin")]
        // GET: Clientes/Create
        public async Task<IActionResult> CreateAsync()
        {
            var usersList = await _userHelper.GetUsersWithRoleClienteAsync();

            var drpList = new SelectList(String.Empty, String.Empty);
            drpList.Union(new SelectList(usersList, "Id", "UserName"));

            ViewBag.Utilizadores = drpList;

            return View();
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
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

        [Authorize]
        [Authorize(Roles = "Admin")]
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

        [Authorize]
        [Authorize(Roles = "Admin")]
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

        [Authorize]
        [Authorize(Roles = "Admin")]
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

        [Authorize]
        [Authorize(Roles = "Admin")]
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

