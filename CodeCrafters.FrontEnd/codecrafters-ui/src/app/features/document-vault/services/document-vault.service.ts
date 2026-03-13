import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { DocumentVaultItemDto } from '../models/document-vault.models';

@Injectable({ providedIn: 'root' })
export class DocumentVaultService {
  private baseUrl = `${environment.apiUrl}/documentvault`;

  constructor(private http: HttpClient) {}

  getMyDocuments(): Observable<DocumentVaultItemDto[]> {
    return this.http.get<DocumentVaultItemDto[]>(this.baseUrl);
  }

  upload(documentType: string, file: File): Observable<DocumentVaultItemDto> {
    const formData = new FormData();
    formData.append('documentType', documentType);
    formData.append('file', file);
    return this.http.post<DocumentVaultItemDto>(`${this.baseUrl}/upload`, formData);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getDownloadUrl(id: string): string {
    return `${this.baseUrl}/${id}/download`;
  }

  download(id: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/${id}/download`, { responseType: 'blob' });
  }
}
