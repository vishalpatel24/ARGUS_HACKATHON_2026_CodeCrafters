export interface OrganisationDto {
  id: string;
  userId: string;
  name: string;
  registrationNumber: string;
  type: string;
  state: string;
  annualBudget: number;
  contactPersonName: string;
  contactPersonEmail: string;
  contactPersonPhone: string;
  yearOfEstablishment: number;
  isProfileComplete: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface UpsertOrganisationDto {
  name: string;
  registrationNumber: string;
  type: string;
  state: string;
  annualBudget: number;
  contactPersonName: string;
  contactPersonEmail: string;
  contactPersonPhone: string;
  yearOfEstablishment: number;
}
