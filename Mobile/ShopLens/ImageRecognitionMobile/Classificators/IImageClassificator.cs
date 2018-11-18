using System.Collections.Generic;

namespace ImageRecognitionMobile.Classificators
{
    /// <summary>
    /// Interface encapsulating image classification methods.
    /// </summary>
    public interface IImageClassificator
    {
        /// <summary>
        /// Classifies a given image and returns the results as a dictionary of labels to values.
        /// </summary>
        /// <param name="image">Bytes of a .jpeg image.</param>
        /// <returns>
        /// Pairs of 'string - float' that where:
        /// The Key is the name of the label.
        /// The Value is a float in range 0 to 1.
        /// Ex.: {'tomato', 0.95f}
        /// </returns>
        Dictionary<string, float> ClassifyImage(byte[] image);
    }
}