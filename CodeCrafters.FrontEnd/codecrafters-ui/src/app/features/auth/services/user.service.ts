import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable } from 'rxjs';
import { UserResponseDto } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private api: ApiService) {}

  getUsers(): Observable<UserResponseDto[]> {
    return this.api.get<UserResponseDto[]>('/users');
  }

  createUser(data: any): Observable<UserResponseDto> {
    return this.api.post<UserResponseDto>('/users', data);
  }

  updateUser(id: string, data: any): Observable<UserResponseDto> {
    return this.api.put<UserResponseDto>(`/users/${id}`, data);
  }

  deactivateUser(id: string): Observable<any> {
    return this.api.put(`/users/${id}/deactivate`, {});
  }
}
