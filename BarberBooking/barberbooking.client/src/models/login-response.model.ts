import { AuthenticatedUser } from "./authenticated-user.model";

export interface LoginResponse {
  user: AuthenticatedUser,
  token:string
}
