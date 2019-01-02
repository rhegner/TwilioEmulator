import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule, MatButtonModule, MatSidenavModule, MatIconModule, MatListModule,
  MatGridListModule, MatCardModule, MatMenuModule, MatTableModule, MatPaginatorModule, MatSortModule,
  MatDialogModule, MatFormFieldModule, MatSelectModule, MatInputModule } from '@angular/material';
import { TableTestComponent } from './components/table-test/table-test.component';
import { OverviewComponent } from './components/overview/overview.component';
import { NewIncomingCallDialogComponent } from './dialog-components/new-incoming-call-dialog/new-incoming-call-dialog.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

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
    NewIncomingCallDialogComponent
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
    MatTableModule,
    MatPaginatorModule,
    MatSelectModule,
    MatSortModule
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
