
using Microsoft.EntityFrameworkCore;
using Test.Application.Contracts.Persistence;
using Test.Domain.Entity;
using Test.Infrastructure.Persistence;

namespace Test.Infrastructure.Repositories
{
    public class MemberInfoRepository : RepositoryBase<MemberInfo>, IMemberInfoRepository
    {
        public MemberInfoRepository(TestDbContext dbContext): base(dbContext)
        {
            
        }
        public async Task<IEnumerable<MemberInfo>> GetMembersByNationalCode(string nationalCode)
        {
            return await _testDbContext.MemberInfos.Where(x => x.NationalCode == nationalCode).ToListAsync();
        }
    }
}
