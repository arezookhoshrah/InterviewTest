using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Test.Api.Interceptors;
using Test.Grpc.Protos;

namespace Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly GrpcChannel _channel;
        private readonly TestProtoService.TestProtoServiceClient _client;
        private readonly IConfiguration _configuration;
        private ILogger _logger;

        public MembersController(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcSettings:MemberServiceUrl"));
            _logger = loggerFactory.CreateLogger<MembersController>();

            //  var invoker = _channel.Intercept(new ClientLoggingInterceptor());
            _client = new TestProtoService.TestProtoServiceClient(_channel);
        }

        #region Get All Members
        [HttpGet("GetAll")]
        public async Task<ActionResult<GetAllResponse>> GetAll()
        {
            try
            {
                GetAllResponse respose = new GetAllResponse();
                respose = await _client.GetAllAsync(new Empty { });
                return Ok(respose);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Get
        [HttpGet("Get")]
        public async Task<ActionResult<GetAllResponse>> Get(string predicate)
        {

            var respose = await _client.GetAsync(new GetRequest { Predicate = predicate });
            return Ok(respose);
        }
        #endregion

        #region Get By Id
        [HttpGet("GetById")]
        public async Task<ActionResult<MemberModel>> GetById(int id)
        {
            var respose = await _client.GetByIdAsync(new GetByIdRequest { Id = id });
            return Ok(respose);
        }
        #endregion

        #region Create Member
        [HttpPost]
        public async Task<ActionResult<MemberModel>> CreateAsync(MemberModel model)
        {
            try
            {

                var member = new MemberModel
                {
                    Name = model.Name,
                    Family = model.Family,
                    NationalCode = model.NationalCode,
                    BirthDate = model.BirthDate
                };

                var respose = await _client.CreateAsync(new CreateRequest { Member = member });
                return Ok(respose);
            }
            catch (RpcException ex)
            {
                _logger.LogError($"Status Code: {ex.Status.StatusCode}  Detail : {ex.Status.Detail}");
                throw;
            }

        }
        #endregion

        #region Update Member
        [HttpPut]
        public async Task<ActionResult<DeleteOrUpdateResponse>> UpdateAsync(UpdateRequest model)
        {
            try
            {
                var respose = await _client.UpdateAsync(model);
                return Ok(respose);
            }
            catch (RpcException ex)
            {
                _logger.LogError($"Status Code: {ex.Status.StatusCode}  Detail : {ex.Status.Detail}");
                throw;
            }
        }
        #endregion

        #region Delete Member
        [HttpDelete]
        public async Task<ActionResult<DeleteOrUpdateResponse>> DeleteAsync(DeleteRequest model)
        {
            var respose = await _client.DeleteAsync(model);
            return Ok(respose);
        }
        #endregion
        #region Delete Member By Id
        [HttpDelete("DeleteById")]
        public async Task<ActionResult<DeleteOrUpdateResponse>> DeleteByIdAsync(GetByIdRequest model)
        {
            var respose = await _client.DeleteByIdAsync(model);
            return Ok(respose);
        }
        #endregion
    }
}
