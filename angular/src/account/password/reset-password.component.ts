import { Component, Injector, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AccountServiceProxy, ResetPasswordOutput } from '@shared/service-proxies/service-proxies';
import { AuthenticateModel, AuthenticateResultModel, PasswordComplexitySetting, ProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import { LoginService } from '../login/login.service';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { AppUrlService } from '@shared/common/nav/app-url.service';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { ResetPasswordModel } from './reset-password.model';

@Component({
    templateUrl: './reset-password.component.html',
    animations: [accountModuleAnimation()]
})
export class ResetPasswordComponent extends AppComponentBase implements OnInit {

    model: ResetPasswordModel = new ResetPasswordModel();
    passwordComplexitySetting: PasswordComplexitySetting = new PasswordComplexitySetting();
    saving = false;

    constructor(
        injector: Injector,
        private _accountService: AccountServiceProxy,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _loginService: LoginService,
        private _appUrlService: AppUrlService,
        private _appSessionService: AppSessionService,
        private _profileService: ProfileServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.model.userId = this._activatedRoute.snapshot.queryParams['userId'];
        this.model.resetCode = this._activatedRoute.snapshot.queryParams['resetCode'];

        this._appSessionService.changeTenantIfNeeded(
            this.parseTenantId(
                this._activatedRoute.snapshot.queryParams['tenantId']
            )
        );

        this._profileService.getPasswordComplexitySetting().subscribe(result => {
            this.passwordComplexitySetting = result.setting;
        });
    }

    save(): void {
        this.saving = true;
        this._accountService.resetPassword(this.model)
            .finally(() => { this.saving = false; })            
            .subscribe((result: ResetPasswordOutput) => {
                if (!result.canLogin) {
                    window.location.href='http://18.220.58.67/fcs/Login.html'           
                    return;
                }

                this.message.success(this.l('YourPasswordHasChangedSuccessfully'), this.l('ResetSuccessfully')).done(() => {
                    window.location.href='http://18.220.58.67/fcs/Login.html';
                    // navigate to main app after password reset successfull
                });
            });
    }

    parseTenantId(tenantIdAsStr?: string): number {
        let tenantId = !tenantIdAsStr ? undefined : parseInt(tenantIdAsStr);
        if (tenantId === NaN) {
            tenantId = undefined;
        }

        return tenantId;
    }
}
