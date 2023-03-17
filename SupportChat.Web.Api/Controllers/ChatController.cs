using Microsoft.AspNetCore.Mvc;
using SupportChat.Application.Interfaces;
using SupportChat.Web.Api.Models;

namespace SupportChat.Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly ISessionManager _sessionManager;

    public ChatController(ISessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }
    
    [HttpPost("Start", Name = "Start chat session")]
    public StartChatResponse Start()
    {
        var sessionId = _sessionManager.CreateSession();

        return new StartChatResponse(sessionId);
    }

    [HttpGet("Ping/{sessionId:int}", Name = "Ping session status")]
    public PingResponse Ping(int sessionId)
    {
        var response = _sessionManager.CheckSession(sessionId);

        return new PingResponse(response.AgentDescription);
    }
}