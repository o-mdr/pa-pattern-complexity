using PatternPA.Core.Interfaces.Factories;
using PatternPA.Core.Model;
using PatternPA.Infrastructure.Services;

namespace PatternPA.Infrastructure.Factories
{
    public class GroupFactory : IGroupFactory
    {
        public Group Create(string folderPath)
        {
            var group = new Group(folderPath);
            var service = new GroupService();
            service.Initialize(group);

            return group;
        }
    }
}
