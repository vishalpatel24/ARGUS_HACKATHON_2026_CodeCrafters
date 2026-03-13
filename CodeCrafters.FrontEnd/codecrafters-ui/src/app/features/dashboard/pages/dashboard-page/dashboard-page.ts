import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { AuthService } from '../../../auth/services/auth.service';
import { OrganisationService } from '../../../organisations/services/organisation.service';
import { OrganisationDto } from '../../../organisations/models/organisation.models';

@Component({
  selector: 'app-dashboard-page',
  standalone: false,
  templateUrl: './dashboard-page.html',
  styleUrls: ['./dashboard-page.scss']
})
export class DashboardPageComponent implements OnInit {
  authService = inject(AuthService);
  organisationService = inject(OrganisationService);
  cdr = inject(ChangeDetectorRef);
  
  user = this.authService.currentUser();
  isProfileComplete = false;
  isOrgLoading = false;

  ngOnInit(): void {
    if (this.user?.role === 'Applicant') {
      this.checkOrganisation();
    }
  }

  checkOrganisation() {
    this.isOrgLoading = true;
    this.organisationService.getMyOrganisation().subscribe({
      next: (org) => {
        this.isProfileComplete = !!org && org.isProfileComplete;
        this.isOrgLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isProfileComplete = false;
        this.isOrgLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
