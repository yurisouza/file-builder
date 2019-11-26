using FileBuilder.Core;
using FileBuilder.Core.Mapping;
using FileBuilder.Tests.Faker;
using FileBuilder.Tests.Faker.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace FileBuilder.Tests.UnitTests
{
    public class LineBuilderUnitTest
    {
        [Fact(DisplayName = "Given I have a null text When I add in the line Then I should have added an empty text on the current column.")]
        [Trait("LineBuilder", "AddText()")]
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
        [Trait("LineBuilder", "AddText()")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Be_Add_Text_In_Multiple_Columns_Success(int amountColumns)
        {
            //Arrange
            var line = new LineBuilder();

            var items = new Dictionary<int, string>();

            for (int column = 0; column < amountColumns; column++)
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

        [Fact(DisplayName = "Given I have any text When I add in the line Then I should have added the text on the current column.")]
        [Trait("LineBuilder", "AddText()")]
        public void Should_Be_Add_Text_Success()
        {
            //Arrange
            var line = new LineBuilder();
            var text = FakerPrimitiveTypes.GetSampleText(0, 10000);
            line.AddText(text);

            //Act
            var validateTest = line.GetText(0) == text;

            //Assert
            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a new line When I check it Then I should have a line with current position 0")]
        [Trait("LineBuilder", "AddText()")]
        public void Should_Be_Current_Position_0_Without_Parameter()
        {
            //Arrange
            var line = new LineBuilder();

            //Assert
            line.GetCurrentPosition().Should().Be(0);
        }

        [Theory(DisplayName = "Given I have a new line already with text When I check it Then I should have a line with current position equal amount of added texts minus 1.")]
        [Trait("LineBuilder", "AddText()")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void Should_Be_Current_Position_Equal_Amount_Of_Texts(int amount)
        {
            //Arrange
            var texts = new List<(int, string)>();

            for (int i = 0; i < amount; i++)
                texts.Add((i, FakerPrimitiveTypes.GetSampleText(10)));

            var line = new LineBuilder(texts);

            //Assert
            line.GetCurrentPosition().Should().Be(texts.Count() - 1);
        }

        [Fact]
        public void Testing()
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var filesDirectory = Path.Combine(projectDirectory, "Faker", "Files");

            var aa = new FileStream(Path.Combine(filesDirectory, "Pessoa.txt"), FileMode.OpenOrCreate);
            var file = new ReadFile(aa, ";", true);
            var line = file.ReadCurrentLine<Pessoa>();

            var mapping = new EntityMapping<Person>();
            mapping.MapProperty("nome", p => p.FirstName);
            mapping.MapProperty("sobrenome", p => p.LastName);
            mapping.MapProperty("email", p => p.Mail);

            var perso = mapping.Map(file.ReadCurrentLine());
        }
    }
}