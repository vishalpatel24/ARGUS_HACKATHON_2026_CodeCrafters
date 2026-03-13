export interface ScreeningCheckDto {
  id: string;
  checkCode: string;
  checkName: string;
  checkType: string;   // Hard | Soft
  result: string;      // Pass | Fail | Flag
  details: string | null;
  aiScore: number | null;
  displayOrder: number;
}

export interface ScreeningReportDto {
  id: string;
  applicationId: string;
  applicantName: string;
  organisationName: string;
  grantTypeName: string;
  overallResult: string;       // Eligible | Ineligible | PendingReview
  hardChecksPassed: number;
  hardChecksFailed: number;
  softFlags: number;
  officerAction: string | null;
  officerActionReason: string | null;
  reviewedByName: string | null;
  reviewedAt: string | null;
  generatedAt: string;
  checks: ScreeningCheckDto[];
}

export interface ScreeningActionDto {
  action: string;   // ConfirmEligible | OverrideIneligible | ClarificationRequested
  reason?: string;
}
