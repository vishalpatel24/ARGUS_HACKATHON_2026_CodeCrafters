import { Component, inject } from '@angular/core';
import { AuthService } from '../../../auth/services/auth.service';

@Component({
  selector: 'app-dashboard-page',
  standalone: false,
  templateUrl: './dashboard-page.html',
  styleUrls: ['./dashboard-page.scss']
})
export class DashboardPageComponent {
  authService = inject(AuthService);
  user = this.authService.currentUser();
}
