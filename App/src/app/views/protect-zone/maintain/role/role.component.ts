import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { PermissionService } from 'src/app/_core/_service/permission.service';
import { RoleService } from 'src/app/_core/_service/role.service';
import { ActivatedRoute } from '@angular/router';
import { MessageConstants } from 'src/app/_core/_constants/system';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss']
})
export class RoleComponent extends BaseComponent implements OnInit {

  role: any;
  data: any = [];
  @ViewChild('grid') grid: GridComponent;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  fieldsRoleType: object = { text: 'name', value: 'name' };
  filterSettings = { type: 'Excel' };
  roleTypeData: object;
  roleTypeID: any;
  roleItem: string;
  roleID: any;
  constructor(
    private roleService: RoleService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
  ) { super(); }

  ngOnInit() {
    // this.Permission(this.route);
    this.role = {
    };
    this.getAllRole();
  }
  // api
  getAllRole() {
    this.roleService.getAll().subscribe(res => {
      this.data = res;
    });
  }

  create() {
    this.roleService.create(this.role).subscribe(() => {
      this.alertify.success(MessageConstants.CREATED_OK_MSG);
      this.getAllRole();
      this.role = {
      };
    });
  }

  update() {
    this.roleService.update(this.role).subscribe(() => {
      this.alertify.success(MessageConstants.UPDATED_OK_MSG);
      // this.modalReference.close() ;
      this.getAllRole();
      this.role = {
      };
    });
  }
  delete(id) {
    this.alertify.delete("Delete role",'Are you sure you want to delete this role "' + id + '" ?')
    .then((result) => {
      if (result) {
        this.roleService.delete(id).subscribe(() => {
          this.getAllRole();
          this.alertify.success(MessageConstants.DELETED_OK_MSG);
        }, error => {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        });
      }
    })
    .catch((err) => {
      this.getAllRole();
      this.grid.refresh();
    });
  }
  // end api
  // grid event
  toolbarClick(args): void {
    switch (args.item.text) {
      /* tslint:disable */
      case 'Excel Export':
        this.grid.excelExport();
        break;
      /* tslint:enable */
      default:
        break;
    }
  }

  actionBegin(args) {
    if (args.requestType === 'save') {
      if (args.action === 'add') {
        this.role.id = 0;
        this.role.name = args.data.name;
        this.role.code = args.data.code;
        this.create();
      }
      if (args.action === 'edit') {
        this.role.id = args.data.id;
        this.role.name = args.data.name;
        this.role.code = args.data.code;
        // this.alertify.error('Can not update this role', true);
        this.update();
      }
    }
    if (args.requestType === 'delete') {
      this.alertify.error('Can not delete this role', true);
      this.delete(args.data[0].id);
    }
  }
  actionComplete(e: any): void {
    if (e.requestType === 'add') {
      (e.form.elements.namedItem('name') as HTMLInputElement).focus();
      // (e.form.elements.namedItem('id') as HTMLInputElement).disabled = true;
    }
  }
  // end event
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }


}
