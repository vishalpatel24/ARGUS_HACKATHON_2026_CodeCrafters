import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { DocumentVaultPageComponent } from './pages/document-vault-page/document-vault-page';

const routes: Routes = [
  { path: '', component: DocumentVaultPageComponent }
];

@NgModule({
  declarations: [
    DocumentVaultPageComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(routes)
  ]
})
export class DocumentVaultModule { }
