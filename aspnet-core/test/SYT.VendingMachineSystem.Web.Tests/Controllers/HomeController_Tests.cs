using System.Threading.Tasks;
using SYT.VendingMachineSystem.Models.TokenAuth;
using SYT.VendingMachineSystem.Web.Controllers;
using Shouldly;
using Xunit;

namespace SYT.VendingMachineSystem.Web.Tests.Controllers
{
    public class HomeController_Tests: VendingMachineSystemWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}