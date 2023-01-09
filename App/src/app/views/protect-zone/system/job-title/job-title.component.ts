import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { PermissionService } from 'src/app/_core/_service/permission.service';
import { ActivatedRoute } from '@angular/router';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { JobTitleService } from 'src/app/_core/_service/job-title.service';

@Component({
  selector: 'app-job-title',
  templateUrl: './job-title.component.html',
  styleUrls: ['./job-title.component.scss']
})
export class JobTitleComponent extends BaseComponent implements OnInit {
  jobTitle: any;
  data: any = [];
  @ViewChild('grid') grid: GridComponent;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 50 };
  filterSettings = { type: 'Excel' };
  constructor(
    private jobTitleService: JobTitleService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
  ) { super(); }

  ngOnInit() {
    this.jobTitle = {
    };
    this.getAll();
  }
  getAll() {
    this.jobTitleService.getAll().subscribe(res => {
      this.data = res;
    });
  }

  create() {
    this.jobTitleService.addJobTitle(this.jobTitle).subscribe(() => {
      this.alertify.success(MessageConstants.CREATED_OK_MSG);
      this.getAll();
      this.jobTitle = {
      };
    });
  }

  update() {
    this.jobTitleService.updateJobTitle(this.jobTitle).subscribe(() => {
      this.alertify.success(MessageConstants.UPDATED_OK_MSG);
      this.getAll();
      this.jobTitle = {
      };
    });
  }
  delete(id) {
    this.alertify.delete("Delete job title",'Are you sure you want to delete this job title "' + id + '" ?')
    .then((result) => {
      if (result) {
        this.jobTitleService.delete(id).subscribe(() => {
          this.getAll();
          this.alertify.success(MessageConstants.DELETED_OK_MSG);
        }, error => {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        });
      }
    })
    .catch((err) => {
      this.getAll();
      this.grid.refresh();
    });
  }
  toolbarClick(args): void {
    switch (args.item.text) {
      case 'Excel Export':
        this.grid.excelExport();
        break;
      default:
        break;
    }
  }

  actionBegin(args) {
    if (args.requestType === 'save') {
      if (args.action === 'add') {
        this.jobTitle.id = 0;
        this.jobTitle.nameEn = args.data.nameEn;
        this.jobTitle.nameZh = args.data.nameZh;
        this.create();
      }
      if (args.action === 'edit') {
        this.jobTitle.id = args.data.id;
        this.jobTitle.nameEn = args.data.nameEn;
        this.jobTitle.nameZh = args.data.nameZh;
        this.update();
      }
    }
    if (args.requestType === 'delete') {
      this.alertify.error('Can not delete this job title', true);
      this.delete(args.data[0].id);
    }
  }
  actionComplete(e: any): void {
    if (e.requestType === 'add') {
      (e.form.elements.namedItem('nameEn') as HTMLInputElement).focus();
    }
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
