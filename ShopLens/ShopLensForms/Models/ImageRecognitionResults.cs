using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShopLensForms.Models
{
    public class ImageRecognitionResults<T> : IEnumerable<T>, IEquatable<ImageRecognitionResults<T>> 
        where T: IImageRecognitionResultRow
    {
        private readonly HashSet<T> _resultRows = new HashSet<T>();

        public T MostConfidentResult { get; }

        public ImageRecognitionResults(IEnumerable<T> resultRows)
        {
            _resultRows = new HashSet<T>(resultRows);
            MostConfidentResult = _resultRows.OrderByDescending(x => x.Confidence).FirstOrDefault();
        }

        public T this[string index]
        {
            get {
                var result = _resultRows.Where(x => x.Label == index).SingleOrDefault();
                if(result != null)
                {
                    return result;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
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
            return other != null && _resultRows.SequenceEqual(other._resultRows);
        }
    }
}
