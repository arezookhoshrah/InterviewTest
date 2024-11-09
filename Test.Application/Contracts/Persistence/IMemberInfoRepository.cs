
using Test.Domain.Entity;

namespace Test.Application.Contracts.Persistence
{
    public interface IMemberInfoRepository:IAsyncRepository<MemberInfo>
    {
        Task<IEnumerable<MemberInfo>> GetMembersByNationalCode(string nationalCode);
    }
}
