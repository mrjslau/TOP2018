using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImageRecognitionMobile.Classificators;
using ImageRecognitionMobile.Classificators.Helpers;

namespace ImageRecognition.Classificators
{
    // For more see https://southcentralus.dev.cognitive.microsoft.com/docs/services/450e4ba4d72542e889d93fd7b8e960de/operations/5a6264bc40d86a0ef8b2c290
    /// <inheritdoc />
    /// <summary>
    /// This class classifies images by using the customvision.ai API.
    /// </summary>
    public class WebClassificator : IImageClassificator, IAsyncImageClassificator
    {
        /// <inheritdoc/>
        public Dictionary<string, float> ClassifyImage(byte[] image)
        {
            return ClassifyImageAsync(image).GetAwaiter().GetResult();
        }

        /// <inheritdoc cref="ClassifyImage" />
        /// <summary>Same as <see cref="ClassifyImage"/> but performed asynchronously.</summary>
        public async Task<Dictionary<string, float>> ClassifyImageAsync(byte[] image)
        {
            var projectId = Guid.Parse("11e80156-6630-4cd7-a9e6-3441f9da09e2");
            var predictionKey = "13185045e5b84af5bd045d029447d951";
            
            // Form request
            var client = new HttpClient();
            // Request headers
            client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

            // Request parameters
            var uri = 
                $"https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/{projectId}/image";

            var multipartFormDataContent = GetMultipartFormDataContent(image);

            // Execute request
            var response = await client.PostAsync(uri, multipartFormDataContent);
            response.EnsureSuccessStatusCode();
            
            var results = CustomVisionPredictionResponse.FromJson(await response.Content.ReadAsStringAsync());
            return results.Predictions.ToDictionary(x => x.TagName, x => (float) x.Probability);
        }

        private static MultipartFormDataContent GetMultipartFormDataContent(byte[] image)
        {
            var imageData = new MemoryStream(image);

            var multipartFormDataContent = new MultipartFormDataContent();

            var streamContent = new StreamContent(imageData);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            multipartFormDataContent.Add(streamContent, nameof(imageData));
            return multipartFormDataContent;
        }
    }
}