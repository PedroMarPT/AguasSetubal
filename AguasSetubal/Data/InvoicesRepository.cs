using AguasSetubal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AguasSetubal.Data
{
    public class InvoicesRepository : GenericRepository<Fatura>, IInvoicesRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoicesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Fatura GetLastInvoice()
        {
            return _context.Faturas
                .Include(f => f.Cliente)
                .OrderByDescending(f => f.Id)
                .FirstOrDefault();


        }

    }
}
