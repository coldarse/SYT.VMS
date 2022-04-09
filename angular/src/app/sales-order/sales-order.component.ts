import { formatDate } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { ExportExcelService, SaleDto, SaleDtoPagedResultDto, SaleServiceProxy, TenantDto, TenantDtoPagedResultDto, TenantServiceProxy } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { finalize } from 'rxjs/operators';

class PagedSalesRequestDto extends PagedRequestDto {
  keyword: string;
  tenantId: number;
  fromDate: string;
  toDate: string;
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
  fromDate = new Date();
  toDate = new Date();
  dataForExcel = [];

  excelSalesData: any[] = [];


  constructor(
    injector: Injector,
    private _saleService: SaleServiceProxy,
    private _tenantService: TenantServiceProxy,
    public ete: ExportExcelService,
  ) {
    super(injector);
   }

  ngOnInit(): void {
    if(this.appSession.tenantId == null) this.isHost = true;
  }

  generateReport(){
    this._saleService
      .getDataForReport(
        this.keyword,
        this.appSession.tenantId == null ? 1 : this.appSession.tenantId,
        formatDate(this.fromDate, 'yyyy-MM-dd', 'en'),
        formatDate(this.toDate, 'yyyy-MM-dd', 'en')
      )
      .subscribe((result: SaleDto[]) => {
        this.excelSalesData = result;
        if(this.sales.length != 0){
          this.sales.forEach((row: any) => {
            this.dataForExcel.push({
              'Id': row.id,
              'Vending Machine': row.vendingMachine,
              'Item Code': row.itemCode,
              'Order Time': formatDate(row.orderTime, 'yyyy-MM-dd hh:mm:ss', 'en')
            });
          })

          let valueForExcel = [];

          this.dataForExcel.forEach((row: any) => {
            valueForExcel.push(Object.values(row));
          })


          let reportData = {
            title: 'Order Sales Report - ' + formatDate(new Date(), 'yyyy-MM-dd', 'en'),
            data: valueForExcel,
            headers: Object.keys(this.dataForExcel[0])
          }

          this.ete.exportExcel(reportData);
        }
        else{
          this.notify.info("Please filter records to generate report.");
        }
      });

  }


  protected list(
    request: PagedSalesRequestDto,
    pageNumber: number,
    finishedCallback: Function
    ): void {
      request.keyword = this.keyword;
      request.tenantId = this.appSession.tenantId == null ? 1 : this.appSession.tenantId;
      request.fromDate = formatDate(this.fromDate, 'yyyy-MM-dd', 'en');
      request.toDate = formatDate(this.toDate, 'yyyy-MM-dd', 'en');

      if(this.isHost){
        this._tenantService
        .getAll("", undefined, 0, 10)
        .subscribe((res: TenantDtoPagedResultDto) => {
          this.tenants = res.items;
          this._saleService
          .getAll(
            request.keyword,
            request.tenantId,
            request.fromDate,
            request.toDate
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
          request.tenantId,
          request.fromDate,
          request.toDate
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
