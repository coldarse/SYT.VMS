import { formatDate } from '@angular/common';
import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { VendingMachineDto, VendingMachineServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';


import * as moment from 'moment';

@Component({
  selector: 'app-create-update-vending-machine',
  templateUrl: './create-update-vending-machine.component.html',
  styleUrls: ['./create-update-vending-machine.component.css']
})
export class CreateUpdateVendingMachineComponent extends AppComponentBase
 implements OnInit {

  saving = false;
  vendingMachine = new VendingMachineDto();
  id: number;
  isHost = false;
  isCreate = true;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _vendingMachineService: VendingMachineServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if(this.appSession.tenantId == null) this.isHost = true;

    if(this.id != undefined){
      this.isCreate = false;
      this._vendingMachineService.get(this.id).subscribe((result) => {
        this.vendingMachine = result;
      });
    }
  }

  save(): void {
    this.saving = true;

    this.vendingMachine.status = this.vendingMachine.status == undefined ? false : this.vendingMachine.status;
    // this.vendingMachine.lastUpdatedTime = moment(new Date());
    this.vendingMachine.tenantId = this.isHost == true ? 1 : this.appSession.tenantId;
    if(!this.isHost) this.vendingMachine.isSubscribed = this.vendingMachine.isSubscribed == undefined ? true : this.vendingMachine.isSubscribed;

    if(this.id != undefined){
      this._vendingMachineService.update(this.vendingMachine).subscribe(
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
      this._vendingMachineService.create(this.vendingMachine).subscribe(
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
