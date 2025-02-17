export interface CasinoUser {
  userId?: number;
  username?: string;
  password?: string;
  email?: string;
  homePhoneNumber?: string;
  workPhoneNumber?: string;
  mobilePhoneNumber?: string;
  balance?: number;
}

export interface CreateUserRequest {
  username: string;
  password: string;
  email: string;
  homePhoneNumber?: string;
  workPhoneNumber?: string;
  mobilePhoneNumber?: string;
}

export interface UpdateBalanceResponse {
  userId: number;
  status: number;
  message?: string;
  updateAmount: number;
  balance: number;
}
