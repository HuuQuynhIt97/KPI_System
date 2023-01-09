import { NgxSpinnerService } from 'ngx-spinner';
import { DatePipe } from '@angular/common';
import { Component, Input, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Column, GridComponent, IEditCell, TextWrapSettingsModel } from '@syncfusion/ej2-angular-grids';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { Action } from 'src/app/_core/_model/action';
import { Target } from 'src/app/_core/_model/target';
import { TargetYTD } from 'src/app/_core/_model/targetytd';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { forkJoin } from 'rxjs';
import { isNumeric } from 'rxjs/util/isNumeric';
import { DatePicker } from '@syncfusion/ej2-angular-calendars';
declare var $: any;

@Component({
  selector: 'app-planStringType',
  templateUrl: './planStringType.component.html',
  styleUrls: ['./planStringType.component.scss'],
  providers: [DatePipe]
})
export class PlanStringTypeComponent implements OnInit, AfterViewInit {

  @Input() data: any;
  @ViewChild('grid') grid: GridComponent;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  gridData: any;
  toolbarOptions = ['Add', 'Delete', 'Search'];
  policy = '效率精進';
  kpi = 'SHC CTB IE 工時達成率';
  pic = '生產中心 Lai He';
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  @Input() currentTime: any;
  public dpParams: IEditCell;
  actions: Action[] = [];
  target: Target;
  targetYTD: TargetYTD;
  targetValue = null;
  targetYTDValue = null;
  typeText: string;
  contentText: string;
  targetText: string;
  deadlineText: string;
  userId: number
  public elem: HTMLElement;
  public datePickerObj: DatePicker;
  dataAdd = [];
  public wrapSettings: TextWrapSettingsModel;
  constructor(
    public activeModal: NgbActiveModal,
    public todolist2Service: Todolist2Service,
    private alertify: AlertifyService,
    private datePipe: DatePipe,
    private spinner: NgxSpinnerService

  ) { }
  ngAfterViewInit(): void {
    $(function () {
      $('[data-toggle="tooltip"]').tooltip()
    })
  }

  ngOnInit() {
    this.wrapSettings = { wrapMode: 'Content' };
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.loadData();
    this.dpParams = {
      params: {
        value: new Date() ,
        min: new Date()
      },
      create: () => {
        this.elem = document.createElement('input');
        return this.elem;
      },
      read: () => {
          return this.datePickerObj.value;
      },
      destroy: () => {
          this.datePickerObj.destroy();
      },
      write: (args: { rowData: object, column: Column }) => {
        this.datePickerObj = new DatePicker({
          value: args.rowData[args.column.field] === undefined ? new Date() : new Date(args.rowData[args.column.field]) ,
          floatLabelType: 'Never'
        });
        this.datePickerObj.appendTo(this.elem);
      }

    };
  }
  onChangeTarget(value) {
    if (this.target != null) {
      this.target.value = +value;
    } else {
      this.target = {
        id: 0,
        value: +value,
        performance: 0,
        kPIId: this.data.id,
        targetTime: new Date().toISOString(),
        createdTime: new Date().toISOString(),
        modifiedTime: null,
        yTD: 0,
        createdBy: +JSON.parse(localStorage.getItem('user')).id,
        submitted: true
      };
    }

  }
  onChangeTargetYTD(value) {
    if (this.targetYTD != null) {
      this.targetYTD.value = +value;
    } else {
      this.targetYTD = {
        id: 0,
        value: +value,
        createdTime: new Date().toISOString(),
        modifiedBy: null,
        modifiedTime: null,
        createdBy: +JSON.parse(localStorage.getItem('user')).id,
        kPIId: this.data.id
      };
    }
  }

  submit(){
    this.grid.editModule.endEdit()
    if (this.validate(true) == false) return;
    this.spinner.show();
    if(this.typeText === 'string') {
      this.target = {
        id: 0,
        value: 0,
        performance: 0,
        kPIId: this.data.id,
        targetTime: new Date().toISOString(),
        createdTime: new Date().toISOString(),
        modifiedTime: null,
        yTD: 0,
        createdBy: +JSON.parse(localStorage.getItem('user')).id,
        submitted: true
      };
      this.targetYTD = {
        id: 0,
        value: 0,
        createdTime: new Date().toISOString(),
        modifiedBy: null,
        modifiedTime: null,
        createdBy: +JSON.parse(localStorage.getItem('user')).id,
        kPIId: this.data.id
      };
    }
    const dataSource = this.grid.dataSource as Action[];
    if(this.dataAdd.length === 0)
      this.dataAdd = dataSource
    const actions = this.dataAdd.map(x => {
      return {
        id: x.id ?? 0,
        target: x.target,
        content: x.content,
        deadline: typeof(x.deadline) != "string" ? (x.deadline as Date).toLocaleDateString('en-US'): x.deadline,
        accountId: x.accountId ? x.accountId : +JSON.parse(localStorage.getItem('user')).id,
        kPIId: this.data.id,
        statusId: x.statusId,
        createdTime: new Date().toISOString(),
        modifiedTime: null
      }
    })
    const request = {
      actions: actions,
      target: this.target,
      targetYTD: this.targetYTD ,
      userId: this.userId,
      currentTime: (this.currentTime as Date).toLocaleDateString('en-US')
    };

    const post = this.todolist2Service.submitAction(request)
    const submitKPINew = this.todolist2Service.submitKPINew(this.data.id);
    forkJoin([post, submitKPINew]).subscribe(response => {
      const arr = response.map(x=> x.success);
      const checker = arr.every(Boolean);
      if (checker) {
        this.todolist2Service.changeMessage(true);
        this.dataAdd = []
        this.activeModal.close();
        this.spinner.hide();

      } else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        this.spinner.hide();
      }
    })
  }
  back(){
    this.post(()=>{
      this.todolist2Service.changeMessage(true);
      this.activeModal.close();
    }, false);
    // this.activeModal.close();

  }
  validate(isSubmit) {
    if(this.typeText !== 'string') {
      if (!this.target) {
        this.alertify.warning('Please input next month target');
        return false;
      }
      if (!this.targetYTD) {
        this.alertify.warning('Please input target YTD');
        return false;
      }

      if (!isNaN(this.targetValue) === false){
        this.alertify.warning('Next month target value is number');
        return false;
      }
      if (!isNaN(this.targetYTDValue) === false){
        this.alertify.warning('Target YTD value is number');
        return false;
      }
    }
    const dataSource = (this.grid.dataSource as Action[]) || [];
    if (isSubmit) {
      if (dataSource.length == 0) {
        this.alertify.warning('Please create actions');
        return false;
      }
    }


    return true;
  }
  actionBegin(args) {
    if(args.requestType === 'save' && args.action === 'add')  {
      setTimeout(() => {
        const data = {
          target: args.data.target,
          content: args.data.content,
          deadline: args.data.deadline,
          createdTime: this.datePipe.transform(new Date(), 'yyyy/MM/dd HH:mm:ss')
        }
        this.dataAdd.push(data)
      }, 300);
    }
    if(args.requestType === 'save' && args.action === 'edit') {
      for (let item in this.grid.dataSource) {
        if(this.grid.dataSource[item].id !== undefined && this.grid.dataSource[item].id === args.data.id) {
          this.grid.dataSource[item].content = args.data.content
          this.grid.dataSource[item].target = args.data.target
          this.grid.dataSource[item].deadline = args.data.deadline
        }
      }
      this.dataAdd = this.grid.dataSource as any;
    }
    if (args.requestType === 'delete') {
      if (args.data[0].id === undefined) {
        this.alertify.success("成功刪除");
      } else {
        this.delete(args.data[0].id);
      }
    }
  }
  delete(id) {
    this.todolist2Service.deleteAc(id).subscribe(
      (res) => {
        if (res === true) {
          this.alertify.success("成功刪除");
          this.loadData();
        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
        }
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );
  }
  post(callBack, isSubmit = true) {
    this.grid.editModule.endEdit()
    if (this.validate(isSubmit) == false) return;
    const dataSource = this.grid.dataSource as Action[];
    if(this.typeText === 'string') {
      this.target = {
        id: 0,
        value: 0,
        performance: 0,
        kPIId: this.data.id,
        targetTime: new Date().toISOString(),
        createdTime: new Date().toISOString(),
        modifiedTime: null,
        yTD: 0,
        createdBy: +JSON.parse(localStorage.getItem('user')).id,
        submitted: true
      };
      this.targetYTD = {
        id: 0,
        value: 0,
        createdTime: new Date().toISOString(),
        modifiedBy: null,
        modifiedTime: null,
        createdBy: +JSON.parse(localStorage.getItem('user')).id,
        kPIId: this.data.id
      };
    }
    if(this.dataAdd.length === 0)
      this.dataAdd = dataSource
    const actions = this.dataAdd.map(x => {
      return {
        id: x.id,
        target: x.target,
        content: x.content,
        deadline: typeof(x.deadline) != "string" ? (x.deadline as Date).toLocaleDateString(): x.deadline,
        accountId: x.accountId ? x.accountId : +JSON.parse(localStorage.getItem('user')).id,
        kPIId: this.data.id,
        statusId: x.statusId,
        createdTime: x.createdTime,
        modifiedTime: this.datePipe.transform(new Date(), 'yyyy/MM/dd HH:mm:ss')
      }
    })
    const request = {
      actions: actions,
      target: this.target,
      targetYTD: this.targetYTD,
      currentTime: (this.currentTime as Date).toLocaleDateString('en-US')
    };

    this.todolist2Service.saveAction(request).subscribe(
      (res) => {
        if (res.success === true) {
          this.dataAdd = []
          callBack();

        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );
  }
  submitKPINew() {
    this.todolist2Service.submitKPINew(this.data.id).subscribe(
      (res) => {
        this.todolist2Service.changeMessage(true);
        this.activeModal.close();
        this.spinner.hide();
      },
      (err) => {this.spinner.hide();}
    );
  }

  loadData() {
    this.gridData = [];
    this.todolist2Service.getActionsForL0(this.data.id || 0, this.userId).subscribe(res => {
      this.typeText = res.typeText;
      this.actions = res.actions as Action[] || [];
      this.pic = res.pic;
      this.policy = res.policy;
      this.kpi = res.kpi;
      this.target = res.target;
      this.targetYTD = res.targetYTD;
      this.targetValue = this.target?.value !== 0 ? this.target?.value : "";
      this.targetYTDValue = this.targetYTD?.value !== 0 ? this.targetYTD?.value : "";
    });
  }

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
