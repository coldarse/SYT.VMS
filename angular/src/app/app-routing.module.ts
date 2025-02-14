import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { VendingMachineComponent } from './vending-machine/vending-machine.component';
import { ActivityLogComponent } from './activity-log/activity-log.component';
import { SalesOrderComponent } from './sales-order/sales-order.component';
import { ItemComponent } from './item/item.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'home', component: HomeComponent,  canActivate: [AppRouteGuard] },
                    { path: 'users', component: UsersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    { path: 'roles', component: RolesComponent, data: { permission: 'Pages.Roles' }, canActivate: [AppRouteGuard] },
                    { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },
                    { path: 'about', component: AboutComponent, canActivate: [AppRouteGuard] },
                    { path: 'update-password', component: ChangePasswordComponent, canActivate: [AppRouteGuard] },
                    { path: 'vending-machine', data: { permission: 'Pages.VendingMachine' }, component: VendingMachineComponent, canActivate: [AppRouteGuard] },
                    { path: 'activity-log', data: { permission: 'Pages.ActivityLog' }, component: ActivityLogComponent, canActivate: [AppRouteGuard] },
                    { path: 'sales-order', data: { permission: 'Pages.SalesOrder' }, component: SalesOrderComponent, canActivate: [AppRouteGuard] },
                    { path: 'item', data: { permission: 'Pages.Item' }, component: ItemComponent, canActivate: [AppRouteGuard] }
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
