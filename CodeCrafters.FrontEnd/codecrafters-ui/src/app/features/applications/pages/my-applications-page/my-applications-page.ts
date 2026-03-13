import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { ApplicationService } from '../../services/application.service';
import { ApplicationListItemDto } from '../../models/application.models';

@Component({
  selector: 'app-my-applications-page',
  standalone: false,
  templateUrl: './my-applications-page.html',
  styleUrls: ['./my-applications-page.scss']
})
export class MyApplicationsPageComponent implements OnInit {
  applications: ApplicationListItemDto[] = [];
  isLoading = true;
  error: string | null = null;

  constructor(
    private applicationService: ApplicationService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadApplications();
  }

  loadApplications(): void {
    this.isLoading = true;
    this.error = null;
    this.applicationService.getMyApplications().subscribe({
      next: (list) => {
        this.applications = list;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = err?.error?.message ?? 'Failed to load applications.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  formatDate(iso: string): string {
    if (!iso) return '—';
    const d = new Date(iso);
    return d.toLocaleDateString(undefined, { dateStyle: 'medium' }) + ' ' + d.toLocaleTimeString(undefined, { timeStyle: 'short' });
  }

  goToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  startNewApplication(): void {
    this.router.navigate(['/applications/new']);
  }
}
