import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable } from 'rxjs';
import { ApplicationResponseDto, CreateApplicationDto } from '../models/application.models';

@Injectable({ providedIn: 'root' })
export class ApplicationService {
  constructor(private api: ApiService) {}

  getMyApplications(): Observable<ApplicationResponseDto[]> {
    return this.api.get<ApplicationResponseDto[]>('/applications/my');
  }

  submitApplication(dto: CreateApplicationDto): Observable<ApplicationResponseDto> {
    return this.api.post<ApplicationResponseDto>('/applications', dto);
  }

  getApplicationById(id: string): Observable<ApplicationResponseDto> {
    return this.api.get<ApplicationResponseDto>(`/applications/${id}`);
  }
}
