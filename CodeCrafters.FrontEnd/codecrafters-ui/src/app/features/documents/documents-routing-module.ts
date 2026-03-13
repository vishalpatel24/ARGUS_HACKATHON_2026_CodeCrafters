import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DocumentVaultPageComponent } from './pages/document-vault-page/document-vault-page';

const routes: Routes = [
  { path: '', component: DocumentVaultPageComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentsRoutingModule { }
