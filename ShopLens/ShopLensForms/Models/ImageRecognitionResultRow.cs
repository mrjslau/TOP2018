namespace ShopLensForms.Models
{
    public interface IImageRecognitionResultRow
    {
        string Label { get; set; }
        double Confidence { get; set; }
    }

    public struct ImageRecognitionResultRow : IImageRecognitionResultRow
    {
        public string Label { get; set; }
        public double Confidence { get; set; }
        
        public ImageRecognitionResultRow(string label, double confidence)
        {
            Label = label;
            Confidence = confidence;
        }
    }
}
