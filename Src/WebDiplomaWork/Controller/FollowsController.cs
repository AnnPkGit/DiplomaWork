using Application.Common.Models;
using Application.Follows.Commands; 
using Application.Follows.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Follows.Queries.GetFollowsCommands;


namespace WebDiplomaWork.Controller
{
    public class FollowsController : ApiV1ControllerBase
    {
        [HttpPost("follow"), Authorize]
        public async Task<FollowDto> CreateFollow([FromBody] CreateFollowCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в методе CreateFollow: {ex}");
                throw; 
            }
        }
    

        [HttpPost("unfollow"), Authorize]
        public async Task<IActionResult> Unfollow(UnfollowCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
        
        //Отримамти всіх на кого підписан поточний user
        [HttpGet("following"), Authorize]
        public async Task<PaginatedList<FollowDto>> GetAllFollows([FromQuery] GetAllFollowsCommand command)
        {
            return await Mediator.Send(command);
        }
        
        //Отримамти всіх хто підписан на поточного user
        [HttpGet("followers"), Authorize]
        public async Task<PaginatedList<FollowDto>> GetFollowers([FromQuery] GetFollowersCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}


