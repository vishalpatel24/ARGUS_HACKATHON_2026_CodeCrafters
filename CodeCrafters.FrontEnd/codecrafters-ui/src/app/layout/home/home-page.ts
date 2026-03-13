import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { GrantService, GrantTypeDto } from '../../features/grants/services/grant.service';
import { AuthService } from '../../features/auth/services/auth.service';
import { OrganisationService } from '../../features/organisations/services/organisation.service';
interface PreCheckForm {
  orgType: string;
  district: string;
  fundingAmount: number | null;
  durationMonths: number | null;
  overheadAmount: number | null;
}

interface CheckReason {
  pass: boolean;
  message: string;
}

interface PreCheckResultModel {
  eligible: boolean;
  reasons: CheckReason[];
}


@Component({
  selector: 'app-home-page',
  standalone: false,
  templateUrl: './home-page.html',
  styleUrls: ['./home-page.scss']
})
export class HomePageComponent implements OnInit {
  private grantService = inject(GrantService);
  private authService = inject(AuthService);
  private organisationService = inject(OrganisationService);
  private cdr = inject(ChangeDetectorRef);

  grants: GrantTypeDto[] = [];
  selectedGrant: GrantTypeDto | null = null;
  isLoading = true;
  error: string | null = null;

  // Pre-check state
  showPreCheck = false;
  preCheckResult: PreCheckResultModel | null = null;
  preCheck: PreCheckForm = { orgType: '', district: '', fundingAmount: null, durationMonths: null, overheadAmount: null };

  user = this.authService.currentUser();
  isProfileComplete = false;

  ngOnInit(): void {
    this.loadGrants();
    this.checkUserAndProfile();
  }

  checkUserAndProfile() {
    this.user = this.authService.currentUser();
    if (this.user?.role === 'Applicant') {
      this.organisationService.getMyOrganisation().subscribe({
        next: (org) => {
          this.isProfileComplete = !!org && org.isProfileComplete;
          this.cdr.detectChanges();
        },
        error: () => {
          this.isProfileComplete = false;
          this.cdr.detectChanges();
        }
      });
    }
  }

  loadGrants() {
    this.isLoading = true;
    this.grantService.getGrants().subscribe({
      next: (data) => {
        this.grants = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load grants', err);
        this.error = 'Failed to load grants. Please check your connection.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  scrollToGrants(): void {
    const el = document.querySelector('.grants-section');
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  viewDetails(grantId: string) {
    this.grantService.getGrantById(grantId).subscribe({
      next: (data) => {
        this.selectedGrant = data;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load grant details', err);
        this.cdr.detectChanges();
      }
    });
  }

  closeDetails() {
    this.selectedGrant = null;
  }

  closeOnBackdrop(event: MouseEvent) {
    if ((event.target as HTMLElement).classList.contains('detail-overlay')) {
      this.closeDetails();
    }
  }

  // Formatting methods

  formatLakh(value: number): string {
    return (value / 100000).toFixed(1) + ' Lakh';
  }

  formatCrore(value: number): string {
    return (value / 10000000).toFixed(1) + ' Crore';
  }
}
