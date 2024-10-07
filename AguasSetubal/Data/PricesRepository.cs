using AguasSetubal.Models;

namespace AguasSetubal.Data
{
    public class PricesRepository : GenericRepository<TabelaPrecos>, IPricesRepository
    {
        public PricesRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
