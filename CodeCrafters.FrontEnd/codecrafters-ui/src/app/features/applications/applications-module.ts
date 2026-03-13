import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ApplicationsRoutingModule } from './applications-routing-module';
import { MyApplicationsPageComponent } from './pages/my-applications-page/my-applications-page';
import { ApplicationNewPageComponent } from './pages/application-new-page/application-new-page';
import { ApplicationWizardPageComponent } from './pages/application-wizard-page/application-wizard-page';
import { SharedModule } from '../../shared/shared-module';

@NgModule({
  declarations: [
    MyApplicationsPageComponent,
    ApplicationNewPageComponent,
    ApplicationWizardPageComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ApplicationsRoutingModule,
    SharedModule
  ]
})
export class ApplicationsModule { }
