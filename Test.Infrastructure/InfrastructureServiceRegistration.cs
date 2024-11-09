using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Application.Contracts.Persistence;
using Test.Domain.Entity;
using Test.Infrastructure.Persistence;
using Test.Infrastructure.Repositories;

namespace Test.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TestConnectionString");
            services.AddDbContext<TestDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IMemberInfoRepository, MemberInfoRepository>();

            
            return services;
        }


    }
}
