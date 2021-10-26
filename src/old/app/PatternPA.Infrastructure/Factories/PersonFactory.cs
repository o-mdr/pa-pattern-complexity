using PatternPA.Core.Interfaces.Factories;
using PatternPA.Core.Model;
using PatternPA.Infrastructure.Services;

namespace PatternPA.Infrastructure.Factories
{
    public class PersonFactory : IPersonFactory 
    {
        public Person Create(string filePath)
        {
            var person = new Person(filePath);
            var personService = new PersonService();
            personService.Initialize(person);

            return person;
        }
    }
}
