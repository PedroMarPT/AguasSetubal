using AguasSetubal.Models;

namespace AguasSetubal.Data
{
    public class ClientsRepository : GenericRepository<Cliente>, IClientsRepository
    {

        public ClientsRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
