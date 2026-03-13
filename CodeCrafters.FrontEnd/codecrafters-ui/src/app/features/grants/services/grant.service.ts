import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable } from 'rxjs';

export interface GrantTypeDto {
  id: string;
  code: string;
  name: string;
  purpose: string;
  fundingMinAmount: number;
  fundingMaxAmount: number;
  projectDurationMinMonths: number;
  projectDurationMaxMonths: number;
  eligibleApplicants: string;
  geographicFocus: string;
  annualCycle: string;
  maximumAwardsPerCycle: number;
  totalProgrammeBudget: number;
}

@Injectable({ providedIn: 'root' })
export class GrantService {
  constructor(private api: ApiService) {}

  getGrants(): Observable<GrantTypeDto[]> {
    return this.api.get<GrantTypeDto[]>('/grants');
  }

  getGrantById(id: string): Observable<GrantTypeDto> {
    return this.api.get<GrantTypeDto>(`/grants/${id}`);
  }
}
