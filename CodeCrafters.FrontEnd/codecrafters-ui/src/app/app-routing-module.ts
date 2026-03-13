import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './layout/home/home-page';
import { MainLayoutComponent } from './layout/main-layout/main-layout';
import { authGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', component: HomePageComponent },
      { 
        path: '', 
        loadChildren: () => import('./features/auth/auth-module').then(m => m.AuthModule) 
      },
      { 
        path: 'dashboard', 
        canActivate: [authGuard],
        loadChildren: () => import('./features/dashboard/dashboard-module').then(m => m.DashboardModule) 
      },
      { 
        path: 'users', 
        canActivate: [authGuard],
        loadChildren: () => import('./features/users/users-module').then(m => m.UsersModule) 
      },
      {
        path: 'organisations',
        canActivate: [authGuard],
        loadChildren: () => import('./features/organisations/organisations-module').then(m => m.OrganisationsModule)
      },
      {
        path: 'document-vault',
        canActivate: [authGuard],
        loadChildren: () => import('./features/document-vault/document-vault-module').then(m => m.DocumentVaultModule)
      },
      {
        path: 'screening',
        canActivate: [authGuard],
        loadChildren: () => import('./features/screening/screening-module').then(m => m.ScreeningModule)
      },
      {
        path: 'applications',
        canActivate: [authGuard],
        loadChildren: () => import('./features/applications/applications-module').then(m => m.ApplicationsModule)
      }
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
