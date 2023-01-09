import { JobTitleService } from 'src/app/_core/_service/job-title.service';
import { RoleService } from 'src/app/_core/_service/role.service';
import { filter } from 'rxjs/operators';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EditService, ToolbarService, PageService, GridComponent } from '@syncfusion/ej2-angular-grids';
import { ModalDismissReasons, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { Account2Service } from 'src/app/_core/_service/account2.service';
import { Account } from 'src/app/_core/_model/account';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { AccountGroupService } from 'src/app/_core/_service/account.group.service';
import { AccountGroup } from 'src/app/_core/_model/account.group';
import { OcService } from 'src/app/_core/_service/oc.service';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { IRole } from 'src/app/_core/_model/role';
import { Authv2Service } from 'src/app/_core/_service/authv2.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
  providers: [ToolbarService, EditService, PageService]
})
export class AccountComponent extends BaseComponent implements OnInit {
  data: Account[] = [];
  password = '';
  modalReference: NgbModalRef;
  fields: object = { text: 'name', value: 'id' };
  leaderFields: object = { text: 'fullName', value: 'id' };
  managerFields: object = { text: 'fullName', value: 'id' };
  l1Fields: object = { text: 'fullName', value: 'id' };
  l2Fields: object = { text: 'fullName', value: 'id' };
  functionalLeaderFields: object = { text: 'fullName', value: 'id' };
  isL0: boolean = false;
  isGhr: boolean = false;
  isGm: boolean = false;
  isGmScore: boolean = false;
  editing: boolean;
  deleteAccount: any;
  ocFields: object = { text: 'name', value: 'id' };
  // toolbarOptions = ['Search'];
  passwordFake = `aRlG8BBHDYjrood3UqjzRl3FubHFI99nEPCahGtZl9jvkexwlJ`;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  @ViewChild('password') content: any;
  closeResult = '';
  passwordAdmin = '';
  userAdmin = JSON.parse(localStorage.getItem('user')).username;
  @ViewChild('ddlelement')
  public dropDownListObject: DropDownListComponent;
  accountCreate: Account;
  accountUpdate: Account;
  setFocus: any;
  locale = localStorage.getItem('lang');
  accountGroupData: AccountGroup[];
  accountGroupItem: any;
  leaders: any[] = [];
  managers: any[] = [];
  l1s: any[] = [];
  l2s: any[] = [];
  functionalLeaders: any[] = [];
  leaderId: number;
  managerId: number;
  l1Id: number;
  l2Id: number;
  functionalLeaderId: number;
  factId: number;
  centerId: number;
  deptId: number;
  accounts: any[];
  dataOclv3: any;
  dataOclv4: any;
  dataOclv5: any;
  dataOc: any;
  roles: IRole[];
  userID: any;
  roleID: any;
  fieldsRole: object = { text: 'name', value: 'name' };
  jobTitles: any[] = [];
  jobTitleId: number;
  fieldsJobTitle: object = { text: 'jobTitle', value: 'id'};
  systemFlows: object[] = [{id:0, name: "N/A"}, {id:1, name: "1"}, {id:2, name: "2"}, {id:3, name: "3"}, {id:4, name: "4"}, {id:5, name: "5"}, {id:6, name: "6"}];
  systemFlow: number = 5;
  fieldsSystemFlow: object = { text: 'name', value: 'id'};
  constructor(
    private service: Account2Service,
    private roleService: RoleService,
    private authService: Authv2Service,
    private accountGroupService: AccountGroupService,
    public modalService: NgbModal,
    private ocService: OcService,
    private jobTitleService: JobTitleService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private translate: TranslateService
  ) { super(); }

  ngOnInit() {
    // this.Permission(this.route);
    this.loadData();
    this.getAccounts();
    this.getAllOc();
    this.loadAccountGroupData();
    this.getAllRoles();
    this.getAllJobTitles();
  }

  filterCenter(args) {
    this.deptId = 0
    this.dataOclv5 = this.dataOc.filter(x => x.parentId === args.value)
    this.dataOclv5.unshift({ name: "N/A", id: 0 });
  }

  filterFact(args) {
    this.centerId = 0
    this.deptId = 0
    this.dataOclv4 = this.dataOc.filter(x => x.parentId === args.value)
    this.dataOclv4.unshift({ name: "N/A", id: 0 });
  }

  getAllOc(){
    this.ocService.getAll().subscribe((res: any) => {
      this.dataOc = res

      //Oclv3
      this.dataOclv3 = res.filter(x => x.level === 3)
      this.dataOclv3.unshift({ id: 0, name: 'N/A'  });

    })
  }

  onChangeRole(args, data) {
    this.userID = data.id;
    this.roleID = args.itemData.id;
  }
  onChangeJobTitle(args) {
    this.jobTitleId = args.itemData.id;
  }
  onChangeSystemFlow(args) {
    this.systemFlow = args.itemData.id;
  }

  getAllRoles() {
    this.roleService.getAll().subscribe(res => {
      this.roles = res
    })
  }

  getAllJobTitles() {
    this.jobTitleService.getAllByLang(this.locale).subscribe(res => {
      this.jobTitles = res
      this.jobTitles.unshift({ id: 0, jobTitle: 'N/A'});
    })
  }

  loadData() {
    this.service.getAllByLang(this.locale).subscribe(data => {
      this.data = data;
    });
  }
  // life cycle ejs-grid
  createdManager($event, data) {
    this.managers = this.accounts;
    this.managers = this.managers.filter(x => x.id !== data.id);
  }

  createdLeader($event, data) {
    this.leaders = this.accounts.filter(x => x.isLeader);
    this.leaders = this.leaders.filter(x => x.id !== data.id);
  }

  createdL1($event, data) {
    this.l1s = this.accounts;
    this.l1s = this.l1s.filter(x => x.id !== data.id);
    this.l1s.unshift({ id: 0, fullName: 'N/A'});
  }

  createdL2($event, data) {
    this.l2s = this.accounts;
    this.l2s = this.l2s.filter(x => x.id !== data.id);
    this.l2s.unshift({ id: 0, fullName: 'N/A'});
  }

  createdFunctionalLeader($event, data) {
    this.functionalLeaders = this.accounts;
    this.functionalLeaders = this.functionalLeaders.filter(x => x.id !== data.id);
    this.functionalLeaders.unshift({ id: 0, fullName: 'N/A'});
  }

  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }

  initialModel() {
    this.accountGroupItem = [];
    this.leaderId = 0;
    this.managerId = 0;
    this.l1Id = 0;
    this.l2Id = 0;
    this.functionalLeaderId = 0;
    this.factId = 0;
    this.centerId = 0;
    this.deptId = 0;
    this.isL0 = false;
    this.isGhr = false;
    this.isGm = false;
    this.isGmScore = false;
    this.jobTitleId = 0;
    this.systemFlow = 2;

    this.accountCreate = {
      id: 0,
      username: null,
      password: null,
      fullName: null,
      email: null,
      accountTypeId: 2,
      isLock: false,
      createdBy: 0,
      createdTime: new Date().toLocaleDateString('en-US'),
      modifiedBy: 0,
      modifiedTime: null,
      accountGroupIds: null,
      accountGroupText: null,
      accountType: null,
      factId: this.factId,
      centerId: this.centerId,
      deptId: this.deptId,
      leader: this.leaderId,
      manager: this.managerId,
      leaderName: null,
      managerName: null,
      l1: this.l1Id,
      l1Name: null,
      l2: this.l2Id,
      l2Name: null,
      functionalLeader: this.functionalLeaderId,
      functionalLeaderName: null,
      ghr: false,
      l0: false,
      gm: false,
      gmScore: false,
      jobTitleId: 0,
      systemFlow: 2
    };

  }

  updateModel(data) {
    this.accountGroupItem = data.accountGroupIds;
    this.getAllOc();
    this.managerId = data.manager;
    this.leaderId = data.leader;
    this.functionalLeaderId = data.functionalLeader;
    this.l1Id = data.l1;
    this.l2Id = data.l2;
    this.factId = data.factId;
    this.centerId = data.centerId;
    this.deptId = data.deptId;
    this.jobTitleId = data.jobTitleId;
    this.systemFlow = data.systemFlow;

  }
  actionBegin(args) {

    if (args.requestType === 'add') {
      this.initialModel();
    }

    if (args.requestType === 'beginEdit') {
      this.isL0 = args.rowData.l0;
      this.isGhr = args.rowData.ghr;
      this.isGm = args.rowData.gm;
      this.isGmScore = args.rowData.gmScore;
      this.editing = true;
      const item = args.rowData;
      this.updateModel(item);
      if(this.factId > 0 && this.centerId === 0) {
        this.dataOclv4 = this.dataOc.filter(x => x.parentId === this.factId)
        this.dataOclv4.unshift({ name: "N/A", id: 0 });
      }

      if(this.factId > 0 && this.centerId > 0) {

        this.dataOclv4 = this.dataOc.filter(x => x.parentId === this.factId)
        this.dataOclv4.unshift({ name: "N/A", id: 0 });

        this.dataOclv5 = this.dataOc.filter(x => x.parentId === this.centerId)
        this.dataOclv5.unshift({ name: "N/A", id: 0 });

      }
    }

    if (args.requestType === 'save' && args.action === 'add') {
      this.accountCreate = {
        id: 0,
        username: args.data.username ,
        password: args.data.password,
        fullName: args.data.fullName,
        email: args.data.email,
        accountTypeId: 2,
        isLock: false,
        createdBy: 0,
        createdTime: new Date().toLocaleDateString('en-US'),
        modifiedBy: 0,
        modifiedTime: null,
        accountType: null,
        deptId: this.deptId,
        centerId: this.centerId,
        factId: this.factId,
        accountGroupIds: this.accountGroupItem,
        accountGroupText: null,
        leader: this.leaderId,
        manager: this.managerId,
        leaderName: null,
        managerName: null,
        l1: this.l1Id,
        l1Name: null,
        l2: this.l2Id,
        l2Name: null,
        functionalLeader: this.functionalLeaderId,
        functionalLeaderName: null,
        ghr: this.isGhr,
        l0: this.isL0,
        gm: this.isGm,
        gmScore: this.isGmScore,
        jobTitleId: this.jobTitleId,
        systemFlow: this.systemFlow
      };

      if (args.data.username === undefined) {
        this.alertify.error('Please key in a account! <br> Vui lòng nhập tài khoản đăng nhập!');
        args.cancel = true;
        return;
      }

      if (args.data.password === undefined) {
        this.alertify.error('Please key in a password! <br> Vui lòng nhập mật khẩu!');
        args.cancel = true;
        return;
      }

      if (this.factId === 0) {
        this.alertify.error('Please Select Factory! <br> Vui lòng chọn Factory!');
        args.cancel = true;
        return;
      }

      if (this.roleID > 0) {
        this.create();
      } else {
        args.cancel = true;
        this.alertify.error('Please select a role! <br>');
        return;
      }
      // this.create();
    }

    if (args.requestType === 'save' && args.action === 'edit') {
      this.editing = false;

      this.accountUpdate = {
        id: args.data.id,
        username: args.data.username ,
        password: args.data.password,
        fullName: args.data.fullName,
        email: args.data.email,
        isLock: args.data.isLock,
        accountTypeId: args.data.accountTypeId,
        createdBy: args.data.createdBy,
        createdTime: args.data.createdTime,
        modifiedBy:args.data.modifiedBy,
        modifiedTime: args.data.modifiedTime,
        accountType: null,
        factId: this.factId,
        centerId: this.centerId,
        deptId: this.deptId,
        accountGroupIds: this.accountGroupItem,
        accountGroupText: null,
        leader: this.leaderId,
        manager: this.managerId,
        leaderName: null,
        managerName: null,
        l1: this.l1Id,
        l1Name: null,
        l2: this.l2Id,
        l2Name: null,
        functionalLeader: this.functionalLeaderId,
        functionalLeaderName: null,
        ghr: false,
        l0: this.isL0,
        gm: this.isGm,
        gmScore: this.isGmScore,
        jobTitleId: this.jobTitleId,
        systemFlow: this.systemFlow
      };
      this.update();
    }

    if (args.requestType === 'delete') {
      this.delete(args.data[0].id);
    }
  }

  toolbarClick(args) {
    switch (args.item.id) {
      case 'grid_excelexport':
        this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }
  actionComplete(args) {

    if (args.requestType === 'add') {
      args.form.elements.namedItem('username').focus(); // Set focus to the Target element
    }
    if (args.requestType === 'beginEdit') {
      this.editing = true;
    }
    if (args.requestType === 'cancel') {
      this.editing = false;
    }
    if (args.requestType === 'refresh') {
      this.isL0 = false;
      this.isGhr = false;
      this.isGm = false;
      this.isGmScore = false;
    }


  }

  // end life cycle ejs-grid

  // api

  lock(id): void {
    this.service.lock(id).subscribe(
      (res) => {
        if (res.success === true) {
          const message = res.message;
          this.alertify.success(message);
          this.loadData();
        } else {
           this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );
  }

  onClickL0(id): void {
    if (id != null && this.editing != true) {
      this.service.updateL0(id).subscribe(
        (res) => {
          if (res.success === true) {
            const message = res.message;
            this.alertify.success("successfully");
            this.loadData();
          } else {
             this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
          }
        },
        (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
      );
    } else {
      this.isL0 = !this.isL0
    }
  }


  onClickGhr(id): void {

    if (id != null && this.editing != true) {
      this.service.updateGhr(id).subscribe(
        (res) => {
          if (res.success === true) {
            const message = res.message;
            this.alertify.success("successfully");
            this.loadData();
          } else {
             this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
          }
        },
        (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
      );
    } else {
      this.isGhr = !this.isGhr
    }
  }

  onClickGm(id): void {

    if (id != null && this.editing != true) {
      this.service.updateGm(id).subscribe(
        (res) => {
          if (res.success === true) {
            const message = res.message;
            this.alertify.success("successfully");
            this.loadData();
          } else {
             this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
          }
        },
        (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
      );
    } else {
      this.isGm = !this.isGm
    }
  }

  onClickGmScore(id): void {

    if (id != null && this.editing != true) {
      this.service.updateGmScore(id).subscribe(
        (res) => {
          if (res.success === true) {
            const message = res.message;
            this.alertify.success("successfully");
            this.loadData();
          } else {
             this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
          }
        },
        (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
      );
    } else {
      this.isGmScore = !this.isGmScore
    }
  }

  getAccounts() {
    this.service.getAccounts().subscribe(data => {
      this.accounts = data;
      this.leaders = data.filter(x => x.isLeader);
      this.managers = data;
    });
  }

  loadAccountGroupData() {
    this.accountGroupService.getAll().subscribe(data => {
      this.accountGroupData = data;
    });
  }

  delete(id) {
    this.alertify.delete("Delete Account",'Are you sure you want to delete this account "' + id + '" ?')
    .then((result) => {
      if (result) {
        this.deleteAccount = id;
        this.openModal()
        // this.service.delete(id).subscribe(
        //   (res) => {
        //     if (res.success === true) {
        //       this.alertify.success(MessageConstants.DELETED_OK_MSG);
        //       this.loadData();
        //     } else {
        //       this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        //     }
        //   },
        //   (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
        // );
      }
    })
    .catch((err) => {
      this.loadData();
      this.grid.refresh();
    });
  }
  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
  openModal() {
    this.modalService.open(this.content).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
      this.loadData();
      this.grid.refresh();
      this.passwordAdmin = "";
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      this.loadData();
      this.grid.refresh();
      this.passwordAdmin = "";
    });
  }
  checkPasswordAdmin() {
    this.authService.checkPassword(this.userAdmin, this.passwordAdmin).subscribe(res => {
      if (res) {
        this.service.delete(this.deleteAccount).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(MessageConstants.DELETED_OK_MSG);
              this.modalService.dismissAll(this.content);
              this.loadData();
              this.grid.refresh();
            } else {
              this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
            }
          },
          (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
        );
      } else {
        this.alertify.warning(this.translate.instant('MESSAGE_PASSWORD_INCORRECT'));
      }
    })
  }

  mapUserRole(userID: number, roleID: number) {
    this.roleService.mapUserRole(userID, roleID).subscribe((res: any) => {
      if (res.status) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.loadData();
        this.roleID = 0;
      } else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    });
  }

  create() {
    this.service.add(this.accountCreate).subscribe((res) => {
        if (res.success === true) {
          this.mapUserRole(res.data, this.roleID)
          this.alertify.success(MessageConstants.CREATED_OK_MSG);
          this.loadData();
          this.getAccounts();

          this.accountCreate = {} as Account;
        } else {
          this.alertify.warning(res.message);
          this.loadData();
        }
      }
    );
  }

  update() {
    this.service.update(this.accountUpdate).subscribe(
      (res) => {
        if (res.success === true) {
          if (this.userID > 0 && this.roleID > 0) {
            this.mapUserRole(this.userID, this.roleID);
          }
          this.alertify.success(MessageConstants.UPDATED_OK_MSG);
          this.loadData();
        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      },
      (error) => {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    );
  }
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
