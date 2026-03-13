import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { ScreeningListPageComponent } from './pages/screening-list-page/screening-list-page';
import { ScreeningDetailPageComponent } from './pages/screening-detail-page/screening-detail-page';

const routes: Routes = [
  { path: '', component: ScreeningListPageComponent },
  { path: ':id', component: ScreeningDetailPageComponent }
];

@NgModule({
  declarations: [
    ScreeningListPageComponent,
    ScreeningDetailPageComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(routes)
  ]
})
export class ScreeningModule { }
