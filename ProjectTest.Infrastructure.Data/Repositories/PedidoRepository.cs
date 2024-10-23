using Microsoft.EntityFrameworkCore;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Repository;
using ProjectTest.Infrastructure.Data.Context;
using ProjectTest.Infrastructure.Data.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Infrastructure.Data.Repositories
{
    public class PedidoRepository : GenericAsyncRepository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(ProjectTestContext _dbContext) : base(_dbContext)
        {
        }

    }
}
