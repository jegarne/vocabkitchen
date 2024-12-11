export interface IUserAccess {
  isAdmin: boolean;
  isTeacher: boolean;
  isStudent: boolean;
  adminOrgIds: string[];
  teacherOrgIds: string[];
  studentOrgIds: string[];
}
