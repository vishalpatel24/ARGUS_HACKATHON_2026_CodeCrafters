export interface ApplicationResponseDto {
  id: string;
  referenceNumber: string;
  title: string;
  statusLabel: string;
  requestedAmount: number;
  submissionDate: string;
  grantTypeName: string;
  currentStageId: string;
}

export interface CreateApplicationDto {
  grantTypeId: string;
  title: string;
  requestedAmount: number;
  legalName: string;
  registrationNumber: string;
  organisationType: string;
  yearOfEstablishment: number;
  stateOfRegistration: string;
  primaryContactName: string;
  primaryContactEmail: string;
  primaryContactPhone: string;
  annualOperatingBudget: number;
  projectTitle: string;
  projectStartDate?: string;
  projectEndDate?: string;
  personnelCosts: number;
  equipmentAndMaterials: number;
  travelAndLogistics: number;
  trainingAndWorkshops: number;
  technologySoftware: number;
  contentDevelopment: number;
  saplingsAndSeeds: number;
  communityEngagementWages: number;
  overheads: number;
  otherCosts: number;
  budgetJustification?: string;
  authorisedSignatoryName: string;
  designation: string;
  // Dynamic fields
  problemStatement?: string;
  proposedSolution?: string;
  targetBeneficiariesCount?: number;
  innovationType?: string;
  innovationDescription?: string;
  thematicArea?: string;
  environmentalProblemDesc?: string;
}
