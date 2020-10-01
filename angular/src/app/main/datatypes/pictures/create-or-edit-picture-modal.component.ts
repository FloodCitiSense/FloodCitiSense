import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { PicturesServiceProxy, CreateOrEditPictureDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';


@Component({
    selector: 'createOrEditPictureModal',
    templateUrl: './create-or-edit-picture-modal.component.html'
})
export class CreateOrEditPictureModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    picture: CreateOrEditPictureDto = new CreateOrEditPictureDto();

    constructor(
        injector: Injector,
        private _picturesServiceProxy: PicturesServiceProxy
    ) {
        super(injector);
    }

    show(pictureId?: string): void {
        if (!pictureId) {
            this.picture = new CreateOrEditPictureDto();
            //this.picture.id = pictureId;

            this.active = true;
            this.modal.show();
        } else {
            // this._picturesServiceProxy.getPictureForEdit(pictureId).subscribe(result => {
            //     this.picture = result;

            //     this.active = true;
            //     this.modal.show();
            // });
        }
    }

    save(): void {
            this.saving = true;
            this._picturesServiceProxy.createOrEdit(this.picture)
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
