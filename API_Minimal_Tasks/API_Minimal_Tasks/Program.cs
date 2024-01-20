using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseInMemoryDatabase("TarefasDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
char[] vogals = new char[]
{
    'a','e','i','o','u'
};
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
int VerifyVogals(string word)
{
    int count_vogals = 0;
    char[] separate_chars = word.ToCharArray();
    foreach (char character in separate_chars)
    {
        foreach (char vogal in vogals)
        {
            if (character == vogal)
            {
                count_vogals++;
            }
        }
    }
    return count_vogals;
}
app.MapGet("take_number_vogals", (string word) =>
{
    return VerifyVogals(word);
});
app.MapGet("tarefas_concluidas", async (AppDbContext context) =>
{
    return await context.Tarefas.Where(t => t.IsFinally).ToListAsync();
});
app.MapGet("/get_tarefas_by_id", async (AppDbContext context, int id) =>
{
    return await context.Tarefas.FindAsync(id) is Tasks task ? Results.Ok(task) : Results.NotFound();
});
app.MapGet("/tasks", async (AppDbContext context) =>
{
    return await context.Tarefas.ToListAsync();
});
app.MapPost("/create", async (AppDbContext context, Tasks tasks) =>
{
    context.Tarefas.AddAsync(tasks);
    await context.SaveChangesAsync();
    return Results.Created($"/create/{tasks.Id}", tasks);

});
app.MapGet("/randomString", () => RandomString());
app.MapGet("/frases", async () => await new HttpClient().GetStringAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes"));
app.Run();
public class Tasks
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsFinally { get; set; }

}
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    public DbSet<Tasks>? Tarefas => Set<Tasks>();
}