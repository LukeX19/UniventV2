export interface UserResponse {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  birthday: string;
  pictureUrl?: string | null;
  role: number;
  year: number;
  isAccountConfirmed: boolean;
  universityId?: string | null;
}