import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Account } from 'src/app/_core/_model/account';
import { MeetingService } from 'src/app/_core/_service/meeting.service';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import * as Chart from 'chart.js';
import { DatePipe } from '@angular/common';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/_core/_service/data.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-CHM',
  templateUrl: './CHM.component.html',
  styleUrls: ['./CHM.component.scss'],
  providers: [DatePipe]
})
export class CHMComponent extends BaseComponent implements OnInit {

  data: Account[] = [];
  modalReference: NgbModalRef;
  fields: object = { text: 'name', value: 'id' };
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  setFocus: any;
  editSettingsMeeting = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: false, allowDeleting: false, mode: 'Normal' };
  locale = localStorage.getItem('lang');
  typeId: any;
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
  YTD: any;
  targetYTD: any;
  typeText: string;
  changeLocalHome = [];
  dataHeight: any = [];
  ytds: any;
  roleUser: any;
  toolbar = ["Search"];
  userId: number;
  constructor(
    public modalService: NgbModal,
    private datePipe: DatePipe,
    private meetingService: MeetingService,
    private alertify: AlertifyService,
    public todolist2Service: Todolist2Service,
    private spinner: NgxSpinnerService,
    private dataService: DataService
  ) { super();

  }

  ngOnInit() {
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.getAllKpi();
    this.roleUser = JSON.parse(localStorage.getItem('level')).code;
    this.currentTime = new Date();

  }
  ngOnDestroy() {
    this.changeLocalHome.forEach(item => item.unsubscribe());
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


  getAllKpi() {
    this.spinner.show();
    this.meetingService.getAllKpiCHM(this.userId).subscribe((res: any) => {
      this.policyData = res.result
      this.policyDataTamp = res.result
      this.spinner.hide()
    })
  }

  getAllLevel() {
    this.meetingService.getAllLevel().subscribe((res: any) => {
      this.dataLevel = res;
      this.dataLevel.unshift({ name: "All", id: 0 });
    })
  }

  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
