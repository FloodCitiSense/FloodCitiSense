import { Component, OnInit, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { NotificationServiceProxy, GetNotificationSettingsOutput, UpdateNotificationSettingsInput, NotificationSubscriptionDto } from '@shared/service-proxies/service-proxies';

import * as _ from 'lodash';

@Component({
    selector: 'notificationSettingsModal',
    templateUrl: './notification-settings-modal.component.html'
})
export class NotificationSettingsModalComponent extends AppComponentBase {

    @ViewChild('modal') modal: ModalDirective;

    saving = false;

    settings: GetNotificationSettingsOutput;

    constructor(
        injector: Injector,
        private _notificationService: NotificationServiceProxy
    ) {
        super(injector);
    }

    show() {
        this.getSettings(() => {
            this.modal.show();
        });
    }

    onShown(): void {
        $('#ReceiveNotifications').bootstrapSwitch('state', this.settings.receiveNotifications);
        $('#ReceiveNotifications').bootstrapSwitch('onSwitchChange', (el, value) => {
            this.settings.receiveNotifications = value;
        });
    }

    save(): void {
        const input = new UpdateNotificationSettingsInput();
        input.receiveNotifications = this.settings.receiveNotifications;
        input.notifications = _.map(this.settings.notifications,
            (n) => {
                let subscription = new NotificationSubscriptionDto();
                subscription.name = n.name;
                subscription.isSubscribed = n.isSubscribed;
                return subscription;
            });

        this.saving = true;
        this._notificationService.updateNotificationSettings(input)
            .finally(() => this.saving = false)
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
            });
    }

    close(): void {
        this.modal.hide();
    }

    private getSettings(callback: () => void) {
        this._notificationService.getNotificationSettings().subscribe((result: GetNotificationSettingsOutput) => {
            this.settings = result;
            callback();
        });
    }
}
