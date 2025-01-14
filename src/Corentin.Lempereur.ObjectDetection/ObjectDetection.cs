using ObjectDetection;

namespace Corentin.Lempereur.ObjectDetection;

public class ObjectDetection
{
    public async Task<IList<ObjectDetectionResult>> DetectObjectInScenesAsync(IList<byte[]> imagesSceneData)
    {
        var results = new List<ObjectDetectionResult>();
        var tinyYolo = new Yolo();

        var tasks = imagesSceneData.Select(imageData => Task.Run(() =>
        {
            var detectedBoxes = tinyYolo.Detect(imageData);

            return new ObjectDetectionResult { ImageData = detectedBoxes.ImageData, Box = detectedBoxes.Boxes };
        })).ToList();

        var detectionResults = await Task.WhenAll(tasks);
        results.AddRange(detectionResults);

        return results;
    }
}