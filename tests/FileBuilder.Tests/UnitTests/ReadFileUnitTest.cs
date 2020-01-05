using FileBuilder.Core;
using FileBuilder.Tests.Faker.Entities;
using FileBuilder.Tests.Faker.EntitiesMapping;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace FileBuilder.Tests.UnitTests
{
    public class ReadFileUnitTest
    {
        [Fact(DisplayName = "Given I have a file with header different of the my object When I read line using mapping Then I should have an object based on my mapping.")]
        [Trait("ReadFile", "ReadCurrentLine(mapping)")]
        public void Should_Be_Readed_A_Line_Using_Mapping()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            var personMapping = new PersonMapping();
            var person = readFile.ReadCurrentLine(personMapping);

            //Act
            var validTest = readFile.ReadText("nome") == person.FirstName
                            && readFile.ReadText("sobrenome") == person.LastName
                            && readFile.ReadText("email") == person.Mail;

            //Assert
            validTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a file with header equal of the my object When I read line define my object Then I should have an object.")]
        [Trait("ReadFile", "ReadCurrentLine<T>()")]
        public void Should_Be_Readed_A_Line()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            var pessoa = readFile.ReadCurrentLine<Pessoa>();

            //Act
            var validTest = readFile.ReadText("nome") == pessoa.Nome
                            && readFile.ReadText("sobrenome") == pessoa.Sobrenome
                            && readFile.ReadText("email") == pessoa.Email;

            //Assert
            validTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a file When I read next text of the line Then I should have the text.")]
        [Trait("ReadFile", "ReadNextText()")]
        public void Should_Be_ReadNextText()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            //Act
            var validTest = readFile.ReadNextText() == "Iron" &&
                            readFile.ReadNextText() == "Man" &&
                            readFile.ReadNextText() == "man.iron@marvel.com";

            //Assert
            validTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a file with empty lines When I read file Then I should have ignored empty lines and only consider filled lines.")]
        [Trait("ReadFile", "ReadFile()")]
        public void Should_Be_Readed_Ignore_Empty_Lines_With_Success()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa-With-Empty-Lines.txt"), ";", true, skipEmptyLine: true);

            var numberLinesConsideredInTheFile = 2;

            //Act
            var validateTest = 0;

            while (readFile.NextLine())
                validateTest += 1;

            //Assert
            validateTest.Should().Be(numberLinesConsideredInTheFile);
        }

        [Fact(DisplayName = "Given I have a file with empty lines When I read file Then I should haven't ignored empty lines and consider all lines.")]
        [Trait("ReadFile", "ReadFile()")]
        public void Should_Be_Readed_Not_Ignore_Empty_Lines_With_Success()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa-With-Empty-Lines.txt"), ";", true, skipEmptyLine: false);

            var numberLinesConsideredInTheFile = 3;

            //Act
            var validateTest = 0;

            while (readFile.NextLine())
                validateTest += 1;

            //Assert
            validateTest.Should().Be(numberLinesConsideredInTheFile);
        }

        [Fact(DisplayName = "Given I have a file When I read file Then I should have the current line corretly.")]
        [Trait("ReadFile", "ReadFile()")]
        public void Should_Have_CurrentLine_Corretly()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true, skipEmptyLine: true);

            //Act
            var currentlyLine = readFile.GetCurrentLine();

            //Assert
            while (readFile.NextLine())
            {
                currentlyLine += 1;
                currentlyLine.Should().Be(readFile.GetCurrentLine());
            }
        }

        [Fact(DisplayName = "Given I have a file empty with header When I read file and verify if it is empty consider header Then I should have unsuccess.")]
        [Trait("ReadFile", "Empty()")]
        public void Should_Not_Be_Empty_Consider_Header()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Empty-File-With-Header.txt"), ";", true);

            //Assert
            readFile.IsEmpty(true).Should().BeFalse();
        }

        [Fact(DisplayName = "Given I have a file empty with header When I read file and verify if it is empty without consider header Then I should have success.")]
        [Trait("ReadFile", "Empty()")]
        public void Should_Be_Empty_Disconsider_Header()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Empty-File-With-Header.txt"), ";", true);

            //Assert
            readFile.IsEmpty(false).Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a file with header When I read file and verify if it have header Then I should have success.")]
        [Trait("ReadFile", "HasHeader()")]
        public void Should_Have_Header()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            //Assert
            readFile.HasHeader().Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a file without header When I read file and verify if it have header Then I should have unsuccess.")]
        [Trait("ReadFile", "HasHeader()")]
        public void Should_Havent_Header()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa-Without-Header.txt"), ";", false);

            //Assert
            readFile.HasHeader().Should().BeFalse();
        }

        [Fact(DisplayName = "Given I have a file When I read file and go to next line Then I should have a new line.")]
        [Trait("ReadFile", "NextLine()")]
        public void Should_Have_NewLine()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            //Assert
            readFile.NextLine().Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a file When I am end file and go to next line Then I should have a unsuccess.")]
        [Trait("ReadFile", "NextLine()")]
        public void Should_Havent_NewLine()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            //ACT
            readFile.NextLine();
            readFile.NextLine();

            //Assert
            readFile.NextLine().Should().BeFalse();
        }

        [Fact(DisplayName = "Given I have a file and I am reading a line and When read a text specific the column position Then I should have the text.")]
        [Trait("ReadFile", "ReadText(position)")]
        public void Should_Have_The_Text_Of_Column_Position()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            //ACT
            var name = readFile.ReadText(1);
            var lastname = readFile.ReadText(2);
            var mail = readFile.ReadText(3);

            var validateTest = name == "Iron" &&
                                lastname == "Man" &&
                                mail == "man.iron@marvel.com";
            //Assert
            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a file and I am reading a line and When read a text specific the column position that not exists Then I should have an empty text.")]
        [Trait("ReadFile", "ReadText(position)")]
        public void Should_Have_The_Text_Of_Column_Position_Empty()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            //Assert
            readFile.ReadText(5).Should().Be(string.Empty);
        }

        [Fact(DisplayName = "Given I have a file and I am reading a line and When read a text specific the header name Then I should have the text.")]
        [Trait("ReadFile", "ReadText(headerName)")]
        public void Should_Have_The_Text_Of_Header_Name()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            //ACT
            var name = readFile.ReadText("nome");
            var lastname = readFile.ReadText("sobrenome");
            var mail = readFile.ReadText("email");

            var validateTest = name == "Iron" &&
                                lastname == "Man" &&
                                mail == "man.iron@marvel.com";
            //Assert
            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "Given I have a file and I am reading a line and When read a text specific the header name that not exists Then I should have an empty text.")]
        [Trait("ReadFile", "ReadText(headerName)")]
        public void Should_Have_The_Text_Of_Header_Name_Empty()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            //Assert
            readFile.ReadText("xpto").Should().Be(string.Empty);
        }

        [Fact(DisplayName = "Given I have a file When I reset line position Then I should have the line position equal to one.")]
        [Trait("LineBuilder", "ResetCurrentPosition()")]
        public void Should_Be_Zero_Line_Position()
        {
            //Arrange
            var readFile = new ReadFile(LoadFakerFile("Pessoa.txt"), ";", true);

            //Act
            readFile.ResetLinePosition();

            var pessoa = readFile.ReadCurrentLine<Pessoa>();

            //Act
            var validTest = "Iron" == pessoa.Nome
                            && "Man" == pessoa.Sobrenome
                            && "man.iron@marvel.com" == pessoa.Email;

            //Assert
            validTest.Should().BeTrue();
        }

        private Stream LoadFakerFile(string fileName)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var filesDirectory = Path.Combine(projectDirectory, "Faker", "Files");

            return new FileStream(Path.Combine(filesDirectory, fileName), FileMode.OpenOrCreate);
        }
    }
}