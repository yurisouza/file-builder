using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FileBuilder.Core
{
    public class LineBuilder
    {
        private int _columnPosition;
        private readonly List<(int, string)> _texts;

        /// <summary>
        /// Create an instance of line builder
        /// </summary>
        public LineBuilder()
        {
            _columnPosition = 0;
            _texts = new List<(int, string)>();
        }

        /// <summary>
        /// Create an instance of line builder
        /// </summary>
        /// <param name="texts">Texts with yours columns defined</param>
        /// <param name="position">Column position to consider for reading e writing</param>
        public LineBuilder(List<(int, string)> texts, int position = -1)
        {
            _texts = texts;

            if (position < 0)
                position = texts.Count - 1;

            _columnPosition = position;
        }

        /// <summary>
        /// Adds a text to the current line in the specific column.
        /// </summary>
        /// <param name="column">Number of column. Minimum value it's 1.</param>
        /// <param name="text">Text to be insert at column.</param>
        public void AddText(int column, string text)
        {
            _texts.Add((column, text ?? ""));
        }

        /// <summary>
        /// Adds a text to the current line in next column available.
        /// </summary>
        /// <param name="text">Text to be insert at column available.</param>
        public void AddText(string text)
        {
            _texts.Add((_columnPosition, text ?? ""));
            _columnPosition++;
        }

        /// <summary>
        /// Adds texts to the current line in columns available sequentially.
        /// </summary>
        /// <param name="texts">Texts to be insert.</param>
        public void AddTexts(IEnumerable<string> texts)
        {
            foreach (var text in texts)
                AddText(text);
        }

        /// <summary>
        /// Adds texts to the current line in columns available sequentially.
        /// </summary>
        /// <param name="texts">Texts to be insert.</param>
        public void AddTexts(params string[] texts)
        {
            foreach (var text in texts)
                AddText(text);
        }

        /// <summary>
        /// Add texts in line with same concatenated before and after.
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="concatBefore">Text to be concatenated before.</param>
        /// <param name="concatAfter">Text to be concatenated after.</param>
        public void AddTextsConcat(string concatBefore, string concatAfter, params string[] texts)
        {
            foreach (var text in texts)
                AddText(string.Concat(concatBefore, text, concatAfter));
        }

        /// <summary>
        /// Builds a text with the texts added based in columns position and separated by a specific separator.
        /// </summary>
        /// <param name="separator">The separator to the texts.</param>
        /// <returns>Returns a text with the datas of line</returns>
        public string BuildLine(string separator)
        {
            return string.Join(separator, _texts.OrderBy(d => d.Item1).Select(d => SanitizeText(d.Item2)));
        }

        /// <summary>
        /// Replaces all texts that contains the term specific.
        /// </summary>
        /// <param name="term">The term should have in the text.</param>
        /// <param name="replaceTo">Text that should replace.</param>
        public void ReplaceContains(string term, string replaceTo)
        {
            foreach (var text in _texts.ToList())
            {
                if (text.Item2.Contains(term))
                    Replace(text.Item2, replaceTo);
            }
        }

        /// <summary>
        /// Replaces one text with another 
        /// </summary>
        /// <param name="oldText">Text to be replaced.</param>
        /// <param name="newText">Text that should replace.</param>
        public void Replace(string oldText, string newText)
        {
            var index = _texts.FindIndex(w => w.Item2 == oldText);

            if (index >= 0 && index < _texts.Count)
                _texts[index] = (index, newText ?? "");
        }

        /// <summary>
        /// Clone the line.
        /// </summary>
        /// <returns>Return a new instance of line.</returns>
        public LineBuilder Clone()
        {
            var objStr = JsonConvert.SerializeObject(new LineBuilder(_texts, _columnPosition));
            return JsonConvert.DeserializeObject<LineBuilder>(objStr);
        }

        /// <summary>
        /// Gets the position of the text specific.
        /// </summary>
        /// <param name="text">Text of the searched column.</param>
        /// <returns>Return the column position.</returns>
        public int GetPosition(string text)
        {
            return _texts.FirstOrDefault(w => w.Item2 == text).Item1;
        }

        /// <summary>
        /// Gets the text from the specified column and makes this column the current one.
        /// </summary>
        /// <param name="columnPosition">Desired column position.</param>
        /// <returns>Return the text.</returns>
        public string GetText(int columnPosition)
        {
            var text = _texts.FirstOrDefault(w => w.Item1 == columnPosition).Item2;

            if (text is null)
                return string.Empty;

            _columnPosition = columnPosition;

            return text;
        }

        /// <summary>
        /// Gets text of next column available.
        /// </summary>
        /// <returns>Return the text of next column available.</returns>
        public string NextText()
        {
            _columnPosition++;
            var text = GetText(_columnPosition);
            return text;
        }
        
        /// <summary>
        /// Remove breakline of the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string SanitizeText(string text) => text.Replace("\r\n", "").Replace("\n", "");
    }
}