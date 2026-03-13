export interface UserResponseDto {
  id: string;
  name: string;
  organisationName?: string;
  email: string;
  phone?: string;
  role: string;
  isActive: boolean;
  createdAt: string;
}

export interface LoginResponseDto {
  token: string;
  user: UserResponseDto;
}
