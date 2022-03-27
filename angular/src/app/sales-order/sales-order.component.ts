import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { SaleDto, SaleDtoPagedResultDto, SaleServiceProxy, TenantDto, TenantDtoPagedResultDto, TenantServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

class PagedSalesRequestDto extends PagedRequestDto {
  keyword: string;
  tenantId: number;
}


@Component({
  selector: 'app-sales-order',
  templateUrl: './sales-order.component.html',
  styleUrls: ['./sales-order.component.css'],
  animations: [appModuleAnimation()]
})
export class SalesOrderComponent extends PagedListingComponentBase<SaleDto> {

  keyword = "";
  sales: any[] = [];
  isHost = false;
  tenants: TenantDto[] = [];

  constructor(
    injector: Injector,
    private _saleService: SaleServiceProxy,
    private _tenantService: TenantServiceProxy,
  ) {
    super(injector);
   }

  ngOnInit(): void {
    if(this.appSession.tenantId == null) this.isHost = true;
  }

  protected list(
    request: PagedSalesRequestDto,
    pageNumber: number,
    finishedCallback: Function
    ): void {
      request.keyword = this.keyword;
      request.tenantId = this.appSession.tenantId == null ? 1 : this.appSession.tenantId;

      if(this.isHost){
        this._tenantService
        .getAll("", undefined, 0, 10)
        .subscribe((res: TenantDtoPagedResultDto) => {
          this.tenants = res.items;
          this._saleService
          .getAll(
            request.keyword,
            request.tenantId
          ).pipe(
            finalize(() => {
              finishedCallback();
            })
          )
          .subscribe((result: SaleDtoPagedResultDto) => {
            this.sales = [];
            result.items.forEach((element: SaleDto) => {
              let tempTenant = "";
              this.tenants.forEach((elem: TenantDto) => {
                if(elem.id == element.tenantId) tempTenant = elem.tenancyName;
              });

              let tempActivityLog = {
                id: element.id,
                vendingMachine: element.vendingMachine,
                itemCode: element.itemCode,
                orderTime: element.orderTime,
                tenant: tempTenant
              }

              this.sales.push(tempActivityLog);
            });
            this.showPaging(result, pageNumber);
          });
        });
      }
      else{
        this._saleService
        .getAll(
          request.keyword,
          request.tenantId
        ).pipe(
          finalize(() => {
            finishedCallback();
          })
        )
        .subscribe((result: SaleDtoPagedResultDto) => {
          this.sales = result.items
          this.showPaging(result, pageNumber);
        });
      }
  }

  protected delete(entity: SaleDto): void {
    throw new Error('Method not implemented.');
  }

}
