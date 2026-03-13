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

  viewDetails(grantId: string) {
    this.grantService.getGrantById(grantId).subscribe({
      next: (data) => {
        this.selectedGrant = data;
        this.resetPreCheck();
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
    this.resetPreCheck();
  }

  closeOnBackdrop(event: MouseEvent) {
    if ((event.target as HTMLElement).classList.contains('detail-overlay')) {
      this.closeDetails();
    }
  }

  // Pre-check methods
  openPreCheck(): void {
    this.showPreCheck = true;
  }

  closePreCheck(): void {
    this.showPreCheck = false;
  }

  resetPreCheck(): void {
    this.showPreCheck = false;
    this.preCheckResult = null;
    this.preCheck = { orgType: '', district: '', fundingAmount: null, durationMonths: null, overheadAmount: null };
  }

  runPreCheck(): void {
    if (!this.selectedGrant) return;
    const g = this.selectedGrant;
    const reasons: CheckReason[] = [];

    // Rule 1: Funding amount within grant range
    const requested = Number(this.preCheck.fundingAmount ?? 0);
    const fundingPass = requested >= g.fundingMinAmount && requested <= g.fundingMaxAmount;
    reasons.push({
      pass: fundingPass,
      message: fundingPass
        ? `Requested amount ₹${this.formatLakh(requested)} is within the grant range (₹${this.formatLakh(g.fundingMinAmount)} – ₹${this.formatLakh(g.fundingMaxAmount)})`
        : `Requested amount ₹${this.formatLakh(requested)} is outside the grant range (₹${this.formatLakh(g.fundingMinAmount)} – ₹${this.formatLakh(g.fundingMaxAmount)})`
    });

    // Rule 2: Org type mentioned in eligible applicants text
    const eligibleText = g.eligibleApplicants.toLowerCase();
    const orgKeywords: Record<string, string[]> = {
      'NGO':        ['ngo', 'non-profit', 'nonprofit', 'trust', 'society', 'charitable'],
      'Government': ['government', 'municipality', 'panchayat', 'public body', 'departm'],
      'University': ['university', 'college', 'research', 'institute', 'academic'],
      'CBO':        ['community', 'cbo', 'self-help', 'cooperative'],
      'Private':    ['private', 'company', 'enterprise', 'industry', 'startup'],
      'Individual': ['individual', 'self-employed', 'freelance', 'entrepreneur'],
    };
    const keywords = orgKeywords[this.preCheck.orgType] ?? [];
    const orgPass = keywords.length === 0 || keywords.some(k => eligibleText.includes(k));
    reasons.push({
      pass: orgPass,
      message: orgPass
        ? `${this.preCheck.orgType} organisations are mentioned as eligible applicants`
        : `${this.preCheck.orgType} organisations may not be listed as eligible applicants for this grant`
    });

    // Rule 3: Geographic focus (loose keyword match)
    const geoText = g.geographicFocus.toLowerCase();
    const districtLower = this.preCheck.district.toLowerCase().trim();
    const nationalTerms = ['all', 'india', 'national', 'pan-india', 'pan india', 'any'];
    const geoPass = nationalTerms.some(t => geoText.includes(t)) || geoText.includes(districtLower);
    reasons.push({
      pass: geoPass,
      message: geoPass
        ? `Geographic focus covers your district/region`
        : `Grant focus (${g.geographicFocus}) may not cover your project district — verify with the programme guidelines`
    });

    // Rule 4: Project Duration
    const duration = Number(this.preCheck.durationMonths ?? 0);
    const durationPass = duration >= g.projectDurationMinMonths && duration <= g.projectDurationMaxMonths;
    reasons.push({
      pass: durationPass,
      message: durationPass
        ? `Project duration (${duration} months) is within the allowed range (${g.projectDurationMinMonths}–${g.projectDurationMaxMonths} months)`
        : `Project duration (${duration} months) is outside the allowed range (${g.projectDurationMinMonths}–${g.projectDurationMaxMonths} months)`
    });

    // Rule 5: Overhead Budget Cap (<= 15%)
    const overhead = Number(this.preCheck.overheadAmount ?? 0);
    const maxOverhead = requested * 0.15;
    const overheadPass = overhead <= maxOverhead;
    reasons.push({
      pass: overheadPass,
      message: overheadPass
        ? `Overhead budget (₹${this.formatLakh(overhead)}) is within the 15% cap (Max: ₹${this.formatLakh(maxOverhead)})`
        : `Overhead budget (₹${this.formatLakh(overhead)}) exceeds the 15% cap (Max: ₹${this.formatLakh(maxOverhead)})`
    });

    this.preCheckResult = {
      eligible: reasons.every(r => r.pass),
      reasons
    };
    this.showPreCheck = false;
    this.cdr.detectChanges();
  }

  formatLakh(value: number): string {
    return (value / 100000).toFixed(1) + ' Lakh';
  }

  formatCrore(value: number): string {
    return (value / 10000000).toFixed(1) + ' Crore';
  }
}
