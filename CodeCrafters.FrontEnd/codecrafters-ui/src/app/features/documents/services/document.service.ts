import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { UserDocumentDto } from '../models/document.models';

@Injectable({ providedIn: 'root' })
export class DocumentService {
  constructor(private api: ApiService) {}

  getMyDocuments(): Observable<UserDocumentDto[]> {
    return this.api.get<UserDocumentDto[]>('/documents/me');
  }

  upload(documentType: string, file: File): Observable<UserDocumentDto> {
    const formData = new FormData();
    formData.append('documentType', documentType);
    formData.append('file', file, file.name);
    return this.api.post<UserDocumentDto>('/documents/upload', formData);
  }

  delete(id: string): Observable<void> {
    return this.api.delete<void>(`/documents/${id}`);
  }
}
