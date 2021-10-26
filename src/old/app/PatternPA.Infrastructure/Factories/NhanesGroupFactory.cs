using PatternPA.Core.Interfaces.Factories;
using PatternPA.Core.Model;
using PatternPA.Infrastructure.Services;

namespace PatternPA.Infrastructure.Factories
{
    public class NhanesGroupFactory : IGroupFactory
    {
        public Group Create(string folderPath)
        {
            var group = new Group(folderPath);
            var service = new NhanesGroupService();
            service.Initialize(group);

            return group;
        }
    }
}