/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { L2ScoreAttitudeComponent } from './l2-score-attitude.component';

describe('L2ScoreAttitudeComponent', () => {
  let component: L2ScoreAttitudeComponent;
  let fixture: ComponentFixture<L2ScoreAttitudeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ L2ScoreAttitudeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(L2ScoreAttitudeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
