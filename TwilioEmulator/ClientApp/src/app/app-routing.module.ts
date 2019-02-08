import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TableTestComponent } from './components/table-test/table-test.component';
import { OverviewComponent } from './components/overview/overview.component';
import { CallComponent } from './components/call/call.component';
import { DashboardTestComponent } from './components/dashboard-test/dashboard-test.component';
import { AddressFormTestComponent } from './components/address-form-test/address-form-test.component';
import { CallsComponent } from './components/calls/calls.component';
import { ConferencesComponent } from './components/conferences/conferences.component';

const routes: Routes = [
  { path: '', redirectTo: 'overview', pathMatch: 'full' },
  { path: 'overview', component: OverviewComponent },
  { path: 'calls', component: CallsComponent },
  { path: 'calls/:callSid', component: CallComponent },
  { path: 'conferenceresources', component: ConferencesComponent },

  { path: 'table', component: TableTestComponent },
  { path: 'dashboard', component: DashboardTestComponent },
  { path: 'address-form', component: AddressFormTestComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
