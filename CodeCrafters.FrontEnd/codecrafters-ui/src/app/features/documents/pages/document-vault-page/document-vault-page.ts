import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DocumentService } from '../../services/document.service';
import { UserDocumentDto } from '../../models/document.models';

const DOCUMENT_TYPES = [
  { value: 'RegistrationCertificate', label: 'Registration Certificate' },
  { value: 'AuditedFinancials', label: 'Audited Financials' },
  { value: 'Tax80G', label: '80G Certificate' },
  { value: 'Other', label: 'Other' }
];

@Component({
  selector: 'app-document-vault-page',
  standalone: false,
  templateUrl: './document-vault-page.html',
  styleUrls: ['./document-vault-page.scss']
})
export class DocumentVaultPageComponent implements OnInit {
  documents: UserDocumentDto[] = [];
  isLoading = true;
  error = '';
  selectedType = 'RegistrationCertificate';
  uploadFile: File | null = null;
  isUploading = false;
  documentTypes = DOCUMENT_TYPES;

  constructor(
    private documentService: DocumentService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDocuments();
  }

  loadDocuments(): void {
    this.isLoading = true;
    this.documentService.getMyDocuments().subscribe({
      next: (list) => {
        this.documents = list;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load documents.';
        this.isLoading = false;
      }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    this.uploadFile = file || null;
  }

  upload(): void {
    if (!this.uploadFile || this.isUploading) return;
    this.isUploading = true;
    this.error = '';
    this.documentService.upload(this.selectedType, this.uploadFile).subscribe({
      next: () => {
        this.uploadFile = null;
        (document.getElementById('file-input') as HTMLInputElement).value = '';
        this.loadDocuments();
        this.isUploading = false;
      },
      error: () => {
        this.error = 'Upload failed.';
        this.isUploading = false;
      }
    });
  }

  deleteDoc(doc: UserDocumentDto): void {
    if (!confirm(`Delete "${doc.fileName}"?`)) return;
    this.documentService.delete(doc.id).subscribe({
      next: () => this.loadDocuments(),
      error: () => (this.error = 'Delete failed.')
    });
  }

  formatSize(bytes: number): string {
    if (bytes < 1024) return bytes + ' B';
    if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB';
    return (bytes / (1024 * 1024)).toFixed(1) + ' MB';
  }

  formatDate(s: string): string {
    return new Date(s).toLocaleDateString();
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
