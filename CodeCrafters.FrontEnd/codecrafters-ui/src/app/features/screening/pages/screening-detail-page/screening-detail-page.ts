import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ScreeningService } from '../../services/screening.service';
import { ScreeningReportDto, ScreeningActionDto } from '../../models/screening.models';

@Component({
  selector: 'app-screening-detail-page',
  standalone: false,
  templateUrl: './screening-detail-page.html',
  styleUrls: ['./screening-detail-page.scss']
})
export class ScreeningDetailPageComponent implements OnInit {
  report: ScreeningReportDto | null = null;
  isLoading = true;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  actionReason = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private screeningService: ScreeningService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.router.navigate(['/screening']);
      return;
    }
    this.loadReport(id);
  }

  loadReport(applicationId: string): void {
    this.screeningService.getByApplication(applicationId).subscribe({
      next: (report) => {
        this.report = report;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load screening report.';
        this.cdr.detectChanges();
      }
    });
  }

  get hardChecks() {
    return this.report?.checks.filter(c => c.checkType === 'Hard') || [];
  }

  get softChecks() {
    return this.report?.checks.filter(c => c.checkType === 'Soft') || [];
  }

  getResultClass(result: string): string {
    switch (result) {
      case 'Pass': return 'result-pass';
      case 'Fail': return 'result-fail';
      case 'Flag': return 'result-flag';
      default: return '';
    }
  }

  takeAction(action: string): void {
    if (!this.report) return;

    if (action !== 'ConfirmEligible' && !this.actionReason.trim()) {
      this.errorMessage = 'Please provide a reason for this action.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.cdr.detectChanges();

    const dto: ScreeningActionDto = { action, reason: this.actionReason || undefined };

    this.screeningService.takeAction(this.report.id, dto).subscribe({
      next: (updated) => {
        this.report = updated;
        this.isSubmitting = false;
        this.successMessage = `Action "${action}" applied successfully.`;
        this.cdr.detectChanges();
        setTimeout(() => { this.successMessage = ''; this.cdr.detectChanges(); }, 3000);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || err.error || 'Action failed.';
        this.cdr.detectChanges();
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/screening']);
  }
}
