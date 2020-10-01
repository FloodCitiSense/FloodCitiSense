import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { RoleServiceProxy, RoleEditDto, CreateOrUpdateRoleInput } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PermissionTreeComponent } from '../shared/permission-tree.component';

import * as _ from 'lodash';

@Component({
    selector: 'createOrEditRoleModal',
    templateUrl: './create-or-edit-role-modal.component.html'
})
export class CreateOrEditRoleModalComponent extends AppComponentBase {

    @ViewChild('roleNameInput') roleNameInput: ElementRef;
    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('permissionTree') permissionTree: PermissionTreeComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    role: RoleEditDto = new RoleEditDto();
    constructor(
        injector: Injector,
        private _roleService: RoleServiceProxy
    ) {
        super(injector);
    }

    ngAfterViewChecked(): void {
        //Temporary fix for: https://github.com/valor-software/ngx-bootstrap/issues/1508
        $('tabset ul.nav').addClass('m-tabs-line');
        $('tabset ul.nav li a.nav-link').addClass('m-tabs__link');
    }

    show(roleId?: number): void {
        const self = this;
        self.active = true;

        self._roleService.getRoleForEdit(roleId).subscribe(result => {
            self.role = result.role;
            this.permissionTree.editData = result;

            self.modal.show();
        });
    }

    onShown(): void {
        $(this.roleNameInput.nativeElement).focus();
    }

    save(): void {
        const self = this;

        const input = new CreateOrUpdateRoleInput();
        input.role = self.role;
        input.grantedPermissionNames = self.permissionTree.getGrantedPermissionNames();

        this.saving = true;
        this._roleService.createOrUpdateRole(input)
            .finally(() => this.saving = false)
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
