import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { PicturesServiceProxy, PictureDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditPictureModalComponent } from './create-or-edit-picture-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

@Component({
    templateUrl: './pictures.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PicturesComponent extends AppComponentBase {

    @ViewChild('createOrEditPictureModal') createOrEditPictureModal: CreateOrEditPictureModalComponent;
    @ViewChild('dataTable') dataTable: DataTable;
    @ViewChild('paginator') paginator: Paginator;

    filterText = '';

    constructor(
        injector: Injector,
        private _http: Http,
        private _picturesServiceProxy: PicturesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
    }

    getPictures(event?: LazyLoadEvent) {
        if (this.primengDatatableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengDatatableHelper.showLoadingIndicator();

        this._picturesServiceProxy.getAll(
            this.filterText,
            this.primengDatatableHelper.getSorting(this.dataTable),
            this.primengDatatableHelper.getSkipCount(this.paginator, event),
            this.primengDatatableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengDatatableHelper.totalRecordsCount = result.totalCount;
            this.primengDatatableHelper.records = result.items;
            this.primengDatatableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createPicture(): void {
        this.createOrEditPictureModal.show();
    }

    deletePicture(picture: PictureDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._picturesServiceProxy.delete(picture.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
}
