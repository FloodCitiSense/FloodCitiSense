import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { IncidentApprovalsServiceProxy, CreateOrEditIncidentApprovalDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';


@Component({
    selector: 'createOrEditIncidentApprovalModal',
    templateUrl: './create-or-edit-incidentApproval-modal.component.html'
})
export class CreateOrEditIncidentApprovalModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    incidentApproval: CreateOrEditIncidentApprovalDto = new CreateOrEditIncidentApprovalDto();

    constructor(
        injector: Injector,
        private _incidentApprovalsServiceProxy: IncidentApprovalsServiceProxy
    ) {
        super(injector);
    }

    show(incidentApprovalId?: number): void {
        if (!incidentApprovalId) {
            this.incidentApproval = new CreateOrEditIncidentApprovalDto();
            this.incidentApproval.id = incidentApprovalId;

            this.active = true;
            this.modal.show();
        } else {
            this._incidentApprovalsServiceProxy.getIncidentApprovalForEdit(incidentApprovalId).subscribe(result => {
                //this.incidentApproval = result;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;
            this._incidentApprovalsServiceProxy.createOrEdit(this.incidentApproval)
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
