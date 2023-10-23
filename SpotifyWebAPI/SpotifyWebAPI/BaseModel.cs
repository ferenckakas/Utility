using System.Collections.Generic;

namespace SpotifyWebAPI
{
    /// <summary>
    /// Model base class, used to house common methods
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Create comma-ified list of strings (typically used in spotify urls)
        /// </summary>
        /// <param name="strings"></param>
        /// <returns>comma-ified string</returns>
        public static string CreateCommaSeperatedList(List<string> strings)
        {
            string output = string.Empty;
            foreach (string str in strings)
                output += str + ",";
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        /// <summary>
        /// Create request string style key=value pairs
        /// </summary>
        /// <param name="strings"></param>
        /// <returns>comma-ified string</returns>
        public static string CreateKeyValueAmpersandSeperatedList(Dictionary<string, string> keyValuePairs)
        {
            string output = string.Empty;
            foreach (KeyValuePair<string, string> str in keyValuePairs)
                output += str.Key + "=" + str.Value + "&";
            output = output.Substring(0, output.Length - 1);

            return output;
        }
    }
}
