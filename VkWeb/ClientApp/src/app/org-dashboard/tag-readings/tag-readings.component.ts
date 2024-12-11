import { Component, OnInit, OnDestroy } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { OrgService } from '@services/org.service';
import { ReadingInfo, Tag, Org } from '@models/org';
import { FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { zip } from 'rxjs';
import { ReadingService } from '@services/reading.service';

@Component({
  selector: 'tag-readings',
  templateUrl: './tag-readings.component.html',
  styleUrls: ['./tag-readings.component.css']
})
export class TagReadingsComponent implements OnInit, OnDestroy {
  private sub = new Subscription();

  orgId: string;
  tagId: string;
  tagReadings: ReadingInfo[];
  tag: Tag = new Tag();
  org: Org = new Org();

  readingsFormGroup: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private orgService: OrgService,
    private readingService: ReadingService,
    private formBuilder: FormBuilder,
    private location: Location
  ) {
  }

  ngOnInit() {
    this.readingsFormGroup = this.formBuilder.group({
      readings: this.formBuilder.array([])
    });

    this.sub.add(this.route.params.subscribe(params => {
      this.orgId = params['orgId'];
      this.tagId = params['tagId'];
      zip(this.orgService.get(this.orgId),
        this.orgService.getTaggedReadings(this.orgId, this.tagId))
        .subscribe(([org, taggedReadings]) => {
          this.org = org as Org;
          this.tag = this.org.tags.find(x => x.id === this.tagId);
          this.tagReadings = taggedReadings as ReadingInfo[];
          this.buildCheckboxes(this.org.readings, this.tagReadings);
        });
    }));
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  back() {
    this.location.back();
  }

  buildCheckboxes(orgReadings, tabReadings) {
    const formArray = this.readingsFormGroup.get('readings') as FormArray;
    orgReadings.forEach(orgReading => {
      if (tabReadings.some(r => r.id === orgReading.id)) {
        formArray.push(new FormControl(true));
      } else {
        formArray.push(new FormControl(false));
      }
    });
  }

  saveSelectedReadings() {
    const selectedReadingIds = this.readingsFormGroup.value.readings.reduce(
      (accumulator, checked, i) => {
        if (checked) {
          accumulator.push(this.org.readings[i].id);
        }
        return accumulator;
      },
      []
    );
    this.readingService.mergeTags(this.orgId, this.tagId, selectedReadingIds)
      .subscribe(result => {
        this.location.back();
      });
  }
}
