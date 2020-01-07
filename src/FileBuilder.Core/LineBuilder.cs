using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileBuilder.Core
{
    public class LineBuilder
    {
        private int _columnPosition;
        private readonly List<(int Position, string Value)> _texts;

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
                position = texts.Count;

            _columnPosition = position;
        }

        /// <summary>
        /// Adds a text to the current line in the specific column.
        /// </summary>
        /// <param name="column">Number of column. Minimum value it's 1.</param>
        /// <param name="text">Text to be insert at column.</param>
        public void AddText(int column, string text)
        {
            if (column < 1)
                throw new Exception(message: "Invalid column position. Minimum value it's 1.");

            var existentItem = _texts.FirstOrDefault(t => t.Position == column);

            if (existentItem.Value != null)
                _texts.Remove(existentItem);

            _texts.Add((column, text ?? ""));
        }

        /// <summary>
        /// Adds a text to the current line in next column available.
        /// </summary>
        /// <param name="text">Text to be insert at column available.</param>
        public void AddText(string text)
        {
            _columnPosition++;
            _texts.Add((_columnPosition, text ?? ""));
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
            var result = string.Empty;
            var textsOrdened = _texts.OrderBy(d => d.Position).ToList();
            var lastTextOrder = _texts.LastOrDefault().Position;

            for (int i = 1; i <= lastTextOrder; i++)
            {
                var content = textsOrdened.FirstOrDefault(t => t.Position == i).Value ?? string.Empty;

                result = string.Concat(result, SanitizeText(content));

                if (i != lastTextOrder)
                    result = string.Concat(result, separator);
            }

            return result;
        }

        /// <summary>
        /// Replaces all texts that contains the term specific.
        /// </summary>
        /// <param name="term">The term should have in the text.</param>
        /// <param name="replaceTo">Text that should replace.</param>
        public void ReplaceAllContains(string term, string replaceTo)
        {
            foreach (var (_, Value) in _texts.ToList())
            {
                if (Value.Contains(term))
                    Replace(Value, replaceTo);
            }
        }

        /// <summary>
        /// Replaces all old text with another 
        /// </summary>
        /// <param name="oldText">Text to be replaced.</param>
        /// <param name="newText">Text that should replace.</param>
        public void ReplaceAll(string oldText, string newText)
        {
            for (int i = 0; i < _texts.Count; i++)
                Replace(oldText, newText);
        }

        /// <summary>
        /// Replaces one text with another 
        /// </summary>
        /// <param name="oldText">Text to be replaced.</param>
        /// <param name="newText">Text that should replace.</param>
        public void Replace(string oldText, string newText)
        {
            var index = _texts.FindIndex(w => w.Value == oldText);

            if (index >= 0 && index < _texts.Count)
                _texts[index] = (_texts[index].Position, newText ?? "");
        }

        /// <summary>
        /// Clone the line.
        /// </summary>
        /// <returns>Return a new instance of line.</returns>
        public LineBuilder Clone()
        {
            var objTexts = JsonConvert.SerializeObject(_texts);
            return new LineBuilder(JsonConvert.DeserializeObject<List<(int, string)>>(objTexts), _columnPosition);
        }

        /// <summary>
        /// Gets the position of the text specific.
        /// </summary>
        /// <param name="text">Text of the searched column.</param>
        /// <returns>Return the column position. If not exists then return -1.</returns>
        public int GetPosition(string text)
        {
            var result = _texts.FirstOrDefault(w => w.Value == text);
            return result.Value == null ? -1 : result.Position;
        }

        /// <summary>
        /// Gets the text from the specified column and makes this column the current one.
        /// </summary>
        /// <param name="columnPosition">Desired column position. Begin in: 1 (one)</param>
        /// <returns>Return the text.</returns>
        public string GetText(int columnPosition)
        {
            var text = _texts.FirstOrDefault(w => w.Position == columnPosition).Value;

            if (text is null)
                return string.Empty;

            _columnPosition = columnPosition;

            return text;
        }

        /// <summary>
        /// Gets the current column position
        /// </summary>
        /// <returns>Return the current column position.</returns>
        public int GetCurrentPosition() => _columnPosition <= 0 ? 1 : _columnPosition;

        /// <summary>
        /// Gets text of next column available.
        /// </summary>
        /// <returns>Return the text of next column available.</returns>
        public string NextText()
        {
            var text = GetText(_columnPosition);
            _columnPosition += 1;
            return text;
        }

        /// <summary>
        /// Define column position as zero.
        /// </summary>
        public void ResetCurrentPosition()
        {
            _columnPosition = 1;
        }
        
        /// <summary>
        /// Remove breakline of the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string SanitizeText(string text) => text.Replace("\r\n", "").Replace("\n", "");
    }
}