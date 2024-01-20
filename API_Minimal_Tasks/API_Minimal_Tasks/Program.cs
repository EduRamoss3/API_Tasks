using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
static string RandomString()
{
    char[] chars = new char[]
    {
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
    };
    string final_string_random = "";
    Random random_int = new Random();
    for (int i = 0; i < 10; i++)
    {
        final_string_random = final_string_random.Insert(i, chars[random_int.Next(25)].ToString());
    }
    return final_string_random;
}
app.MapGet("randomString", () => RandomString());
app.MapGet("frases", async () => await new HttpClient().GetStringAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes"));
app.Run();