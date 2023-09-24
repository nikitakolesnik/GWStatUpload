using Microsoft.AspNetCore.Mvc;

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
		[HttpPost]
		public async Task<IActionResult> Post()
		{
			string content = await new StreamReader(Request.Body).ReadToEndAsync();
			_logger.LogInformation($"got json body:{Environment.NewLine}{content}");
			return Ok("posted and toasted");
		}
	}
}