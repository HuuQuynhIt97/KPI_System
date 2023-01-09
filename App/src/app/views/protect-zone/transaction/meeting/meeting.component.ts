import { element } from 'protractor';
import { filter } from 'rxjs/operators';
import { OcPolicyService } from './../../../../_core/_service/OcPolicy.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import {GridComponent} from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Account } from 'src/app/_core/_model/account';
import { AccountGroup } from 'src/app/_core/_model/account.group';
import { OcService } from 'src/app/_core/_service/oc.service';
import { MeetingService } from 'src/app/_core/_service/meeting.service';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import * as Chart from 'chart.js';
import { DatePipe } from '@angular/common';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/_core/_service/data.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { FilterRequest } from 'src/app/_core/_model/filterRequest';
import { TranslateService } from '@ngx-translate/core';
// import { saveAs } from 'file-saver';
@Component({
  selector: 'app-meeting',
  templateUrl: './meeting.component.html',
  styleUrls: ['./meeting.component.scss'],
  providers: [DatePipe]
})
export class MeetingComponent extends BaseComponent implements OnInit , AfterViewInit {

  data: Account[] = [];
  password = '';
  modalReference: NgbModalRef;
  fields: object = { text: 'name', value: 'id' };
  leaderFields: object = { text: 'fullName', value: 'id' };
  managerFields: object = { text: 'fullName', value: 'id' };
  // toolbarOptions = ['Search'];
  passwordFake = `aRlG8BBHDYjrood3UqjzRl3FubHFI99nEPCahGtZl9jvkexwlJ`;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  setFocus: any;
  editSettingsMeeting = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: false, allowDeleting: false, mode: 'Normal' };
  locale = localStorage.getItem('lang');
  accountGroupData: AccountGroup[];
  accountGroupItem: any;
  leaders: any[] = [];
  managers: any[] = [];
  leaderId: number;
  managerId: number;
  accounts: any[];
  typeId: any;
  oclv3Data: any [];
  modalRef: NgbModalRef;
  policyData: Object;
  @ViewChild('detailModal') detailModal: NgbModalRef;
  @ViewChild('fileModal') fileModal: NgbModalRef;
  unit: string = null;
  period: string = null
  plugins: any = [pluginDataLabels]

  options: any = {
    responsive: true,
    maintainAspectRatio: false,
    showScale: false,
    plugins: {
      datalabels: {
        backgroundColor: function(context) {
          return context.dataset.backgroundColor;
        },
        borderRadius: 4,
        color: 'white',
        font: {
          weight: 'bold'
        },
        formatter: function(value, context) {
          return value
        }
      }
    },
    title: {
      display: true,
      text: '',
      fontSize: 14,
      fontColor: 'black'
    },
    elements: {
      point: {
        radius: 0
      },
      line: {
        tension: 0.2
      }
    },
    scales: {
      yAxes: [
        {
          stacked: true,
          display: true,
          position: 'left',
          ticks: {
            beginAtZero: true,
            padding: 10,
            fontSize: 12,
            stepSize: 10,
            min: -2
          },
          scaleLabel: {
            display: true,
            labelString: this.unit,
            fontSize: 12,
            fontStyle: 'normal'
          }
        }
      ],
      xAxes: [
        {
          gridLines: {
            display: true,
            tickMarkLength: 8
          },
          ticks: {
            fontSize: 12
          },
          scaleLabel: {
            display: true,
            labelString: this.period,
            fontSize: 12,
            fontStyle: 'normal'
          }
        }
      ]
    }
  }
  pointBackgroundColors: any[] = []
  chart: any = {}
  datasets: any;
  targets: any;
  perfomance: any;
  labels: any;
  unitId: any;
  unitName: any;
  dataTable: any;
  @ViewChild('gridDataTable') public grid2: GridComponent;
  dataLevel: any
  dataFact: any
  dataCenter: any
  dataDept: any
  dataPic: any
  levelFields: object = { text: 'name', value: 'id' };
  factoryFields: object = { text: 'name', value: 'name' };
  picFields: object = { text: 'name', value: 'id' };
  policyDataTamp: any;
  levelId: number = 0
  factId: number = 0
  centerId: number = 0
  deptId: number = 0
  picId: number = 0
  currentTime: any;
  policyTitle: string
  kpiTitle: string
  levelTitle: string
  picTitle: string
  @ViewChild('content', { static: true }) elementView: ElementRef;
  contentHeight: number;
  files = [];
  kpiId: any;
  base = environment.apiUrl.replace('/api/', '');
  base_download = environment.apiUrl;
  YTD: any;
  targetYTD: any;
  dataOc: any;
  typeText: string;
  factName: string;
  centerName: string;
  deptName: string;
  changeLocalHome = [];
  dataHeight: any = [];
  ytds: any;
  roleUser: any;
  toolbar = ["Search"];
  userId: number;
  filterRequest: FilterRequest = {
    factory: '',
    center: '',
    dept: '',
    level: 0
  };
  deptId_default: number = 0;
  centerId_default: number = 0;
  factId_default: number = 0;
  constructor(
    public modalService: NgbModal,
    private ocService: OcService,
    private datePipe: DatePipe,
    private meetingService: MeetingService,
    private alertify: AlertifyService,
    private translate: TranslateService,
    public todolist2Service: Todolist2Service,
    private spinner: NgxSpinnerService,
    private dataService: DataService
  ) { super();

  }

  ngOnInit() {
    this.getAllOcLv3();
    this.getAllOc();
    this.getAllLevel()
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.roleUser = JSON.parse(localStorage.getItem('level')).code;
    this.currentTime = new Date();
    setTimeout(() => {
      this.getAllKpi();
    }, 300);

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
      const url_check = `${this.base_download}UploadFile/CheckFile`
      this.meetingService.checkFile(url_check, file).subscribe(res => {
        if (res) {
          this.meetingService.download(url,file).subscribe(data =>{
            const blob = new Blob([data]);
            const downloadURL = window.URL.createObjectURL(data);
            const link = document.createElement('a');
            link.href = downloadURL;
            link.download = `${item.name}`;
            link.click();
          })
        } else {
          this.alertify.error(this.translate.instant('FILE_NOT_EXIST'))
        }
      })

    }
  }
  ngOnDestroy() {
    this.changeLocalHome.forEach(item => item.unsubscribe());
  }
  getAllOc(){
    this.ocService.getAll().subscribe((res: any) => {
      this.dataOc = res
      this.dataFact = res.filter(x => x.level === 3)
      this.dataFact.unshift({ name: "All", id: 0 });
    })
  }
  ngAfterViewInit() {

  }
  createdSearch(args) {
    var gridElement = this.grid.element;
    var span = document.createElement("span");
    span.className = "e-clear-icon";
    span.id = gridElement.id + "clear";
    span.onclick = this.cancelBtnClick.bind(this);
    gridElement.querySelector(".e-toolbar-item .e-input-group").appendChild(span);
  }
  public cancelBtnClick(args) {
    this.grid.searchSettings.key = "";
    (this.grid.element.querySelector(".e-input-group.e-search .e-input") as any).value = "";
  }

  filterFact(args) {
    this.factId = args.value
    this.factName = args.itemData.name
    this.dataCenter = this.dataOc.filter(x => x.parentId === this.factId)
    this.dataCenter.unshift({ name: "All", id: 0 });
    if(args.itemData.name === 'All')
      this.filterRequest.factory = ''
    else this.filterRequest.factory = args.itemData.name
    this.filterQuery(this.filterRequest)
  }

  filterCenter(args) {
    this.centerId = args.value
    this.centerName = args.itemData.name
    this.dataDept = this.dataOc.filter(x => x.parentId === this.centerId)
    this.dataDept.unshift({ name: "All", id: 0 });
    if(args.itemData.name === 'All')
      this.filterRequest.center = ''
    else this.filterRequest.center = args.itemData.name
    this.filterQuery(this.filterRequest)
  }

  filterDept(args) {
    this.deptId = args.value
    this.deptName = args.itemData.name
    if(args.itemData.name === 'All')
      this.filterRequest.dept = ''
    else this.filterRequest.dept = args.itemData.name
    this.filterQuery(this.filterRequest)
  }

  filterlevel(args) {
    this.levelId = args.value
    if(args.itemData.name === 'All')
      this.filterRequest.level = 0
    else this.filterRequest.level = args.value
    this.filterQuery(this.filterRequest)
  }

  filterQuery(request: FilterRequest) {

    if (request.factory === '' && request.center === '' && request.dept === '' && request.level === 0)
    {
      this.policyData = this.policyDataTamp
    }

    if (request.factory !== '' && request.center === '' && request.dept === '' && request.level > 0)
    {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.level === request.level)
      return;
    }

    if (request.factory !== '' && request.center === '' && request.dept === '' && request.level === 0)
    {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName))
    }

    if (request.factory !== '' && request.center !== '' && request.dept === '' && request.level === 0 )
    {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName))
    }
    if (request.factory !== '' && request.center !== '' && request.dept === '' && request.level > 0)
    {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName)  && x.centerName.includes(this.centerName) && x.level === request.level)
      return;
    }

    if (request.factory !== '' && request.center !== '' && request.dept !== '' && request.level === 0)
    {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.deptName.includes(this.deptName))
    }

    if (request.factory !== '' && request.center !== '' && request.dept !== '' && request.level > 0)
    {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.deptName.includes(this.deptName) && x.level === request.level)
      return;
    }

    if (request.level > 0)
    {
      this.policyData = this.policyDataTamp.filter(x => x.level === request.level)
    }

  }

  scroll(el: HTMLElement) {
    el.scrollIntoView();
  }

  openModal(data, model) {
    this.typeText = data.typeText
    this.kpiTitle = data.name
    this.levelTitle = data.level
    this.picTitle = data.picName
    this.unitId = data.typeId
    this.unitName = data.typeName
    this.kpiId = data.id
    this.loadDataModel(this.kpiId,this.typeText)
    this.modalRef = this.modalService.open(model, { size: 'lg', backdrop: 'static' });
    this.modalRef.result.then((result) => {
      this.perfomance = []
      this.targets = []
      this.labels = []
      this.dataTable = []
      this.changeLocalHome.forEach(item => item.unsubscribe());
      this.dataHeight = []
    }, (reason) => {
      this.perfomance = []
      this.targets = []
      this.labels = []
      this.dataTable = []
      this.changeLocalHome.forEach(item => item.unsubscribe());
      this.dataHeight = []

    });

  }

  createChart(chartId, labels, unit) {
    const ctx = document.getElementById(chartId) as HTMLCanvasElement;
    let optionss: any = {
      plugins: {
        datalabels: {
          backgroundColor: function(context) {
            return context.dataset.backgroundColor;
          },
          borderRadius: 4,
          color: 'white',
          font: {
            weight: 'bold'
          },
          formatter: function(value, context) {
            // if(unit === 'Percentage'){
            //   return value + '%';
            // }else {
            // }
            return value
          }
        }
      },
      scales: {
        yAxes: [
          {
            display: true,
            stacked: false,
            position: 'left',
            ticks: {
              beginAtZero: true,
              padding: 10,
              fontSize: 12,
              stepSize: 10,
              min: -2
            },
            scaleLabel: {
              display: true,
              fontSize: 12,
              fontStyle: 'normal'
            }
          }
        ],
        xAxes: [
          {
            gridLines: {
              display: true,
              tickMarkLength: 8
            },
            ticks: {
              fontSize: 12
            },
            scaleLabel: {
              display: true,
              fontSize: 12,
              fontStyle: 'normal'
            }
          }
        ]
      },
      title: {
        display: true,
        text: this.options.title.text,
        fontSize: 14,
        fontColor: 'black'
      },
      elements: {
        point: {
          radius: 0
        },
        line: {
          tension: 0.2
        }
      },
    }
    const myChart = new Chart(ctx, {
      type: 'line',
      data:{
        // tslint:disable-next-line: object-literal-shorthand
        labels: labels,
        datasets: [
          {
            // tslint:disable-next-line: object-literal-shorthand
            label: "This month perfomance",
            backgroundColor: '#008cff',
            borderColor: '#008cff',
            fill: false,
            data: this.perfomance,
          },
          {
            label: 'This month targets',
            data: this.targets,
            backgroundColor: '#ff8c00',
            borderColor: '#ff8c00',
            fill: false,
          }
        ]
      },
      options: optionss
    })
    this.chart = myChart
  }

  download(date, model) {
    this.modalRef = this.modalService.open(model, { size: 'sm', backdrop: 'static' });
    this.todolist2Service
    .getDownloadFilesMeeting(this.kpiId,date)
    .subscribe((res: any) => {
      const files = res as any || [];
      this.files = files.map(x=> {
        return {
          name: x.name,
          path: this.base + x.path
        }
      });
    })

  }


  loadDataModel(id, typeText) {
    this.spinner.show()
    this.meetingService
    .getChartWithTime(id,this.datePipe.transform(this.currentTime, "yyyy-MM-dd HH:mm"))
    .subscribe((res: any) => {
      if (res.status) {
        this.ytds = res.ytds
        this.policyTitle = res.policy
        this.typeId = res.typeId,
        this.YTD = res.ytd
        this.targetYTD = res.targetYTD
        this.perfomance = res.perfomances
        this.targets = res.targets
        this.labels = res.labels

        this.dataTable = res.dataTable?.filter(x => x.currentMonthData.length > 0)
        const dataTable = res.dataTable?.filter(x => x.currentMonthData.length > 0)

        if(typeText !== 'string') {
          this.createChart(
            'planet-chart',
            this.labels,
            this.unitName
          )
        }
        this.changeLocalHome.push(this.dataService.currentMessagesTarget.subscribe((res: any)=>{
          if(res === 0)
            return
          if(res.value > 0 || res.value !== undefined)
            this.dataHeight = []
            this.dataHeight.push(
              {
                value: res.value,
                actionId: res.actionId,
                month: res.month
              }
            )
            for (let item of dataTable) {
              for (let items in item.nextMonthData) {
                let i = Number(items);
                if(item.nextMonthData[i].month === res.month) {
                  item.nextMonthData[i].heightT = res.value / 2
                  break;
                }
              }
            }
        }))

        this.changeLocalHome.push(this.dataService.currentMessagesTD.subscribe((res: any)=>{
          if(res === 0)
            return
          if(res.value > 0 || res.value !== undefined)

            for (let item of dataTable) {
                for (let items in item.nextMonthData) {
                  let i = Number(items);
                  if(item.nextMonthData[i].month === res.month) {
                    item.nextMonthData[i].heightA = res.value / 2
                    break;
                  }
                }
            }
        }))

      } else {
        this.alertify.message("The data display interval has not been set, please reset it", true)
      }
      this.spinner.hide()

    })
  }

  getAllKpiWithFilterQuery() {
    this.spinner.show();
    this.meetingService.getAllKpiWithFilterQuery(this.filterRequest).subscribe((res: any) => {
      this.policyData = res
      this.policyDataTamp = res
      this.spinner.hide()
    })
  }

  getAllKpi() {
    this.spinner.show();
    this.meetingService.getAllKpi(this.userId).subscribe((res: any) => {
      this.policyDataTamp = res.result
      this.factId = res.factId
      this.centerId = res.centerId
      this.deptId = res.deptId

      this.factId_default = res.factId
      this.centerId_default = res.centerId
      this.deptId_default = res.deptId

      if(res.factId > 0 && res.centerId === 0 || res.factId === 0 && res.centerId === 0){
        this.policyData = res.result
        this.dataCenter = this.dataOc.filter(x => x.parentId === res.factId)
        this.dataCenter.unshift({ name: "All", id: 0 });
        this.filterRequest.factory = res.factName
        this.factName = res.factName
      }

      if(res.factId > 0 && res.centerId > 0 ) {
        this.policyData = res.result.filter(x => x.factName.includes(res.factName)  && x.centerName.includes(res.centerName))
        this.dataCenter = this.dataOc.filter(x => x.parentId === res.factId)
        this.dataCenter.unshift({ name: "All", id: 0 });

        this.dataDept = this.dataOc.filter(x => x.parentId === res.centerId)
        this.dataDept.unshift({ name: "All", id: 0 });
        this.filterRequest.factory = res.factName
        this.factName = res.factName
        this.filterRequest.center = res.centerName
        this.centerName = res.centerName

      }

      this.spinner.hide()
    })
  }

  getAllLevel() {
    this.meetingService.getAllLevel().subscribe((res: any) => {
      this.dataLevel = res;
      this.dataLevel.unshift({ name: "All", id: 0 });
    })
  }

  getAllOcLv3() {
    this.ocService.getAllLv3().subscribe((res: any) => {
      this.oclv3Data = res
    })
  }

  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
