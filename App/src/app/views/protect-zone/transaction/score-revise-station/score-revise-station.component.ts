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
import { AttitudeScoreService } from 'src/app/_core/_service/attitude-score.service';

@Component({
  selector: 'app-score-revise-station',
  templateUrl: './score-revise-station.component.html',
  styleUrls: ['./score-revise-station.component.scss'],
  providers: [ToolbarService, EditService, PageService]
})
export class ScoreReviseStationComponent implements OnInit {
  data: any = [];
  kpiStationFields: object = { text: 'value', value: 'value' };
  attStationFields: object = { text: 'value', value: 'value' };
  editing: boolean;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  userAdmin = JSON.parse(localStorage.getItem('user')).username;
  @ViewChild('ddlelement')
  public dropDownListObject: DropDownListComponent;
  locale = localStorage.getItem('lang');
  toolbarOptions = ['Cancel', 'Search'];
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  kpiStation: string = null
  attStation: string = null
  campainId: number = 0
  submitTo: number = 0

  constructor(
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private translate: TranslateService,
    private attitudeScoreService: AttitudeScoreService,

  ) {  }

  ngOnInit() {
    // this.Permission(this.route);
    this.loadData();
  }


  onchangeAttStation(args,data) {
    this.attStation = args.value
  }

  onchangeKPIStation(args,data) {
    this.kpiStation = args.value
  }

  loadData() {
    this.attitudeScoreService.getAllScoreStation().subscribe(res => {
      console.log(res);
      this.data = res;
    });
  }

  onDoubleClick(args: any): void {
  }

  initialModel() {

  }

  updateModel(data) {


  }
  actionBegin(args) {
    if (args.requestType === 'beginEdit') {
      // this.editing = true;
      // const item = args.rowData;
      this.attStation = args.rowData.currentAtt
      if(args.rowData.currentAtt === "N/A") {
        this.attStation = "NA"
      }
      this.kpiStation = args.rowData.currentKPI
      if(args.rowData.currentKPI === "N/A") {
        this.kpiStation = "NA"
      }
      // this.updateModel(item);
    }


    if (args.requestType === 'save' && args.action === 'edit') {
      this.campainId = args.data.campaignID;
      this.submitTo = args.data.id;
      this.update();
    }

  }


  actionComplete(args) {

    if (args.requestType === 'beginEdit') {
      this.editing = true;
    }
    if (args.requestType === 'cancel') {
      this.editing = false;
    }
    if (args.requestType === 'refresh') {
    }


  }

  // end life cycle ejs-grid

  // api





  update() {
    this.attitudeScoreService.updateAttitudeSubmit(this.campainId, this.submitTo, this.kpiStation, this.attStation).subscribe(
      (res : any) => {
        this.alertify.success(res.message);
        this.loadData();
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
