import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { OrgService } from '@services/org.service';
import { Tag, Org } from '@models/org';
import { FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { zip } from 'rxjs';
import { StudentService } from '@services/student.service';

@Component({
  selector: 'tag-students',
  templateUrl: './tag-students.component.html',
  styleUrls: ['./tag-students.component.css']
})
export class TagStudentsComponent implements OnInit {

  private sub = new Subscription();

  orgId: string;
  tagId: string;
  tag: Tag = new Tag();
  org: Org = new Org();
  areAllSelected = false;

  studentsFormGroup: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private orgService: OrgService,
    private studentService: StudentService,
    private formBuilder: FormBuilder,
    private location: Location
  ) {
  }

  ngOnInit() {
    this.studentsFormGroup = this.formBuilder.group({
      students: this.formBuilder.array([])
    });

    this.sub.add(this.route.params.subscribe(params => {
      this.orgId = params['orgId'];
      this.tagId = params['tagId'];
      zip(this.orgService.get(this.orgId),
        this.orgService.getTaggedStudents(this.orgId, this.tagId))
        .subscribe(([org, taggedStudents]) => {
          this.org = org as Org;
          this.tag = this.org.tags.find(x => x.id === this.tagId);
          this.buildCheckboxes(this.org.students, taggedStudents);
        });
    }));
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  back() {
    this.location.back();
  }

  buildCheckboxes(orgStudents, taggedStudents) {
    const formArray = this.studentsFormGroup.get('students') as FormArray;
    let areAllChecked = true;
    orgStudents.forEach(orgStudent => {
      if (taggedStudents.some(r => r.id === orgStudent.id)) {
        formArray.push(new FormControl(true));
      } else {
        formArray.push(new FormControl(false));
        areAllChecked = false;
      }
    });
    this.areAllSelected = areAllChecked;
  }

  toggleCheckboxes($event) {
    this.areAllSelected = $event.checked;
    this.studentsFormGroup.controls['students'].setValue(
      this.studentsFormGroup.controls['students'].value
        .map(value => this.areAllSelected));
  }

  saveSelectedStudents() {
    const selectedIds = this.studentsFormGroup.value.students.reduce(
      (accumulator, checked, i) => {
        if (checked) {
          accumulator.push(this.org.students[i].id);
        }
        return accumulator;
      },
      []
    );
    this.studentService.mergeTags(this.orgId, this.tagId, selectedIds)
      .subscribe(result => {
        this.location.back();
      });
  }

}
