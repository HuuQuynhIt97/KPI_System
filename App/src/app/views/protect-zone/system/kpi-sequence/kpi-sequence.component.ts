import { SystemCode } from './../../../../_core/enum/system';
import { Component, HostListener, OnInit, TemplateRef, ViewChild } from '@angular/core'
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap'
import { CalendarView } from '@syncfusion/ej2-angular-calendars'
import { TreeGridComponent } from '@syncfusion/ej2-angular-treegrid'
import { ModalDirective } from 'ngx-bootstrap/modal'
import { MessageConstants } from 'src/app/_core/_constants/system'
import { Account } from 'src/app/_core/_model/account'
import { Account2Service } from 'src/app/_core/_service/account2.service'
import { AlertifyService } from 'src/app/_core/_service/alertify.service'
import { KpinewService } from 'src/app/_core/_service/kpinew.service'
import { OcPolicyService } from 'src/app/_core/_service/OcPolicy.service'

import { OcService } from './../../../../_core/_service/oc.service'
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-kpi-sequence',
  templateUrl: './kpi-sequence.component.html',
  styleUrls: ['./kpi-sequence.component.scss'],
  providers: [DatePipe]
})
export class KpiSequenceComponent implements OnInit {

  @ViewChild('content', { static: true }) content: TemplateRef<any>;
  toolbar: object;
  data: any;
  dataTamp: any;
  editing: any;
  contextMenuItems: any;
  pageSettings: any;
  editparams: { params: { format: string } };
  @ViewChild('childModal', { static: false }) childModal: ModalDirective;
  @ViewChild("treegrid")
  public treeGridObj: TreeGridComponent;
  @ViewChild("buildingModal")
  buildingModal: any;
  oc: { id: 0; name: ""; level: 1; parentID: null };
  edit: {
    Id: 0;
    name: "";
    PolicyId: 0,
    TypeId: 0,
    Pic: 0
  };
  title: string;
  picFields: object = { text: 'fullName', value: 'id' };
  typeFields: object = { text: 'name', value: 'id' };
  policyFields: object = { text: 'name', value: 'id' };
  policyData: Object;
  policyId: number = 0;
  picId: number = 0;
  typeId: number = 0;
  parentId: null
  level: number = 1
  typeData: Object;
  accountData: Account[];
  modalReference: NgbModalRef
  kpiname: any = null
  picItem: any = [];
  userId: number = 0
  public yearValue: number = new Date().getFullYear() - 2;
  public yearValueAdd: number = new Date().getFullYear();
  yearSelect: number = new Date().getFullYear();
  yearData: any =  []
  @ViewChild('ddlelement')
  public dropDownListObject: DropDownListComponent;
  isCollslape: boolean = false;
  startTime = new Date();
  sequenceCHM: number = 0
  fieldsRole: object = { text: 'fullName', value: 'id' };
  lastEditer: number = 0
  values: number = 0
  endTime = new Date(new Date().getFullYear(), 11, 31);
  codeDefault = SystemCode.SYSTEMADMIN
  code: string;
  userData: Account[];
  public start: CalendarView = 'Year';
  public depth: CalendarView = 'Year';
  public format: string = 'MMMM yyyy'
  constructor(
    private ocService: OcService,
    private ocPolicyService: OcPolicyService,
    private accountService: Account2Service,
    private kpiNewService: KpinewService,
    private spinner: NgxSpinnerService,
    private alertify: AlertifyService,
    private datePipe: DatePipe

  ) {}

  ngOnInit() {
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.code = JSON.parse(localStorage.getItem('level')).code;
    this.editing = { allowDeleting: true, allowEditing: true, mode: "Row" };
    this.optionTreeGrid();
    this.onService();
    // this.getAllPolicy()
    this.getAllUsers();
    this.getAllType()
    this.pushYearData()
    this.getAccounts();
  }

  getAccounts() {
    this.accountService.getAll().subscribe(data => {
      this.userData = data;
      // this.userData.unshift({ username: "admin", fullName: "Default(Admin)" });
    });

  }
  ngAfterViewInit() {
    // Set null value to value property for clear the selected item
    document.getElementById('btn').onclick = () => {
      this.dropDownListObject.value = null;
    }
  }

  onChangeYear(args) {
    this.data = this.dataTamp.filter(x => Number(x.entity.year) === args.value)
  }
  onClickDefault() {
    this.yearSelect = 0
    this.data = this.dataTamp
  }

  getAllType() {
    const lang = localStorage.getItem('lang');
    this.kpiNewService.getAllType(lang).subscribe(res => {
      this.typeData = res
    })
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
  getAllUsers() {
    this.accountService.getAll().subscribe((res: any) => {
      this.accountData = res ;
    });
  }
  getAllPolicy() {
    this.ocPolicyService.getAllPolicy().subscribe(res => {
      this.policyData = res
    })
  }
  refreshData() {
    this.kpiname = null
    this.parentId = null
    this.picItem = []
    this.level  = 1
    this.policyId = 0
    this.picId = 0
    this.typeId = 0
    this.yearValue = new Date().getFullYear()
  }

  optionTreeGrid() {
    this.toolbar = [
      "Update",
      "Search",
      "Cancel",
      "ExpandAll",
      "CollapseAll",
    ];
    this.editing = {
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      mode: "Row",
    };
    this.pageSettings = { pageSize: 20 };
    this.editparams = { params: { format: "n" } };
  }

  created() {
    this.getKPIAsTreeView();
    // document.getElementById(this.treeGridObj.element.id + "_searchbar").addEventListener('keyup', () => {
    //   this.treeGridObj.search((event.target as HTMLInputElement).value)
    // });
  }

  onService() {
    this.ocService.currentMessage.subscribe((arg) => {
      if (arg === 200) {
        this.getKPIAsTreeView();
      }
    });
  }

  toolbarClick(args) {
    switch (args.item.id) {
      case "treegrid_gridcontrol_expandall":
        this.isCollslape = false;
        break;
      case "treegrid_gridcontrol_collapseall":
        this.isCollslape = true;
        break;
      default:
        break;
    }
  }
  rowDB(args) {
    const data = args.data.entity as any;
    if (data.level === 1) {
      args.row.style.background= 'gainsboro';
    }
  }
  dataBound() {
    if (this.isCollslape) {
      this.treeGridObj.collapseAll()
    } else {
      this.treeGridObj.expandAll()
    }
    // this.treeGridObj.collapseAll()
  }

  updateModel(data) {
    this.policyId = data.policyId
    this.typeId = data.typeId
    this.picId = data.pic
    this.picItem = data.pics;
    this.lastEditer = data.updateBy
    this.values = data.updateBy
    this.sequenceCHM = data.sequenceCHM
    if(data.startDisplayMeetingTime !== null)
      this.startTime = new Date(data.startDisplayMeetingTime)
    if(data.endDisplayMeetingTime !== null)
      this.endTime = new Date(data.endDisplayMeetingTime)
  }

  @HostListener('window:keyup', ['$event'])
  keyEvent(event: KeyboardEvent) {
    if (event.keyCode === 13) {
      this.editEnd()
    }
   }

  editEnd() {
    this.treeGridObj.endEdit()
  }

  onChange(e) {
    this.picItem = e
  }

  actionBegin(args) {
    if (args.requestType === 'beginEdit') {
      var tooltips = args.row.querySelectorAll('.e-tooltip');
      for (var i = 0; i < tooltips.length; i++) {
        tooltips[i].ej2_instances[0].destroy();
      }
      const item = args.rowData.entity;
      this.updateModel(item);
    }
    if (args.requestType === 'save' && args.action === 'edit') {

      const model = {
        Id: args.data.entity.id,
        Name: args.data.entity.name,
        PolicyId: this.policyId,
        TypeId: this.typeId,
        Sequence: args.data.entity.sequence,
        SequenceCHM: this.sequenceCHM,
        Level: args.data.entity.level,
        ParentId: args.data.entity.parentId,
        Pic: this.picId,
        UpdateBy: this.code === this.codeDefault ? this.lastEditer : 0,
        KpiIds: this.picItem,
        StartDisplayMeetingTime: this.datePipe.transform(this.startTime, "yyyy-MM-dd"),
        EndDisplayMeetingTime: this.datePipe.transform(this.endTime, "yyyy-MM-dd")
      }
      this.update(model);
    }
  }

  // actionComplete(args) {
  //   if (args.requestType === 'beginEdit') {
  //     var tooltips = args.row.querySelectorAll('.e-tooltip');
  //     for (var i = 0; i < tooltips.length; i++) {
  //       tooltips[i].ej2_instances[0].destroy();
  //     }
  //     const item = args.rowData.entity;
  //     this.updateModel(item);
  //   }
  //   if (args.requestType === 'save' && args.action === 'edit') {

  //     const model = {
  //       Id: args.data.entity.id,
  //       Name: args.data.entity.name,
  //       PolicyId: this.policyId,
  //       TypeId: this.typeId,
  //       Sequence: args.data.entity.sequence,
  //       SequenceCHM: this.sequenceCHM,
  //       Level: args.data.entity.level,
  //       ParentId: args.data.entity.parentId,
  //       Pic: this.picId,
  //       UpdateBy: this.code === this.codeDefault ? this.lastEditer : 0,
  //       KpiIds: this.picItem,
  //       StartDisplayMeetingTime: this.datePipe.transform(this.startTime, "yyyy-MM-dd"),
  //       EndDisplayMeetingTime: this.datePipe.transform(this.endTime, "yyyy-MM-dd")
  //     }
  //     this.update(model);
  //   }
  // }

  update(model) {
    this.kpiNewService.updateSequence(model).subscribe(res => {
      if(res) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.getKPIAsTreeView()
        this.refreshData()
      }else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    })
  }


  getKPIAsTreeView() {
    this.spinner.show()
    const lang = localStorage.getItem('lang');
    this.kpiNewService.getTree(lang).subscribe((res: any) => {
      if(this.yearSelect > 0)
        this.data = res.filter(x => Number(x.entity.year) === this.yearSelect);
      this.spinner.hide()
    });
  }


}
