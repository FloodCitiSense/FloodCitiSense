import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { CitiesServiceProxy, CityDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditCityModalComponent } from '@app/main/datatypes/cities/create-or-edit-city-modal.component';
import { ViewCityModalComponent } from '@app/main/datatypes/cities/view-city-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './cities.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CitiesComponent extends AppComponentBase {

    @ViewChild('createOrEditCityModal') createOrEditCityModal: CreateOrEditCityModalComponent;
    @ViewChild('viewCityModalComponent') viewCityModal: ViewCityModalComponent;
    @ViewChild('dataTable') dataTable: DataTable;
    @ViewChild('paginator') paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    userNameFilter = '';




    constructor(
        injector: Injector,
        private _citiesServiceProxy: CitiesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getCities(event?: LazyLoadEvent) {
        if (this.primengDatatableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengDatatableHelper.showLoadingIndicator();

        this._citiesServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.userNameFilter,
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

    createCity(): void {
        this.createOrEditCityModal.show();
    }

    deleteCity(city: CityDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._citiesServiceProxy.delete(city.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._citiesServiceProxy.getCitiesToExcel(
            this.filterText,
            this.nameFilter,
            this.userNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
