export type UserRole = 1 | 2;

export type UserStatus = 1 | 2 | 3;

export const UserRoleValue = {
  User: 1,
  Admin: 2,
} as const satisfies Record<string, UserRole>;

export const UserStatusValue = {
  Active: 1,
  Suspended: 2,
  Deleted: 3,
} as const satisfies Record<string, UserStatus>;

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
