
using Casino.User.Api.Persistence;
using Casino.User.Api.Services;

namespace Casino.User.Api
{
  public class Program
  {
    private Program()
    {
    }

    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services.AddSingleton<IValidationService, ValidationService>();
      builder.Services.AddSingleton<IUserService, UserService>();
      builder.Services.AddSingleton<IConnectionProvider, ConnectionProvider>();
      builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

      builder.Services.AddControllers();

      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      var app = builder.Build();
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseAuthorization();
      app.MapControllers();
      app.Run();
    }
  }
}
