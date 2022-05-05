using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace SYT.VendingMachineSystem.Authorization
{
    public class VendingMachineSystemAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_ChangePassword, L("UsersChangePassword"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            context.CreatePermission(PermissionNames.Pages_VendingMachine, L("VendingMachine"));
            context.CreatePermission(PermissionNames.Pages_VendingMachine_Create, L("VendingMachineCreate"));
            context.CreatePermission(PermissionNames.Pages_VendingMachine_Edit, L("VendingMachineEdit"));
            context.CreatePermission(PermissionNames.Pages_VendingMachine_Delete, L("VendingMachineDelete"));
            context.CreatePermission(PermissionNames.Pages_ActivityLog, L("ActivityLog"));
            context.CreatePermission(PermissionNames.Pages_SalesOrder, L("SalesOrder"));
            context.CreatePermission(PermissionNames.Pages_SalesOrder_GenerateReport, L("SalesOrderGenerateReport"));
            context.CreatePermission(PermissionNames.Pages_Item, L("Item"));
            context.CreatePermission(PermissionNames.Pages_Item_Create, L("ItemCreate"));
            context.CreatePermission(PermissionNames.Pages_Item_Edit, L("ItemEdit"));
            context.CreatePermission(PermissionNames.Pages_Item_Delete, L("ItemDelete"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, VendingMachineSystemConsts.LocalizationSourceName);
        }
    }
}
