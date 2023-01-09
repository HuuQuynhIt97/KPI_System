import { EvaluationService } from './../../../../_core/_service/evaluation.service';
import { StartCampaignService } from './../../../../_core/_service/start-campaign.service';
import { MonthlySettingService } from './../../../../_core/_service/monthly-setting.service';
import { OcPolicyService } from './../../../../_core/_service/OcPolicy.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EditService, ToolbarService, PageService, GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { Account2Service } from 'src/app/_core/_service/account2.service';
import { Account } from 'src/app/_core/_model/account';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { AccountGroupService } from 'src/app/_core/_service/account.group.service';
import { AccountGroup } from 'src/app/_core/_model/account.group';
import { OcService } from 'src/app/_core/_service/oc.service';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-start-campaign',
  templateUrl: './start-campaign.component.html',
  styleUrls: ['./start-campaign.component.scss'],
  providers: [ToolbarService, EditService, PageService,DatePipe]
})
export class StartCampaignComponent extends BaseComponent implements OnInit {

  data: Account[] = [];
  password = '';
  modalReference: NgbModalRef;
  fields: object = { text: 'name', value: 'id' };
  leaderFields: object = { text: 'fullName', value: 'id' };
  managerFields: object = { text: 'fullName', value: 'id' };
  // toolbarOptions = ['Search'];
  passwordFake = `aRlG8BBHDYjrood3UqjzRl3FubHFI99nEPCahGtZl9jvkexwlJ`;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 15 };
  @ViewChild('grid') public grid: GridComponent;
  accountCreate: Account;
  accountUpdate: Account;
  setFocus: any;
  locale = localStorage.getItem('lang');
  accountGroupData: AccountGroup[];
  accountGroupItem: any;
  leaders: any[] = [];
  managers: any[] = [];
  leaderId: number;
  managerId: number;
  accounts: any[];
  oclv3Data: any [];
  campaignData: Object;
  displayTime: any
  monthTime: any
  yearSelect: number = new Date().getFullYear();
  typeFields: object = { text: 'name', value: 'id' };
  typeFieldsMonth: object = { text: 'name' };
  yearData: any =  []
  toolbarCampaign = ['Add', 'Update','Edit', 'Delete' , 'Cancel', 'Search'];
  monthData: any =  [
    {
      start: 1,
      name: 'Jan-June',
      end: 6
    },
    {
      start: 7,
      name: 'July-December',
      end: 12
    }
  ];
  startMonth: number = 0
  endMonth: number = 0
  Monthname: string = null
  year: number = new Date().getFullYear();
  public yearValue: number = new Date().getFullYear() - 2;
  constructor(
    private service: Account2Service,
    private accountGroupService: AccountGroupService,
    public modalService: NgbModal,
    private ocService: OcService,
    private ocPolicyService: OcPolicyService,
    private campaignService: StartCampaignService,
    private elvService: EvaluationService,
    private alertify: AlertifyService,
    private spinner: NgxSpinnerService,
  ) { super(); }

  ngOnInit() {
    this.getAll();
    this.pushYearData()
  }
  onClickDefault() {
    // this.data = this.dataTamp
  }
  pushYearData() {
    this.yearData.push({
      id: this.yearValue,
      name: this.yearValue
    })
    for (var i = 1; i < 10; i++) {
      this.yearData.push({
        id: this.yearValue + i,
        name: this.yearValue + i
      });
    }
  }

  onChangeYear(args) {
    this.year = args.value
    // if(args.isInteracted)
    //   this.data = this.dataTamp.filter(x => Number(x.entity.year) === args.value)
  }

  onChangeMonth(args) {
    this.startMonth = args.itemData.start
    this.endMonth = args.itemData.end
    this.Monthname = args.itemData.name
    // if(args.isInteracted)
    //   this.data = this.dataTamp.filter(x => Number(x.entity.year) === args.value)
  }
  generateEvaluation(campaignID) {
    this.spinner.show()
    this.campaignService.generateEvaluation(campaignID).subscribe((res: any) => {
      if(res.success) {
        this.elvService.generateAttitudeSubmit(campaignID).subscribe((res: any) => {
          if(res.success) {
            this.alertify.success('generate successfully')
            this.getAll()
          }
        })
      }
      this.spinner.hide()
    })
  }

  getAll(){
    this.campaignService.getAll().subscribe(res => {
      this.campaignData = res
      // this.policyData = res ;
    })
  }
  getAllOcLv3() {
    this.ocService.getAllLv3().subscribe((res: any) => {
      this.oclv3Data = res
    })
  }
  // life cycle ejs-grid
  createdManager($event, data) {
    this.managers = this.accounts;
    this.managers = this.managers.filter(x=> x.id !== data.id);
  }

  createdLeader($event, data) {
    this.leaders = this.accounts.filter(x=> x.isLeader);
    this.leaders = this.leaders.filter(x=> x.id !== data.id);
  }

  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }

  initialModel() {
    this.accountGroupItem = [];
    this.displayTime = new Date();
    this.monthTime = new Date();
  }

  updateModel(data) {
    this.accountGroupItem = data.factory;
    this.displayTime = new Date(data.displayTime);
    this.monthTime = new Date(data.month);
  }

  actionBegin(args) {
    if (args.requestType === 'add') {
      this.initialModel();
    }
    if (args.requestType === 'beginEdit') {
      const item = args.rowData;
      this.updateModel(item);
    }

    if (args.requestType === 'save' && args.action === 'add') {

      const model = {
        CreatedBy: JSON.parse(localStorage.getItem('user')).id,
        Name: args.data.name,
        MonthName: this.Monthname,
        Year: this.year,
        StartMonth: this.startMonth,
        EndMonth: this.endMonth
      }
      if (this.Monthname === null) {
        this.alertify.error('Please choose the campaign month')
        args.cancel = true
        return;
      }
      this.create(model);
    }

    if (args.requestType === 'save' && args.action === 'edit') {
      const model = {
        ID: args.data.id,
        CreatedBy: JSON.parse(localStorage.getItem('user')).id,
        Name: args.data.name,
        MonthName: args.data.monthName,
        Year: args.data.year,
        IsStart: args.data.isStart,
        StartMonth: args.data.startMonth,
        EndMonth: args.data.endMonth
      }
      this.update(model);
    }

    if (args.requestType === 'delete') {
      this.deleteOption(args.data[0].id);
    }
  }

  deleteOption(id) {
    this.campaignService.delete(id).subscribe(res => {
      if(res) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.getAll()
      } else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    })
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

  loadData() {
    this.service.getAll().subscribe(data => {
      this.data = data;
    });
  }

  getAccounts() {
    this.service.getAccounts().subscribe(data => {
      this.accounts = data;
      this.leaders = data.filter(x=> x.isLeader);
      this.managers = data;
    });
  }

  loadAccountGroupData() {
    this.accountGroupService.getAll().subscribe(data => {
      this.accountGroupData = data;
    });
  }

  delete(id) {
    this.ocPolicyService.deletePolicy(id).subscribe(res => {
      if(res) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.getAll()
        this.accountGroupItem = []
      } else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    })
  }

  create(model) {
    this.campaignService.add(model).subscribe((res: any) => {
      if(res.success) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.getAll()
      } else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    })
  }

  update(model) {
    this.campaignService.update(model).subscribe(res => {
      if(res){
        this.alertify.success('Successfully')
      }
    })
    // this.settingMonthlyService.update(model).subscribe(res => {
    //   if(res) {
    //     this.alertify.success(MessageConstants.CREATED_OK_MSG);
    //     this.getAll()
    //     this.accountGroupItem = []
    //   } else {
    //     this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
    //   }
    // })
  }
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
