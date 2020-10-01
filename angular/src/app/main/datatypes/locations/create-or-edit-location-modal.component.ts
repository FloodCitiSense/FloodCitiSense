import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { LocationsServiceProxy, CreateOrEditLocationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';


@Component({
    selector: 'createOrEditLocationModal',
    templateUrl: './create-or-edit-location-modal.component.html'
})
export class CreateOrEditLocationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    location: CreateOrEditLocationDto = new CreateOrEditLocationDto();

    constructor(
        injector: Injector,
        private _locationsServiceProxy: LocationsServiceProxy
    ) {
        super(injector);
    }

    show(locationId?: number): void {
        if (!locationId) {
            this.location = new CreateOrEditLocationDto();
            this.location.id = locationId;

            this.active = true;
            this.modal.show();
        } else {
            this._locationsServiceProxy.getLocationForEdit(locationId).subscribe(result => {
                //this.location = result;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;
            this._locationsServiceProxy.createOrEdit(this.location)
             .finally(() => { this.saving = false; })
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
