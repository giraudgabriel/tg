using Project.Infrastructure;

namespace Project.Service.ApplicationService
{
    public class ApplicationService : GenericApplicationService
    {
        public ApplicationService(UnitOfWork uow) : base(uow)
        {
        }
    }
}