using Microsoft.AspNetCore.Mvc;
using StatServer.Models;
using Newtonsoft.Json;
using StatServer.Services;

// https://wiki.guildwars.com/wiki/Game_link:Skill_{id}

namespace StatServer.Controllers
{
	[Route("stats")]
	[ApiController]
	public class StatsController : ControllerBase
	{
		private readonly ILogger<StatsController> _logger;
		private readonly MatchRepository _repo;

		public StatsController(ILogger<StatsController> logger, MatchRepository repo)
		{
			_logger = logger;
			_logger.LogInformation("constructed");
			_repo = repo;
		}

		[Route("marco")]
		[HttpGet]
		public IActionResult HealthCheck()
		{
			_logger?.LogInformation("got response");
			return Ok("polo");
		}

		[Route("submit_match")]
		//[RequestSizeLimit(2147483648)] // e.g. 2 GB request limit
		[HttpPost]
		public async Task<IActionResult> SubmitMatch()//[FromBody] MatchEntryDTO matchEntry) // getting it this way gives "Failed to read the request form. Form key length limit 2048 exceeded." i will figure it out later maybe
		{
			string content = await new StreamReader(Request.Body).ReadToEndAsync();
			MatchEntryDTO_Incoming matchEntry = JsonConvert.DeserializeObject<MatchEntryDTO_Incoming>(content);

			if (matchEntry.Players.Count == 0)
				return BadRequest("No player rows");

			_logger?.LogInformation($"got json body with {matchEntry?.Players.Count ?? -1} players");

			await _repo.AddMatchEntry(matchEntry);
			
			return Ok("posted and toasted");
		}

		[Route("get_match_page")]
		[HttpGet]
		public async Task<IEnumerable<MatchEntryDTO_Outgoing>> GetMatchPage(int offset = 0, int pageSize = 10)
		{
			return await _repo.GetMatchEntriesForMatches(offset, pageSize);
		}
	}
}