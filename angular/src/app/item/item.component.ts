import { Component, Injector, OnInit } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { ItemDto, ItemDtoPagedResultDto, ItemServiceProxy, TenantDto, TenantDtoPagedResultDto, TenantServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CreateUpdateItemComponent } from './create-update-item/create-update-item.component';


class PagedItemsRequestDto extends PagedRequestDto {
  keyword: string;
  tenantId: number;
}

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent extends PagedListingComponentBase<ItemDto> {

  keyword = "";
  items: any[] = [];
  isHost = false;
  tenants: TenantDto[] = [];

  constructor(
    injector: Injector,
    private _itemService: ItemServiceProxy,
    private _tenantService: TenantServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
    if(this.appSession.tenantId == null) this.isHost = true;
  }

  createItem(){
    this.showCreateOrEditItemDialog();
  }

  editItem(entity: ItemDto){
    this.showCreateOrEditItemDialog(entity.id, entity.vendingMachine);
  }

  private showCreateOrEditItemDialog(id?: number, name?: string): void {
    let createOrEditUserDialog: BsModalRef;
    if (!id) {
      createOrEditUserDialog = this._modalService.show(
        CreateUpdateItemComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditUserDialog = this._modalService.show(
        CreateUpdateItemComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
            name: name,
          },
        }
      );
    }

    createOrEditUserDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }

  clearFilters(): void {
    this.keyword = '';
    this.getDataPage(1);
  }

  protected list(
    request: PagedItemsRequestDto,
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
          this._itemService
          .getAll(
            request.keyword,
            request.tenantId
          ).pipe(
            finalize(() => {
              finishedCallback();
            })
          )
          .subscribe((result: ItemDtoPagedResultDto) => {
            this.items = [];
            result.items.forEach((element: ItemDto) => {
              let tempTenant = "";
              this.tenants.forEach((elem: TenantDto) => {
                if(elem.id == element.tenantId) tempTenant = elem.tenancyName;
              });

              let tempVendingMachine = {
                id: element.id,
                vendingMachine: element.vendingMachine,
                itemCode: element.itemCode,
                tenant: tempTenant,
                tenantId: element.tenantId,
              }

              this.items.push(tempVendingMachine);
            });
            this.showPaging(result, pageNumber);
          });
        });
      }
      else{
        this._itemService
        .getAll(
          request.keyword,
          request.tenantId
        ).pipe(
          finalize(() => {
            finishedCallback();
          })
        )
        .subscribe((result: ItemDtoPagedResultDto) => {
          this.items = [];
            result.items.forEach((element: ItemDto) => {

              let tempVendingMachine = {
                id: element.id,
                vendingMachine: element.vendingMachine,
                itemCode: element.itemCode
              }

              this.items.push(tempVendingMachine);
            });
            this.showPaging(result, pageNumber);
        });
      }
  }
  
  protected delete(entity: ItemDto): void {
    abp.message.confirm(
      this.l('UserDeleteWarningMessage', entity.itemCode),
      undefined,
      (result: boolean) => {
        if (result) {
          this._itemService.delete(entity.id).subscribe(() => {
            abp.notify.success(this.l('SuccessfullyDeleted'));
            this.refresh();
          });
        }
      }
    );
  }

  isButtonVisible(action: string): boolean {
    return this.permission.isGranted("Pages.Item." + action);
  }

}
