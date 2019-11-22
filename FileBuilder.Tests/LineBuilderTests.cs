using FileBuilder.Core;
using FileBuilder.Tests.Faker;
using FluentAssertions;
using Xunit;

namespace FileBuilder.Tests
{
    public class LineBuilderTests
    {
        [Fact(DisplayName = "Give a text When call AddText() Then should be added a new text on current column")]
        [Trait("LineBuilder", "Unit")]
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
    }
}