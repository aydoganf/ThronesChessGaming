using ChessPlaying.API.Model.Request;
using ChessPlaying.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thrones.Gaming.Chess.SessionManagement;

namespace ChessPlaying.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChessController : ControllerBase
    {
        private string sessionText = "{\"CurrentIndexer\": \"0\", \"FullTypeName\": \"ChessPlaying.API.ChessAPISession\", \"Name\":\"test-session\",\"Players\":[{\"Nickname\":\"faruk\"},{\"Nickname\":\"ali\"}],\"Table\":{\"Stones\":[{\"Type\":\"King\",\"Color\":\"Black\",\"Location\":\"4|8\"},{\"Type\":\"Queen\",\"Color\":\"Black\",\"Location\":\"3|8\"},{\"Type\":\"King\",\"Color\":\"White\",\"Location\":\"4|1\"},{\"Type\":\"Queen\",\"Color\":\"White\",\"Location\":\"3|1\"}]}}";

        private readonly IChessDbService _chessDbService;

        public ChessController(IChessDbService chessDbService)
        {
            _chessDbService = chessDbService;
        }

        [HttpPost("create")]
        public SessionInformation Create([FromBody] CreateSessionRequest request)
        {
            var name = Guid.NewGuid().ToString();

            var session = SessionFactory
                .CreateOne<ChessAPISession>(name)
                .AddPlayers(request.Players[0].Nickname, request.Players[1].Nickname);

            var sessionInfo = Newtonsoft.Json.JsonConvert.SerializeObject(session.GetInformation());

            _chessDbService.CreateSession(name, sessionInfo);

            return session.GetInformation();
        }

        [HttpGet]
        public ActionResult<SessionInformation> GetSession([FromHeader(Name = "SessionName")] string sessionName)
        {
            if (string.IsNullOrEmpty(sessionName))
            {
                return Unauthorized();
            }

            var sessionInfo = _chessDbService.GetSession(sessionName).SessionInfo;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SessionInformation>(sessionInfo);
        }


        [HttpPost]
        public SessionInformation Command([FromBody] CommandRequest request, [FromHeader(Name = "SessionName")] string sessionName)
        {
            var session = _chessDbService.GetSession(sessionName);

            var chessSession = SessionFactory.CreateFrom(session.SessionInfo);
            var sessionInfo = chessSession.Command(request.Command);

            session.SessionInfo = Newtonsoft.Json.JsonConvert.SerializeObject(sessionInfo);
            _chessDbService.UpdateSession(session);

            return sessionInfo;
        }
    }
}
