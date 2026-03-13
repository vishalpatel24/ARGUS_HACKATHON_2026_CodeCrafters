import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationService } from '../../services/application.service';
import { OrganisationService } from '../../../organisations/services/organisation.service';
import { ApplicationDraftDto, UpdateApplicationDto } from '../../models/application.models';

const STEPS = [
  { id: 1, label: 'Organisation' },
  { id: 2, label: 'Project' },
  { id: 3, label: 'Team' },
  { id: 4, label: 'Budget' },
  { id: 5, label: 'Documents' },
  { id: 6, label: 'Review & Submit' }
];

@Component({
  selector: 'app-application-wizard-page',
  standalone: false,
  templateUrl: './application-wizard-page.html',
  styleUrls: ['./application-wizard-page.scss']
})
export class ApplicationWizardPageComponent implements OnInit {
  applicationId: string | null = null;
  draft: ApplicationDraftDto | null = null;
  form: FormGroup;
  steps = STEPS;
  currentStep = 1;
  isLoading = true;
  isSaving = false;
  isSubmitting = false;
  error = '';

  organisationTypes = ['NGO', 'Trust', 'Section 8 Company', 'Society', 'FPO', 'University', 'Research Institution'];
  indianStates = [
    'Andhra Pradesh', 'Arunachal Pradesh', 'Assam', 'Bihar', 'Chhattisgarh', 'Goa', 'Gujarat',
    'Haryana', 'Himachal Pradesh', 'Jharkhand', 'Karnataka', 'Kerala', 'Madhya Pradesh',
    'Maharashtra', 'Manipur', 'Meghalaya', 'Mizoram', 'Nagaland', 'Odisha', 'Punjab',
    'Rajasthan', 'Sikkim', 'Tamil Nadu', 'Telangana', 'Tripura', 'Uttar Pradesh', 'Uttarakhand',
    'West Bengal', 'Andaman and Nicobar Islands', 'Chandigarh', 'Dadra and Nagar Haveli and Daman and Diu',
    'Delhi', 'Jammu and Kashmir', 'Ladakh', 'Lakshadweep', 'Puducherry'
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private applicationService: ApplicationService,
    private organisationService: OrganisationService
  ) {
    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      legalName: ['', [Validators.required, Validators.maxLength(300)]],
      registrationNumber: ['', [Validators.required, Validators.maxLength(100)]],
      organisationType: ['', Validators.required],
      yearOfEstablishment: [null as number | null, [Validators.required, Validators.min(1900), Validators.max(2100)]],
      stateOfRegistration: ['', Validators.required],
      primaryContactName: ['', [Validators.required, Validators.maxLength(200)]],
      primaryContactEmail: ['', [Validators.required, Validators.email]],
      primaryContactPhone: ['', [Validators.required, Validators.pattern(/^[0-9]{10,20}$/)]],
      annualOperatingBudget: [0, [Validators.required, Validators.min(0)]],
      projectTitle: ['', [Validators.required, Validators.maxLength(300)]],
      totalRequestedAmount: [0, [Validators.required, Validators.min(0)]],
      projectStartDate: [null as string | null],
      projectEndDate: [null as string | null],
      problemStatement: [''],
      proposedSolution: [''],
      targetBeneficiariesCount: [null as number | null],
      expectedOutcomes: [''],
      personnelCosts: [0, [Validators.min(0)]],
      equipmentAndMaterials: [0, [Validators.min(0)]],
      travelAndLogistics: [0, [Validators.min(0)]],
      overheads: [0, [Validators.min(0)]],
      otherCosts: [0, [Validators.min(0)]],
      budgetJustification: [''],
      authorisedSignatoryName: ['', [Validators.required, Validators.maxLength(200)]],
      designation: ['', [Validators.required, Validators.maxLength(100)]],
      declarationAccepted: [false, Validators.requiredTrue]
    });
  }

  get declarationAccepted(): boolean {
    return !!this.form.get('declarationAccepted')?.value;
  }

  ngOnInit(): void {
    this.applicationId = this.route.snapshot.paramMap.get('id');
    if (!this.applicationId) {
      this.router.navigate(['/applications']);
      return;
    }
    this.applicationService.getDraft(this.applicationId).subscribe({
      next: (draft) => {
        this.draft = draft;
        this.form.patchValue({
          title: draft.title,
          legalName: draft.legalName ?? '',
          registrationNumber: draft.registrationNumber ?? '',
          organisationType: draft.organisationType ?? '',
          yearOfEstablishment: draft.yearOfEstablishment || null,
          stateOfRegistration: draft.stateOfRegistration ?? '',
          primaryContactName: draft.primaryContactName ?? '',
          primaryContactEmail: draft.primaryContactEmail ?? '',
          primaryContactPhone: draft.primaryContactPhone ?? '',
          annualOperatingBudget: draft.annualOperatingBudget ?? 0,
          projectTitle: draft.projectTitle ?? '',
          totalRequestedAmount: draft.totalRequestedAmount ?? 0,
          projectStartDate: draft.projectStartDate ? draft.projectStartDate.slice(0, 10) : null,
          projectEndDate: draft.projectEndDate ? draft.projectEndDate.slice(0, 10) : null,
          problemStatement: draft.problemStatement ?? '',
          proposedSolution: draft.proposedSolution ?? '',
          targetBeneficiariesCount: draft.targetBeneficiariesCount ?? null,
          expectedOutcomes: draft.expectedOutcomes ?? '',
          personnelCosts: draft.personnelCosts ?? 0,
          equipmentAndMaterials: draft.equipmentAndMaterials ?? 0,
          travelAndLogistics: draft.travelAndLogistics ?? 0,
          overheads: draft.overheads ?? 0,
          otherCosts: draft.otherCosts ?? 0,
          budgetJustification: draft.budgetJustification ?? '',
          authorisedSignatoryName: draft.authorisedSignatoryName ?? '',
          designation: draft.designation ?? ''
        });
        this.isLoading = false;
        this.tryPreFillFromOrganisation();
      },
      error: () => {
        this.error = 'Application not found or not a draft.';
        this.isLoading = false;
      }
    });
  }

  tryPreFillFromOrganisation(): void {
    this.organisationService.getMyOrganisation().subscribe({
      next: (org) => {
        if (org && !this.form.get('legalName')?.value) {
          this.form.patchValue({
            legalName: org.name ?? '',
            registrationNumber: org.registrationNumber ?? '',
            organisationType: org.type ?? '',
            stateOfRegistration: org.state ?? '',
            annualOperatingBudget: org.annualBudget ?? 0,
            primaryContactName: org.contactPersonName ?? '',
            primaryContactEmail: org.contactPersonEmail ?? '',
            primaryContactPhone: org.contactPersonPhone ?? ''
          });
        }
      },
      error: () => {}
    });
  }

  get budgetTotal(): number {
    const p = this.form.get('personnelCosts')?.value ?? 0;
    const e = this.form.get('equipmentAndMaterials')?.value ?? 0;
    const t = this.form.get('travelAndLogistics')?.value ?? 0;
    const o = this.form.get('overheads')?.value ?? 0;
    const ot = this.form.get('otherCosts')?.value ?? 0;
    return Number(p) + Number(e) + Number(t) + Number(o) + Number(ot);
  }

  get budgetMatchesTotal(): boolean {
    const total = this.form.get('totalRequestedAmount')?.value ?? 0;
    return this.budgetTotal > 0 && Math.abs(this.budgetTotal - total) < 0.01;
  }

  getStepFormValid(step: number): boolean {
    switch (step) {
      case 1:
        return !!(this.form.get('legalName')?.valid && this.form.get('registrationNumber')?.valid &&
          this.form.get('organisationType')?.valid && this.form.get('yearOfEstablishment')?.valid &&
          this.form.get('stateOfRegistration')?.valid && this.form.get('primaryContactName')?.valid &&
          this.form.get('primaryContactEmail')?.valid && this.form.get('primaryContactPhone')?.valid &&
          this.form.get('annualOperatingBudget')?.valid);
      case 2:
        return !!(this.form.get('projectTitle')?.valid && this.form.get('totalRequestedAmount')?.valid);
      case 3:
        return !!(this.form.get('authorisedSignatoryName')?.valid && this.form.get('designation')?.valid);
      case 4:
        return this.budgetMatchesTotal;
      case 5:
        return true;
      case 6:
        return !!this.form.get('declarationAccepted')?.value;
      default:
        return false;
    }
  }

  canProceed(step: number): boolean {
    return this.getStepFormValid(step);
  }

  next(): void {
    if (this.currentStep < 6 && this.canProceed(this.currentStep)) {
      this.saveCurrentStep();
      this.currentStep++;
      this.error = '';
    }
  }

  prev(): void {
    if (this.currentStep > 1) {
      this.currentStep--;
      this.error = '';
    }
  }

  goToStep(step: number): void {
    if (step >= 1 && step <= 6 && step <= this.currentStep) {
      this.currentStep = step;
    }
  }

  buildUpdateDto(): UpdateApplicationDto {
    const v = this.form.value;
    return {
      title: v.title,
      legalName: v.legalName,
      registrationNumber: v.registrationNumber,
      organisationType: v.organisationType,
      yearOfEstablishment: v.yearOfEstablishment,
      stateOfRegistration: v.stateOfRegistration,
      primaryContactName: v.primaryContactName,
      primaryContactEmail: v.primaryContactEmail,
      primaryContactPhone: v.primaryContactPhone,
      annualOperatingBudget: v.annualOperatingBudget,
      projectTitle: v.projectTitle,
      totalRequestedAmount: v.totalRequestedAmount,
      projectStartDate: v.projectStartDate || undefined,
      projectEndDate: v.projectEndDate || undefined,
      problemStatement: v.problemStatement || undefined,
      proposedSolution: v.proposedSolution || undefined,
      targetBeneficiariesCount: v.targetBeneficiariesCount ?? undefined,
      expectedOutcomes: v.expectedOutcomes || undefined,
      personnelCosts: v.personnelCosts,
      equipmentAndMaterials: v.equipmentAndMaterials,
      travelAndLogistics: v.travelAndLogistics,
      overheads: v.overheads,
      otherCosts: v.otherCosts,
      budgetJustification: v.budgetJustification || undefined,
      authorisedSignatoryName: v.authorisedSignatoryName,
      designation: v.designation
    };
  }

  saveCurrentStep(): void {
    if (!this.applicationId) return;
    this.isSaving = true;
    this.applicationService.updateDraft(this.applicationId, this.buildUpdateDto()).subscribe({
      next: () => { this.isSaving = false; },
      error: () => {
        this.error = 'Failed to save. Please try again.';
        this.isSaving = false;
      }
    });
  }

  submitApplication(): void {
    if (!this.applicationId || !this.form.get('declarationAccepted')?.value) return;
    this.isSubmitting = true;
    this.error = '';
    this.applicationService.updateDraft(this.applicationId, this.buildUpdateDto()).subscribe({
      next: () => {
        this.applicationService.submit(this.applicationId!).subscribe({
          next: () => this.router.navigate(['/applications']),
          error: (err) => {
            this.error = err.error?.message || 'Submission failed.';
            this.isSubmitting = false;
          }
        });
      },
      error: () => {
        this.error = 'Failed to save before submit.';
        this.isSubmitting = false;
      }
    });
  }

  backToList(): void {
    this.router.navigate(['/applications']);
  }
}
