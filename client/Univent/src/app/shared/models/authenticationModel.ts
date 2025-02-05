export interface LoginRequest {
  email: string;
  password: string;
};

export interface LoginResponse {
  userId: string;
  token: string;
}