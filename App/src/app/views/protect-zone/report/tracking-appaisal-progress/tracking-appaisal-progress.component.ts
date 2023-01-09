import { TrackingAppraisalProgressDetailComponent } from './tracking-appraisal-progress-detail/tracking-appraisal-progress-detail.component';
import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgxSpinnerService } from 'ngx-spinner';
import { StartCampaignService } from 'src/app/_core/_service/start-campaign.service';
import { TrackingProcessService } from 'src/app/_core/_service/tracking-process.service';

@Component({
  selector: 'app-tracking-appaisal-progress',
  templateUrl: './tracking-appaisal-progress.component.html',
  styleUrls: ['./tracking-appaisal-progress.component.scss'],
  providers: [DatePipe]
})
export class TrackingAppaisalProgressComponent implements OnInit {
  toolbarOptions = ['Search'];
  data: any
  filterSettings = { type: 'Excel' };
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 15 };
  userId: number;
  campaignData: any;
  locale = localStorage.getItem('lang');
  @ViewChild('grid') public grid: GridComponent;

  constructor(
    private spinner: NgxSpinnerService,
    public modalService: NgbModal,
    private datePipe: DatePipe,
    private campaignService: StartCampaignService,
  ) { }

  ngOnInit() {
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.getAll()
  }
  openDetail(data) {
    const modalRef = this.modalService.open(TrackingAppraisalProgressDetailComponent, { size: 'xxl', backdrop: 'static', keyboard: false });
      modalRef.componentInstance.data = data;
      modalRef.result.then((result) => {
      }, (reason) => {
    });
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
  getAll(){
    this.spinner.show();
    this.campaignService.getAll().subscribe(res => {
      this.campaignData = res
      this.spinner.hide();
    })
  }

}
