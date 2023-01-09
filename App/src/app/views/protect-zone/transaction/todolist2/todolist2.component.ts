import { Action } from './../../../../_core/_model/application-user';
import { PdcaStringTypeComponent } from './pdcaStringType/pdcaStringType.component';
import { PlanStringTypeComponent } from './planStringType/planStringType.component';
import { PdcaComponent } from './pdca/pdca.component';
import { PlanComponent } from './plan/plan.component';
import { PerformanceService } from './../../../../_core/_service/performance.service';
import { Subscription } from 'rxjs';
import { AccountGroupService } from './../../../../_core/_service/account.group.service';
import { Component, OnInit, TemplateRef, ViewChild, QueryList, ViewChildren, OnDestroy } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { ObjectiveService } from 'src/app/_core/_service/objective.service';
import { AccountGroup } from 'src/app/_core/_model/account.group';
import { Todolistv2Service } from 'src/app/_core/_service/todolistv2.service';
import { PeriodType, SystemRole, ToDoListType, SystemScoreType } from 'src/app/_core/enum/system';
import { environment } from 'src/environments/environment';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { Performance } from 'src/app/_core/_model/performance';
import { DatePipe } from '@angular/common';
import { SpreadsheetComponent } from '@syncfusion/ej2-angular-spreadsheet';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { NgTemplateNameDirective } from '../ng-template-name.directive';
import { Router } from '@angular/router';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { TranslateService } from '@ngx-translate/core';
import { DataService } from 'src/app/_core/_service/data.service';
@Component({
  selector: 'app-todolist2',
  templateUrl: './todolist2.component.html',
  styleUrls: ['./todolist2.component.scss'],
  providers: [DatePipe]
})
export class Todolist2Component implements OnInit, OnDestroy {
  @ViewChild('grid') grid: GridComponent;
  @ViewChildren(NgTemplateNameDirective) public Gridtemplates: QueryList<NgTemplateNameDirective>;
  gridData: object;
  toolbarOptions = ['Search'];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  accountGroupData: AccountGroup[];
  KPI = ToDoListType.KPI as string;
  Attitude = ToDoListType.Attitude as string;
  scoreType: SystemScoreType;
  modalReference: NgbModalRef;
  @ViewChild('importModal') importModal: NgbModalRef;
  @ViewChild('message') message: NgbModalRef;
  file: any;
  excelDownloadUrl: string;
  currentTime: any;
  currentTimeRequest: any;
  index: any = 1;
  subscription: Subscription[] = [];
  @ViewChild('remoteDataBinding')
  public spreadsheetObj: SpreadsheetComponent;
  public sheetName = 'Upload Details';
  isAuthorize = false;
  content: string;
  roleUser: any;
  userId: number
  userName: string;
  thisMonthYTD: any;
  thisMonthPerformance: any;
  thisMonthTarget: any;
  targetYTD: any;
  nextMonthTarget: any;
  performanceValue: any;
  thisMonthTargetValue: any;
  nextMonthTargetValue: any;
  ytdValue: any;
  thisMonthYTDValue: any;
  gridDataSubmit: any[];
  result: any;
  actions: any[];
  target: { id: any; value: any; performance: number; kPIId: any; targetTime: any; createdTime: any; modifiedTime: any; yTD: number; createdBy: any; submitted: any; };
  kpiId: any;
  kpiData: any;
  constructor(
    private service: ObjectiveService,
    private router: Router,
    private alertify: AlertifyService,
    public todolistService: Todolistv2Service,
    public todolist2Service: Todolist2Service,
    private accountGroupService: AccountGroupService,
    public modalService: NgbModal,
    private datePipe: DatePipe,
    private spinner: NgxSpinnerService,
    private translate: TranslateService,
    private dataService: DataService,
    private performanceService: PerformanceService
  ) {
    if(localStorage.getItem('anonymous') === "yes") {
      this.userName = "admin";
    }
  }
  ngOnDestroy(): void {
    this.subscription.forEach(item => item.unsubscribe());
  }
  onChangeReportTime(value: Date): void {
    this.spinner.show()
    this.loadData();
  }

  ngOnInit(): void {
    this.currentTime = new Date();
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.content = '';
    this.roleUser = JSON.parse(localStorage.getItem('level')).code;
    this.loadAccountGroupData();
    this.subscription.push(this.todolist2Service.currentMessage.subscribe(message => { if (message)
    {
      this.spinner.show()
      this.loadData();
    } }));

    this.dataService.locale.subscribe((res: any)=>{
      this.translate.addLangs([res])
      this.translate.use(res)
    })
  }
  async getAsyncData(id) {
    await this.loadTargetData(id);
    await this.loadPDCAAndResultData(id);
    await this.loadActionData(id);
  }
 submitKPI(data) {
   this.kpiData = data
   this.openVerticallyCentered(this.message)
    // this.alertify.delete(this.translate.instant('MESSAGE_SUBMIT_PDCA_1'),this.translate.instant('MESSAGE_SUBMIT_PDCA_2'))
    // .then((result) => {
    //   if (result) {
    //     const res =  this.getAsyncData(data.id);
    //     setTimeout(() => {
    //       this.post(data,true)
    //     }, 300);
    //   }
    // })
    // .catch((err) => {
    // });

  }

  openVerticallyCentered(content) {
    this.modalService.open(content, { centered: true });
  }
  async yesConfirm() {
    this.spinner.show();
    await this.getAsyncData(this.kpiData.id);
    await this.post(this.kpiData,true)
    // await setTimeout(() => {
    //   this.modalService.dismissAll();
    // }, 300);
  }
  post(data,submitted) {
    // this.grid.editModule.endEdit()

    if (this.validate(data,submitted) == false) return;
    this.target = {
      id: this.thisMonthTarget.id,
      value: this.thisMonthTargetValue,
      performance: 0,
      kPIId: data.id,
      targetTime: this.thisMonthYTD.targetTime,
      createdTime: this.thisMonthYTD.createdTime,
      modifiedTime: this.thisMonthYTD.modifiedTime,
      yTD: 0,
      createdBy: this.thisMonthYTD.createdBy,
      submitted: submitted
    };

    this.nextMonthTarget = {
      id: 0,
      value: 0,
      performance: 0,
      kPIId: data.id,
      targetTime: new Date().toISOString(),
      createdTime: new Date().toISOString(),
      modifiedTime: null,
      yTD: 0,
      createdBy: +JSON.parse(localStorage.getItem('user')).id,
      submitted: false
    };
    const updatePDCA = this.gridDataSubmit;
    // const dataSource = this.grid.dataSource as Action[];
    // if(this.dataAdd.length === 0)
    //   this.dataAdd = dataSource
    console.log(this.actions);
    const actions = this.actions.map(x => {
      return {
        id: x.id,
        target: x.target,
        content: x.content,
        deadline: this.datePipe.transform(x.deadline, 'MM/dd/yyyy'),
        accountId: x.accountId ? x.accountId : +JSON.parse(localStorage.getItem('user')).id,
        kPIId: data.id,
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
      userId: this.userId,
      currentTime: this.datePipe.transform(this.currentTime, 'MM/dd/yyyy'),
    }
    this.todolist2Service.submitUpdatePDCA(request).subscribe(
      (res) => {
        if (res.success === true) {
          this.todolist2Service.changeMessage(true);
          this.alertify.success(this.translate.instant('SUBMIT_PDCA_SUCCESS'));
          this.modalService.dismissAll();
          this.spinner.hide()
        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
          this.spinner.hide()
        }
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );
  }

  validate(data,submitted) {
    if(data.typeText !== 'string'){
      if (this.nextMonthTargetValue === null) {
        this.alertify.warning('Please input next month target');
        return false;
      }
    }

    return true;
  }

  loadActionData(id) {
    this.actions = [];
    const currentTime = this.datePipe.transform((this.currentTime as Date).toLocaleDateString('en-US'), "yyyy-MM-dd");
    return new Promise((result, rej) => {
      this.todolist2Service.getActionsForUpdatePDCA(id || 0, currentTime, this.userId ).subscribe(
        (res: any) => {
          this.actions = res.actions as Action[] || [];
          console.log('loadActionData',this.actions);
          result(result);
        },
        (error) => {
          rej(error);
        }
      );
    });

  }
  loadPDCAAndResultData(id) {
    this.gridDataSubmit = [];
    const currentTime = this.datePipe.transform((this.currentTime as Date).toLocaleDateString('en-US'), "yyyy-MM-dd");
    return new Promise((result, rej) => {
      this.todolist2Service.getPDCAForL0(id || 0, currentTime, this.userId).subscribe(
        (res: any) => {
          this.gridDataSubmit = res.data;
          this.result = res.result;
          this.content = this.result?.content;
          result(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadTargetData(id) {
    const currentTime = this.datePipe.transform((this.currentTime as Date).toLocaleDateString('en-US'), "yyyy-MM-dd");
    return new Promise((result, rej) => {
      this.todolist2Service.getTargetForUpdatePDCA(id || 0, currentTime).subscribe(
        (res: any) => {
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
          result(result);
        },
        (error) => {
          rej(error);
        }
      );
    });

  }

  isAllowAccess(position: number) {
    const positions = JSON.parse(localStorage.getItem('user')).accountGroupPositions as number[] || [];
    return positions.includes(position);
  }

  loadData() {
    this.currentTimeRequest = this.datePipe.transform(this.currentTime, "yyyy-MM-dd HH:mm");
    let systemRole = 1;
    // if (this.content === '') {
    //   this.content = this.accountGroupData[0]?.name;
    //   systemRole =SystemRole[this.accountGroupData[0]?.name] as any  || 0;
    // } else {
    //   systemRole =SystemRole[this.content] as any  || 0;

    // }
    switch (systemRole) {
      case SystemRole.L0:
        this.scoreType = SystemScoreType.L0;
        this.loadDataL0();
        break;
      case SystemRole.L1:
        this.scoreType = SystemScoreType.L1;
        //this.loadDataL1();
        break;
      case SystemRole.FunctionalLeader:
        this.scoreType = SystemScoreType.FunctionalLeader;
        //this.loadDataFunctionalLeader();
        break;
      case SystemRole.L2:
        this.scoreType = SystemScoreType.L2;
        //this.loadDataL2();
        break;
      case SystemRole.Updater:
        this.scoreType = SystemScoreType.FHO;
        //this.loadDataUpdater();
        break;
      case SystemRole.GHR:
        this.scoreType = SystemScoreType.GHR;
        //this.loadDataGHR();
        break;
      case SystemRole.GM:
        //this.loadDataGM();
        this.scoreType = SystemScoreType.GM;
        break;
    }
  }

  selected(args) {
    this.currentTimeRequest = this.datePipe.transform(this.currentTime, "yyyy-MM-dd HH:mm");
    const index = args.selectedIndex + 1;
    this.index = index;
    let systemRole = 0;
    this.content =args.selectedItem.outerText;
    if (this.content === '') {
      this.content = this.accountGroupData[0]?.name;
      systemRole =SystemRole[this.accountGroupData[0]?.name] as any  || 0;
    } else {
      systemRole = SystemRole[args.selectedItem.outerText] as any ;

    }
    switch (systemRole) {
      case SystemRole.L0:
        this.scoreType = SystemScoreType.L0;
        this.loadDataL0();
        break;
      case SystemRole.L1:
        this.scoreType = SystemScoreType.L0;
        //this.loadDataL1();
        break;
      case SystemRole.GFL:
        this.scoreType = SystemScoreType.L0;
        //this.loadDataFunctionalLeader();
        break;
      case SystemRole.L2:
        this.scoreType = SystemScoreType.L0;
        //this.loadDataL2();
        break;
      case SystemRole.Updater:
        this.scoreType = SystemScoreType.L0;
        //this.loadDataUpdater();
        break;
      case SystemRole.GHR:
        this.scoreType = SystemScoreType.L0;
        //this.loadDataGHR();
        break;
      case SystemRole.GM:
       // this.loadDataGM();
        this.scoreType = SystemScoreType.L0;
        break;
    }
  }

  getGridTemplate(name): TemplateRef<any> {
    const dir = this.Gridtemplates.find(dir => dir.name === name + '');
  return dir ? dir.template : null
  }

  loadDataL0() {
    this.gridData = [];
    this.todolist2Service.l0(this.currentTimeRequest, this.userId).subscribe(data => {
      this.gridData = data;
      this.spinner.hide()
    });
  }

  loadDataL1() {
    this.gridData = [];
    this.todolistService.l1(this.currentTimeRequest).subscribe(data => {
      this.gridData = data;
    });
    this.gridData = [];

  }

  loadDataUpdater() {
    this.gridData = [];
    this.todolistService.updater(this.currentTimeRequest).subscribe(data => {
      this.gridData = data;
    });
  }
  loadDataFunctionalLeader() {
    this.gridData = [];
    this.todolistService.functionalLeader(this.currentTimeRequest).subscribe(data => {
      this.gridData = data;
    });
  }
  loadDataL2() {
    this.gridData = [];
    this.todolistService.l2(this.currentTimeRequest).subscribe(data => {
      this.gridData = data;
    });
  }

  loadDataFHO() {
    this.gridData = [];
    this.todolistService.fho(this.currentTimeRequest).subscribe(data => {
      this.gridData = data;
    });
  }

  loadDataGHR() {
    this.gridData = [];
    this.todolistService.ghr(this.currentTimeRequest).subscribe(data => {
      this.gridData = data;
    });
  }

  loadDataGM() {
    this.gridData = [];
    this.todolistService.gm(this.currentTimeRequest).subscribe(data => {
      this.gridData = data;
    });
  }

  loadAccountGroupData() {
    this.accountGroupService.getAccountGroupForTodolistByAccountId().subscribe(data => {
      this.accountGroupData = data;
      this.loadData();
    });
  }

  gotoUpdate() {
    return this.router.navigateByUrl('/transaction/upload-kpi-objective');
  }

  openActionModalComponent(data) {
    if(data.typeText !== 'string') {
      const modalRef = this.modalService.open(PlanComponent, { size: 'xl', backdrop: 'static', keyboard: false });
      modalRef.componentInstance.data = data;
      modalRef.componentInstance.currentTime = this.currentTime;

      modalRef.result.then((result) => {
      }, (reason) => {
      });
    } else {
      const modalRef = this.modalService.open(PlanStringTypeComponent, { size: 'xl', backdrop: 'static', keyboard: false });
      modalRef.componentInstance.data = data;
      modalRef.componentInstance.currentTime = this.currentTime;

      modalRef.result.then((result) => {
      }, (reason) => {
      });
    }
  }

  openUpdatePdcaModalComponent(data) {
    if(data.typeText !== 'string') {
      const modalRef = this.modalService.open(PdcaComponent, { size: 'xxl', backdrop: 'static', keyboard: false });
      modalRef.componentInstance.data = data;
      modalRef.componentInstance.currentTime = this.currentTime;
      modalRef.result.then((result) => {
      }, (reason) => {
      });
    } else {
      const modalRef = this.modalService.open(PdcaStringTypeComponent, { size: 'xxl', backdrop: 'static', keyboard: false });
      modalRef.componentInstance.data = data;
      modalRef.componentInstance.currentTime = this.currentTime;
      modalRef.result.then((result) => {
      }, (reason) => {
      });
    }

  }

  openSelfScoreModalComponent(data) {
    // const modalRef = this.modalService.open(SelfScoreComponent, { size: 'xl', backdrop: 'static' });
    // modalRef.componentInstance.data = data;
    // modalRef.componentInstance.periodTypeCode = PeriodType.Monthly;
    // modalRef.componentInstance.scoreType = this.scoreType;

    // modalRef.result.then((result) => {
    // }, (reason) => {
    // });
  }
  openKPIScoreModalComponent(data) {
    // const modalRef = this.modalService.open(KpiScoreComponent, { size: 'xl', backdrop: 'static' });
    // modalRef.componentInstance.data = data;
    // modalRef.componentInstance.periodTypeCode = PeriodType.Quarterly;
    // modalRef.componentInstance.scoreType = this.scoreType;
    // modalRef.componentInstance.currentTime = this.currentTime;
    // modalRef.result.then((result) => {
    // }, (reason) => {
    // });
  }
  openKPIScoreL2ModalComponent(data) {
    // const modalRef = this.modalService.open(KpiScoreL2Component, { size: 'xl', backdrop: 'static' });
    // modalRef.componentInstance.data = data;
    // modalRef.componentInstance.periodTypeCode = PeriodType.Quarterly;
    // modalRef.componentInstance.scoreType = this.scoreType;
    // modalRef.componentInstance.currentTime = this.currentTime;
    // modalRef.result.then((result) => {
    // }, (reason) => {
    // });
  }
  openKPIScoreGHRModalComponent(data) {
    // const modalRef = this.modalService.open(KpiScoreGHRComponent, { size: 'xl', backdrop: 'static' });
    // modalRef.componentInstance.data = data;
    // modalRef.componentInstance.periodTypeCode = PeriodType.Quarterly;
    // modalRef.componentInstance.scoreType = this.scoreType;
    // modalRef.componentInstance.currentTime = this.currentTime;

    // modalRef.result.then((result) => {
    // }, (reason) => {
    // });
  }
  openKPIScoreGMModalComponent(data) {
    // const modalRef = this.modalService.open(KpiScoreGMComponent, { size: 'xl', backdrop: 'static' });
    // modalRef.componentInstance.data = data;
    // modalRef.componentInstance.periodTypeCode = PeriodType.Quarterly;
    // modalRef.componentInstance.scoreType = this.scoreType;
    // modalRef.componentInstance.currentTime = this.currentTime;

    // modalRef.result.then((result) => {
    // }, (reason) => {
    // });
  }
  openAttitudeScoreModalComponent(data) {
    // const modalRef = this.modalService.open(AttitudeScoreComponent, { size: 'xl', backdrop: 'static' });
    // modalRef.componentInstance.data = data;
    // modalRef.componentInstance.scoreType = this.scoreType;
    // modalRef.result.then((result) => {
    // }, (reason) => {
    // });
  }
  openAttitudeScoreL2ModalComponent(data) {
    // const modalRef = this.modalService.open(AttitudeScoreL2Component, { size: 'xl', backdrop: 'static' });
    // modalRef.componentInstance.data = data;
    // modalRef.componentInstance.scoreType = this.scoreType;
    // modalRef.result.then((result) => {
    // }, (reason) => {
    // });
  }

  openAttitudeScoreGHRModalComponent(data) {
    // const modalRef = this.modalService.open(AttitudeScoreGHRComponent, { size: 'xl', backdrop: 'static' });
    // modalRef.componentInstance.data = data;
    // modalRef.componentInstance.scoreType = this.scoreType;
    // modalRef.result.then((result) => {
    // }, (reason) => {
    // });
  }
  openAttitudeScoreFunctionalLeaderModalComponent(data) {
    // const modalRef = this.modalService.open(AttitudeScoreFunctionalLeaderComponent, { size: 'xl', backdrop: 'static' });
    // modalRef.componentInstance.data = data;
    // modalRef.componentInstance.scoreType = this.scoreType;
    // modalRef.result.then((result) => {
    // }, (reason) => {
    // });
  }
  openImportExcelModalComponent() {
    this.excelDownloadUrl = `${environment.apiUrl}todolist/ExcelExport`;
    this.modalReference = this.modalService.open(this.importModal, { size: 'xl' });
  }
  fileProgress(event) {
    this.file = event.target.files[0];
  }
  uploadFile() {
    const uploadBy = JSON.parse(localStorage.getItem('user')).id;
    this.todolistService.import(this.file, uploadBy)
      .subscribe((res: any) => {
        this.loadDataFHO();
        this.alertify.success('The excel has been imported into system!');
        this.modalService.dismissAll();
      }, error => {
        this.alertify.error(error, true);
      });
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
