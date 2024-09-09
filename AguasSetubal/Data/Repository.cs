using AguasSetubal.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json.Linq;
using Syncfusion.EJ2.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AguasSetubal.Data
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Cliente> GetClients()
        {
            return _context.Clientes.OrderBy(c => c.Nome);
        }

        public Cliente GetClient(int id)
        {
            return _context.Clientes.Find(id);
        }

        public void AddClient(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
        }

        public void UpdateClient(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
        }

        public void RemoveClient(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
        }

        /// <summary>
        /// Método que grava tudo o que está pendente para gravar na base de dados
        /// </summary>
        /// <returns>booleano true or false consonate a gravaçção correu bem ou mal</returns>
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool ClientExists(int id)
        {
            return _context.Clientes.Any(c => c.Id == id);
        }
    }
}
