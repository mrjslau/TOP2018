using System.Collections.Generic;

namespace ImageRecognition.Classificators
{
    public interface IImageClassifying
    {
        Dictionary<string, float> ClassifyImage(byte[] image);
    }
}