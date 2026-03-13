import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const baseUrl = environment.apiUrl;
  let url = req.url;

  if (!url.startsWith('http') && url.startsWith('/api')) {
    url = url;
  } else if (!url.startsWith('http') && !url.startsWith(baseUrl)) {
    url = url.startsWith('/') ? `${baseUrl}${url}` : `${baseUrl}/${url}`;
  }

  const cloned = req.clone({
    url,
    setHeaders: {
      'Content-Type': 'application/json',
      Accept: 'application/json'
    }
  });

  return next(cloned).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 0) {
        console.error('Network or CORS error:', error.message);
      } else if (error.status >= 400) {
        console.error(`API error ${error.status}:`, error.error ?? error.message);
      }
      return throwError(() => error);
    })
  );
};
