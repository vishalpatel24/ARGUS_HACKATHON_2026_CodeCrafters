import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OrganisationService } from '../../services/organisation.service';
import { UpsertOrganisationDto } from '../../models/organisation.models';

@Component({
  selector: 'app-organisation-profile-page',
  standalone: false,
  templateUrl: './organisation-profile-page.html',
  styleUrls: ['./organisation-profile-page.scss']
})
export class OrganisationProfilePageComponent implements OnInit {
  profileForm: FormGroup;
  isLoading = true;
  isSaving = false;
  successMessage = '';
  errorMessage = '';

  organisationTypes = [
    'NGO', 'Trust', 'Section 8 Company', 'Society', 'FPO', 'University', 'Research Institution'
  ];

  indianStates = [
    'Andhra Pradesh', 'Arunachal Pradesh', 'Assam', 'Bihar', 'Chhattisgarh', 'Goa', 'Gujarat', 
    'Haryana', 'Himachal Pradesh', 'Jharkhand', 'Karnataka', 'Kerala', 'Madhya Pradesh', 
    'Maharashtra', 'Manipur', 'Meghalaya', 'Mizoram', 'Nagaland', 'Odisha', 'Punjab', 
    'Rajasthan', 'Sikkim', 'Tamil Nadu', 'Telangana', 'Tripura', 'Uttar Pradesh', 
    'Uttarakhand', 'West Bengal', 'Andaman and Nicobar Islands', 'Chandigarh', 
    'Dadra and Nagar Haveli and Daman and Diu', 'Delhi', 'Jammu and Kashmir', 
    'Ladakh', 'Lakshadweep', 'Puducherry'
  ];

  constructor(
    private fb: FormBuilder,
    private organisationService: OrganisationService,
    private cdr: ChangeDetectorRef
  ) {
    this.profileForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      registrationNumber: ['', [Validators.required, Validators.maxLength(100)]],
      type: ['', [Validators.required]],
      state: ['', [Validators.required]],
      annualBudget: [0, [Validators.required, Validators.min(0)]],
      contactPersonName: ['', [Validators.required, Validators.maxLength(200)]],
      contactPersonEmail: ['', [Validators.required, Validators.email]],
      contactPersonPhone: ['', [Validators.required, Validators.pattern('^[0-9]{10,20}$')]]
    });
  }

  ngOnInit(): void {
    this.organisationService.getMyOrganisation().subscribe({
      next: (org) => {
        if (org) {
          this.profileForm.patchValue(org);
        }
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  onSubmit(): void {
    if (this.profileForm.invalid) {
      return;
    }

    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';
    this.cdr.detectChanges();

    const dto: UpsertOrganisationDto = this.profileForm.value;

    this.organisationService.upsertMyOrganisation(dto).subscribe({
      next: () => {
        this.isSaving = false;
        this.successMessage = 'Organization profile updated successfully!';
        this.cdr.detectChanges();
        setTimeout(() => {
          this.successMessage = '';
          this.cdr.detectChanges();
        }, 3000);
      },
      error: (err) => {
        this.isSaving = false;
        this.errorMessage = err.error?.message || 'Failed to update organization profile.';
        this.cdr.detectChanges();
      }
    });
  }
}
