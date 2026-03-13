import { Component, inject } from '@angular/core';
import { AuthService } from '../../features/auth/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main-layout',
  standalone: false,
  templateUrl: './main-layout.html',
  styleUrls: ['./main-layout.scss']
})
export class MainLayoutComponent {
  authService = inject(AuthService);
  router = inject(Router);

  logout() {
    this.authService.logout();
  }
}
