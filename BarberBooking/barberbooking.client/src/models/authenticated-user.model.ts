import { Role } from "./role.model";

export interface AuthenticatedUser {
  name: string,
  surname: string,
  email: string,
  role: Role
}
