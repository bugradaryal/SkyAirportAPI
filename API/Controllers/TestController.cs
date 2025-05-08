using Business.Abstract;
using Business.Concrete;
using DTO;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public readonly ILoggerServices _loggerServices;
        public TestController(IMapper mapper)
        {
            _loggerServices = new LoggerManager(mapper);
        }
        [HttpPost("Test")]
        public IActionResult Index() 
        {
            _loggerServices.Logger(new LogDTO
            {
                Message = "Kullanıcı giriş denemesi başarısız oldu. / Invalid password for user 'admin'",
                Action_type = Action_Type.Login,
                Target_table = "Users",
                loglevel_id = 4,
                AdditionalData = new List<string>
                {
                    "IP: 192.168.1.10",
                    "Browser: Chrome",
                    "AttemptCount: 3"
                },
                user_id = "5d3f2a8e-bb33-4f2a-9c1a-913b96cb6543"
            },null);

            return Ok();
        }
    }
}
