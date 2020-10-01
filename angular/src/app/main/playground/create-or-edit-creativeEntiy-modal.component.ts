import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { PlaygroundServiceProxy, CreateOrEditCreativeEntiyDto} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'createOrEditCreativeEntiyModal',
    templateUrl: './create-or-edit-creativeEntiy-modal.component.html'
})
export class CreateOrEditCreativeEntiyModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    creativeEntiy: CreateOrEditCreativeEntiyDto = new CreateOrEditCreativeEntiyDto();

    constructor(
        injector: Injector,
        private _playgroundServiceProxy: PlaygroundServiceProxy
    ) {
        super(injector);
    }

    show(creativeEntiyId?: number): void {
        if (!creativeEntiyId) { 
			this.creativeEntiy = new CreateOrEditCreativeEntiyDto();
			this.creativeEntiy.id = creativeEntiyId;
			this.active = true;
			this.modal.show();
        }
		else{
			this._playgroundServiceProxy.getCreativeEntiyForEdit(creativeEntiyId).subscribe(creativeEntiyResult => {
				this.creativeEntiy = creativeEntiyResult;
				this.active = true;
				this.modal.show();
			});
		}  
    }

    save(): void {
			this.saving = true;
			this._playgroundServiceProxy.createOrEdit(this.creativeEntiy)
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