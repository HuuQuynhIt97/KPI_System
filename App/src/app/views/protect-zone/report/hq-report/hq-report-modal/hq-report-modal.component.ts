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
import { PeopleCommitteeService } from 'src/app/_core/_service/people-committee.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-hq-report-modal',
  templateUrl: './hq-report-modal.component.html',
  styleUrls: ['./hq-report-modal.component.scss'],
  providers: [ToolbarService, EditService, PageService]
})
export class HqReportModalComponent implements OnInit {
  filterSettings = { type: 'Excel' };
  toolbarOptions = ['Search','ExcelExport'];
  @Input() data: any;
  dataHQReport: any = [];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  // toolbarOptions = [];
  locale = localStorage.getItem('lang');
  name: string;
  frozen: boolean;
  titleH1: string = "H1_HQ_REPORT_LABEL";

  constructor(
    private service: PeopleCommitteeService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private router: Router,
    private translate: TranslateService,
    private spinner: NgxSpinnerService,
    public activeModal: NgbActiveModal,
    public modalService: NgbModal,
  ) { }

  ngOnInit() {
    this.loadTitleH1();
    this.spinner.show();
    this.loadFrozenBtn();
    this.loadData();
  }

  loadData() {
    this.service.getAllHQReport(this.locale, this.data.id).subscribe(res => {
      this.dataHQReport = res;
      this.spinner.hide()
    });
  }

  loadTitleH1() {
    this.service.getTitleH1HQReport(this.data.id).subscribe(res=> {
      this.titleH1 = res
    });
  }

  toolbarClick = (args: ClickEventArgs) => {
    switch (args.item.id) {
      case 'grid_excelexport':
        const excelExportProperties: ExcelExportProperties = {
          fileName: 'HQ Report.xlsx'
        };
        this.grid.excelExport(excelExportProperties);
        // this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }

  loadFrozenBtn() {
    this.service.getFrozen(this.data.id).subscribe(res => {
      this.frozen = res;
    });
  }

  lockUpdate() {
    this.service.lockUpdate(this.data.id).subscribe((res: any) => {
      if (res.success === true) {
        this.loadFrozenBtn();
        this.alertify.success(this.translate.instant(res.message))
      } else {
        this.loadFrozenBtn();
        this.alertify.error(this.translate.instant(res.message))
      }
    });
  }

  clickDetail(dataAc) {
    window.open(`#${this.router.url}/people-committee-detail/${dataAc.appraiseeID}/${this.data.id}`,'_blank')
  }

  search(args) {
    this.grid.search(this.name)
  }

  rowDrop(e){
    setTimeout(() => {
      const model = {
        AppraiseeID: e.data[0].appraiseeID,
        FromIndex: e.fromIndex + 1,
        ToIndex: e.dropIndex + 1,
        CampaignID: this.data.id
      }
      this.service.updateCommitteeSequence(model).subscribe(res => {
        this.loadData();
      })
    }, 300);

  }

  dataBound() {
    this.grid.autoFitColumns();
  }

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
