import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../features/auth/services/auth.service';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const user = authService.currentUser();
  if (!user) {
    return router.parseUrl('/login');
  }

  const expectedRoles: string[] = route.data['roles'] || [];
  if (expectedRoles.length > 0 && !expectedRoles.includes(user.role)) {
    return router.parseUrl('/dashboard');
  }

  return true;
};
