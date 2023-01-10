import { filter } from 'rxjs/operators';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EditService, ToolbarService, PageService, ExcelExportProperties, GridComponent } from '@syncfusion/ej2-angular-grids';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import { ModalDismissReasons, NgbModal, NgbModalRef, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { Account } from 'src/app/_core/_model/account';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { Authv2Service } from 'src/app/_core/_service/authv2.service';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { CoreCompetenciesService } from 'src/app/_core/_service/core-competencies.service';
import { SelectEventArgs } from '@syncfusion/ej2-navigations';

@Component({
  selector: 'app-core-competencies-analysis-modal',
  templateUrl: './core-competencies-analysis-modal.component.html',
  styleUrls: ['./core-competencies-analysis-modal.component.scss'],
  providers: [ToolbarService, EditService, PageService]
})
export class CoreCompetenciesAnalysisModalComponent implements OnInit {

  filterSettings = { type: 'Excel' };
  toolbarOptions = ['Search', 'ExcelExport'];
  @Input() data: any;
  dataCore: any = [];
  dataCoreSum: any = [];
  dataCoreScore2: any = [];
  dataCoreScoreThan2: any = [];
  dataCoreScoreThan3: any = [];
  dataCoreScoreAverage: any = [];
  dataCoreScorePercentile: any = [];
  dataCoreBehaviorChecked: any = [];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('gridShowAll') public gridShowAll: GridComponent;
  @ViewChild('gridShowAllSum') public gridShowAllSum: GridComponent;
  @ViewChild('gridShowScore2') public gridShowScore2: GridComponent;
  @ViewChild('gridShowScoreThan2') public gridShowScoreThan2: GridComponent;
  @ViewChild('gridShowScoreThan3') public gridShowScoreThan3: GridComponent;
  @ViewChild('gridShowScoreAverage') public gridShowScoreAverage: GridComponent;
  @ViewChild('gridShowScorePercentile') public gridShowScorePercentile: GridComponent;
  @ViewChild('gridShowBehaviorChecked') public gridShowBehaviorChecked: GridComponent;

  // toolbarOptions = [];
  locale = localStorage.getItem('lang');
  name: string;
  frozen: boolean;
  public animation: object = {
    previous: { effect: "", duration: 0, easing: "" },
    next: { effect: "", duration: 0, easing: "" }
  };
  public select (e: SelectEventArgs) {
    if (e.isSwiped) {
      e.cancel = true;
    }
  }

  constructor(
    private service: CoreCompetenciesService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private router: Router,
    private translate: TranslateService,
    private spinner: NgxSpinnerService,
    public activeModal: NgbActiveModal,
    public modalService: NgbModal,
  ) { }

  ngOnInit() {
    this.spinner.show();
    this.loadData();
    this.loadDataSum();
    this.loadDataScore2();
    this.loadDataScoreThan2();
    // this.loadDataScoreThan3();
    this.loadDataScoreAverage();
    this.loadDataScorePercentile();
    // this.loadDataBehaviorChecked();
  }

  // loadData() {
  //   this.service.getAllCoreCompetencies(this.locale, this.data.id).subscribe(res => {
  //     this.dataCore = res;
  //     this.spinner.hide()
  //   });
  // }

  loadData() {
    this.service.getAllNewCoreCompetencies(this.locale, this.data.id).subscribe(res => {
      this.dataCore = res;
      this.spinner.hide()
    });
  }

  loadDataSum() {
    this.service.getNewCoreCompetencies(this.locale, this.data.id).subscribe(res => {
      this.dataCoreSum = res;
    });
  }

  // loadDataScore2() {
  //   this.service.getAllCoreCompetenciesScoreEquals2(this.locale, this.data.id).subscribe(res => {
  //     this.dataCoreScore2 = res;
  //   });
  // }

  loadDataScore2() {
    this.service.getNewCoreCompetenciesScoreEquals2(this.locale, this.data.id).subscribe(res => {
      this.dataCoreScore2 = res;
    });
  }

  // loadDataScoreThan2() {
  //   this.service.getAllCoreCompetenciesScoreThan2(this.locale, this.data.id).subscribe(res => {
  //     this.dataCoreScoreThan2 = res;
  //   });
  // }

  // loadDataScoreThan3() {
  //   this.service.getAllCoreCompetenciesScoreThan3(this.locale, this.data.id).subscribe(res => {
  //     this.dataCoreScoreThan3 = res;
  //   });
  // }

  loadDataScoreThan2() {
    this.service.getNewCoreCompetenciesScoreThan2(this.locale, this.data.id).subscribe((res: any) => {
      this.dataCoreScoreThan2 = res.resultThan2;
      this.dataCoreScoreThan3 = res.resultThan3;
    });
  }

  // loadDataScoreAverage() {
  //   this.service.getAllCoreCompetenciesAverage(this.locale, this.data.id).subscribe(res => {
  //     this.dataCoreScoreAverage = res;
  //   });
  // }

  // loadDataScorePercentile() {
  //   this.service.getAllCoreCompetenciesPercentile(this.locale, this.data.id).subscribe(res => {
  //     this.dataCoreScorePercentile = res;
  //   });
  // }

  loadDataScoreAverage() {
    this.service.getNewCoreCompetenciesAverage(this.locale, this.data.id).subscribe(res => {
      this.dataCoreScoreAverage = res;
    });
  }

  loadDataScorePercentile() {
    this.service.getNewCoreCompetenciesPercentile(this.locale, this.data.id).subscribe(res => {
      this.dataCoreScorePercentile = res;
    });
  }

  loadDataBehaviorChecked() {
    this.service.getAllCoreCompetenciesAttitudeBehavior(this.locale, this.data.id).subscribe(res => {
      this.dataCoreBehaviorChecked = res;
    });
  }

  toolbarClickExcelExport(args: ClickEventArgs): void {
    if (args.item.id == 'gridShowAll_excelexport' ||
        args.item.id == 'gridShowAllSum_excelexport' ||
        args.item.id == 'gridShowScore2_excelexport' ||
        args.item.id == 'gridShowScoreThan2_excelexport' ||
        args.item.id == 'gridShowScoreThan3_excelexport' ||
        args.item.id == 'gridShowScoreAverage_excelexport' ||
        args.item.id == 'gridShowScorePercentile_excelexport' ||
        args.item.id == 'gridShowBehaviorChecked_excelexport') {
      this.exportExcelCoreComp();
    }
  }

  exportExcelCoreComp() {
    const lang = localStorage.getItem('lang');
    this.spinner.show();
    this.service.exportExcelNewCoreCompetencies(this.locale, this.data.id).subscribe((data: any) => {
      const blob = new Blob([data],
        { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      const downloadURL = window.URL.createObjectURL(data);
      const link = document.createElement('a');
      link.href = downloadURL;
      link.download = `核心職能資料分析.xlsx`;
      link.click();
      this.spinner.hide();
    });
  }

  toolbarClick = (args: ClickEventArgs) => {
    switch (args.item.id) {
      case 'gridShowAll_excelexport':
        const dataCoreNew = this.dataCore.map(item =>{
          return {
            index: item.index,
            attHeading: this.translate.instant(item.attHeading),
            center: item.center,
            comment: item.comment,
            dept: item.dept,
            factory: item.factory,
            fl: item.fl,
            l0: item.l0,
            l1: item.l1,
            l2: item.l2,
            score: item.score,
            scoreBy: item.scoreBy,
          }
        })
        const excelExportProperties: ExcelExportProperties = {
          dataSource: dataCoreNew,
          fileName: 'Core Competencies Analysis.xlsx'
        };
        this.gridShowAll.excelExport(excelExportProperties);

        // this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }

  toolbarClickScore2 = (args: ClickEventArgs) => {
    switch (args.item.id) {
      case 'gridShowScore2_excelexport':
        const dataCoreScore2New = this.dataCoreScore2.map(item =>{
          return {
            index: item.index,
            attHeading: this.translate.instant(item.attHeading),
            center: item.center,
            comment: item.comment,
            dept: item.dept,
            factory: item.factory,
            fl: item.fl,
            l0: item.l0,
            l1: item.l1,
            l2: item.l2,
            score: item.score,
            scoreBy: item.scoreBy,
          }
        })
        const excelExportProperties: ExcelExportProperties = {
          dataSource: dataCoreScore2New,
          fileName: '核心職能評2分項目.xlsx'
        };
        this.gridShowScore2.excelExport(excelExportProperties);

        // this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }

  toolbarClickScoreThan2 = (args: ClickEventArgs) => {
    switch (args.item.id) {
      case 'gridShowScoreThan2_excelexport':
        const dataCoreScoreThan2New = this.dataCoreScoreThan2.map(item =>{
          return {
            index: item.index,
            attHeading: this.translate.instant(item.attHeading),
            center: item.center,
            comment: item.comment,
            dept: item.dept,
            factory: item.factory,
            fl: item.fl,
            l0: item.l0,
            l1: item.l1,
            l2: item.l2,
            score: item.score,
            selfScore: item.selfScore,
            scoreBy: item.scoreBy,
          }
        })
        const excelExportProperties: ExcelExportProperties = {
          dataSource: dataCoreScoreThan2New,
          fileName: 'Gap 差距2分.xlsx'
        };
        this.gridShowScoreThan2.excelExport(excelExportProperties);

        // this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }

  toolbarClickScoreThan3 = (args: ClickEventArgs) => {
    switch (args.item.id) {
      case 'gridShowScoreThan3_excelexport':
        const dataCoreScoreThan3New = this.dataCoreScoreThan3.map(item =>{
          return {
            index: item.index,
            attHeading: this.translate.instant(item.attHeading),
            center: item.center,
            comment: item.comment,
            dept: item.dept,
            factory: item.factory,
            fl: item.fl,
            l0: item.l0,
            l1: item.l1,
            l2: item.l2,
            score: item.score,
            selfScore: item.selfScore,
            scoreBy: item.scoreBy,
          }
        })
        const excelExportProperties: ExcelExportProperties = {
          dataSource: dataCoreScoreThan3New,
          fileName: 'Gap 差距3分.xlsx'
        };
        this.gridShowScoreThan3.excelExport(excelExportProperties);

        // this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }

  toolbarClickScoreAverage = (args: ClickEventArgs) => {
    switch (args.item.id) {
      case 'gridShowScoreAverage_excelexport':
        // const dataCoreScoreThan3New = this.dataCoreScoreThan3.map(item =>{
        //   return {
        //     index: item.index,
        //     attHeading: this.translate.instant(item.attHeading),
        //     center: item.center,
        //     comment: item.comment,
        //     dept: item.dept,
        //     factory: item.factory,
        //     fl: item.fl,
        //     l0: item.l0,
        //     l1: item.l1,
        //     l2: item.l2,
        //     score: item.score,
        //     selfScore: item.selfScore,
        //     scoreBy: item.scoreBy,
        //   }
        // })
        const excelExportProperties: ExcelExportProperties = {
          // dataSource: dataCoreScoreThan3New,
          fileName: '廠區核心職能分佈狀態.xlsx'
        };
        this.gridShowScoreAverage.excelExport(excelExportProperties);

        // this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }

  toolbarClickScorePercentile = (args: ClickEventArgs) => {
    switch (args.item.id) {
      case 'gridShowScorePercentile_excelexport':

        const excelExportProperties: ExcelExportProperties = {

          fileName: '廠區核心職能分佈狀態.xlsx'
        };
        this.gridShowScorePercentile.excelExport(excelExportProperties);

        // this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }

  toolbarClickBehaviorChecked = (args: ClickEventArgs) => {
    switch (args.item.id) {
      case 'gridShowBehaviorChecked_excelexport':

        const excelExportProperties: ExcelExportProperties = {

          fileName: '行為細項勾選狀態.xlsx'
        };
        this.gridShowBehaviorChecked.excelExport(excelExportProperties);

        // this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }


  dataBound() {
    this.gridShowAll.autoFitColumns();
  }

  search(args) {
    this.gridShowAll.search(this.name)
  }

  NO(index) {
    return (this.gridShowAll.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

  dataBoundShowAllSum() {
    this.gridShowAllSum.autoFitColumns();
  }

  dataBoundShowScore2() {
    this.gridShowScore2.autoFitColumns();
  }

  dataBoundShowScoreThan2() {
    this.gridShowScoreThan2.autoFitColumns();
  }

  dataBoundShowScoreThan3() {
    this.gridShowScoreThan3.autoFitColumns();
  }

  dataBoundShowScoreAverage() {
    this.gridShowScoreAverage.autoFitColumns();
  }

  dataBoundShowScorePercentile() {
    this.gridShowScorePercentile.autoFitColumns();
  }

  dataBoundShowBehaviorChecked() {
    this.gridShowBehaviorChecked.autoFitColumns();
  }

  NOShow2(index) {
    return (this.gridShowScore2.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
