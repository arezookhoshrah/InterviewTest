using Microsoft.EntityFrameworkCore;
using Test.Domain.Entity;

namespace Test.Infrastructure.Persistence
{
    public class DbInitializer
    {
        private readonly TestDbContext _dbContext;

        public DbInitializer(TestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Run()
        {
            if (!_dbContext.MemberInfos.Any())
            {
                _dbContext.Database.Migrate();
                await _dbContext.MemberInfos.AddRangeAsync(GetPreconfiguredMembers());
                await _dbContext.SaveChangesAsync();
            }
        }

        private IEnumerable<MemberInfo> GetPreconfiguredMembers()
        {
            return new List<MemberInfo>
            {
                new MemberInfo
                {
                    //Id = 1,
                    Name = "Some Member Name",
                    Family = "Some Member Family",
                    NationalCode = "4322240348",
                    BirthDate = new DateTime(1985,1,1)
                }
            };
        }
    }
}
