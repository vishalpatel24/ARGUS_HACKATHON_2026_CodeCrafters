import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../features/auth/services/auth.service';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const baseUrl = environment.apiUrl;
  let url = req.url;
  const router = inject(Router);
  const authService = inject(AuthService);

  if (!url.startsWith('http') && url.startsWith('/api')) {
    url = url;
  } else if (!url.startsWith('http') && !url.startsWith(baseUrl)) {
    url = url.startsWith('/') ? `${baseUrl}${url}` : `${baseUrl}/${url}`;
  }

  const token = localStorage.getItem('auth_token');
  let headers = req.headers
    .set('Content-Type', 'application/json')
    .set('Accept', 'application/json');

  if (token) {
    headers = headers.set('Authorization', `Bearer ${token}`);
  }

  const cloned = req.clone({
    url,
    headers
  });

  return next(cloned).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        authService.clearSession();
        router.navigate(['/login']);
      } else if (error.status === 403) {
        router.navigate(['/dashboard']);
      } else if (error.status === 0) {
        console.error('Network or CORS error:', error.message);
      } else if (error.status >= 400) {
        console.error(`API error ${error.status}:`, error.error ?? error.message);
      }
      return throwError(() => error);
    })
  );
};
