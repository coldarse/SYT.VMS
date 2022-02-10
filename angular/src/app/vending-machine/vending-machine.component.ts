import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
  VendingMachineServiceProxy,
  VendingMachineDto,
  VendingMachineDtoPagedResultDto,
  TenantServiceProxy,
  TenantDto,
  TenantDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateUpdateVendingMachineComponent } from './create-update-vending-machine/create-update-vending-machine.component';

class PagedVendingMachinesRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
  tenantId: number;
}

@Component({
  selector: 'app-vending-machine',
  templateUrl: './vending-machine.component.html',
  styleUrls: ['./vending-machine.component.css'],
  animations: [appModuleAnimation()]
})
export class VendingMachineComponent extends PagedListingComponentBase<VendingMachineDto> {


  keyword = "";
  isActive: boolean | null;
  advancedFiltersVisible = false;
  vendingMachines: any[] = [];
  isHost = false;
  tenants: TenantDto[] = [];

  constructor(
    injector: Injector,
    private _vendingMachineService: VendingMachineServiceProxy,
    private _tenantService: TenantServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if(this.appSession.tenantId == null) this.isHost = true;
  }

  createVendingMachine(){
    this.showCreateOrEditVendingMachineDialog();
  }

  editUser(entity: VendingMachineDto){
    this.showCreateOrEditVendingMachineDialog(entity.id);
  }

  restart(entity: VendingMachineDto){
    entity.restart = true;
    this._vendingMachineService.update(entity).subscribe(
      () => {
        this.notify.info(this.l('SavedSuccessfully'));
      }
    );
  }

  private showCreateOrEditVendingMachineDialog(id?: number): void {
    let createOrEditUserDialog: BsModalRef;
    if (!id) {
      createOrEditUserDialog = this._modalService.show(
        CreateUpdateVendingMachineComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditUserDialog = this._modalService.show(
        CreateUpdateVendingMachineComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
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
    this.isActive = undefined;
    this.getDataPage(1);
  }

  protected list(
    request: PagedVendingMachinesRequestDto,
    pageNumber: number,
    finishedCallback: Function
    ): void {
      request.keyword = this.keyword;
      request.isActive = this.isActive;
      request.tenantId = this.appSession.tenantId == null ? 1 : this.appSession.tenantId;

      if(this.isHost){
        this._tenantService
        .getAll("", undefined, 0, 10)
        .subscribe((res: TenantDtoPagedResultDto) => {
          this.tenants = res.items;
          this._vendingMachineService
          .getAll(
            request.keyword,
            request.isActive,
            request.tenantId
          ).pipe(
            finalize(() => {
              finishedCallback();
            })
          )
          .subscribe((result: VendingMachineDtoPagedResultDto) => {
            this.vendingMachines = [];
            result.items.forEach((element: VendingMachineDto) => {
              let tempTenant = "";
              this.tenants.forEach((elem: TenantDto) => {
                if(elem.id == element.tenantId) tempTenant = elem.tenancyName;
              });

              let tempVendingMachine = {
                id: element.id,
                name: element.name,
                status: element.status,
                address1: element.address1,
                address2: element.address2,
                isSubscribed: element.isSubscribed,
                tenant: tempTenant,
                restart: element.restart
              }

              this.vendingMachines.push(tempVendingMachine);
            });
            this.showPaging(result, pageNumber);
          });
        });
      }
      else{
        this._vendingMachineService
        .getAll(
          request.keyword,
          request.isActive,
          request.tenantId
        ).pipe(
          finalize(() => {
            finishedCallback();
          })
        )
        .subscribe((result: VendingMachineDtoPagedResultDto) => {
          this.vendingMachines = result.items
          this.showPaging(result, pageNumber);
        });
      }



  }

  protected delete(entity: VendingMachineDto): void {
    abp.message.confirm(
      this.l('UserDeleteWarningMessage', entity.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._vendingMachineService.delete(entity.id).subscribe(() => {
            abp.notify.success(this.l('SuccessfullyDeleted'));
            this.refresh();
          });
        }
      }
    );
  }





}
