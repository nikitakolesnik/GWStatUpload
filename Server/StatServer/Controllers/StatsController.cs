using Microsoft.AspNetCore.Mvc;
using StatServer.DTOs;
using System.Text.Json.Serialization;

namespace StatServer.Controllers
{
	[Route("stats")]
	[ApiController]
	public class StatsController : ControllerBase
	{
		private readonly ILogger<StatsController> _logger;

		public StatsController(ILogger<StatsController> logger)
		{
			_logger = logger;
			_logger.LogInformation("constructed");
		}

		[Route("marco")]
		[HttpGet]
		public IActionResult Get()
		{
			_logger.LogInformation("got response");
			return Ok("polo");
		}

		[Route("posttest")]
		[RequestSizeLimit(2147483648)] // e.g. 2 GB request limit
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] MatchEntryDTO matchEntry)
		{
			string json = "{\r\n  \"players\": [\r\n    {\r\n      \"name\": \"S La M\",\r\n      \"skills\": [\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Auspicious Blow\",\r\n          \"skill_id\": 905\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Hammer Bash\",\r\n          \"skill_id\": 331\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Protector's Strike\",\r\n          \"skill_id\": 326\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Enraged Smash\",\r\n          \"skill_id\": 993\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Flail\",\r\n          \"skill_id\": 1404\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Crushing Blow\",\r\n          \"skill_id\": 352\r\n        },\r\n        {\r\n          \"count\": 1,\r\n          \"name\": \"Sprint\",\r\n          \"skill_id\": 349\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Resurrection Signet\",\r\n          \"skill_id\": 2\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"name\": \"Acolyte Sousuke\",\r\n      \"skills\": [\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Blinding Surge\",\r\n          \"skill_id\": 1367\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Lightning Strike\",\r\n          \"skill_id\": 222\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Chain Lightning\",\r\n          \"skill_id\": 223\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Shell Shock\",\r\n          \"skill_id\": 2059\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Air Attunement\",\r\n          \"skill_id\": 225\r\n        },\r\n        {\r\n          \"count\": 1,\r\n          \"name\": \"Aura of Restoration\",\r\n          \"skill_id\": 180\r\n        },\r\n        {\r\n          \"count\": 1,\r\n          \"name\": \"\\\"Fall Back!\\\"\",\r\n          \"skill_id\": 1595\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Resurrection Signet\",\r\n          \"skill_id\": 2\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"name\": \"Zhed Shadowhoof\",\r\n      \"skills\": [\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Mind Burn\",\r\n          \"skill_id\": 185\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Immolate\",\r\n          \"skill_id\": 191\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Smoldering Embers\",\r\n          \"skill_id\": 1090\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Glowing Gaze\",\r\n          \"skill_id\": 1379\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Fire Attunement\",\r\n          \"skill_id\": 184\r\n        },\r\n        {\r\n          \"count\": 1,\r\n          \"name\": \"Aura of Restoration\",\r\n          \"skill_id\": 180\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Cure Hex\",\r\n          \"skill_id\": 2003\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Healing Breeze\",\r\n          \"skill_id\": 288\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"name\": \"Dunkoro\",\r\n      \"skills\": [\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Healing Burst\",\r\n          \"skill_id\": 1118\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Patient Spirit\",\r\n          \"skill_id\": 2061\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Signet of Rejuvenation\",\r\n          \"skill_id\": 887\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Dwayna's Kiss\",\r\n          \"skill_id\": 283\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Cure Hex\",\r\n          \"skill_id\": 2003\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Vigorous Spirit\",\r\n          \"skill_id\": 254\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Spotless Soul\",\r\n          \"skill_id\": 2065\r\n        },\r\n        {\r\n          \"count\": 0,\r\n          \"name\": \"Eremite's Zeal\",\r\n          \"skill_id\": 1524\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";

			string content = await new StreamReader(Request.Body).ReadToEndAsync();
			_logger.LogInformation($"got json body:{Environment.NewLine}{content}");
			return Ok("posted and toasted");
		}
	}
}