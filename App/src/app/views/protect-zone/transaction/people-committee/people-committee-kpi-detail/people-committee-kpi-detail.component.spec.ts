/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PeopleCommitteeKpiDetailComponent } from './people-committee-kpi-detail.component';

describe('PeopleCommitteeKpiDetailComponent', () => {
  let component: PeopleCommitteeKpiDetailComponent;
  let fixture: ComponentFixture<PeopleCommitteeKpiDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PeopleCommitteeKpiDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PeopleCommitteeKpiDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
