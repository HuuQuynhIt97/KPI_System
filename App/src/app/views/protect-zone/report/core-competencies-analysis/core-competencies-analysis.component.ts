import { Component, OnInit, ViewChild } from '@angular/core';
import { EditService, ToolbarService, PageService, GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { StartCampaignService } from 'src/app/_core/_service/start-campaign.service';
import { CoreCompetenciesAnalysisModalComponent } from './core-competencies-analysis-modal/core-competencies-analysis-modal.component';

@Component({
  selector: 'app-core-competencies-analysis',
  templateUrl: './core-competencies-analysis.component.html',
  styleUrls: ['./core-competencies-analysis.component.scss'],
  providers: [ToolbarService, EditService, PageService]
})
export class CoreCompetenciesAnalysisComponent implements OnInit {

  closeResult = '';
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
    private campaignService: StartCampaignService,
  ) { }

  ngOnInit() {
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.getAll()
  }

  openDetail(data) {
    const modalRef = this.modalService.open(CoreCompetenciesAnalysisModalComponent, { size: 'xxl', backdrop: 'static', keyboard: false });
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
