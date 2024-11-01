using Microsoft.EntityFrameworkCore;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Repository;
using ProjectTest.Infrastructure.Data.Context;
using ProjectTest.Infrastructure.Data.Repositories.Common;
using System.Threading.Tasks;

namespace ProjectTest.Infrastructure.Data.Repositories
{
    public class ProdutoRepository : GenericAsyncRepository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ProjectTestContext _dbContext) : base(_dbContext)
        {
        }


        public async Task<Produto?> ObterProdutoPorNomeAsync(string nomeProduto)
        {
            return await _dbContext.Set<Produto>().Where(entity => !entity.IsDeleted)
                .FirstOrDefaultAsync(p => p.NomeProduto.ToLower() == nomeProduto.ToLower());
        }
    }
}
