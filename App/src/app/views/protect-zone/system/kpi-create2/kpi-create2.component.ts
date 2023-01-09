import { filter } from 'rxjs/operators';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core'
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap'
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
import { NgxSpinnerService } from 'ngx-spinner';
import { TranslateService } from '@ngx-translate/core';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-kpi-create2',
  templateUrl: './kpi-create2.component.html',
  styleUrls: ['./kpi-create2.component.scss'],
  providers: [DatePipe]
})
export class KpiCreate2Component implements OnInit {

  @ViewChild('content', { static: true }) content: TemplateRef<any>;
  @ViewChild('content2', { static: true }) content_addLL: TemplateRef<any>;
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
  pdcaId: number = 0;
  isYesPDCA: boolean = true
  parentId: null
  level: number = 1
  typeData: Object;
  public pdcaData: any
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
  currentLevel: any;
  yearAdd: any;
  public start: CalendarView = 'Year';
  public depth: CalendarView = 'Year';
  public format: string = 'MMMM yyyy'
  startTime = new Date();
  endTime = new Date(new Date().getFullYear(), 11, 31);
  constructor(
    private ocService: OcService,
    private modalServices: NgbModal,
    private ocPolicyService: OcPolicyService,
    private accountService: Account2Service,
    private kpiNewService: KpinewService,
    private spinner: NgxSpinnerService,
    private translate: TranslateService,
    private alertify: AlertifyService,
    private datePipe: DatePipe,

  ) {}

  ngOnInit() {
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.editing = { allowDeleting: true, allowEditing: true, mode: "Row" };
    // this.toolbar = ["Delete", "Search", "Update", "Cancel"];
    this.optionTreeGrid();
    this.onService();
    // this.getAllPolicy()
    this.getAllUsers();
    this.getAllType()
    this.pushYearData()
  }
  ngAfterViewInit() {
    // Set null value to value property for clear the selected item
    document.getElementById('btn').onclick = () => {
      this.dropDownListObject.value = null;
    }
  }
  pdcaChange(args) {
    if(args.isInteracted) {
      if(args.itemData.name === 'YES') {
        this.isYesPDCA = false
      }else {
        this.isYesPDCA = true
      }
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
  validation() {
    if (this.kpiname === null) {
      this.alertify.error('Please key in kpi name!');
      return false;
    }
    // if (this.policyId === 0) {
    //   this.alertify.error('Please select Policy !');
    //   return false;
    // }
    // if (this.typeId === 0) {
    //   this.alertify.error('Please select a Type! ');
    //   return false;
    // }
    if (this.picItem.length === 0) {
      this.alertify.error('Please select a PIC! ');
      return false;
    }
    return true;

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
  createOC() {
    const model = {
      Name: this.kpiname,
      PolicyId: this.policyId,
      TypeId: this.typeId,
      ParentId: this.parentId,
      Level: this.level,
      UpdateBy: this.userId,
      Pic: this.picId,
      Year: this.yearValueAdd,
      KpiIds: this.picItem
    }
    if (this.validation()) {
      this.kpiNewService.add(model).subscribe(res => {
        if(res) {
          this.alertify.success(MessageConstants.CREATED_OK_MSG);
          this.modalReference.close();
          this.getKPIAsTreeView()
          this.refreshData()
        }else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      })
    }
  }

  createLLKPI() {
    const model = {
      Name: this.kpiname,
      PolicyId: this.policyId,
      TypeId: this.typeId,
      ParentId: this.parentId,
      Level: this.level,
      UpdateBy: this.userId,
      Pic: this.picId,
      isDisplayTodo: this.isYesPDCA,
      Year: this.yearValueAdd,
      KpiIds: this.picItem,
      StartDisplayMeetingTime: this.datePipe.transform(this.startTime, "yyyy-MM-dd"),
      EndDisplayMeetingTime: this.datePipe.transform(this.endTime, "yyyy-MM-dd")
    }
    if (this.pdcaId === 0) {
      this.alertify.error('Please select status PDCA! ');
      return false;
    }

    if (this.typeId === 0 && this.isYesPDCA == false) {
      this.alertify.error('Please select Type! ');
      return false;
    }
    if (this.validation()) {
      this.kpiNewService.add(model).subscribe(res => {
        if(res) {
          this.alertify.success(MessageConstants.CREATED_OK_MSG);
          this.modalReference.close();
          this.getKPIAsTreeView()
          this.refreshData()
        }else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      })
    }
  }

  optionTreeGrid() {
    this.contextMenuItems = [
      {
        text: "Delete",
        iconCss: " e-icons e-delete",
        target: ".e-content",
        id: "DeleteOC",
      },
    ];
    const lang = localStorage.getItem('lang') ;
    if(lang === 'en') {
      this.toolbar = [
        {
          text: 'Search',
          tooltipText: 'Search',
          id: 'Search',
        },
        {
          text: 'Add',
          prefixIcon: 'e-add',
          tooltipText: 'Add KPI',
          id: 'treegrid_gridcontrol_add',
        },
        {
          text: 'Add LL KPI',
          prefixIcon: 'e-add',
          tooltipText: 'Add lower level KPI',
          id: 'treegrid_gridcontrol_addLL',
        },

        {
          text: 'Update',
          prefixIcon: 'e-update',
          id: 'treegrid_gridcontrol_update',
        },
        {
          text: 'Cancel',
          prefixIcon: 'e-cancel',
          id: 'treegrid_gridcontrol_cancel',
        },
        {
          text: 'ExpandAll',
          prefixIcon: 'e-expand',
          id: 'treegrid_gridcontrol_expandall',
        },
        {
          text: 'CollapseAll',
          prefixIcon: 'e-collapse',
          id: 'treegrid_gridcontrol_collapseall',
        }
      ];
    }else {
      this.toolbar = [
        {
          text: 'Search',
          tooltipText: 'Search',
          id: 'Search',
        },
        {
          text: '新增',
          prefixIcon: 'e-add',
          tooltipText: '新增',
          id: 'treegrid_gridcontrol_add',
        },
        {
          text: '新增下一階KPI',
          prefixIcon: 'e-add',
          tooltipText: '新增下一階KPI',
          id: 'treegrid_gridcontrol_addLL',
        },

        {
          text: '更新資料',
          prefixIcon: 'e-update',
          id: 'treegrid_gridcontrol_update',
        },
        {
          text: '取消',
          prefixIcon: 'e-cancel',
          id: 'treegrid_gridcontrol_cancel',
        },
        {
          text: '展開全部',
          prefixIcon: 'e-expand',
          id: 'treegrid_gridcontrol_expandall',
        },
        {
          text: '全部收縮',
          prefixIcon: 'e-collapse',
          id: 'treegrid_gridcontrol_collapseall',
        }
      ];
    }
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
  }

  onService() {
    this.ocService.currentMessage.subscribe((arg) => {
      if (arg === 200) {
        this.getKPIAsTreeView();
      }
    });
  }

  toolbarClick(args) {
    // const lang = localStorage.getItem('lang');
    // const message = lang == 'vi' ? 'Hiện tại không thể tạo KPIs lớn hơn cấp độ 3!' : lang === 'en' ? 'Currently, you cannot create KPIs bigger than this level 3' : '目前無法建立小於這階的KPI';
    if(this.level > 1) {
      switch (args.item.id) {
        case "treegrid_gridcontrol_addLL":
          args.cancel = true;
          this.openSubModal();
          break;
        case "treegrid_gridcontrol_add":
          args.cancel = true;
          this.openMainModal();
          break;
        case "treegrid_gridcontrol_expandall":
          this.isCollslape = false;
          break;
        case "treegrid_gridcontrol_collapseall":
          this.isCollslape = true;
          break;
        default:
          break;
      }
    } else {

      if (args.item.id === 'treegrid_gridcontrol_addLL') {
        args.cancel = true;
        this.alertify.error(this.translate.instant('MESSAGE_PARENT_KPI'));
      } else {
        switch (args.item.id) {
          case "treegrid_gridcontrol_expandall":
            this.isCollslape = false;
            break;
          case "treegrid_gridcontrol_collapseall":
            this.isCollslape = true;
            break;
          case "treegrid_gridcontrol_add":
            args.cancel = true;
            this.openMainModal();
            break;
          
          default:
            break;
        }
      }
    }
    // if (this.currentLevel === 3 && args.item.id === 'treegrid_gridcontrol_addLL') {
    //   args.cancel = true;
    //   this.alertify.warning(message);
    //   return;
    // }
    // else {

    // }
   
  }
  dataBound() {
    if (this.isCollslape) {
      this.treeGridObj.collapseAll()
    } else {
      this.treeGridObj.expandAll()
    }
    // this.treeGridObj.collapseAll()
  }
  contextMenuClick(args) {
    switch (args.item.id) {
      case "DeleteOC":
        this.delete(args.rowInfo.rowData.entity);
        break;
      case "Add-Sub-Item":
        this.openSubModal();
        break;
      default:
        break;
    }
  }

  delete(data) {
    this.alertify.confirm(
      "Delete KPI",
      'Are you sure you want to delete this KPI "' + data.name + '" ?',
      () => {
        this.kpiNewService.delete(data.id).subscribe(res => {
          if(res) {
            this.alertify.success(MessageConstants.CREATED_OK_MSG);
            this.getKPIAsTreeView()
            this.refreshData();
          }else {
            this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
          }
        })
      }
    );
  }
  
  updateModel(data) {
    this.policyId = data.policyId
    this.typeId = data.typeId
    this.picId = data.pic
    this.picItem = data.pics;

  }

  actionComplete(args) {
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
        Level: args.data.entity.level,
        ParentId: args.data.entity.parentId,
        Pic: this.picId,
        UpdateBy: this.userId,
        KpiIds: this.picItem
      }
      this.update(model);
    }
  }

  update(model) {
    this.kpiNewService.update(model).subscribe(res => {
      if(res) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.getKPIAsTreeView()
        this.refreshData()
      }else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    })
  }

  rowSelected(args) {
    // this.parentId = args.data.entity.id
    // this.level = args.data.entity.level + 1

    this.parentId = args.data.entity.id
    this.level = args.data.entity.level + 1
    this.currentLevel = args.data.entity.level
    this.yearAdd = args.data.entity.year === null || args.data.entity.year === '' ? args.data.entity.createdTimeYear : args.data.entity.year
  }
  rowDeSelected(args) {
    this.refreshData()
  }
  getKPIAsTreeView() {
    this.spinner.show()
    const lang = localStorage.getItem('lang');
    this.kpiNewService.getTree(lang).subscribe((res: any) => {
      if(this.yearSelect > 0)
        this.data = res.filter(x => Number(x.entity.year) === this.yearSelect);
      this.dataTamp = res;
      this.spinner.hide()
    });
  }

  clearFrom() {
    this.oc = {
      id: 0,
      name: "",
      parentID: null,
      level: 1,
    };
  }

  rename() {
    this.ocService.rename(this.edit).subscribe((res) => {
      this.getKPIAsTreeView();
      this.alertify.success("The oc has been changed!!!");
    });
  }
  openMainModal() {
    this.refreshData()
    this.modalReference = this.modalServices.open(this.content, { size: "lg"});
    this.title = "Add KPI";
  }

 
  openSubModal() {
    // this.getAllPolicy();
    this.modalReference = this.modalServices.open(this.content_addLL, {
      size: "lg",
    });
    this.pdcaData  = [
      {
        id: 1,
        name: 'YES'
      },
      {
        id: 2,
        name: 'NO'
      }
    ];
    this.modalReference.result.then((result) => {
      this.pdcaData = [] 
    }, (reason) => {
      this.pdcaData = [] 
    });
    this.title = "Add Sub KPI";
  }

}
