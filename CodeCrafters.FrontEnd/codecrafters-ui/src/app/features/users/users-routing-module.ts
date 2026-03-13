import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserListPageComponent } from './pages/user-list-page/user-list-page';
import { UserCreatePageComponent } from './pages/user-create-page/user-create-page';
import { UserEditPageComponent } from './pages/user-edit-page/user-edit-page';
import { roleGuard } from '../../core/guards/role.guard';

const routes: Routes = [
  { path: '', component: UserListPageComponent },
  { path: 'create', component: UserCreatePageComponent, canActivate: [roleGuard], data: { roles: ['PlatformAdmin'] } },
  { path: 'edit/:id', component: UserEditPageComponent, canActivate: [roleGuard], data: { roles: ['PlatformAdmin'] } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule { }
