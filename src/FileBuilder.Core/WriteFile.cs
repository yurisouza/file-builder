using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileBuilder.Core
{
    public class WriteFile
    {
        private readonly List<LineBuilder> _lines;
        private readonly string _separator;
        private LineBuilder _currentLine;
        private LineBuilder _header;

        /// <summary>
        /// Create an instance of a write construtor file
        /// </summary>
        /// <param name="separator">Character used to separate column</param>
        public WriteFile(string separator)
        {
            _separator = separator;
            _lines = new List<LineBuilder>();
        }

        /// <summary>
        /// Adds a header to the file.
        /// </summary>
        /// <param name="line"></param>
        public void AddHeader(LineBuilder line) => _header = line;

        /// <summary>
        /// Adds a new line on file.
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(LineBuilder line) => _lines.Add(line);

        /// <summary>
        /// Adds a text to the current line in the specific column.
        /// </summary>
        /// <param name="column">Number of column. Minimum value it's 1.</param>
        /// <param name="text">Text to be insert at column.</param>
        public void AddText(int column, string text)
        {
            if (_currentLine == null)
                NewLine();

            _currentLine.AddText(column, text);
        }

        /// <summary>
        /// Adds a text to the current line in next column available.
        /// </summary>
        /// <param name="text">Text to be insert at column available.</param>
        public void AddText(string text)
        {
            if (_currentLine == null)
                NewLine();

            _currentLine.AddText(text);
        }

        /// <summary>
        /// Adds texts to the current line in columns available sequentially.
        /// </summary>
        /// <param name="texts">Texts to be insert.</param>
        public void AddTexts(params string[] texts)
        {
            if (_currentLine == null)
                NewLine();

            _currentLine.AddTexts(texts);
        }

        /// <summary>
        /// Builds the file with the header, if any, and all added lines.
        /// </summary>
        /// <returns>Returns file as array byte.</returns>
        public byte[] BuildFileStream()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter objstreamwriter = new StreamWriter(stream, Encoding.GetEncoding("utf-8"));

                objstreamwriter.Write(BuildFileString());
                objstreamwriter.Flush();
                objstreamwriter.Close();

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Builds the file with the header, if any, and all added lines.
        /// </summary>
        /// <returns>Returns file as string.</returns>
        public string BuildFileString()
        {
            var output = new StringBuilder();
            var breakLine = "\r\n";

            if (_header != null)
                output.Append(_header.BuildLine(_separator)).Append(breakLine);

            foreach (var line in _lines)
                output.Append(line.BuildLine(_separator)).Append(breakLine);

            return output.ToString();
        }

        /// <summary>
        /// Create a new line to the file.
        /// </summary>
        public void NewLine()
        {
            _currentLine = new LineBuilder();
            _lines.Add(_currentLine);
        }
    }
}