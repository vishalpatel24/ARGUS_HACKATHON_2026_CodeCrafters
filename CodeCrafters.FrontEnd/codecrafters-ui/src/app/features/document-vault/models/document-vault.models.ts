export interface DocumentVaultItemDto {
  id: string;
  documentType: string;
  originalFileName: string;
  contentType: string;
  fileSizeBytes: number;
  uploadedAt: string;
}

export const DOCUMENT_TYPES = [
  { value: 'RegistrationCertificate', label: 'Registration Certificate' },
  { value: 'AuditedFinancials', label: 'Audited Financials' },
  { value: '80GCertificate', label: '80G Certificate' },
  { value: 'FCRACertificate', label: 'FCRA Certificate' },
  { value: 'Other', label: 'Other' }
];
