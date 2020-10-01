import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CitiesComponent } from '@datatypes/cities/cities.component';
import { ViewCityModalComponent } from '@datatypes/cities/view-city-modal.component';
import { CreateOrEditCityModalComponent } from '@datatypes/cities/create-or-edit-city-modal.component';
import { UserLookupTableModalComponent } from '@datatypes/cities/user-lookup-table-modal.component';

import { SensorsComponent } from '@datatypes/sensors/sensors.component';
import { ViewSensorModalComponent } from '@datatypes/sensors/view-sensor-modal.component';
import { CreateOrEditSensorModalComponent } from '@datatypes/sensors/create-or-edit-sensor-modal.component';

import { IncidentApprovalsComponent } from '@datatypes/incidentApprovals/incidentApprovals.component';
import { CreateOrEditIncidentApprovalModalComponent } from '@datatypes/incidentApprovals/create-or-edit-incidentApproval-modal.component';

import { IncidentsComponent } from '@datatypes/incidents/incidents.component';
import { CreateOrEditIncidentModalComponent } from '@datatypes/incidents/create-or-edit-incident-modal.component';

import { LocationsComponent } from '@datatypes/locations/locations.component';
import { CreateOrEditLocationModalComponent } from '@datatypes/locations/create-or-edit-location-modal.component';

import { PicturesComponent } from '@datatypes/pictures/pictures.component';
import { CreateOrEditPictureModalComponent } from '@datatypes/pictures/create-or-edit-picture-modal.component';

import { PlaygroundComponent } from './playground/playground.component';
import { CreateOrEditCreativeEntiyModalComponent } from './playground/create-or-edit-creativeEntiy-modal.component';
import { DataTableModule } from 'primeng/primeng';
import { FileUploadModule } from 'primeng/primeng';
import { AutoCompleteModule } from 'primeng/primeng';
import { PaginatorModule } from 'primeng/primeng';
import { EditorModule } from 'primeng/primeng';
import { InputMaskModule } from 'primeng/primeng';
import { ModalModule, TabsModule, TooltipModule } from 'ngx-bootstrap';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { MainRoutingModule } from './main-routing.module';
import { CountoModule } from 'angular2-counto';
import { EasyPieChartModule } from 'ng2modules-easypiechart';

@NgModule({
    imports: [
        DataTableModule,
        FileUploadModule,
        AutoCompleteModule,
        PaginatorModule,
        EditorModule,
        InputMaskModule,
        CommonModule,
        FormsModule,
        ModalModule,
        TabsModule,
        TooltipModule,
        AppCommonModule,
        UtilsModule,
        MainRoutingModule,
        CountoModule,
        EasyPieChartModule
    ],
    declarations: [
        CitiesComponent,
        ViewCityModalComponent, CreateOrEditCityModalComponent,
        UserLookupTableModalComponent,
        SensorsComponent,
        ViewSensorModalComponent, CreateOrEditSensorModalComponent,
        IncidentApprovalsComponent,
        CreateOrEditIncidentApprovalModalComponent,
        IncidentsComponent,
        CreateOrEditIncidentModalComponent,
        LocationsComponent,
        CreateOrEditLocationModalComponent,
        PicturesComponent,
        CreateOrEditPictureModalComponent,
        PlaygroundComponent,
        CreateOrEditCreativeEntiyModalComponent,
        DashboardComponent
    ]
})
export class MainModule { }
