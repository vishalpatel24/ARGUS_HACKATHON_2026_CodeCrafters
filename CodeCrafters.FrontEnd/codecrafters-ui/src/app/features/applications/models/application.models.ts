export interface ApplicationListItemDto {
  id: string;
  referenceNumber: string;
  title: string;
  grantTypeName: string;
  statusLabel: string;
  currentStageName: string;
  lastUpdated: string;
  submissionDate?: string;
}

export interface CreateApplicationDto {
  grantTypeId: string;
  title: string;
}

export interface ApplicationDraftDto {
  id: string;
  referenceNumber: string;
  grantTypeId: string;
  grantTypeName: string;
  title: string;
  statusLabel: string;
  legalName?: string;
  registrationNumber?: string;
  organisationType?: string;
  yearOfEstablishment: number;
  stateOfRegistration?: string;
  primaryContactName?: string;
  primaryContactEmail?: string;
  primaryContactPhone?: string;
  annualOperatingBudget: number;
  projectTitle?: string;
  totalRequestedAmount: number;
  projectStartDate?: string;
  projectEndDate?: string;
  personnelCosts: number;
  equipmentAndMaterials: number;
  travelAndLogistics: number;
  overheads: number;
  otherCosts: number;
  budgetJustification?: string;
  authorisedSignatoryName?: string;
  designation?: string;
  problemStatement?: string;
  proposedSolution?: string;
  targetBeneficiariesCount?: number;
  expectedOutcomes?: string;
}

export interface UpdateApplicationDto {
  title?: string;
  legalName?: string;
  registrationNumber?: string;
  organisationType?: string;
  yearOfEstablishment?: number;
  stateOfRegistration?: string;
  primaryContactName?: string;
  primaryContactEmail?: string;
  primaryContactPhone?: string;
  annualOperatingBudget?: number;
  projectTitle?: string;
  totalRequestedAmount?: number;
  projectStartDate?: string;
  projectEndDate?: string;
  personnelCosts?: number;
  equipmentAndMaterials?: number;
  travelAndLogistics?: number;
  overheads?: number;
  otherCosts?: number;
  budgetJustification?: string;
  authorisedSignatoryName?: string;
  designation?: string;
  problemStatement?: string;
  proposedSolution?: string;
  targetBeneficiariesCount?: number;
  expectedOutcomes?: string;
}
