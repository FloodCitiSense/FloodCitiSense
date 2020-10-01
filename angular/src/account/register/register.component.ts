import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountServiceProxy, PasswordComplexitySetting, ProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LoginService } from '../login/login.service';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { RegisterModel } from './register.model';
import { AppConsts } from '@shared/AppConsts';

@Component({
    templateUrl: './register.component.html',
    animations: [accountModuleAnimation()]
})
export class RegisterComponent extends AppComponentBase implements OnInit {

    model: RegisterModel = new RegisterModel();
    passwordComplexitySetting: PasswordComplexitySetting = new PasswordComplexitySetting();
    recaptchaSiteKey: string = AppConsts.recaptchaSiteKey;

    saving = false;

    constructor(
        injector: Injector,
        private _accountService: AccountServiceProxy,
        private _router: Router,
        private readonly _loginService: LoginService,
        private _profileService: ProfileServiceProxy
    ) {
        super(injector);
    }

    ngOnInit() {
        //Prevent to register new users in the host context
        if (this.appSession.tenant == null) {
            this._router.navigate(['account/login']);
            return;
        }

        this._profileService.getPasswordComplexitySetting().subscribe(result => {
            this.passwordComplexitySetting = result.setting;
        });
    }

    get useCaptcha(): boolean {
        return this.setting.getBoolean('App.UserManagement.UseCaptchaOnRegistration');
    }

    save(): void {
        if (this.useCaptcha && !this.model.captchaResponse) {
            this.message.warn(this.l('CaptchaCanNotBeEmpty'));
            return;
        }

        this.saving = true;
        this._accountService.register(this.model)
            .finally(() => { this.saving = false; })
            .subscribe((result) => {
                if (!result.canLogin) {
                    this.notify.success(this.l('SuccessfullyRegistered'));
                    this._router.navigate(['account/login']);
                    return;
                }

                //Autheticate
                this.saving = true;
                this._loginService.authenticateModel.userNameOrEmailAddress = this.model.userName;
                this._loginService.authenticateModel.password = this.model.password;
                this._loginService.authenticate(() => { this.saving = false; });
            });
    }

    captchaResolved(captchaResponse: string): void {
        this.model.captchaResponse = captchaResponse;
    }
}
