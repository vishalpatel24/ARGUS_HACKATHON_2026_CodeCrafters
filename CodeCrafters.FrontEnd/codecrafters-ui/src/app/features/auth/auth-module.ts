import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthRoutingModule } from './auth-routing-module';
import { LoginPageComponent } from './pages/login/login-page';
import { RegisterPageComponent } from './pages/register/register-page';
import { SharedModule } from '../../shared/shared-module';

@NgModule({
  declarations: [
    LoginPageComponent,
    RegisterPageComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AuthRoutingModule,
    SharedModule
  ]
})
export class AuthModule { }
