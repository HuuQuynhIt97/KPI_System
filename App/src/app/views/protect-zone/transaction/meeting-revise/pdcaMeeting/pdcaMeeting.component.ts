import { MeetingService } from 'src/app/_core/_service/meeting.service';
import { EnvService } from './../../../../../_core/_service/env.service';
import { environment } from './../../../../../../environments/environment';
import { UploadFileComponent } from '../../todolist2/upload-file/upload-file.component';
import { DatePipe } from '@angular/common';
import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Column, ColumnModel, GridComponent, IEditCell } from '@syncfusion/ej2-angular-grids';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { Action } from 'src/app/_core/_model/action';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { Subscription } from 'rxjs';
import { DatePicker } from '@syncfusion/ej2-angular-calendars';
declare var $: any;

@Component({
  selector: 'app-pdcaMeeting',
  templateUrl: './pdcaMeeting.component.html',
  styleUrls: ['./pdcaMeeting.component.scss'],
  providers: [DatePipe]
})
export class PdcaMeetingComponent implements OnInit, AfterViewInit, OnDestroy {

  @Input() data: any;
  @Input() currentTime: any;
  @Input() text: any;
  @ViewChild('grid') public grid: GridComponent;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  toolbarOptions = ['Add', 'Delete', 'Search'];
  policy = '';
  kpi = '';
  pic = '';
  gridData =[];
  months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
  month = '';
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };

  actions: Action[] = [];
  thisMonthYTD: any;
  thisMonthPerformance: any;
  thisMonthTarget: any;
  targetYTD: any;
  nextMonthTarget: any;
  status: any[];
  result: {
    id: number,
    content: string,
    updateTime: string,
    modifiedTime: string,
    createdTime: string,
    kPIId: number
  };
  content: any;
  performanceValue;
  thisMonthTargetValue;
  nextMonthTargetValue;
  ytdValue;
  thisMonthYTDValue;
  subscription: Subscription[] = [];
  base: any
  public allowExtensions: string = '.doc, .docx, .xls, .xlsx, .pdf';
  files = [];
  filesLeft = [];
  filesRight = [];
  type: any;
  public dpParams: IEditCell;
  typeText: any;
  target: { id: any; value: any; performance: any; kPIId: any; targetTime: any; createdTime: any; modifiedTime: any; yTD: any; createdBy: any; submitted: any; };
  userId: number;
  public elem: HTMLElement;
  public datePickerObj: DatePicker;
  dataAdd = [];
  valueYTD: number = 0;
  thisMonthValue: any;
  base_download: string;
  constructor(
    public activeModal: NgbActiveModal,
    public todolist2Service: Todolist2Service,
    private datePipe: DatePipe,
    private alertify: AlertifyService,
    public modalService: NgbModal,
    public meetingService: MeetingService,
    private env: EnvService
  ) {
    this.base = this.env.apiUrl.replace('/api/', '')
    this.base_download = this.env.apiUrl
   }
  ngOnDestroy(): void {
    this.subscription.forEach(item => item.unsubscribe());
  }
  ngAfterViewInit(): void {
    $(function () {
      $('[data-toggle="tooltip"]').tooltip()
    })

  }

  ngOnInit() {
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
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
    this.subscription.push(this.todolist2Service.currentUploadMessage.subscribe(message => { if (message) { this.getDownloadFiles(); } }));
    const month = this.currentTime.getMonth();
    this.month = this.months[month == 1 ? 12 : month - 1];
    this.getDownloadFiles();
    this.loadStatusData();
    this.loadData();
  }
  downloadFile(item) {
    const file_open_brower = ['png', 'jpg','pdf']
    var ext =  item.name.split('.').pop();
    if(file_open_brower.includes(ext)) {
      window.open(item.path,'_blank')
    } else {
      const file = {
        Name: item.name
      }
      const url = `${this.base_download}UploadFile/DownloadFileMeeting2`
      this.meetingService.download(url, file).subscribe(data =>{
        const blob = new Blob([data]);
        const downloadURL = window.URL.createObjectURL(data);
        const link = document.createElement('a');
        link.href = downloadURL;
        const ct = new Date();
        link.download = `${item.name}`;
        link.click();
      })
    }
  }
  dataBound(args){
    var headercelldiv = this.grid.element.getElementsByClassName("e-headercelldiv") as any;
    for (var i=0; i<headercelldiv.length; i++){
      headercelldiv[i].style.height = 'auto';
    };
  }

  openUploadModalComponent() {
    const modalRef = this.modalService.open(UploadFileComponent, { size: 'md', backdrop: 'static', keyboard: false });
    modalRef.componentInstance.data = this.data;
    modalRef.componentInstance.currentTime = this.currentTime;
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

  onChangeThisMonthPerformance(value) {
    if (this.thisMonthPerformance != null) {
      this.thisMonthPerformance.performance = +value;
    } else {
      this.thisMonthPerformance = {
        id: 0,
        value: 0,
        performance: 0,
        kPIId: this.data.id,
        targetTime: new Date().toISOString(),
        createdTime: new Date().toISOString(),
        modifiedTime: null,
        yTD: 0,
        createdBy: +JSON.parse(localStorage.getItem('user')).id,
      };
    }
  }

  onChangeThisMonthTarget(value) {
    if (this.thisMonthTarget != null) {
      this.thisMonthTarget.value = +value;
    } else {
      this.thisMonthTarget = {
        id: 0,
        value: +value,
        performance: 0,
        kPIId: this.data.id,
        targetTime: new Date().toISOString(),
        createdTime: new Date().toISOString(),
        modifiedTime: null,
        yTD: 0,
        createdBy: +JSON.parse(localStorage.getItem('user')).id,
      };
    }
  }

  onChangeNextMonthTarget(value) {
    if (this.nextMonthTarget != null) {
      this.nextMonthTarget.value = +value;
    } else {
      this.nextMonthTarget = {
        id: 0,
        value: +value,
        performance: 0,
        kPIId: this.data.id,
        targetTime: new Date().toISOString(),
        createdTime: new Date().toISOString(),
        modifiedTime: null,
        yTD: 0,
        createdBy: +JSON.parse(localStorage.getItem('user')).id,
        submitted: false
      };
    }
    this.thisMonthValue = +value
    // this.thisMonthTarget.value = +value;

  }

  onChangeThisMonthYTD(value) {
    if (this.thisMonthYTD != null) {
      this.thisMonthYTD.yTD = +value;
    } else {
      this.thisMonthYTD = {
        id: 0,
        value: this.thisMonthTargetValue,
        performance: this.performanceValue,
        kPIId: this.data.id,
        targetTime: new Date().toISOString(),
        createdTime: new Date().toISOString(),
        modifiedTime: null,
        yTD: 0,
        createdBy: +JSON.parse(localStorage.getItem('user')).id,
      };
    }
  }

  download() {
    this.todolist2Service.download(this.data.id, (this.currentTime as Date).toLocaleDateString('en-US') ).subscribe((data: any) => {
      const blob = new Blob([data],
        { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      const downloadURL = window.URL.createObjectURL(data);
      const link = document.createElement('a');
      link.href = downloadURL;
      const ct = new Date();
      link.download = `${ct.getFullYear()}${ct.getMonth()}${ct.getDay()}_archive.zip`;
      link.click();
    });
  }

  onChangeTargetYTD(value) {
    if (this.targetYTD != null) {
      this.targetYTD.value = +value;
      this.valueYTD = +value;
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

  onChangeContent(value, i) {
    this.gridData[i].doContent = value;
  }

  onChangeArchivement(value, i) {
    this.gridData[i].achievement = value;
  }

  onChangeStatus(value, i, item) {
    this.addOrUpdateStatus(item, (res) => {
      this.gridData[i].statusId = JSON.parse(value);
      this.gridData[i].actionStatusId = res.data.id;
    });
  }

  onChangeResult(value, i) {
    this.gridData[i].resultContent = value;
  }

  loadData() {
    this.loadKPIData();
    this.loadTargetData();
    this.loadPDCAAndResultData();
    this.loadActionData();
  }

  loadPDCAAndResultData() {
    const currentTime = this.datePipe.transform((this.currentTime as Date).toLocaleDateString('en-US'), "yyyy-MM-dd");
    this.gridData = [];
    this.todolist2Service.getPDCAForL0Revise(this.data.id || 0, currentTime, this.userId).subscribe(res => {
      console.log(res.data);
      this.month = res.month
      this.gridData = res.data;
      this.result = res.result;
      this.content = this.result?.content;
    });
  }

  loadActionData() {
    this.actions = [];
    const currentTime = this.datePipe.transform((this.currentTime as Date).toLocaleDateString('en-US'), "yyyy-MM-dd");
    this.todolist2Service.getActionsForUpdatePDCA(this.data.id || 0, currentTime, this.userId).subscribe(res => {
      this.actions = res.actions as Action[] || [];
    });
  }

  loadKPIData() {
    const currentTime = this.datePipe.transform((this.currentTime as Date).toLocaleDateString('en-US'), "yyyy-MM-dd");
    this.todolist2Service.getKPIForUpdatePDC(this.data.id || 0, currentTime).subscribe(res => {
      this.typeText = res.typeText
      this.type  = res.type
      this.kpi = res.kpi;
      this.policy = res.policy;
      this.pic = res.pic;
    });
  }

  loadTargetData() {
    const currentTime = this.datePipe.transform((this.currentTime as Date).toLocaleDateString('en-US'), "yyyy-MM-dd");
    this.todolist2Service.getTargetForUpdatePDCA(this.data.id || 0, currentTime).subscribe(res => {
      this.thisMonthYTD = res.thisMonthYTD;
      this.thisMonthPerformance = res.thisMonthPerformance;
      this.thisMonthTarget = res.thisMonthTarget;
      this.targetYTD = res.targetYTD;
      this.nextMonthTarget = res.nextMonthTarget;

      this.performanceValue = this.thisMonthPerformance !== null ? this.thisMonthPerformance?.performance : null;
      this.thisMonthTargetValue = this.thisMonthTarget?.value;
      this.nextMonthTargetValue = this.nextMonthTarget !== null ? this.nextMonthTarget?.value :  null;
      this.ytdValue = this.targetYTD?.value;
      this.thisMonthYTDValue = this.thisMonthYTD !== null ? this.thisMonthYTD?.ytd : null
    });
  }

  loadStatusData() {
    this.status = [];
    this.todolist2Service.getStatus().subscribe(res => {
      this.status = res || [];
    });
  }

  submit() {
    this.post(true);
  }

  back() {
    //this.post(false);
    this.save(false);
    // this.activeModal.close();

  }
  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 47 && charCode < 58 || charCode === 46  || charCode === 45) {
      return true;
    }
    return false;
  }
  validate(submitted) {
    if (submitted === true && this.thisMonthTarget !== null) {
      if (!this.performanceValue) {
        this.alertify.warning('Please input this month performance');
        return false;
      }

      if (!this.thisMonthYTDValue) {
        this.alertify.warning('Please input this month YTD');
        return false;
      }
    }
    if (this.nextMonthTargetValue === null) {
      this.alertify.warning('Please input next month target');
      return false;
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
          createdTime: this.datePipe.transform(this.currentTime, 'yyyy/MM/dd HH:mm:ss')
        }
        this.dataAdd.push(data)
        this.actions = this.actions.sort((x, y) => +new Date(x.deadline) - +new Date(y.deadline))
        this.grid.refresh()
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
          this.alertify.success("成功刪除")
          this.loadActionData();
        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
        }
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );

  }

  save(submitted) {
    this.grid.editModule.endEdit()
    if (this.validate(submitted) == false) return;

    if(submitted === false && this.nextMonthTarget === null)
    {
      this.target = {
        id: this.thisMonthTarget !== null ? this.thisMonthTarget.id : 0,
        value: this.thisMonthTargetValue !== undefined ? this.thisMonthTargetValue : 0,
        performance: this.performanceValue !== "" && this.performanceValue !== null ? this.performanceValue : 0,
        kPIId: this.data.id,
        targetTime: this.thisMonthYTD !== null ? this.thisMonthYTD.targetTime : new Date().toISOString(),
        createdTime: this.thisMonthYTD !== null ? this.thisMonthYTD.createdTime : new Date().toISOString(),
        modifiedTime: this.thisMonthYTD !== null ? this.thisMonthYTD.modifiedTime : null,
        yTD: this.thisMonthYTDValue !== "" && this.thisMonthYTDValue !== null ? this.thisMonthYTDValue :  0,
        createdBy: this.thisMonthYTD !== null? this.thisMonthYTD.createdBy : +JSON.parse(localStorage.getItem('user')).id,
        submitted: true
      }
      this.nextMonthTarget = {
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
    } else {
      this.target = {
        id: this.thisMonthTarget !== null ? this.thisMonthTarget.id : 0,
        value: this.thisMonthTargetValue !== undefined ? this.thisMonthTargetValue : 0,
        performance: this.performanceValue !== "" && this.performanceValue !== null ? this.performanceValue : 0,
        kPIId: this.data.id,
        targetTime: this.thisMonthYTD !== null ? this.thisMonthYTD.targetTime : new Date().toISOString(),
        createdTime: this.thisMonthYTD !== null ? this.thisMonthYTD.createdTime : new Date().toISOString(),
        modifiedTime: this.thisMonthYTD !== null ? this.thisMonthYTD.modifiedTime : null,
        yTD: this.thisMonthYTDValue !== "" && this.thisMonthYTDValue !== null ? this.thisMonthYTDValue :  0,
        createdBy: this.thisMonthYTD !== null? this.thisMonthYTD.createdBy : +JSON.parse(localStorage.getItem('user')).id,
        submitted: true
      }

    }
    const updatePDCA = this.gridData;
    const dataSource = this.grid.dataSource as Action[];
    if(this.dataAdd.length === 0)
      this.dataAdd = dataSource
    const actions = this.dataAdd.map(x => {
      return {
        id: x.id,
        target: x.target,
        content: x.content,
        deadline: this.datePipe.transform(x.deadline, 'MM/dd/yyyy'),
        accountId: x.accountId ? x.accountId : +JSON.parse(localStorage.getItem('user')).id,
        kPIId: this.data.id,
        statusId: x.statusId,
        createdTime: x.createdTime,
        modifiedTime: this.datePipe.transform(this.currentTime, 'MM/dd/yyyy HH:mm:ss')
      }
    })
    const request = {
      target: this.target,
      targetYTD: this.targetYTD,
      nextMonthTarget: this.nextMonthTarget,
      actions: actions,
      updatePDCA: updatePDCA,
      result: this.result,
      currentTime: this.datePipe.transform(this.currentTime, 'MM/dd/yyyy'),
    }
    this.todolist2Service.saveUpdatePDCA(request).subscribe(
      (res) => {
        if (res.success === true) {
          this.todolist2Service.changeMessage(true);
          this.dataAdd = []
          this.activeModal.close();
        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );
  }

  post(submitted) {
    this.grid.editModule.endEdit()
    if (this.validate(submitted) == false) return;
    // if(submitted === true)
    // {
    //   this.target = {
    //     id: this.thisMonthTarget !== null ? this.thisMonthTarget.id : 0,
    //     value: this.thisMonthTargetValue,
    //     performance: this.performanceValue ?? 0,
    //     kPIId: this.data.id,
    //     targetTime: this.thisMonthYTD !== null ? this.thisMonthYTD.targetTime : new Date().toISOString(),
    //     createdTime: this.thisMonthYTD !== null ? this.thisMonthYTD.createdTime : new Date().toISOString(),
    //     modifiedTime: this.thisMonthYTD !== null ? this.thisMonthYTD.modifiedTime : null,
    //     yTD: this.thisMonthYTDValue ?? 0,
    //     createdBy: this.thisMonthYTD !== null? this.thisMonthYTD.createdBy : +JSON.parse(localStorage.getItem('user')).id,
    //     submitted: submitted
    //   }
    // };

    if(submitted === false && this.nextMonthTarget === null)
    {
      this.target = {
        id: this.thisMonthTarget !== null ? this.thisMonthTarget.id : 0,
        value: this.thisMonthTargetValue,
        performance: this.performanceValue ?? 0,
        kPIId: this.data.id,
        targetTime: this.thisMonthYTD !== null ? this.thisMonthYTD.targetTime : new Date().toISOString(),
        createdTime: this.thisMonthYTD !== null ? this.thisMonthYTD.createdTime : new Date().toISOString(),
        modifiedTime: this.thisMonthYTD !== null ? this.thisMonthYTD.modifiedTime : null,
        yTD: this.thisMonthYTDValue ?? 0,
        createdBy: this.thisMonthYTD !== null? this.thisMonthYTD.createdBy : +JSON.parse(localStorage.getItem('user')).id,
        submitted: true
      }
        this.nextMonthTarget = {
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
    } else {
      this.target = {
        id: this.thisMonthTarget !== null ? this.thisMonthTarget.id : 0,
        value: this.thisMonthTargetValue === undefined ? this.thisMonthValue : this.thisMonthTargetValue,
        performance: this.performanceValue ?? 0,
        kPIId: this.data.id,
        targetTime: this.thisMonthYTD !== null ? this.thisMonthYTD.targetTime : new Date().toISOString(),
        createdTime: this.thisMonthYTD !== null ? this.thisMonthYTD.createdTime : new Date().toISOString(),
        modifiedTime: this.thisMonthYTD !== null ? this.thisMonthYTD.modifiedTime : null,
        yTD: this.thisMonthYTDValue ?? 0,
        createdBy: this.thisMonthYTD !== null? this.thisMonthYTD.createdBy : +JSON.parse(localStorage.getItem('user')).id,
        submitted: true
      }
    }
    const updatePDCA = this.gridData;
    const dataSource = this.grid.dataSource as Action[];
    if(this.dataAdd.length === 0)
      this.dataAdd = dataSource
    const actions = dataSource.map(x => {
      return {
        id: x.id,
        target: x.target,
        content: x.content,
        deadline: this.datePipe.transform(x.deadline, 'MM/dd/yyyy'),
        accountId: x.accountId ? x.accountId : +JSON.parse(localStorage.getItem('user')).id,
        kPIId: this.data.id,
        statusId: x.statusId,
        createdTime:  this.datePipe.transform(this.currentTime, 'MM/dd/yyyy'),
        modifiedTime: null
      }
    })
    const request = {
      target: this.target,
      targetYTD: this.targetYTD,
      nextMonthTarget: this.nextMonthTarget,
      actions: actions,
      updatePDCA: updatePDCA,
      result: this.result,
      userId: this.userId,
      currentTime: this.datePipe.transform(this.currentTime, 'MM/dd/yyyy'),
    }
    this.todolist2Service.submitUpdatePDCA(request).subscribe(
      (res) => {
        if (res.success === true) {

          this.todolist2Service.changeMessage(true);
          this.dataAdd = []
          this.activeModal.close();

        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );
  }

  addOrUpdateStatus(data, callBack) {
    const request = {
      actionId: data.actionId,
      statusId: +data.statusId,
      actionStatusId: data.actionStatusId || 0,
      currentTime: (this.currentTime as Date).toLocaleDateString('en-US'),
    }
    this.todolist2Service.addOrUpdateStatus(request).subscribe(
      (res) => {
        callBack(res);
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );
  }

  getDownloadFiles() {
    this.todolist2Service.getDownloadFiles(this.data.id, (this.currentTime as Date).toLocaleDateString('en-US')).subscribe(res => {
      this.files = [];
      const files = res as any || [];

      this.files = files.map(x=> {
        return {
          name: x.name,
          path: this.base + x.path
        }
      });
      this.filesLeft = [];
      this.filesRight = [];
      let i = 0;
      for (const item of this.files) {
        i++;
        if (i <= 3) {
          this.filesLeft.push(item);
        } else {
          this.filesRight.push(item);
        }
      }

    });
  }

}
