import { formatDate } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { ActivityLogDto, ActivityLogDtoPagedResultDto, ActivityLogServiceProxy, TenantDto, TenantDtoPagedResultDto, TenantServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

class PagedVendingMachinesRequestDto extends PagedRequestDto {
  vendingMachine: string;
  tenantId: number;
  fromDate: string;
  toDate: string;
}

@Component({
  selector: 'app-activity-log',
  templateUrl: './activity-log.component.html',
  styleUrls: ['./activity-log.component.css'],
  animations: [appModuleAnimation()]
})
export class ActivityLogComponent extends PagedListingComponentBase<ActivityLogDto> {

  keyword = "";
  vendingMachine = "";
  activityLogs: any[] = [];
  isHost = false;
  tenants: TenantDto[] = [];
  fromDate = new Date();
  toDate = new Date();

  constructor(
    injector: Injector,
    private _activityLogService: ActivityLogServiceProxy,
    private _tenantService: TenantServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if(this.appSession.tenantId == null) this.isHost = true;
  }

  protected list(
    request: PagedVendingMachinesRequestDto,
    pageNumber: number,
    finishedCallback: Function
    ): void {
      request.vendingMachine = this.keyword;
      request.tenantId = this.appSession.tenantId == null ? 1 : this.appSession.tenantId;
      request.fromDate = formatDate(this.fromDate, 'yyyy-MM-dd', 'en');
      request.toDate = formatDate(this.toDate, 'yyyy-MM-dd', 'en');

      if(this.isHost){
        this._tenantService
        .getAll("", undefined, 0, 10)
        .subscribe((res: TenantDtoPagedResultDto) => {
          this.tenants = res.items;
          this._activityLogService
          .getAll(
            request.vendingMachine,
            request.tenantId,
            request.fromDate,
            request.toDate
          ).pipe(
            finalize(() => {
              finishedCallback();
            })
          )
          .subscribe((result: ActivityLogDtoPagedResultDto) => {
            this.activityLogs = [];
            result.items.forEach((element: ActivityLogDto) => {
              let tempTenant = "";
              this.tenants.forEach((elem: TenantDto) => {
                if(elem.id == element.tenantId) tempTenant = elem.tenancyName;
              });

              let tempActivityLog = {
                id: element.id,
                vendingMachineId: element.vendingMachineId,
                vendingMachineName: element.vendingMachineName,
                activityDescription: element.activityDescription,
                lastUpdatedTime: element.lastUpdatedTime,
                tenant: tempTenant
              }

              this.activityLogs.push(tempActivityLog);
            });
            this.showPaging(result, pageNumber);
          });
        });
      }
      else{
        this._activityLogService
        .getAll(
          request.vendingMachine,
          request.tenantId,
          request.fromDate,
          request.toDate
        ).pipe(
          finalize(() => {
            finishedCallback();
          })
        )
        .subscribe((result: ActivityLogDtoPagedResultDto) => {
          this.activityLogs = result.items
          this.showPaging(result, pageNumber);
        });
      }
  }
  protected delete(entity: ActivityLogDto): void {
    throw new Error('Method not implemented.');
  }

}
