import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { SensorsServiceProxy, CreateOrEditSensorDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';


@Component({
    selector: 'createOrEditSensorModal',
    templateUrl: './create-or-edit-sensor-modal.component.html'
})
export class CreateOrEditSensorModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
	

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    sensor: CreateOrEditSensorDto = new CreateOrEditSensorDto();
	

    constructor(
        injector: Injector,
        private _sensorsServiceProxy: SensorsServiceProxy
    ) {
        super(injector);
    }

    show(sensorId?: number): void {
        if (!sensorId) { 
			this.sensor = new CreateOrEditSensorDto();
			this.sensor.id = sensorId;
			
			this.active = true;
			this.modal.show();
        }
		else{
			this._sensorsServiceProxy.getSensorForEdit(sensorId).subscribe(result => {
				this.sensor = result.sensor;
				
				this.active = true;
				this.modal.show();
			});
		}  
    }

    save(): void {
			this.saving = true;
			this._sensorsServiceProxy.createOrEdit(this.sensor)
			 .pipe(finalize(() => { this.saving = false; }))
			 .subscribe(() => {
			    this.notify.info(this.l('SavedSuccessfully'));
				this.close();
				this.modalSave.emit(null);
             });
    }

	

	

	

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}