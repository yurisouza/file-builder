using FileBuilder.Core;
using FileBuilder.Tests.Faker;
using FluentAssertions;
using Xunit;

namespace FileBuilder.Tests.UnitTests
{
    public class WriteFileUnitTest
    {
        private const string _separator = ";";
        private const string _breakline = "\r\n";

        [Fact(DisplayName = "Given I have a write file When I add header Then I should have a final text with header.")]
        [Trait("WriteFile", "AddHeader()")]
        public void Should_Be_Added_A_Header()
        {
            //Arrange
            var file = new WriteFile(_separator);

            var header = new LineBuilder();
            header.AddTexts("name", "lastname");

            //Act
            file.AddHeader(header);

            //Assert
            var expectedResult = $"name{_separator}lastname{_breakline}";

            file.BuildFileString().Should().Be(expectedResult);
        }

        [Fact(DisplayName = "Given I have a write file When I add line Then I should have a final text with line.")]
        [Trait("WriteFile", "AddLine()")]
        public void Should_Be_Added_A_Line()
        {
            //Arrange
            var file = new WriteFile(_separator);

            var line = new LineBuilder();
            line.AddTexts("Iron", "Man");

            //Act
            file.AddLine(line);

            //Assert
            var expectedResult = $"Iron{_separator}Man{_breakline}";

            file.BuildFileString().Should().Be(expectedResult);
        }

        [Fact(DisplayName = "Given I have a new line and any text When I add it on a specific column Then I should have added the text in the current line and on a specific column.")]
        [Trait("WriteFile", "AddText(int column, string text)")]
        public void Should_Be_Add_Text_In_Specific_Column()
        {
            //Arrange
            var file = new WriteFile(_separator);

            var header = new LineBuilder();
            header.AddTexts("name", "lastname");
            file.AddHeader(header);

            file.NewLine();
            var text = "Iron Man";

            var firstColumn = $"{_separator}";

            var expectedHeader = $"name{_separator}lastname{_breakline}";
            var expectedLine = $"{firstColumn}{text}{_breakline}";

            var expectedResult = string.Concat(expectedHeader, expectedLine);

            //Act
            file.AddText(2, text);

            //Assert
            var result = file.BuildFileString();

            result.Should().Be(expectedResult);
        }

        [Fact(DisplayName = "Given I have a new line and any text When I add it Then I should have added the text in the current line and in next column available.")]
        [Trait("WriteFile", "AddText(string text)")]
        public void Should_Be_Add_Text()
        {
            //Arrange
            var file = new WriteFile(_separator);

            file.NewLine();
            var text = "Iron Man";

            var expectedLine = $"{text}{_breakline}";

            //Act
            file.AddText(text);

            //Assert
            var result = file.BuildFileString();

            result.Should().Be(expectedLine);
        }

        [Fact(DisplayName = "Given I have a new line and any texts When I add it Then I should have added the texts in the current line and in nexts column availables.")]
        [Trait("WriteFile", "AddTexts(params string[] texts)")]
        public void Should_Be_Add_Texts()
        {
            //Arrange
            var file = new WriteFile(_separator);

            file.NewLine();
            var text = "Iron Man";
            var secondText = "Captain America";

            var expectedLine = $"{text}{_separator}{secondText}{_breakline}";

            //Act
            file.AddTexts(text, secondText);

            //Assert
            var result = file.BuildFileString();

            result.Should().Be(expectedLine);
        }

        [Fact(DisplayName = "Given I have a write file When I build file Then I should have the lines and words compile as a only string.")]
        [Trait("WriteFile", "BuildFileString()")]
        public void Should_Be_BuildFileString()
        {
            //Arrange
            var file = new WriteFile(_separator);

            var header = new LineBuilder();
            header.AddTexts("name", "lastname");
            file.AddHeader(header);
            var expectedHeader = $"name{_separator}lastname{_breakline}";

            file.NewLine();
            file.AddTexts("Iron", "Man");
            var expectedFirstLine = $"Iron{_separator}Man{_breakline}";

            file.NewLine();
            file.AddTexts("Captain", "America");
            var expectedSecondLine = $"Captain{_separator}America{_breakline}";

            var expectedResult = string.Concat(expectedHeader, expectedFirstLine, expectedSecondLine);

            //Act
            var result = file.BuildFileString();

            //Assert
            result.Should().Be(expectedResult);
        }
    }
}