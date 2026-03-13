import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable } from 'rxjs';
import { ScreeningReportDto, ScreeningActionDto } from '../models/screening.models';

@Injectable({ providedIn: 'root' })
export class ScreeningService {
  constructor(private api: ApiService) {}

  runScreening(applicationId: string): Observable<ScreeningReportDto> {
    return this.api.post<ScreeningReportDto>(`/screening/run/${applicationId}`, {});
  }

  getByApplication(applicationId: string): Observable<ScreeningReportDto> {
    return this.api.get<ScreeningReportDto>(`/screening/application/${applicationId}`);
  }

  getAll(resultFilter?: string): Observable<ScreeningReportDto[]> {
    const query = resultFilter ? `?result=${resultFilter}` : '';
    return this.api.get<ScreeningReportDto[]>(`/screening${query}`);
  }

  takeAction(reportId: string, action: ScreeningActionDto): Observable<ScreeningReportDto> {
    return this.api.post<ScreeningReportDto>(`/screening/${reportId}/action`, action);
  }

  syncAll(): Observable<any> {
    return this.api.post('/screening/sync-all', {});
  }
}
