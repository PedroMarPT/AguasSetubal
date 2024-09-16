using AguasSetubal.Models;

namespace AguasSetubal.Data
{
    public class InvoicesRepository : GenericRepository<Fatura>, IInvoicesRepository
    {

        public InvoicesRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}