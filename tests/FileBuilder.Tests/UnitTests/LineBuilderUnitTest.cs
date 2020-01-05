using FileBuilder.Core;
using FileBuilder.Tests.Faker;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FileBuilder.Tests.UnitTests
{
    public class LineBuilderUnitTest
    {
        [Fact(DisplayName = "Given I have a null text When I add in the line Then I should have added an empty text on the current column.")]
        [Trait("LineBuilder", "AddText(string text)")]
        public void Should_Be_Add_An_Null_Text_Success()
        {
            //Arrange
            var line = new LineBuilder();
            string text = null;
            line.AddText(text);

            //Act
            var validateTest = line.GetText(0) == string.Empty;

            //Assert
            validateTest.Should().BeTrue();
        }

        [Theory(DisplayName = "Given I have some texts When I add in the line Then I should have added the text on yourself column.")]
        [Trait("LineBuilder", "AddText(string text)")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Be_Add_Text_In_Multiple_Columns_Success(int amountColumns)
        {
            //Arrange
            var line = new LineBuilder();

            var items = new Dictionary<int, string>();

            for (int column = 1; column <= amountColumns; column++)
            {
                var text = FakerPrimitiveTypes.GetSampleText(0, 10000);
                line.AddText(text);

                items.Add(column, text);
            }

            //Act
            var validateTest = items.All(i => line.GetText(i.Key) == i.Value);

            //Assert
            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have any text When I add in the line Then I should have an exception by invalid column.")]
        [Trait("LineBuilder", "AddText(int column, string text)")]
        public void Should_Be_Add_Text_In_Specific_Column_Exception_Column()
        {
            //Arrange
            var line = new LineBuilder();
            var text = FakerPrimitiveTypes.GetSampleText(1, 100);

            //Act
            Action act = () => line.AddText(0, text);

            //Assert
            act.Should().Throw<Exception>().WithMessage("Invalid column position. Minimum value it's 1.");
        }

        [Fact(DisplayName = "Given I have any text When I add in the line Then I should have added the text on the specific column.")]
        [Trait("LineBuilder", "AddText(int column, string text)")]
        public void Should_Be_Add_Text_In_Specific_Column_Success()
        {
            //Arrange
            var line = new LineBuilder();
            var text = FakerPrimitiveTypes.GetSampleText(0, 10000);
            line.AddText(2, text);

            //Act
            var validateTest = line.GetText(2) == text;

            //Assert
            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have any text When I add in the line Then I should have added the text on the current column.")]
        [Trait("LineBuilder", "AddText(string text)")]
        public void Should_Be_Add_Text_Success()
        {
            //Arrange
            var line = new LineBuilder();
            var text = FakerPrimitiveTypes.GetSampleText(0, 10000);
            line.AddText(text);

            //Act
            var validateTest = line.GetText(1) == text;

            //Assert
            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have some texts or only a text When I add in the line Then I should have added the texts sequentially with the word specific before and after.")]
        [Trait("LineBuilder", "AddTextsConcat(string concatBefore, string concatAfter, params string[] texts)")]
        public void Should_Be_Add_Texts_Concat_Sequentially_Success()
        {
            //Arrange
            var line = new LineBuilder();

            var firstText = FakerPrimitiveTypes.GetSampleText(0, 10000);
            var secondText = FakerPrimitiveTypes.GetSampleText(0, 10000);

            line.AddTextsConcat("before-", "-after", firstText, secondText);

            //Act
            var validateTest = line.GetCurrentPosition() == 2
                                && line.GetPosition($"before-{firstText}-after") == 1
                                && line.GetPosition($"before-{secondText}-after") == 2;

            //Assert
            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a list of texts When I add in the line Then I should have added the texts sequentially.")]
        [Trait("LineBuilder", "AddTexts(IEnumerable<string> texts)")]
        public void Should_Be_Add_Texts_Sequentially_Using_List_Success()
        {
            //Arrange
            var line = new LineBuilder();

            IEnumerable<string> texts = new List<string>()
            {
                FakerPrimitiveTypes.GetSampleText(0, 10000),
                FakerPrimitiveTypes.GetSampleText(0, 10000),
                FakerPrimitiveTypes.GetSampleText(0, 10000),
                FakerPrimitiveTypes.GetSampleText(0, 10000)
            };

            line.AddTexts(texts);

            //Act
            var validateTest = line.GetCurrentPosition() == texts.Count()
                                && texts.Select((text, index) => new { index, text })
                                        .All(data => data.index + 1 == line.GetPosition(data.text));

            //Assert
            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have some texts or only a text When I add in the line Then I should have added the texts sequentially.")]
        [Trait("LineBuilder", "AddTexts(params string[] texts)")]
        public void Should_Be_Add_Texts_Sequentially_Using_Params_Success()
        {
            //Arrange
            var line = new LineBuilder();

            var firstText = FakerPrimitiveTypes.GetSampleText(0, 10000);
            var secondText = FakerPrimitiveTypes.GetSampleText(0, 10000);

            line.AddTexts(firstText, secondText);

            //Act
            var validateTest = line.GetCurrentPosition() == 2
                                && line.GetPosition(firstText) == 1
                                && line.GetPosition(secondText) == 2;

            //Assert
            validateTest.Should().BeTrue();
        }

        [Theory(DisplayName = "Given I have a line with texts added When I build it Then I should have a string with all texts added separated by separator used.")]
        [Trait("LineBuilder", "BuildLine()")]
        [InlineData(" ")]
        [InlineData(";")]
        [InlineData("\t")]
        public void Should_Be_Building_Line_With_Success(string separator)
        {
            //Arrange
            var line = new LineBuilder();
            line.AddTexts("Working", "with", "files", "using", "File", "Builder");

            //Act
            var validateTest = line.BuildLine(separator);

            //Assert
            validateTest.Should().Be($"Working{separator}with{separator}files{separator}using{separator}File{separator}Builder");
        }

        [Theory(DisplayName = "Given I have a line with texts that have break line added When I build it Then I should have a string with all texts added separated by separator used, but without break line.")]
        [Trait("LineBuilder", "BuildLine()")]
        [InlineData(" ")]
        [InlineData(";")]
        [InlineData("\t")]
        public void Should_Be_Building_Line_With_Success_Without_Break_Line(string separator)
        {
            //Arrange
            var line = new LineBuilder();
            line.AddTexts("Workin\r\ng", "with\r\n", "\nfiles", "us\ning", "\r\nFile", "Builder\n");

            //Act
            var validateTest = line.BuildLine(separator);

            //Assert
            validateTest.Should().Be($"Working{separator}with{separator}files{separator}using{separator}File{separator}Builder");
        }

        [Fact(DisplayName = "Given I have a line When I clone it Then I should have two lines equals.")]
        [Trait("LineBuilder", "Clone()")]
        public void Should_Be_Cloned()
        {
            //Arrange
            var line = new LineBuilder();

            var firstText = "Captain America";
            var secondText = "Iron Main";

            line.AddTexts(firstText, secondText);

            var clone = line.Clone();

            //Act
            var validateTest = line.GetCurrentPosition() == clone.GetCurrentPosition()
                                && line.GetPosition(firstText) == clone.GetPosition(firstText)
                                && line.GetPosition(secondText) == clone.GetPosition(secondText);

            //Assert
            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a new line When I check it Then I should have a line with current position 1")]
        [Trait("LineBuilder", "LineBuilder()")]
        public void Should_Be_Current_Position_1_Without_Parameter()
        {
            //Arrange
            var line = new LineBuilder();

            //Assert
            line.GetCurrentPosition().Should().Be(1);
        }

        [Theory(DisplayName = "Given I have a new line with texts When I check it Then I should have a line with current position equal amount of added texts.")]
        [Trait("LineBuilder", "LineBuilder()")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void Should_Be_Current_Position_Equal_Amount_Of_Texts(int amount)
        {
            //Arrange
            var texts = new List<(int, string)>();

            for (int i = 1; i <= amount; i++)
                texts.Add((i, FakerPrimitiveTypes.GetSampleText(10)));

            var line = new LineBuilder(texts);

            //Assert
            line.GetCurrentPosition().Should().Be(amount);
        }

        [Fact(DisplayName = "Given I have a line with texts When I reset current position Then I should have the column position equal to one.")]
        [Trait("LineBuilder", "ResetCurrentPosition()")]
        public void Should_Be_One_Current_Position()
        {
            //Arrange
            var line = new LineBuilder();

            var texts = new List<string>()
            {
                $"Iron Man",
                $"Captain Marvel",
                $"Captain America",
                $"Thor",
                $"Black Panther",
                $"Hulk"
            };

            line.AddTexts(texts);

            //Act
            line.ResetCurrentPosition();

            //Assert
            line.GetCurrentPosition().Should().Be(1);
        }

        [Theory(DisplayName = "Given I have some texts When I replace Then I should have new texts in place of the old texts")]
        [Trait("LineBuilder", "ReplaceAll()")]
        [InlineData("Captain", "Iron Man")]
        [InlineData(" ", ";")]
        public void Should_Be_Replaced_All_Old_Text_With_Another(string oldText, string newText)
        {
            //Arrange
            var line = new LineBuilder();

            line.AddText(oldText);
            line.AddText(oldText);

            //Act
            line.ReplaceAll(oldText, newText);

            //Assert
            var validate = line.GetText(1) == newText &&
                            line.GetText(2) == newText;

            validate.Should().BeTrue();
        }

        [Theory(DisplayName = "Given I have added texts with any term specific When I replace where contains this term Then I should have a new text in place of the old text")]
        [Trait("LineBuilder", "ReplaceAllContains()")]
        [InlineData("{{BEFORE}}")]
        [InlineData("Heroes")]
        public void Should_Be_Replaced_All_Text_With_Terms(string term)
        {
            //Arrange
            var line = new LineBuilder();

            var texts = new List<(int Position, string Value)>()
            {
                (1, $"{term} Iron Man"),
                (2, $"Captain Marvel"),
                (3, $"{term} Captain America"),
                (4, $"{term} Thor"),
                (5, $"Black Panther"),
                (6, $"{term} Hulk")
            };

            line.AddTexts(texts.Select(t => t.Value));

            //Act
            line.ReplaceAllContains(term, "Marvel Studio Hero");

            //Assert
            for (int i = 1; i <= texts.Count; i++)
            {
                var withoutTerm = new int[2] { 2, 5 };

                if (withoutTerm.Contains(i))
                    line.GetText(i).Should().Be(texts.FirstOrDefault(t => t.Position == i).Value);
                else
                    line.GetText(i).Should().Be("Marvel Studio Hero");
            }
        }
        [Theory(DisplayName = "Given I have any text When I replace Then I should have a new text in place of the old text")]
        [Trait("LineBuilder", "Replace()")]
        [InlineData("Captain", "Iron Man")]
        [InlineData(" ", ";")]
        public void Should_Be_Replaced_One_Text_With_Another(string oldText, string newText)
        {
            //Arrange
            var line = new LineBuilder();

            line.AddText(oldText);

            //Act
            line.Replace(oldText, newText);

            //Assert
            line.GetText(1).Should().Be(newText);
        }
        [Theory(DisplayName = "Given I have a line When I add texts Then I should have the current position equals to amount texts added.")]
        [Trait("LineBuilder", "GetCurrentPosition()")]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(100)]
        public void Should_Be_The_Current_Position(int amount)
        {
            //Arrange
            var line = new LineBuilder();

            var texts = new List<string>();

            for (int i = 1; i <= amount; i++)
                texts.Add(FakerPrimitiveTypes.GetSampleText(20));

            line.AddTexts(texts);

            //ACT
            var validateTest = line.GetCurrentPosition() == amount;

            //Assert
            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a line with texts When I get position of one text inexistent Then I should have the position -1.")]
        [Trait("LineBuilder", "GetPosition()")]
        public void Should_Be_The_Position_Of_The_Inexistent_Text()
        {
            //Arrange
            var line = new LineBuilder();

            var texts = new List<string>()
            {
                $"Iron Man",
                $"Captain Marvel",
                $"Captain America",
                $"Thor",
                $"Black Panther",
                $"Hulk"
            };

            line.AddTexts(texts);

            //Assert
            line.GetPosition("xpto").Should().Be(-1);
        }

        [Fact(DisplayName = "Given I have a line with texts When I get position of one text Then I should have a correct position.")]
        [Trait("LineBuilder", "GetPosition()")]
        public void Should_Be_The_Position_Of_The_Specific_Text()
        {
            //Arrange
            var line = new LineBuilder();

            var texts = new List<(int Position, string Value)>()
            {
                (1, $"Iron Man"),
                (2, $"Captain Marvel"),
                (3, $"Captain America"),
                (4, $"Thor"),
                (5, $"Black Panther"),
                (6, $"Hulk")
            };

            line.AddTexts(texts.Select(t => t.Value));

            //Assert
            for (int i = 1; i <= texts.Count; i++)
                line.GetPosition(texts.FirstOrDefault(t => t.Position == i).Value).Should().Be(i);
        }
        [Theory(DisplayName = "Given I have a line with texts and current position is zero When I get a next column Then I should have the text that column.")]
        [Trait("LineBuilder", "NextText()")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Be_The_Text_Of_The_Next_Column_Available_Filled(int initialPosition)
        {
            //Arrange
            var texts = new List<(int, string)>()
            {
                (0, "Iron Man"),
                (1, "Captain Marvel"),
                (2, "Captain America"),
                (3, "Thor")
            };

            var line = new LineBuilder(texts, initialPosition);

            //Assert
            for (int i = initialPosition; i < texts.Count; i++)
                line.NextText().Should().Be(texts[i].Item2);
        }

        [Fact(DisplayName = "Given I have a line with texts When I specify a column position Then I should have the text that column.")]
        [Trait("LineBuilder", "GetText()")]
        public void Should_Be_The_Text_Of_The_Specific_Column()
        {
            //Arrange
            var line = new LineBuilder();

            var texts = new List<(int Position, string Value)>()
            {
                (1, $"Iron Man"),
                (2, $"Captain Marvel"),
                (3, $"Captain America"),
                (4, $"Thor"),
                (5, $"Black Panther"),
                (6, $"Hulk")
            };

            line.AddTexts(texts.Select(t => t.Value));

            //Assert
            for (int i = 1; i <= texts.Count; i++)
                line.GetText(i).Should().Be(texts.FirstOrDefault(t => t.Position == i).Value);
        }
    }
}