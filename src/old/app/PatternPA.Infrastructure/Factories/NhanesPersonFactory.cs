using System;
using PatternPA.Core.Interfaces.Factories;
using PatternPA.Core.Model;
using PatternPA.Infrastructure.Services;

namespace PatternPA.Infrastructure.Factories
{
    public class NhanesPersonFactory : IPersonFactory
    {
        public Person Create(string filePath)
        {
            var person = new Person(filePath);
            var personService = new NhanesPersonService();
            personService.Initialize(person);

            return person;
        }
    }
}