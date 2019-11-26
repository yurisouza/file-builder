using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileBuilder.Core
{
    public class ReadFile
    {
        private readonly LineBuilder _header;
        private readonly List<LineBuilder> _lines;
        private LineBuilder _currentLine;
        private int _currentLinePosition;

        /// <summary>
        /// Creates an instance of constructor file to read the file
        /// </summary>
        /// <param name="data">Stream data</param>
        /// <param name="separator">Character used to separate column</param>
        /// <param name="hasHeader">If there is header in file</param>
        /// <param name="skipEmptyLine">If empty lines it's skipped</param>
        public ReadFile(Stream data, string separator, bool hasHeader, bool skipEmptyLine = true)
        {
            _currentLinePosition = 0;
            _lines = new List<LineBuilder>();

            using (var reader = new StreamReader(data, Encoding.GetEncoding("iso-8859-1")))
            {
                string line = null;
                while ((line = reader.ReadLine()?.Trim()) != null)
                {
                    if (skipEmptyLine && string.IsNullOrEmpty(line))
                        continue;

                    var fileLine = new LineBuilder();
                    var words = line.Split(new string[] { separator }, StringSplitOptions.None);
                    fileLine.AddTexts(words);
                    _lines.Add(fileLine);
                }
            }

            if (hasHeader && _lines.Count() > 0)
            {
                _header = _lines[0];
                _lines.RemoveAt(0);
            }
        }

        /// <summary>
        /// Gets the position of the line currently being read.
        /// The begin position is 1.
        /// </summary>
        /// <returns>Returns a number that corresponds to the line currently being read.</returns>
        public int GetCurrentLine() => _currentLinePosition + 1;

        /// <summary>
        /// Return if file is empty or not.
        /// </summary>
        /// <param name="considerHeader">Header is considered to tell if the file is empty.</param>
        /// <returns>Return true if the file is empty.</returns>
        public bool IsEmpty(bool considerHeader = false)
        {
            if (considerHeader)
                return _header == null && _lines.Count() == 0;
            else
                return _lines.Count() == 0;
        }

        /// <summary>
        /// Go to next line of file if exists.
        /// </summary>
        /// <returns>Returns true if there is a new line in the file.</returns>
        public bool NextLine()
        {
            if (_currentLinePosition <= _lines.Count - 1)
            {
                _currentLine = _lines[_currentLinePosition];
                _currentLinePosition++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets text of next column available.
        /// </summary>
        /// <returns>Return the text of next column available.</returns>
        public string ReadNextText()
        {
            if (_currentLine is null)
                NextLine();

            return _currentLine.NextText();
        }

        /// <summary>
        /// Gets a text in a column of the current line.
        /// </summary>
        /// <param name="position">Column number you want to get the word.</param>
        /// <returns>Returns the text of the current line of a specific column.</returns>
        public string ReadText(int position)
        {
            if (_currentLine is null)
                NextLine();

            return _currentLine.GetText(position);
        }

        /// <summary>
        /// Gets a text in a column of the current line.
        /// </summary>
        /// <param name="headerName">Column number you want to get the word.</param>
        /// <returns>Returns the text of the current line of a specific column.</returns>
        public string ReadText(string headerName)
        {
            if (_currentLine is null)
                NextLine();

            if (_header is null)
                return string.Empty;

            return _currentLine.GetText(_header.GetPosition(headerName));
        }

        /// <summary>
        /// Gets the current line as object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Returns a object of the current line</returns>
        public T ReadCurrentLine<T>() where T : class
        {
            IDictionary<string, object> obj = new ExpandoObject();

            if (_currentLine is null)
                NextLine();

            string headerWord = _header.GetText(0);

            while (headerWord != string.Empty)
            {
                obj.Add(headerWord, _currentLine.GetText(_header.GetPosition(headerWord)));
                headerWord = _header.NextText();
            }

            var textObject = JsonConvert.SerializeObject(obj);

            return JsonConvert.DeserializeObject<T>(textObject);
        }

        public IDictionary<string, string> ReadCurrentLine()
        {
            var obj = new Dictionary<string, string>();

            if (_currentLine is null)
                NextLine();

            string headerWord = _header.GetText(0);

            while (headerWord != string.Empty)
            {
                obj.Add(headerWord, _currentLine.GetText(_header.GetPosition(headerWord)));
                headerWord = _header.NextText();
            }

            return obj;
        }

        /// <summary>
        /// Set the first line of the file to the current line. Header is disregarded, if there is.
        /// </summary>
        public void ResetLinePosition()
        {
            _currentLinePosition = 0;
            NextLine();
        }
    }
}
