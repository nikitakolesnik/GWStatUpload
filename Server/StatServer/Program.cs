using StatServer.Contexts;
using StatServer.Services;

namespace StatServer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Logging.AddConsole();

			builder.Services.AddDbContext<AppDbContext>();
			builder.Services.AddControllers();
			builder.Services.AddScoped<MatchRepository>();
			const string corsPolicyName = "policy";
			builder.Services.AddCors(options =>
			{
				options.AddPolicy(name: corsPolicyName,
					policy =>
					{
						policy.WithOrigins("https://localhost:4200");
							//.WithMethods("PUT", "DELETE", "GET");
					});
			});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}


			app.UseHttpsRedirection();

			app.UseRouting(); //?

			app.UseCors(corsPolicyName);

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}