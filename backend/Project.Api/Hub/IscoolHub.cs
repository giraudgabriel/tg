using Project.Api.Hub.Interfaces;
using Project.CrossCutting.Base;

namespace Project.Api.Hub
{
    public class IscoolHub : GenericHub<IIscoolHub>
    {
        private const string HubName = "IscoolHub";
        public IscoolHub(IHubConnection hubConnection) : base(hubConnection, HubName)
        {
        }
    }

}
