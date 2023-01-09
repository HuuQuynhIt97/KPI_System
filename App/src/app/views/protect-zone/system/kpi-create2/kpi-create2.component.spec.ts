/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { KpiCreate2Component } from './kpi-create2.component';

describe('KpiCreate2Component', () => {
  let component: KpiCreate2Component;
  let fixture: ComponentFixture<KpiCreate2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KpiCreate2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KpiCreate2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
