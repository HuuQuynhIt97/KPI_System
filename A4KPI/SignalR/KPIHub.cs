using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.SignalR
{
    public class KPIHub : Hub
    {
        public KPIHub()
        {
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendData(string message)
        {
            await Clients.All.SendAsync("SentMess", message);
        }

        private static int Count = 0;
        public override Task OnConnectedAsync()
        {

            Count++;
            base.OnConnectedAsync();
            Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }

    }
}
