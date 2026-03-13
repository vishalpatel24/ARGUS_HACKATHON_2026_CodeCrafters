import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import {
  ApplicationDraftDto,
  ApplicationListItemDto,
  CreateApplicationDto,
  UpdateApplicationDto
} from '../models/application.models';

@Injectable({ providedIn: 'root' })
export class ApplicationService {
  constructor(private api: ApiService) {}

  getMyApplications(): Observable<ApplicationListItemDto[]> {
    return this.api.get<ApplicationListItemDto[]>('/applications/me');
  }

  getById(id: string): Observable<ApplicationListItemDto> {
    return this.api.get<ApplicationListItemDto>(`/applications/${id}`);
  }

  getDraft(id: string): Observable<ApplicationDraftDto> {
    return this.api.get<ApplicationDraftDto>(`/applications/${id}/draft`);
  }

  createDraft(dto: CreateApplicationDto): Observable<ApplicationListItemDto> {
    return this.api.post<ApplicationListItemDto>('/applications', dto);
  }

  updateDraft(id: string, dto: UpdateApplicationDto): Observable<ApplicationListItemDto> {
    return this.api.put<ApplicationListItemDto>(`/applications/${id}`, dto);
  }

  submit(id: string): Observable<ApplicationListItemDto> {
    return this.api.post<ApplicationListItemDto>(`/applications/${id}/submit`, {});
  }
}
