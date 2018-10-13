using System.Collections.Generic;
using ShopLensForms.Models;

namespace ShopLensApp.ExtensionMethods
{
    public static class EnumerableToContainerExtensions
    {
        public static ImageRecognitionResults<TSource> ToImageRecognitionResults<TSource>(this IEnumerable<TSource> source)
            where TSource : IImageRecognitionResultRow
        {
            var resultContainer = new ImageRecognitionResults<TSource>(source);
            return resultContainer;
        }
            
    }
}
