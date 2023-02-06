export interface IUser{
  userName: string;
  userEmail?: string;
  password: string;
  mobile?: number;
}

export interface IUserLogin{
  userName: string;
  password: string;
  token: string;
}
