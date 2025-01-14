using Microsoft.AspNetCore.Mvc;

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

app.MapPost("/ObjectDetection", async ([FromForm] IFormFileCollection files) =>
{
    if (files.Count < 1)
        return Results.BadRequest();

    await using var sceneSourceStream = files[0].OpenReadStream();
    using var sceneMemoryStream = new MemoryStream();
    sceneSourceStream.CopyTo(sceneMemoryStream);
    var imageSceneData = sceneMemoryStream.ToArray();
    
    var objectDetection = new Corentin.Lempereur.ObjectDetection.ObjectDetection();
    var detectionResults = await objectDetection.DetectObjectInScenesAsync(new List<byte[]> { imageSceneData });
    
    if (detectionResults.Count == 0 || detectionResults[0].Box.Count == 0) 
        return Results.BadRequest("Aucun objet détecté dans l'image.");
    
    return Results.File(detectionResults[0].ImageData, "image/jpg");
    
}).DisableAntiforgery();

app.Run();