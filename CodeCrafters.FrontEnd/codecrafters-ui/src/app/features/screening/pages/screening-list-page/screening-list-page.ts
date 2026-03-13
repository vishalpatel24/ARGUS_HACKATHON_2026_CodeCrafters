import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { ScreeningService } from '../../services/screening.service';
import { ScreeningReportDto } from '../../models/screening.models';

@Component({
  selector: 'app-screening-list-page',
  standalone: false,
  templateUrl: './screening-list-page.html',
  styleUrls: ['./screening-list-page.scss']
})
export class ScreeningListPageComponent implements OnInit {
  reports: ScreeningReportDto[] = [];
  isLoading = true;
  isSyncing = false;
  errorMessage = '';
  successMessage = '';
  activeFilter = '';

  filters = [
    { value: '', label: 'All' },
    { value: 'PendingReview', label: 'Pending Review' },
    { value: 'Eligible', label: 'Eligible' },
    { value: 'Ineligible', label: 'Ineligible' }
  ];

  constructor(
    private screeningService: ScreeningService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(filter?: string): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.activeFilter = filter || '';
    this.screeningService.getAll(filter).subscribe({
      next: (reports) => {
        this.reports = reports;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load screening reports.';
        this.cdr.detectChanges();
      }
    });
  }

  syncAll(): void {
    this.isSyncing = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.screeningService.syncAll().subscribe({
      next: () => {
        this.isSyncing = false;
        this.successMessage = 'Emergency sync complete! All pending screenings have been processed.';
        this.loadReports(this.activeFilter);
        this.cdr.detectChanges();
      },
      error: () => {
        this.isSyncing = false;
        this.errorMessage = 'Failed to run emergency sync.';
        this.cdr.detectChanges();
      }
    });
  }

  viewDetail(report: ScreeningReportDto): void {
    this.router.navigate(['/screening', report.id]);
  }

  getResultBadgeClass(result: string): string {
    switch (result) {
      case 'Eligible': return 'badge-success';
      case 'Ineligible': return 'badge-danger';
      default: return 'badge-warning';
    }
  }
}
