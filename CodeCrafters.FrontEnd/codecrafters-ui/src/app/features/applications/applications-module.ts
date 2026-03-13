import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ApplyGrantPageComponent } from './pages/apply-grant-page/apply-grant-page';

const routes: Routes = [
  { path: 'apply', component: ApplyGrantPageComponent },
  { path: 'apply/:id', component: ApplyGrantPageComponent }
];

@NgModule({
  imports: [
    CommonModule,
    ApplyGrantPageComponent, // Import standalone component
    RouterModule.forChild(routes)
  ]
})
export class ApplicationsModule { }
