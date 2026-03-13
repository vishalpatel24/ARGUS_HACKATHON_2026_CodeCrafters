import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { OrganisationProfilePageComponent } from './pages/organisation-profile-page/organisation-profile-page';

const routes: Routes = [
  { path: 'profile', component: OrganisationProfilePageComponent }
];

@NgModule({
  declarations: [
    OrganisationProfilePageComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes)
  ]
})
export class OrganisationsModule { }
