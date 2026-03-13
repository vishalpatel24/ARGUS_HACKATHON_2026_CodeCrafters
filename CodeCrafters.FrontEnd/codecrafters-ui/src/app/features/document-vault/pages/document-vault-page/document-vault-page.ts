import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DocumentVaultService } from '../../services/document-vault.service';
import { DocumentVaultItemDto, DOCUMENT_TYPES } from '../../models/document-vault.models';

@Component({
  selector: 'app-document-vault-page',
  standalone: false,
  templateUrl: './document-vault-page.html',
  styleUrls: ['./document-vault-page.scss']
})
export class DocumentVaultPageComponent implements OnInit {
  documents: DocumentVaultItemDto[] = [];
  documentTypes = DOCUMENT_TYPES;
  isLoading = true;
  isUploading = false;
  selectedType = '';
  selectedFile: File | null = null;
  successMessage = '';
  errorMessage = '';

  constructor(
    private vaultService: DocumentVaultService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadDocuments();
  }

  loadDocuments(): void {
    this.isLoading = true;
    this.vaultService.getMyDocuments().subscribe({
      next: (docs) => {
        this.documents = docs;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load documents.';
        this.cdr.detectChanges();
      }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  upload(): void {
    if (!this.selectedType || !this.selectedFile) {
      this.errorMessage = 'Please select a document type and file.';
      this.cdr.detectChanges();
      return;
    }

    this.isUploading = true;
    this.successMessage = '';
    this.errorMessage = '';
    this.cdr.detectChanges();

    this.vaultService.upload(this.selectedType, this.selectedFile).subscribe({
      next: () => {
        this.isUploading = false;
        this.successMessage = 'Document uploaded successfully!';
        this.selectedType = '';
        this.selectedFile = null;
        this.loadDocuments();
        this.cdr.detectChanges();
        setTimeout(() => { this.successMessage = ''; this.cdr.detectChanges(); }, 3000);
      },
      error: (err) => {
        this.isUploading = false;
        this.errorMessage = err.error?.message || err.error || 'Upload failed.';
        this.cdr.detectChanges();
      }
    });
  }

  deleteDocument(doc: DocumentVaultItemDto): void {
    if (!confirm(`Delete "${doc.originalFileName}"?`)) return;

    this.vaultService.delete(doc.id).subscribe({
      next: () => {
        this.successMessage = 'Document deleted.';
        this.loadDocuments();
        this.cdr.detectChanges();
        setTimeout(() => { this.successMessage = ''; this.cdr.detectChanges(); }, 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to delete document.';
        this.cdr.detectChanges();
      }
    });
  }

  downloadDocument(doc: DocumentVaultItemDto): void {
    this.vaultService.download(doc.id).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = doc.originalFileName;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: () => {
        this.errorMessage = 'Failed to download document.';
        this.cdr.detectChanges();
      }
    });
  }

  getTypeLabel(value: string): string {
    return this.documentTypes.find(t => t.value === value)?.label || value;
  }

  formatFileSize(bytes: number): string {
    if (bytes < 1024) return bytes + ' B';
    if (bytes < 1048576) return (bytes / 1024).toFixed(1) + ' KB';
    return (bytes / 1048576).toFixed(1) + ' MB';
  }
}
