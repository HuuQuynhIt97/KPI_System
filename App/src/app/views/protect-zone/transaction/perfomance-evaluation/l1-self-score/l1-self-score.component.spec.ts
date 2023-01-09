/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { L1SelfScoreComponent } from './l1-self-score.component';

describe('L1SelfScoreComponent', () => {
  let component: L1SelfScoreComponent;
  let fixture: ComponentFixture<L1SelfScoreComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ L1SelfScoreComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(L1SelfScoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
