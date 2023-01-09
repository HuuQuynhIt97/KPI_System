/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FlScoreAttitudeComponent } from './fl-score-attitude.component';

describe('FlScoreAttitudeComponent', () => {
  let component: FlScoreAttitudeComponent;
  let fixture: ComponentFixture<FlScoreAttitudeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlScoreAttitudeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlScoreAttitudeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
