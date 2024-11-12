import { Role } from "./role.model";

export interface AuthenticatedUser {
  name: string,
  username: string,
  email: string,
  role: Role
}
