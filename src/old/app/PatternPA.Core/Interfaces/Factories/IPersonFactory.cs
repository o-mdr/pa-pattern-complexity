using PatternPA.Core.Model;

namespace PatternPA.Core.Interfaces.Factories
{
    public interface IPersonFactory
    {
        Person Create(string filePath);
    }
}