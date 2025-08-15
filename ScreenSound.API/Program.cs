using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//INJE��O DE DEPEND�NCIA - DAL<Artista>, DAL<Musica> E ScreenSoundContext
builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();

//IGNORAR CICLOS NA SERIALIZA��O
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>
    (options => options.SerializerOptions.ReferenceHandler = 
                             ReferenceHandler.IgnoreCycles);

var app = builder.Build();

//USO M�TODO DE EXTENS�O
app.AddEndPointsArtistas();
app.AddEndPointsMusicas();

app.Run();
