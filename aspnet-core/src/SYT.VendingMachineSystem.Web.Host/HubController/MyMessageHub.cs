using Abp.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SYT.VendingMachineSystem.VendingMachines;

namespace SYT.VendingMachineSystem.Web.Host.HubController
{
    public class MyMessageHub : Hub
    {
        private readonly IRepository<VendingMachine> _VendingMachineRepository;
        public MyMessageHub(IRepository<VendingMachine> VendingMachineRepository)
        {
            _VendingMachineRepository = VendingMachineRepository;
        }
        public async Task isShutDown(string vmName)
        {
            VendingMachine tempVM = _VendingMachineRepository.GetAll().Where(x => x.Name.ToUpper() == vmName.ToUpper()).FirstOrDefault();

            await Clients.Client(this.Context.ConnectionId).SendAsync("askIsShutDown", tempVM.Restart);

            if (tempVM.Restart)
            {
                tempVM.Restart = false;
                await _VendingMachineRepository.UpdateAsync(tempVM);
            }
        }
    }
}
