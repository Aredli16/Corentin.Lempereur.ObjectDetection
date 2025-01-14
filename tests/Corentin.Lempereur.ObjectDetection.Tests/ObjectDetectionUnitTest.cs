using System.Reflection;
using System.Text.Json;

namespace Corentin.Lempereur.ObjectDetection.Tests;

public class ObjectDetectionUnitTest
{
    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly()
    {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath,
                     "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenesAsync(imageScenesData);
        Assert.Equal(
            "[{\"Dimensions\":{\"X\":275.54422,\"Y\":196.58589,\"Height\":38.619957,\"Width\":37.822857},\"Label\":\"bottle\",\"Confidence\":0.3079701}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[0].Box));
        Assert.Equal(
            "[{\"Dimensions\":{\"X\":224.44443,\"Y\":66.16817,\"Height\":344.24463,\"Width\":160.6837},\"Label\":\"person\",\"Confidence\":0.6512077},{\"Dimensions\":{\"X\":93.927185,\"Y\":79.03261,\"Height\":328.71793,\"Width\":173.46613},\"Label\":\"person\",\"Confidence\":0.6075896}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[1].Box));
    }

    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}