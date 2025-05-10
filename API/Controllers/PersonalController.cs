using Business.Abstract;
using Business.Concrete;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Business.Features.Personal.Queries;
using DTO;
using DTO.Account;
using DTO.Personal;
using Entities;
using Entities.Configuration;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Utilitys.Mapper;

namespace API.Controllers
{
    [Authorize(Roles = "Administrator",Policy = "IsUserSuspended")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly ILoggerServices _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public PersonalController(IMediator mediator, IMapper mapper, ILoggerServices logger, IOptions<JwtBearer> jwt, UserManager<User> userManager) 
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _tokenServices = new TokenManager(jwt, userManager);
        }

        [HttpGet("GetAllPersonals")]
        public async Task<IActionResult> GetAllPersonals()
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllPersonals endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "Personal",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
            }
            var getAllRepository = await _mediator.Send(new GenericGetAllRequest<Personal>());
            if (getAllRepository.error == true)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllRepository.response.Message,
                    Action_type = Action_Type.APIRequest,
                    Target_table = "Personal",
                    loglevel_id = getAllRepository.response.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, getAllRepository.response.Exception);
                return BadRequest(getAllRepository.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllPersonals action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Personal",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getAllRepository.data);
        }


        [HttpGet("GetAllPersonalsByAirportId")]
        public async Task<IActionResult> GetAllPersonalsByAirportId([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetAllPersonalsByAirportId endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Personal",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
            }
            var getAllResponse = await _mediator.Send(new GetAllPersonalByAirportIdRequest(id));
            if (getAllResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getAllResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Personal",
                    loglevel_id = getAllResponse.response.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id ?? null
                }, getAllResponse.response.Exception);
                return BadRequest(getAllResponse.response.Exception);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetAllPersonalsByAirportId action done!",
                Action_type = Action_Type.Update,
                Target_table = "Personal",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getAllResponse.entity);
        }

        [HttpGet("GetPersonalById")]
        public async Task<IActionResult> GetPersonalById([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "GetPersonalById endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Personal",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
            }
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Personal",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var getByIdResponse = await _mediator.Send(new GenericGetByIdRequest<Personal>(id));
            if (getByIdResponse.error)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = getByIdResponse.response.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Personal",
                    loglevel_id = getByIdResponse.response.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, getByIdResponse.response.Exception);
                return BadRequest(getByIdResponse.response);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "GetPersonalById action done!",
                Action_type = Action_Type.APIResponse,
                Target_table = "Personal",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(getByIdResponse.entity);
        }

        [HttpPost("AddPersonal")]
        public async Task<IActionResult> AddPersonal(PersonalAddDTO personalDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "AddPersonal endpoint called!",
                Action_type = Action_Type.APIRequest,
                Target_table = "Personal",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
            }
            var personal = _mapper.Map<Personal, PersonalAddDTO>(personalDTO);
            var addResponse = await _mediator.Send(new GenericAddRequest<Personal>(personal));
            if (addResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = addResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Personal",
                    loglevel_id = addResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, addResponse.Exception);
                return BadRequest(addResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Personal added!",
                Action_type = Action_Type.Create,
                Target_table = "Personal",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Personal added!" });
        }

        [HttpDelete("DeletePersonal")]
        public async Task<IActionResult> DeletePersonal([FromQuery] int id)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "DeletePersonal endpoint called for {" + id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Personal",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
            }
            if (id == null || id == 0)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Invalid Id!!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Personal",
                    loglevel_id = 3,
                    user_id= validateTokenDTO.user.Id
                }, null);
                return BadRequest(new { message = "Invalid Id!!" });
            }
            var deleteResponse = await _mediator.Send(new GenericDeleteRequest<Personal>(id));
            if (deleteResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = deleteResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Personal",
                    loglevel_id = deleteResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id
                }, deleteResponse.Exception);
                return BadRequest(deleteResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Personal deleted!",
                Action_type = Action_Type.Delete,
                Target_table = "Personal",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Personal deleted!" });
        }

        [HttpPut("UpdatePersonal")]
        public async Task<IActionResult> UpdatePersonal(PersonalUpdateDTO personalDTO)
        {
            await _logger.Logger(new LogDTO
            {
                Message = "UpdatePersonal endpoint called for {" + personalDTO.id ?? null + "}",
                Action_type = Action_Type.APIRequest,
                Target_table = "Personal",
                loglevel_id = 1,
            }, null);
            var validateTokenDTO = await _tokenServices.ValidateToken(this.HttpContext);
            if (!validateTokenDTO.IsTokenValid)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = "Token is not valid!",
                    Action_type = Action_Type.APIResponse,
                    Target_table = "User",
                    loglevel_id = 3,
                    user_id = validateTokenDTO.user.Id ?? null
                }, null);
            }
            var data = await _mediator.Send(new GenericGetByIdRequest<Personal>(personalDTO.id));
            var personal = _mapper.Map<Personal, PersonalUpdateDTO>(personalDTO, data.entity);
            var updateResponse = await _mediator.Send(new GenericUpdateRequest<Personal>(personal));
            if (updateResponse != null)
            {
                await _logger.Logger(new LogDTO
                {
                    Message = updateResponse.Message,
                    Action_type = Action_Type.APIResponse,
                    Target_table = "Personal",
                    loglevel_id = updateResponse.Exception.ExceptionLevel,
                    user_id = validateTokenDTO.user.Id ?? null
                }, updateResponse.Exception);
                return BadRequest(updateResponse);
            }

            await _logger.Logger(new LogDTO
            {
                Message = "Personal Updated!",
                Action_type = Action_Type.Update,
                Target_table = "Personal",
                loglevel_id = 1,
                user_id = validateTokenDTO.user.Id
            }, null);
            return Ok(new { message = "Personal Updated!!" });
        }
    }
}
