import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { UsersRoutingModule } from './users-routing-module';
import { UserListPageComponent } from './pages/user-list-page/user-list-page';
import { UserCreatePageComponent } from './pages/user-create-page/user-create-page';
import { UserEditPageComponent } from './pages/user-edit-page/user-edit-page';
import { SharedModule } from '../../shared/shared-module';

@NgModule({
  declarations: [
    UserListPageComponent,
    UserCreatePageComponent,
    UserEditPageComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    UsersRoutingModule,
    SharedModule
  ]
})
export class UsersModule { }
