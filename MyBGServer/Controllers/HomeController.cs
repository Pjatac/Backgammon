using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBGServer.Infra;
using MyBGServer.Models;

namespace MyBGServer.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		private readonly IUserService _userService;
		public HomeController(IUserService userService)
		{
			_userService = userService;
		}
		[AllowAnonymous]
		[HttpGet]
		public async Task<IEnumerable<User>> GetAll()
		{
			try
			{
				return await _userService.GetUsers();
			}
			catch
			{
				throw new Exception();
			}
		}
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login([FromBody] User loginInfo)
		{
			try
			{
				var ownerInfo = await _userService.Login(loginInfo);
				if (ownerInfo != null)
				{
					return new ObjectResult(ownerInfo);
				}
				else
					return NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest();
			}
		}
		[AllowAnonymous]
		[HttpPost]
		[Route("add")]
		public async Task<IActionResult> AddUser([FromBody] User newUser)
		{
			try
			{
				var res = await _userService.AddNewUser(newUser);
				if (res == "ok")
					return Ok();
				else
					return new ObjectResult(res);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
		[HttpDelete]
		[Route("remove")]
		public async Task<IActionResult> RemoveUser([FromBody] User toremoveUser)
		{
			try
			{
				await _userService.Remove(toremoveUser);
				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}
