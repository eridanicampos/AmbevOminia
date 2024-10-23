using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTest.Application.DTO.User;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Helpers;

namespace ProjectTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : BaseController
    {
        private IUserService _userService;
        public UsuarioController(IUserService userService)
        {
            _userService = userService;
            
        }

        [HttpGet]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsuarioAsync()
        {
            return Ok(true);
        }
        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateAsync([FromQuery] UserParmersDTO parameters)
        {
            var user = await _userService.CreateAsync(parameters);
            return Ok(user);
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UserAuthenicationAsync([FromQuery] UserAuthenticateRequestViewModel parameters)
        {
            var auth = await _userService.AuthenticateAsync(parameters);
            return Ok(auth);
        }


        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(CargoResponseDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{
        //    var query = new GetCargoByIdQuery(id);
        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}
    }
}
