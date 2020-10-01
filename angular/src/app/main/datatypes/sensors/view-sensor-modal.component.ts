import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetSensorForView, SensorDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewSensorModal',
    templateUrl: './view-sensor-modal.component.html'
})
export class ViewSensorModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item : GetSensorForView;
	

    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetSensorForView();
        this.item.sensor = new SensorDto();
    }

    show(item: GetSensorForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
