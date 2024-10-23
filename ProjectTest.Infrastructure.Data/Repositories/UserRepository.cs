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
    public class UserRepository : GenericAsyncRepository<Usuario>, IUserRepository
    {
        public UserRepository(ProjectTestContext _dbContext) : base(_dbContext)
        {
        }

        public async Task<Usuario> FindUser(string email)
        {
            try
            {
                var user = await DbSet.Where(x => !x.IsDeleted && x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
