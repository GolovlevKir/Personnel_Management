using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Personal_Management.Hubs
{
    [HubName("employeesHub")]
    public class EmployeesHub : Hub
    {
        public static void BroadcastData()
        {
            //обновление у всех пользователей
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<EmployeesHub>();
            context.Clients.All.refreshEmployeeData();
        }
    }
}