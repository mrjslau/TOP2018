using System.Text.RegularExpressions;

namespace ImageRecognitionMobile.OCR
{
    /// <summary>
    /// Interface for a finder that finds a weight substring in a string.
    /// For example, in "apples, 10kg", it would find the string "10kg".
    /// </summary>
    interface IMetricWeightSubstringFinder
    {
        /// <summary>
        /// Find the first occurence of a weight specifying string.
        /// </summary>
        /// <param name="input">The input string to find the substring in.</param>
        /// <returns></returns>
        string FindWeightSpecifier(string input);
    }
    
    /// <inheritdoc />
    /// <summary>
    /// A class that realises the weight substring search using Regular Expressions.
    /// </summary>
    public class RegexMetricWeightSubstringFinder : IMetricWeightSubstringFinder
    {
        private readonly Regex _rx = new Regex(@"(?<weight>(\d+\s?\.?\s?\d*?\s*?[g,l]|\d+\s?\.?\s?\d*?\s*?[m,k,c][g,l]?)(\s|$))");
        
        /// <inheritdoc cref="IMetricWeightSubstringFinder.FindWeightSpecifier"/>
        public string FindWeightSpecifier(string input)
        {
            var match = _rx.Match(input);
            var matchedWeight = Regex.Replace(match.Groups["weight"].Value, @"\s+", "");
            return matchedWeight;
        }
    }
}