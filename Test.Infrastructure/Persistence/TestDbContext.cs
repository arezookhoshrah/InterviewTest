
using Microsoft.EntityFrameworkCore;
using Test.Domain.Entity;

namespace Test.Infrastructure.Persistence
{
    public class TestDbContext:DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options):base(options)
        {
            
        }

        public DbSet<MemberInfo> MemberInfos { get; set; }
    }
}
