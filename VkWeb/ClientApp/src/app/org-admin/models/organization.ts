import { OrgTeacher } from "./org-teacher";
import { OrgStudent } from "./org-student";

class Organization {
  //constructor(type: string) {
  //  this.name = type;
  //  this.inputText = input;
  //}
  id: string = '';
  name: string = '';
  teachers: OrgTeacher[];
  students: OrgStudent[];
}

export {
  Organization
};
