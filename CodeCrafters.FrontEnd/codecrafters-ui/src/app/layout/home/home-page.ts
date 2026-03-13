import { Component, OnInit } from '@angular/core';
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

  constructor(private grantService: GrantService) {}

  ngOnInit(): void {
    this.grantService.getGrants().subscribe({
      next: (data) => this.grants = data,
      error: (err) => console.error('Failed to load grants', err)
    });
  }

  viewDetails(grantId: string): void {
    this.grantService.getGrantById(grantId).subscribe({
      next: (data) => this.selectedGrant = data,
      error: (err) => console.error('Failed to load grant details', err)
    });
  }

  closeDetails(): void {
    this.selectedGrant = null;
  }
}

