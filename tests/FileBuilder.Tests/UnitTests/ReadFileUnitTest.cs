using FileBuilder.Core;
using FileBuilder.Core.Mapping;
using FileBuilder.Tests.Faker.Entities;
using FileBuilder.Tests.Faker.EntitiesMapping;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        private Stream LoadFakerFile(string fileName)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var filesDirectory = Path.Combine(projectDirectory, "Faker", "Files");

            return new FileStream(Path.Combine(filesDirectory, fileName), FileMode.OpenOrCreate);
        }
    }
}
