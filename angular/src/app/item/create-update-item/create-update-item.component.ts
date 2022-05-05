import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ItemDto, ItemServiceProxy, VendingMachine, VendingMachineDto, VendingMachineServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

import * as moment from 'moment';

@Component({
  selector: 'app-create-update-item',
  templateUrl: './create-update-item.component.html',
  styleUrls: ['./create-update-item.component.css']
})
export class CreateUpdateItemComponent extends AppComponentBase
 implements OnInit {

  saving = false;
  item = new ItemDto();
  id: number;
  name: string;
  isHost = false;
  isCreate = true;
  isVMLoaded = false;
  vmList: VendingMachineDto[] = [];
  vmName = "";

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _itemService: ItemServiceProxy,
    public _vendingMachineService: VendingMachineServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  selectedVM(event: any){
    this.vmList.forEach((element: VendingMachineDto) => {
      if(event == element.id){
        this.item.vendingMachine = element.name;
        this.item.tenantId = element.tenantId;
      }
    })

  }


  ngOnInit(): void {
    let tenantId = 0;
    if(this.appSession.tenantId == null) {
      this.isHost = true;
    }
    else{
      tenantId = this.appSession.tenantId;
    }

    this._vendingMachineService.getListOfVendingMachine(tenantId, this.isHost)
      .subscribe((result: VendingMachine[]) => {
        this.vmList = result;
        this.isVMLoaded = true;
      })

    if(this.id != undefined){
      this.isCreate = false;
      this._itemService.get(this.id).subscribe((result) => {
        this.item = result;
        this.vmName = result.vendingMachine;
      });
    }
  }

  save(): void {

    this.saving = true;


    // this.item.tenantId = this.isHost == true ? 1 : this.appSession.tenantId;

    if(this.id != undefined){
      this._itemService.update(this.item).subscribe(
        () => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.bsModalRef.hide();
          this.onSave.emit();
        },
        () => {
          this.saving = false;
        }
      );
    }
    else{
      this._itemService.create(this.item).subscribe(
        () => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.bsModalRef.hide();
          this.onSave.emit();
        },
        () => {
          this.saving = false;
        }
      );
    }

  }

}
