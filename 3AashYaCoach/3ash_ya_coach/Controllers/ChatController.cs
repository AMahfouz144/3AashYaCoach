using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach._3ash_ya_coach.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _3AashYaCoach._3ash_ya_coach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] ChatMessageDto dto)
        {
            Console.WriteLine($"[ChatController] Received message from {dto.SenderId} to {dto.ReceiverId}: {dto.Text}");
            var result = await _chatService.SaveMessageAsync(dto.SenderId, dto.ReceiverId, dto.Text);
            Console.WriteLine("[ChatController] Message processing complete.");
            return Ok(new { result.Id, result.SentAt });
        }
        [HttpGet("getMessages")]
        public async Task<IActionResult> GetMessages([FromQuery] Guid senderId, [FromQuery] Guid receiverId)
        {
            Console.WriteLine($"[ChatController] Fetching messages between {senderId} and {receiverId}");
            var messages = await _chatService.GetMessagesAsync(senderId, receiverId);
            return Ok(messages);
        }

    }


}
