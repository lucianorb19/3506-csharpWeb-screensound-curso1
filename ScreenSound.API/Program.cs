using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//REGISTRO DE SERVI�OES
//INJE��O DE DEPEND�NCIA - DAL<Artista>, DAL<Musica> E ScreenSoundContext
builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();

//SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ScreenSound API",
        Version = "v1",
        Description = "Uma API para gerenciamento de artistas e m�sicas",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "ScreenSound Support",
            Email = "support@screensound.com",
            Url = new Uri("https://screensound.com/support")
        }
    });
});

//IGNORAR CICLOS NA SERIALIZA��O
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>
    (options => options.SerializerOptions.ReferenceHandler = 
                             ReferenceHandler.IgnoreCycles);

var app = builder.Build();

//USO M�TODO DE EXTENS�O
app.AddEndPointsArtistas();
app.AddEndPointsMusicas();

//USO SWAGGER
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
