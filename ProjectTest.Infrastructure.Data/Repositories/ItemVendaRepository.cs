using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Repository;
using ProjectTest.Infrastructure.Data.Context;
using ProjectTest.Infrastructure.Data.Repositories.Common;

namespace ProjectTest.Infrastructure.Data.Repositories
{
    public class ItemVendaRepository : GenericAsyncRepository<ItemVenda>, IItemVendaRepository
    {
        public ItemVendaRepository(ProjectTestContext _dbContext) : base(_dbContext)
        {
        }

    }
}
