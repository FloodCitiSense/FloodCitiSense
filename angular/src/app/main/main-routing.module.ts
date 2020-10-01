import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CitiesComponent } from '@datatypes/cities/cities.component';
import { SensorsComponent } from '@datatypes/sensors/sensors.component';
import { IncidentApprovalsComponent } from '@datatypes/incidentApprovals/incidentApprovals.component';
import { IncidentsComponent } from '@datatypes/incidents/incidents.component';
import { LocationsComponent } from '@datatypes/locations/locations.component';
import { PicturesComponent } from '@datatypes/pictures/pictures.component';
import { PlaygroundComponent } from './playground/playground.component';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'dataTypes/cities', component: CitiesComponent, data: { permission: 'Pages.Cities' } },
                    { path: 'datatypes/sensors', component: SensorsComponent, data: { permission: 'Pages.Sensors' } },
                    { path: 'datatypes/incidentApprovals', component: IncidentApprovalsComponent, data: { permission: 'Pages.IncidentApprovals' } },
                    { path: 'datatypes/incidents', component: IncidentsComponent, data: { permission: 'Pages.Incidents' } },
                    { path: 'datatypes/locations', component: LocationsComponent, data: { permission: 'Pages.Locations' } },
                    { path: 'datatypes/pictures', component: PicturesComponent, data: { permission: 'Pages.Pictures' } },
                    { path: 'playground', component: PlaygroundComponent, data: { permission: 'Pages.CreativeEntiies' } },
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
