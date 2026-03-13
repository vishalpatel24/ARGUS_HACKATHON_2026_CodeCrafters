import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable } from 'rxjs';
import { OrganisationDto, UpsertOrganisationDto } from '../models/organisation.models';

@Injectable({ providedIn: 'root' })
export class OrganisationService {
  constructor(private api: ApiService) {}

  getMyOrganisation(): Observable<OrganisationDto> {
    return this.api.get<OrganisationDto>('/organisation/me');
  }

  upsertMyOrganisation(data: UpsertOrganisationDto): Observable<OrganisationDto> {
    return this.api.put<OrganisationDto>('/organisation/me', data);
  }
}
