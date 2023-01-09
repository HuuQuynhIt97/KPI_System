/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { L2DownloadFileComponent } from './l2-download-file.component';

describe('L2DownloadFileComponent', () => {
  let component: L2DownloadFileComponent;
  let fixture: ComponentFixture<L2DownloadFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ L2DownloadFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(L2DownloadFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
