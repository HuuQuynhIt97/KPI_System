/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CHMComponent } from './CHM.component';

describe('CHMComponent', () => {
  let component: CHMComponent;
  let fixture: ComponentFixture<CHMComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CHMComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CHMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
