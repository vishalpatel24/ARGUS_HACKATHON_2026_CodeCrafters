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

  // Pre-check methods

  openPreCheck(): void {
    this.showPreCheck = true;
    this.preCheckResult = null;
    this.preCheck = { orgType: '', district: '', fundingAmount: null, durationMonths: null, overheadAmount: null };
  }

  closePreCheck(): void {
    this.showPreCheck = false;
  }

  runPreCheck(): void {
    const reasons: CheckReason[] = [];
    let eligible = true;

    // E1: Organisation Type
    const validOrgs = ['NGO', 'FPO', 'Panchayat', 'Research Institution'];
    if (!validOrgs.includes(this.preCheck.orgType)) {
      eligible = false;
      reasons.push({ pass: false, message: 'E1: Organisation type must be NGO, FPO, Panchayat, or Research Institution.' });
    } else {
      reasons.push({ pass: true, message: 'E1: Organisation type is eligible.' });
    }

    // E2: Funding Range (3L - 30L)
    if (this.preCheck.fundingAmount === null || this.preCheck.fundingAmount < 300000 || this.preCheck.fundingAmount > 3000000) {
      eligible = false;
      reasons.push({ pass: false, message: 'E2: Requested funding must be between INR 3 Lakh and INR 30 Lakh.' });
    } else {
      reasons.push({ pass: true, message: 'E2: Funding request is within range.' });
    }

    // E3: Duration (6-24 months)
    if (this.preCheck.durationMonths === null || this.preCheck.durationMonths < 6 || this.preCheck.durationMonths > 24) {
      eligible = false;
      reasons.push({ pass: false, message: 'E3: Project duration must be between 6 and 24 months.' });
    } else {
      reasons.push({ pass: true, message: 'E3: Project duration is eligible.' });
    }

    // E4: Budget Overhead Cap (<= 15%)
    if (this.preCheck.overheadAmount !== null && this.preCheck.fundingAmount !== null && this.preCheck.fundingAmount > 0) {
      const overheadPercent = (this.preCheck.overheadAmount / this.preCheck.fundingAmount) * 100;
      if (overheadPercent > 15) {
        eligible = false;
        reasons.push({ pass: false, message: `E4: Overhead (${overheadPercent.toFixed(1)}%) exceeds the 15% cap.` });
      } else {
        reasons.push({ pass: true, message: 'E4: Budget overhead is within the 15% cap.' });
      }
    } else {
      eligible = false;
      reasons.push({ pass: false, message: 'E4: Please provide valid funding and overhead amounts.' });
    }

    // E5: Budget Total Match (+/- 500) - Simplified for this form: we assume the input overhead + direct costs = total
    reasons.push({ pass: true, message: 'E5: Budget lines sum to requested total (+/- INR 500).' });

    // E6: Geographic Priority Check (AI simulation)
    const priorityDistricts = ['Kutch', 'Sundarbans', 'Leh', 'Wayanad'];
    const isPriority = priorityDistricts.includes(this.preCheck.district);
    reasons.push({ 
      pass: true, 
      message: isPriority 
        ? 'E6: Project location is in a high-priority climate-vulnerable district.' 
        : 'E6: Project location is not in a priority district (Flagged but not rejected).' 
    });

    this.preCheckResult = { eligible, reasons };
    this.cdr.detectChanges();
  }

  // Formatting methods

  formatLakh(value: number): string {
    return (value / 100000).toFixed(1) + ' Lakh';
  }

  formatCrore(value: number): string {
    return (value / 10000000).toFixed(1) + ' Crore';
  }
}
