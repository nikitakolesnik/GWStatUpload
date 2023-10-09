using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace GetSkillIconsUrlsById
{
	internal class Program
	{
		static void Main(string[] args)
		{
			HttpClient client = new();
			client.DefaultRequestHeaders.UserAgent.ParseAdd("GWToolboxpp/6.10"); // forbidden response without this so we do a bit of lying

			StringBuilder sb = new("const map1: Map<number, string> = new Map([");

			int counter = 0;
			int total = Skills.Map.Count;
			foreach (KeyValuePair<int, string> skill in Skills.Map)
			{
				Console.WriteLine($"{counter++}/{total}");
				//Thread.Sleep(2000); // turns out wiki doesnt give a shit if you basically scrape it

				int id = skill.Key;
				string url = $"https://wiki.guildwars.com/wiki/Game_link:Skill_{id}";
				HttpResponseMessage response = client.GetAsync(url).Result;

				if (!response.IsSuccessStatusCode)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"failure code {response.StatusCode} for skill ID {id}");
					Console.ResetColor();
					continue;
				}

				const string regex = "class=\"skill-image\"[\\s\\S]*?<img[^>]+src=['\"]([^\"']+)([.](png|jpg))";
				string content = response.Content.ReadAsStringAsync().Result;
				Match match = Regex.Match(content, regex);
				string path = match.Groups[1].Value;
				string ext  = match.Groups[2].Value;

				string fullpath = $"https://wiki.guildwars.com{path}{ext}";
				string row = $"\t[{id}, '{fullpath}'],";
				sb.Append(row);
				sb.Append(Environment.NewLine);
			}

			sb.Append("]);");
			string final = sb.ToString();
			Console.ReadKey(); // i don't want to look up the file export syntax... just breakpoint and copy
		}
	}
}