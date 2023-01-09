/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TrackingAppraisalProgressDetailComponent } from './tracking-appraisal-progress-detail.component';

describe('TrackingAppraisalProgressDetailComponent', () => {
  let component: TrackingAppraisalProgressDetailComponent;
  let fixture: ComponentFixture<TrackingAppraisalProgressDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrackingAppraisalProgressDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrackingAppraisalProgressDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
