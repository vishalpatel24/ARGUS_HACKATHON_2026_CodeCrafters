import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApplicationService } from '../../services/application.service';
import { GrantService, GrantTypeDto } from '../../../grants/services/grant.service';

@Component({
  selector: 'app-application-new-page',
  standalone: false,
  templateUrl: './application-new-page.html',
  styleUrls: ['./application-new-page.scss']
})
export class ApplicationNewPageComponent implements OnInit {
  form: FormGroup;
  grants: GrantTypeDto[] = [];
  isLoadingGrants = true;
  isSubmitting = false;
  error = '';

  constructor(
    private fb: FormBuilder,
    private applicationService: ApplicationService,
    private grantService: GrantService,
    private router: Router
  ) {
    this.form = this.fb.group({
      grantTypeId: ['', Validators.required],
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]]
    });
  }

  ngOnInit(): void {
    this.grantService.getGrants().subscribe({
      next: (list) => {
        this.grants = list;
        this.isLoadingGrants = false;
      },
      error: () => {
        this.error = 'Failed to load grant programmes.';
        this.isLoadingGrants = false;
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.isSubmitting) return;
    this.isSubmitting = true;
    this.error = '';
    const dto = {
      grantTypeId: this.form.get('grantTypeId')?.value,
      title: this.form.get('title')?.value?.trim() || 'Untitled'
    };
    this.applicationService.createDraft(dto).subscribe({
      next: (app) => {
        this.router.navigate(['/applications', app.id, 'edit']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to create application.';
        this.isSubmitting = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/applications']);
  }
}
