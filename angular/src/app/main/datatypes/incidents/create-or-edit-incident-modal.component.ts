import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { IncidentsServiceProxy, CreateDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';


@Component({
    selector: 'createOrEditIncidentModal',
    templateUrl: './create-or-edit-incident-modal.component.html'
})
export class CreateOrEditIncidentModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    incident: CreateDto = new CreateDto();

    constructor(
        injector: Injector,
       private _incidentsServiceProxy: IncidentsServiceProxy
    ) {
        super(injector);
    }

    show(incidentId?: number): void {
        if (!incidentId) {
            this.incident = new CreateDto();
            this.incident.id = incidentId;

            this.active = true;
            this.modal.show();
        } else {
            // this._incidentsServiceProxy.getIncidentForEdit(incidentId).subscribe(result => {
            //     this.incident = result;
            //     this.active = true;
            //     this.modal.show();
            // });
        }
    }

    save(): void {
            this.saving = true;
            this._incidentsServiceProxy.create(this.incident)
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
