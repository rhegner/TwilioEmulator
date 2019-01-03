import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule, MatButtonModule, MatSidenavModule, MatIconModule, MatListModule,
  MatGridListModule, MatCardModule, MatMenuModule, MatTableModule, MatPaginatorModule, MatSortModule,
  MatDialogModule, MatFormFieldModule, MatSelectModule, MatInputModule, MatRadioModule, MatTabsModule } from '@angular/material';
import { TableTestComponent } from './components/table-test/table-test.component';
import { OverviewComponent } from './components/overview/overview.component';
import { NewIncomingCallDialogComponent } from './dialog-components/new-incoming-call-dialog/new-incoming-call-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CallResourceComponent } from './components/call-resource/call-resource.component';
import { DashboardTestComponent } from './components/dashboard-test/dashboard-test.component';
import { AddressFormTestComponent } from './components/address-form-test/address-form-test.component';
import { CallsTableComponent } from './views/calls-table/calls-table.component';
import { ConferencesTableComponent } from './views/conferences-table/conferences-table.component';
import { CallResourcesComponent } from './components/call-resources/call-resources.component';
import { ConferenceResourcesComponent } from './components/conference-resources/conference-resources.component';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

export function getBackendBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

@NgModule({
  declarations: [
    AppComponent,
    TableTestComponent,
    OverviewComponent,
    NewIncomingCallDialogComponent,
    CallResourceComponent,
    DashboardTestComponent,
    AddressFormTestComponent,
    CallsTableComponent,
    ConferencesTableComponent,
    CallResourcesComponent,
    ConferenceResourcesComponent
  ],
  entryComponents: [
    NewIncomingCallDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatFormFieldModule,
    MatSidenavModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatGridListModule,
    MatCardModule,
    MatDialogModule,
    MatMenuModule,
    MatTabsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSelectModule,
    MatSortModule,
    MatRadioModule,
    ReactiveFormsModule
  ],
  providers: [
    { provide: 'BASE_URL', useFactory: getBaseUrl },
    { provide: 'BACKEND_BASE_URL', useFactory: getBackendBaseUrl }
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }
