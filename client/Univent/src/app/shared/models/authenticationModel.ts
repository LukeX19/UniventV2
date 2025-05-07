export interface LoginRequest {
  email: string;
  password: string;
};

export interface LoginResponse {
  token: string;
};

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  birthday: string;
  pictureURL?: string | null;
  email: string;
  password: string;
  role: number;
  year: number;
  universityId?: string | null;
};