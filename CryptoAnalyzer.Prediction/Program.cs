var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();