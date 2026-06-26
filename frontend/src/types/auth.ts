export type UserRole = 1 | 2;

export type UserStatus = 1 | 2 | 3;

export interface UserInfo {
  id: string;
  email: string;
  fullName: string;
  avatarUrl: string | null;
  role: UserRole;
  status: UserStatus;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  user: Pick<UserInfo, "id" | "email" | "fullName" | "role">;
}

export interface RefreshTokenResponse {
  accessToken: string;
}
