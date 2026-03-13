import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyApplicationsPageComponent } from './pages/my-applications-page/my-applications-page';
import { ApplicationNewPageComponent } from './pages/application-new-page/application-new-page';
import { ApplicationWizardPageComponent } from './pages/application-wizard-page/application-wizard-page';

const routes: Routes = [
  { path: '', component: MyApplicationsPageComponent },
  { path: 'new', component: ApplicationNewPageComponent },
  { path: ':id/edit', component: ApplicationWizardPageComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ApplicationsRoutingModule { }
