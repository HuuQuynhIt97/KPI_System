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
import { KpiMonthPerfService } from 'src/app/_core/_service/kpi-month-perf.service';
import { CalendarView } from '@syncfusion/ej2-angular-calendars';

@Component({
  selector: 'app-kpi-month-perf',
  templateUrl: './kpi-month-perf.component.html',
  styleUrls: ['./kpi-month-perf.component.scss'],
  providers: [DatePipe]
})
export class KpiMonthPerfComponent extends BaseComponent implements OnInit , AfterViewInit {
  policyData: Object;
  toolbar = ['ExcelExport',"Search"];
  @ViewChild('grid') public grid: GridComponent;
  locale = localStorage.getItem('lang');
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 15 };
  userId: number;
  roleUser: any;
  currentTime: Date;

  public start: CalendarView = 'Year';
  public depth: CalendarView = 'Year';
  public format: string = 'MMMM y'
  systemDate: any = new Date(); 
  constructor(
    public modalService: NgbModal,
    private service: KpiMonthPerfService,
    private datePipe: DatePipe,
    public todolist2Service: Todolist2Service,
    private spinner: NgxSpinnerService,
  ) { super();

  }
  ngAfterViewInit(): void {
    // throw new Error('Method not implemented.');
  }

  ngOnInit() {
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.roleUser = JSON.parse(localStorage.getItem('level')).code;
    this.currentTime = new Date();
    setTimeout(() => {
      this.getAllKpi();
    }, 300);
  }

  onChangeSystemDate(args) {
    this.systemDate = args.value as Date
    this.getAllKpi()
    // this.workPlanDate_system = this.datePipe.transform(args.value,'yyyy-MM')
    // if (this.systemDate !== null)
    //   this.getAllByDate();
  }
  getAllKpi() {

    this.spinner.show();
    this.service.getAll(this.userId,this.datePipe.transform(this.systemDate, 'yyyy-MM-dd')).subscribe((res: any) => {
      this.policyData = res.result
      this.spinner.hide()
    })
  }
  toolbarClick(args) {
    switch (args.item.id) {
      case 'grid_excelexport':
        this.spinner.show()
        const exportProperties = {
          // dataSource: data,
          fileName: 'KPI-Month-Perf.xlsx'
        };
        this.grid.excelExport(exportProperties);
        this.spinner.hide()
        // this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
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

}
