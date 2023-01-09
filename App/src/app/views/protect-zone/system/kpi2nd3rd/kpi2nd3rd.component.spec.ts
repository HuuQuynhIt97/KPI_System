/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { Kpi2nd3rdComponent } from './kpi2nd3rd.component';

describe('Kpi2nd3rdComponent', () => {
  let component: Kpi2nd3rdComponent;
  let fixture: ComponentFixture<Kpi2nd3rdComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Kpi2nd3rdComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Kpi2nd3rdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
