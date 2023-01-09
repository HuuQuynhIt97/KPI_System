/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PdcaStringTypeMeetingComponent } from './pdcaStringTypeMeeting.component';

describe('PdcaStringTypeMeetingComponent', () => {
  let component: PdcaStringTypeMeetingComponent;
  let fixture: ComponentFixture<PdcaStringTypeMeetingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PdcaStringTypeMeetingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PdcaStringTypeMeetingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
