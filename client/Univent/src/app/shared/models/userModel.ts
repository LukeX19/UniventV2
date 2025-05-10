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
  universityId: string;
  universityName: string;
  rating: number;
  createdEvents: number;
  participations: number;
};

export interface UserManagementResponse {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  createdAt: string;
  year: number;
  universityName: string;
  isAccountConfirmed: boolean;
  isAccountBanned: boolean;
};

export interface UpdateUserProfileRequest {
  firstName: string;
  lastName: string;
  birthday: string;
  pictureUrl?: string | null;
  year: number;
  universityId: string;
};