import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TableTestComponent } from './components/table-test/table-test.component';
import { OverviewComponent } from './components/overview/overview.component';
import { CallResourceComponent } from './components/call-resource/call-resource.component';
import { DashboardTestComponent } from './components/dashboard-test/dashboard-test.component';
import { AddressFormTestComponent } from './components/address-form-test/address-form-test.component';
import { CallResourcesComponent } from './components/call-resources/call-resources.component';
import { ConferenceResourcesComponent } from './components/conference-resources/conference-resources.component';

const routes: Routes = [
  { path: '', redirectTo: 'overview', pathMatch: 'full' },
  { path: 'overview', component: OverviewComponent },
  { path: 'callresources', component: CallResourcesComponent },
  { path: 'callresources/:callSid', component: CallResourceComponent },
  { path: 'conferenceresources', component: ConferenceResourcesComponent },

  { path: 'table', component: TableTestComponent },
  { path: 'dashboard', component: DashboardTestComponent },
  { path: 'address-form', component: AddressFormTestComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
