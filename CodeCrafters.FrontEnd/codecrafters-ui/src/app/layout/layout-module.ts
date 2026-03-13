import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MainLayoutComponent } from './main-layout/main-layout';
import { HomePageComponent } from './home/home-page';

@NgModule({
  declarations: [
    MainLayoutComponent,
    HomePageComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    MainLayoutComponent,
    HomePageComponent
  ]
})
export class LayoutModule { }
