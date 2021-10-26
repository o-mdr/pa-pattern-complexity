using PatternPA.Core.Model;

namespace PatternPA.Core.Interfaces.Factories
{
    public interface IGroupFactory
    {
        Group Create(string folderPath);
    }
}