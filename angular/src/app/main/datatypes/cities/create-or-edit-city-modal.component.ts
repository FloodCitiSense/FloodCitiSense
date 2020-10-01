import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { CitiesServiceProxy, CreateOrEditCityDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { UserLookupTableModalComponent } from '@datatypes/cities/user-lookup-table-modal.component';


@Component({
    selector: 'createOrEditCityModal',
    templateUrl: './create-or-edit-city-modal.component.html'
})
export class CreateOrEditCityModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
	 @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    city: CreateOrEditCityDto = new CreateOrEditCityDto();
	userName = '';


    constructor(
        injector: Injector,
        private _citiesServiceProxy: CitiesServiceProxy
    ) {
        super(injector);
    }

    show(cityId?: number): void {
        if (!cityId) {
			this.city = new CreateOrEditCityDto();
			this.city.id = cityId;
			this.userName = '';

			this.active = true;
			this.modal.show();
        }
		else{
			this._citiesServiceProxy.getCityForEdit(cityId).subscribe(result => {
				this.city = result.city;
				this.userName = result.userName;

				this.active = true;
				this.modal.show();
			});
		}
    }

    save(): void {
			this.saving = true;
			this._citiesServiceProxy.createOrEdit(this.city)
			 .pipe(finalize(() => { this.saving = false; }))
			 .subscribe(() => {
			    this.notify.info(this.l('SavedSuccessfully'));
				this.close();
				this.modalSave.emit(null);
             });
    }

	    openSelectUserModal() {
        this.userLookupTableModal.id = this.city.userId;
        this.userLookupTableModal.displayName = this.userName;
        this.userLookupTableModal.show();
    }


	    setUserIdNull() {
        this.city.userId = null;
        this.userName = '';
    }


	    getNewUserId() {
        this.city.userId = this.userLookupTableModal.id;
        this.userName = this.userLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
