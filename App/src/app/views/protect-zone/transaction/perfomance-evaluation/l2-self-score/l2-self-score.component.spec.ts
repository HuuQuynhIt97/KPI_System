/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { L2SelfScoreComponent } from './l2-self-score.component';

describe('L2SelfScoreComponent', () => {
  let component: L2SelfScoreComponent;
  let fixture: ComponentFixture<L2SelfScoreComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ L2SelfScoreComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(L2SelfScoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
