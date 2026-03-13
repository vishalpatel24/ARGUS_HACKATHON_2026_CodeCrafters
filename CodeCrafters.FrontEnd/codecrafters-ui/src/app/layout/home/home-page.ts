// Home page — grants listing & detail overlay
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GrantService, GrantTypeDto } from '../../features/grants/services/grant.service';

@Component({
  selector: 'app-home-page',
  standalone: false,
  templateUrl: './home-page.html',
  styleUrls: ['./home-page.scss']
})
export class HomePageComponent implements OnInit {
  grants: GrantTypeDto[] = [];
  selectedGrant: GrantTypeDto | null = null;
  isLoading = true;
  error: string | null = null;

  constructor(
    private grantService: GrantService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.grantService.getGrants().subscribe({
      next: (data) => {
        this.grants = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load grants', err);
        this.error = 'Unable to load grant programmes. Please try again.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  viewDetails(grantId: string): void {
    this.grantService.getGrantById(grantId).subscribe({
      next: (data) => {
        this.selectedGrant = data;
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Failed to load grant details', err)
    });
  }

  closeDetails(): void {
    this.selectedGrant = null;
  }

  closeOnBackdrop(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('detail-overlay')) {
      this.closeDetails();
    }
  }

  formatLakh(value: number): string {
    if (value >= 10_000_000) return `${(value / 10_000_000).toFixed(0)} Cr`;
    if (value >= 100_000)    return `${(value / 100_000).toFixed(0)} L`;
    return value.toLocaleString('en-IN');
  }

  formatCrore(value: number): string {
    if (value >= 10_000_000) return `${(value / 10_000_000).toFixed(0)} Crore`;
    if (value >= 100_000)    return `${(value / 100_000).toFixed(0)} Lakh`;
    return value.toLocaleString('en-IN');
  }
}
