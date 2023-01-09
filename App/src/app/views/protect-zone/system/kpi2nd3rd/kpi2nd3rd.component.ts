import { DatePipe } from '@angular/common'
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core'
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap'
import { TranslateService } from '@ngx-translate/core'
import { CalendarView } from '@syncfusion/ej2-angular-calendars'
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns'
import { IEditCell } from '@syncfusion/ej2-angular-grids'
import { TreeGridComponent } from '@syncfusion/ej2-angular-treegrid'
import { ModalDirective } from 'ngx-bootstrap/modal'
import { NgxSpinnerService } from 'ngx-spinner'
import { MessageConstants } from 'src/app/_core/_constants/system'
import { Account } from 'src/app/_core/_model/account'
import { Account2Service } from 'src/app/_core/_service/account2.service'
import { AlertifyService } from 'src/app/_core/_service/alertify.service'
import { KpinewService } from 'src/app/_core/_service/kpinew.service'
import { OcService } from './../../../../_core/_service/oc.service'

@Component({
  selector: 'app-kpi2nd3rd',
  templateUrl: './kpi2nd3rd.component.html',
  styleUrls: ['./kpi2nd3rd.component.scss'],
  providers:[DatePipe]
})
export class Kpi2nd3rdComponent implements OnInit {

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
  picItem: any = [];
  typeId: number = 0;
  parentId: null
  level: number = 1
  typeData: Object;
  accountData: Account[];
  modalReference: NgbModalRef
  kpiname: any = null
  userId: number
  currentLevel: any
  startTime = new Date();
  endTime = new Date(new Date().getFullYear(), 11, 31);
  public dpParams: IEditCell;
  yearData: any =  []
  public yearValue: number = new Date().getFullYear() - 2;
  @ViewChild('ddlelement')
  public dropDownListObject: DropDownListComponent;
  yearSelect: number = new Date().getFullYear();
  yearAdd: string
  isCollslape: boolean = false
  public value: Date = new Date(2020, 8);

  public start: CalendarView = 'Year';
  public depth: CalendarView = 'Year';
  public format: string = 'MMMM yyyy'
  constructor(
    private ocService: OcService,
    private modalServices: NgbModal,
    private accountService: Account2Service,
    private kpiNewService: KpinewService,
    private alertify: AlertifyService,
    private datePipe: DatePipe,
    private spinner: NgxSpinnerService,
    private translate: TranslateService

  ) {
    // let message = this.translate.instant('MESSAGE_PARENT_KPI')
  }

  ngOnInit() {
    this.getListPic();
    if (localStorage.getItem('user') !== null) {
      this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    }
    this.dpParams = { params: {
      value: new Date() ,
      min: new Date()
    } };
    
    // this.editing = { allowDeleting: true, allowEditing: true, mode: "Row" };
    // this.toolbar = ["Delete", "Search", "Update", "Cancel"];
    this.optionTreeGrid();
    this.onService();
    this.pushYearData()
    // this.getAllUsers();
    this.getAllType()
  }
 
 
  getKPIAsTreeView() {
    this.spinner.show()
    const lang = localStorage.getItem('lang');
    this.kpiNewService.getTree(lang).subscribe((res: any) => {
      this.data = res.filter(x => Number(x.entity.year) === this.yearSelect);
      this.dataTamp = res;
      this.spinner.hide()
    });
  }
  displayOrHiddenToDo(item) {
    this.kpiNewService.IsDisPlayTodoUpdate(item.id).subscribe(res => {
      if (res) {
        this.alertify.success('success')
        this.getKPIAsTreeView()
      }
    })
  }
  ngAfterViewInit() {
    // Set null value to value property for clear the selected item
    document.getElementById('btn').onclick = () => {
      this.dropDownListObject.value = null;
    }
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
  created() {
    this.getKPIAsTreeView();
  }
  onChangeYear(args) {
    if(args.isInteracted)
      this.data = this.dataTamp.filter(x => Number(x.entity.year) === args.value)
  }

  onClickDefault() {
    this.data = this.dataTamp
  }

  getListPic() {
    this.kpiNewService.getListPic().subscribe((res: any) => {
      this.accountData = res ;
    })
  }

  getAllType() {
    const lang = localStorage.getItem('lang');
    this.kpiNewService.getAllType(lang).subscribe(res => {
      this.typeData = res
    })
  }
  getAllUsers() {
    this.accountService.getAll().subscribe((res: any) => {
      this.accountData = res ;
    });
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
    if (this.typeId === 0) {
      this.alertify.error('Please select a Type! ');
      return false;
    }
    if (this.picItem.length === 0) {
      this.alertify.error('Please select a PIC! ');
      return false;
    }
    return true;

  }
  refreshData() {
    this.kpiname = null
    this.parentId = null
    this.level  = 1
    this.policyId = 0
    this.picId = 0
    this.typeId = 0
    this.picItem = []
  }
  
  createOC() {
    const model = {
      Name: this.kpiname,
      PolicyId: this.policyId,
      TypeId: this.typeId,
      ParentId: this.parentId,
      Level: this.level,
      Pic: this.picId,
      UpdateBy: this.userId,
      KpiIds: this.picItem,
      Year: this.yearAdd,
      StartDisplayMeetingTime: this.datePipe.transform(this.startTime, "yyyy-MM-dd"),
      EndDisplayMeetingTime: this.datePipe.transform(this.endTime, "yyyy-MM-dd")
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
        text: "Add-Sub Item",
        iconCss: " e-icons e-add",
        target: ".e-content",
        id: "Add-Sub-Item",
      },
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
          text: 'Add LL KPI',
          prefixIcon: 'e-add',
          tooltipText: 'Add lower level KPI',
          id: 'treegrid_gridcontrol_add',
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
          text: '新增下一階KPI',
          prefixIcon: 'e-add',
          tooltipText: '新增下一階KPI',
          id: 'treegrid_gridcontrol_add',
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

  

  onService() {
    this.ocService.currentMessage.subscribe((arg) => {
      if (arg === 200) {
        this.getKPIAsTreeView();
      }
    });
  }

  toolbarClick(args) {
    // const lang = localStorage.getItem('lang');
    // const message = lang == 'vi' ? 'Hiện tại không thể tạo KPIs nhỏ hơn cấp độ 3!' : lang === 'en' ? 'Currently, you cannot create KPIs smaller than this level' : '目前無法建立小於這階的KPI';
    if(this.level > 1) {
      switch (args.item.id) {
        case "treegrid_gridcontrol_add":
          args.cancel = true;
          this.openMainModal();
          break;
        default:
          break;
      }
    } else {
      if (args.item.id === 'treegrid_gridcontrol_add') {
        args.cancel = true;
        this.alertify.error(this.translate.instant('MESSAGE_PARENT_KPI'));
      }
    }
    
    // if (this.currentLevel === 3 && args.item.id === 'treegrid_gridcontrol_add') {
    //   args.cancel = true;
    //   this.alertify.warning(message);
    //   return;
    // }
    // else {

    // }
  }

  contextMenuClick(args) {
    const lang = localStorage.getItem('lang')  ;
    const message = lang == 'vi' ? 'Hiện tại không thể tạo KPIs nhỏ hơn cấp độ 3!' : lang === 'en' ? 'Currently, you cannot create KPIs smaller than this level' : '目前無法建立小於這階的KPI';
    if( this.currentLevel === 3 && args.item.id === 'Add-Sub-Item') {
      args.cancel = true;
      this.alertify.warning(message);
      return;
    } else {
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
    if(data.startDisplayMeetingTime !== null)
      this.startTime = new Date(data.startDisplayMeetingTime)
    if(data.endDisplayMeetingTime !== null)
      this.endTime = new Date(data.endDisplayMeetingTime)
  }

  actionComplete(args) {
    if (args.requestType === 'beginEdit') {
      var tooltips = args.row.querySelectorAll('.e-tooltip');
      for (var i = 0; i < tooltips.length; i++) {
        tooltips[i].ej2_instances[0].destroy();
      }
      const item = args.rowData.entity;
      const check_pic = item.pics.includes(this.userId)
      console.log(item.createBy);
      console.log(item.updateBy);
      console.log(this.userId);
      if (item.createBy !== this.userId && item.updateBy !== this.userId) {
        this.alertify.warning(this.translate.instant('MESSAGE_PERMISSION_DENIED_KPI'));
        // this.alertify.warning("you do not access this");
        args.cancel = true;
        this.treeGridObj.refresh()
        return;
      }
      this.updateModel(item);
    }
    if (args.requestType === 'save' && args.action === 'edit') {
      const model = {
        Id: args.data.entity.id,
        Name: args.data.entity.name,
        PolicyId: this.policyId,
        TypeId: this.typeId,
        Level: args.data.entity.level,
        ParentId: args.data.entity.parentId,
        Pic: this.picId,
        UpdateBy: this.userId,
        KpiIds: this.picItem,
        Sequence: args.data.entity.sequence,
        StartDisplayMeetingTime: this.datePipe.transform(this.startTime, "yyyy-MM-dd"),
        EndDisplayMeetingTime: this.datePipe.transform(this.endTime, "yyyy-MM-dd")
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
  contextMenuOpen(args) {
    if (args.rowInfo.rowData.entity.createBy !== this.userId) {
      args.cancel = true;
      return;
    }
    const lang = localStorage.getItem('lang')  ;
    const message = lang == 'vi' ? 'Hiện tại không thể tạo KPIs nhỏ hơn cấp độ 3!' : lang === 'en' ? 'Currently, you cannot create KPIs smaller than this level' : '目前無法建立小於這階的KPI';
    if (this.currentLevel === 3 && args.rowInfo.rowData.entity.createBy !== this.userId) {
      args.cancel = true;
      this.alertify.warning(message);
      return;
    }

    if (this.currentLevel === 3 && args.rowInfo.rowData.entity.createBy === this.userId) {
      document.querySelectorAll('li#Add-Sub-Item')[0].setAttribute('style', 'display: none;');
      document.querySelectorAll('li#DeleteOC')[0].setAttribute('style', 'display: block;');
    } else {
      document.querySelectorAll('li#Add-Sub-Item')[0].setAttribute('style', 'display: block;');
      document.querySelectorAll('li#DeleteOC')[0].setAttribute('style', 'display: block;');
    }
  }
  rowSelected(args) {
    // if (args.data.entity.createBy === this.userId) {
    // }
    this.parentId = args.data.entity.id
    this.level = args.data.entity.level + 1
    this.currentLevel = args.data.entity.level
    this.yearAdd = args.data.entity.year === null || args.data.entity.year === '' ? args.data.entity.createdTimeYear : args.data.entity.year
  }

  clearFrom() {
    this.oc = {
      id: 0,
      name: "",
      parentID: null,
      level: 1
    };
  }

  rename() {
    this.ocService.rename(this.edit).subscribe((res) => {
      this.getKPIAsTreeView();
      this.alertify.success("The oc has been changed!!!");
    });
  }
  openMainModal() {
    this.modalReference = this.modalServices.open(this.content, { size: "lg"});
    this.title = "Add KPI";
    this.getListPic();
    this.startTime = new Date();
    this.endTime = new Date(new Date().getFullYear(), 11, 31);

  }
  openSubModal() {
    // this.getAllPolicy();
    this.modalReference = this.modalServices.open(this.content, {
      size: "lg",
    });
    this.title = "Add Sub KPI";
    this.getListPic();
  }

}
