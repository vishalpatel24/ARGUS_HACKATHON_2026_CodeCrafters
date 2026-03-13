import { Injectable, signal } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable, tap } from 'rxjs';
import { LoginResponseDto, UserResponseDto } from '../models/auth.models';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly USER_KEY = 'auth_user';
  
  public currentUser = signal<UserResponseDto | null>(this.getStoredUser());

  constructor(private api: ApiService, private router: Router) {}

  login(credentials: any): Observable<LoginResponseDto> {
    return this.api.post<LoginResponseDto>('/auth/login', credentials).pipe(
      tap(res => {
        localStorage.setItem(this.TOKEN_KEY, res.token);
        localStorage.setItem(this.USER_KEY, JSON.stringify(res.user));
        this.currentUser.set(res.user);
      })
    );
  }

  register(data: any): Observable<any> {
    return this.api.post('/auth/register', data);
  }

  logout() {
    this.clearSession();
    this.router.navigate(['/']);
  }

  /** Clears token and user from storage and signal without navigating. Use from interceptors on 401. */
  clearSession(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUser.set(null);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem(this.TOKEN_KEY);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getStoredUser(): UserResponseDto | null {
    const userStr = localStorage.getItem(this.USER_KEY);
    return userStr ? JSON.parse(userStr) : null;
  }
}
