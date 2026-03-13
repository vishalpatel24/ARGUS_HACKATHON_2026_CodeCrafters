export interface UserDocumentDto {
  id: string;
  documentType: string;
  fileName: string;
  contentType: string;
  fileSizeBytes: number;
  uploadedAt: string;
}
