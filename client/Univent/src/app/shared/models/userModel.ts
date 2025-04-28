export interface UserResponse {
  id: string;
  firstName: string;
  lastName: string;
  pictureUrl?: string | null;
  isAccountConfirmed: boolean;
};

export interface UserProfileResponse {
  id: string;
  firstName: string;
  lastName: string;
  birthday: string;
  pictureUrl?: string | null;
  createdAt: string;
  year: number;
  universityName: string;
  rating: number;
  createdEvents: number;
  participations: number;
};