using FileBuilder.Core.Mapping;
using FileBuilder.Tests.Faker.Entities;

namespace FileBuilder.Tests.Faker.EntitiesMapping
{
    public class PersonMapping : EntityMapping<Person>
    {
        public PersonMapping()
        {
            MapProperty("nome", p => p.FirstName);
            MapProperty("sobrenome", p => p.LastName);
            MapProperty("email", p => p.Mail);
        }
    }
}