using AutoMapper;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Test.Application.Contracts.Persistence;
using Test.Domain.Entity;
using Test.Grpc.Protos;

namespace Test.Grpc.Services
{
    public class TestService : TestProtoService.TestProtoServiceBase
    {
        private readonly IMemberInfoRepository _memberInfoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TestService(IMemberInfoRepository memberInfoRepository, IMapper mapper, ILogger logger)
        {
            _memberInfoRepository = memberInfoRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async override Task<GetAllResponse> GetAll(Empty request, ServerCallContext context)
        {
            var membersInfo = await _memberInfoRepository.GetAllAsync();
            GetAllResponse response = new GetAllResponse();
            foreach (var item in membersInfo)
            {
                response.Items.Add(_mapper.Map<MemberInfo, MemberModel>(item));
            }

            return response;
        }

        private Expression<Func<MemberInfo, bool>> TestCondition(string condition)
        {
            var expr = DynamicExpressionParser.ParseLambda<MemberInfo, bool>(ParsingConfig.Default, false, condition, new object[0]);
           
            return expr;
        }
        public async override Task<GetAllResponse> Get(GetRequest request, ServerCallContext context)
        {

            var result = await _memberInfoRepository.GetAsync(TestCondition(request.Predicate));

            GetAllResponse response = new GetAllResponse();
            foreach (var item in result)
            {
                response = _mapper.Map<MemberInfo, GetAllResponse>(item);
            }

            return response;

        }

        public override async Task<MemberModel> GetById(GetByIdRequest request, ServerCallContext context)
        {
            var result = await _memberInfoRepository.GetByIdAsync(request.Id);
            return _mapper.Map<MemberInfo, MemberModel>(result);
        }

        public async override Task<MemberModel> Create(CreateRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Member.Name))
            {
                _logger.LogError($"Name is required");
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "Name is required"));
            }
            if (string.IsNullOrWhiteSpace(request.Member.Family))
            {
                _logger.LogError($"Family is required");
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "Family is required"));
            }
            if (string.IsNullOrWhiteSpace(request.Member.NationalCode))
            {
                _logger.LogError($"NationalCode is required");
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "NationalCode is required"));
            }
            if (string.IsNullOrWhiteSpace(request.Member.BirthDate))
            {
                _logger.LogError($"BirthDate is required");
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "BirthDate is required"));
            }

            var member = _mapper.Map<MemberModel, MemberInfo>(request.Member);
            var result = await _memberInfoRepository.AddAsync(member);
            return _mapper.Map<MemberInfo, MemberModel>(result);

        }

        public async override Task<DeleteOrUpdateResponse> Update(UpdateRequest request, ServerCallContext context)
        {


            if (request.Member.Id == 0 || request.Member.Id == null)
            {
                _logger.LogError($"Id is required  ");
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "Id is required"));

            }

            if (string.IsNullOrWhiteSpace(request.Member.Name))
            {
                _logger.LogError($"Name is required ");
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "Name is required"));
            }
            if (string.IsNullOrWhiteSpace(request.Member.Family))
            {
                _logger.LogError($"Family is required");
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "Family is required"));
            }
            if (string.IsNullOrWhiteSpace(request.Member.NationalCode))
            {
                _logger.LogError($"NationalCode is required");
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "NationalCode is required"));
            }
            if (string.IsNullOrWhiteSpace(request.Member.BirthDate))
            {
                _logger.LogError($"BirthDate is required ");
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "BirthDate is required"));

            }

            var member = _mapper.Map<MemberModel, MemberInfo>(request.Member);
            await _memberInfoRepository.UpdateAsync(member);
            return new DeleteOrUpdateResponse { Success = true };

        }

        public async override Task<DeleteOrUpdateResponse> Delete(DeleteRequest request, ServerCallContext context)
        {
            var member = _mapper.Map<MemberModel, MemberInfo>(request.Member);
            await _memberInfoRepository.DeleteAsync(member);
            return new DeleteOrUpdateResponse { Success = true };
        }

        public async override Task<DeleteOrUpdateResponse> DeleteById(GetByIdRequest request, ServerCallContext context)
        {
            var member = await _memberInfoRepository.GetByIdAsync(request.Id, true);
            if (member != null)
            {
                await _memberInfoRepository.DeleteAsync(member);
                return new DeleteOrUpdateResponse { Success = true };
            }
            _logger.LogInformation($"Member Not Found");
            return new DeleteOrUpdateResponse { Success = false };
            //throw new RpcException(new Status(statusCode: StatusCode.NotFound, "Member Not Found"));

        }


    }
}
