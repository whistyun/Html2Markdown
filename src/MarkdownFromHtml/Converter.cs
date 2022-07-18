using System.IO;
using System.Text;

namespace MarkdownFromHtml
{
    /// <summary>
    /// An Html to Markdown converter.
    /// </summary>
    public class Converter
    {
        private ReplaceManager _manager;

        /// <summary>
        /// Create a Converter with the standard Markdown conversion scheme
        /// </summary>
        public Converter()
        {
            _manager = new ReplaceManager();
        }

        public Converter(ReplaceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Converts Html contained in a file to a Markdown string
        /// </summary>
        /// <param name="path">The path to the file which is being converted</param>
        /// <returns>A Markdown representation of the passed in Html</returns>
        public string ConvertFile(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    return Convert(reader.ReadToEnd());
                }
            }
        }

        /// <summary>
        /// Converts an Html string to a Markdown string
        /// </summary>
        /// <param name="html">The Html string you wish to convert</param>
        /// <returns>A Markdown representation of the passed in Html</returns>
        public string Convert(string html)
        {
            var buff = new StringBuilder();

            var normHtml = html.Replace("\r\n", "\n")
                               .Replace("\r", "\n");

            bool isRepeated = false;

            foreach (var block in _manager.Parse(normHtml))
            {
                if (isRepeated)
                {
                    buff.Append('\n');
                }

                foreach (var line in block.ToMarkdown())
                {
                    buff.Append(line).Append('\n');
                }

                isRepeated = true;
            }

            while (buff.Length > 0 && buff[buff.Length - 1] == '\n')
                buff.Remove(buff.Length - 1, 1);

            return buff.ToString();
        }
    }
}