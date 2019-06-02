using PatternPA.Core.Interfaces.Factories;
using PatternPA.Infrastructure.Factories;
using PatternPA.Infrastructure.Services;

namespace PatternPA.Test.Integration
{
    public abstract class AbstractGroupTest : AbstractIntegrationTest
    {
        protected readonly IGroupFactory groupFactory = new GroupFactory();
        protected readonly GroupService groupService = new GroupService();
        protected readonly IGroupFactory nhanesGroupFactory = new NhanesGroupFactory();
        protected readonly NhanesGroupService nhanesGroupService = new NhanesGroupService();
    }
}
