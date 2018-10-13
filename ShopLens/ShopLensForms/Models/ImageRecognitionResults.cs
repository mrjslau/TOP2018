using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShopLensForms.Models
{
    public class ImageRecognitionResults<T> : IEnumerable<T>, IEquatable<ImageRecognitionResults<T>> 
        where T: IImageRecognitionResultRow
    {
        public T MostConfidentResult =>
            _resultRows.OrderByDescending(x => x.Confidence).FirstOrDefault(); 
         
        private readonly List<T> _resultRows;

        public ImageRecognitionResults(IEnumerable<T> resultRows)
        {
            _resultRows = resultRows.ToList();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _resultRows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(ImageRecognitionResults<T> other)
        {
            return other != null && this._resultRows.SequenceEqual(other._resultRows);
        }
    }
}