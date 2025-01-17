export interface RestartPasswordModel {
  userId: number;
  passwordToken: string | null;
  newPassword: string;
  confirmNewPassword: string;
};
