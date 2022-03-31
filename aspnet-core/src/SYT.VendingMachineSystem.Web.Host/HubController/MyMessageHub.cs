using Abp.Domain.Repositories;
using Abp.Domain.Uow;
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
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public MyMessageHub(IRepository<VendingMachine> VendingMachineRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _VendingMachineRepository = VendingMachineRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Someone Connected");
            return base.OnConnectedAsync();
        }

        public async Task isShutDown(string vmName)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                VendingMachine tempVM = _VendingMachineRepository.FirstOrDefault(x => x.Name.ToUpper() == vmName.ToUpper());
                await Clients.Client(this.Context.ConnectionId).SendAsync("askIsShutDown", tempVM.Restart);

                var tid = tempVM.TenantId;
                var restart = tempVM.Restart;

                if (tempVM.Restart)
                {
                    tempVM.Restart = false;
                }
                else
                {
                    tempVM.Restart = restart;
                }

                tempVM.TenantId = tid;
                tempVM.lastUpdatedTime = DateTime.Now;
                await _VendingMachineRepository.UpdateAsync(tempVM);

                unitOfWork.Complete();
            }
        }
    }
}
