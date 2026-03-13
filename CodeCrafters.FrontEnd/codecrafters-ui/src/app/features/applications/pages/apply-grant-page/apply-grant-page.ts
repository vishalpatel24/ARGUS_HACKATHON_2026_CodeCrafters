import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { GrantService, GrantTypeDto } from '../../../grants/services/grant.service';
import { OrganisationService } from '../../../organisations/services/organisation.service';
import { ApplicationService } from '../../services/application.service';
import { CreateApplicationDto } from '../../models/application.models';
import { OrganisationDto } from '../../../organisations/models/organisation.models';

@Component({
  selector: 'app-apply-grant-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './apply-grant-page.html',
  styleUrls: ['./apply-grant-page.scss']
})
export class ApplyGrantPageComponent implements OnInit {
  grantId: string | null = null;
  grant: GrantTypeDto | null = null;
  allGrants: GrantTypeDto[] = [];
  applyForm: FormGroup;
  currentStep = 0; // Step 0: Select Grant Form
  isLoading = true;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private grantService: GrantService,
    private organisationService: OrganisationService,
    private applicationService: ApplicationService,
    private cdr: ChangeDetectorRef
  ) {
    this.applyForm = this.fb.group({
      grantTypeId: [null, Validators.required],
      // Step 1: Basic Info (Pre-filled from Org)
      legalName: ['', Validators.required],
      registrationNumber: ['', Validators.required],
      organisationType: ['', Validators.required],
      yearOfEstablishment: [null, Validators.required],
      stateOfRegistration: ['', Validators.required],
      primaryContactName: ['', Validators.required],
      primaryContactEmail: ['', [Validators.required, Validators.email]],
      primaryContactPhone: ['', Validators.required],
      annualOperatingBudget: [0, [Validators.required, Validators.min(0)]],

      // Step 2: Project Details
      title: ['', [Validators.required, Validators.maxLength(200)]],
      projectTitle: ['', Validators.required],
      requestedAmount: [0, [Validators.required, Validators.min(1)]],
      projectStartDate: [null],
      projectEndDate: [null],
      
      // Feature Specifics
      problemStatement: [''],
      proposedSolution: [''],
      targetBeneficiariesCount: [0],
      innovationType: [''],
      innovationDescription: [''],
      thematicArea: [''],
      environmentalProblemDesc: [''],

      // Step 3: Budget
      personnelCosts: [0, Validators.min(0)],
      equipmentAndMaterials: [0, Validators.min(0)],
      travelAndLogistics: [0, Validators.min(0)],
      trainingAndWorkshops: [0, Validators.min(0)],
      technologySoftware: [0, Validators.min(0)],
      contentDevelopment: [0, Validators.min(0)],
      saplingsAndSeeds: [0, Validators.min(0)],
      communityEngagementWages: [0, Validators.min(0)],
      overheads: [0, Validators.min(0)],
      otherCosts: [0, Validators.min(0)],
      budgetJustification: [''],

      // Step 4: Declaration
      authorisedSignatoryName: ['', Validators.required],
      designation: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.grantId = this.route.snapshot.paramMap.get('id');
    this.loadInitialData();
  }

  loadInitialData(): void {
    this.isLoading = true;
    
    // Fetch all grants for Step 0
    this.grantService.getGrants().subscribe({
      next: (grants) => {
        this.allGrants = grants;
        
        if (this.grantId) {
          const selected = grants.find(g => g.id === this.grantId);
          if (selected) {
            this.selectGrant(selected);
          } else {
            this.isLoading = false;
            this.cdr.detectChanges();
          }
        } else {
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });

    // Also pre-load organisation profile
    this.organisationService.getMyOrganisation().subscribe({
      next: (org: OrganisationDto) => {
        if (org) {
          this.applyForm.patchValue({
            legalName: org.name,
            registrationNumber: org.registrationNumber,
            organisationType: org.type,
            yearOfEstablishment: org.yearOfEstablishment,
            stateOfRegistration: org.state,
            primaryContactName: org.contactPersonName,
            primaryContactEmail: org.contactPersonEmail,
            primaryContactPhone: org.contactPersonPhone,
            annualOperatingBudget: org.annualBudget
          });
        }
      }
    });
  }

  selectGrant(grant: GrantTypeDto): void {
    this.grant = grant;
    this.applyForm.patchValue({ grantTypeId: grant.id });
    this.currentStep = 1; // Move to Step 1: Info
    this.isLoading = false;
    this.cdr.detectChanges();
  }

  nextStep(): void {
    if (this.currentStep < 4) {
      this.currentStep++;
      this.cdr.detectChanges();
    }
  }

  prevStep(): void {
    if (this.currentStep > 0) {
      this.currentStep--;
      this.cdr.detectChanges();
    }
  }

  onSubmit(): void {
    if (this.applyForm.invalid) {
      this.applyForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const dto: CreateApplicationDto = {
      ...this.applyForm.value
    };

    this.applicationService.submitApplication(dto).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.isSubmitting = false;
        alert(err.error?.message || 'Failed to submit application.');
        this.cdr.detectChanges();
      }
    });
  }
}
