import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DocumentsRoutingModule } from './documents-routing-module';
import { DocumentVaultPageComponent } from './pages/document-vault-page/document-vault-page';
import { SharedModule } from '../../shared/shared-module';

@NgModule({
  declarations: [DocumentVaultPageComponent],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    DocumentsRoutingModule,
    SharedModule
  ]
})
export class DocumentsModule { }
