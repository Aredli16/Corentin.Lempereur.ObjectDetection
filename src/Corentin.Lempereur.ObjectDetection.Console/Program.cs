using System.Text.Json;

var imagesDirectory = args[0];
var imagesSceneData = new List<byte[]>();

foreach (var imagePath in Directory.EnumerateFiles(imagesDirectory, "*.jpg"))
{
    var imageBytes = await File.ReadAllBytesAsync(imagePath);
    imagesSceneData.Add(imageBytes);
}

var objectDetection = new Corentin.Lempereur.ObjectDetection.ObjectDetection();
var detectionResults = await objectDetection.DetectObjectInScenesAsync(imagesSceneData);

foreach (var result in detectionResults)
    Console.WriteLine($"Box: {JsonSerializer.Serialize(result.Box)}");