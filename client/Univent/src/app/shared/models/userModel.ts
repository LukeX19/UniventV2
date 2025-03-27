export interface UserResponse {
  id: string;
  firstName: string;
  lastName: string;
  pictureUrl?: string | null;
  role: number;
  isAccountConfirmed: boolean;
};

export interface UserProfileResponse {
  id: string;
  firstName: string;
  lastName: string;
  birthday: string;
  pictureUrl?: string | null;
  role: number;
  createdAt: string;
  year: number;
  universityName: string;
  rating: number;
};