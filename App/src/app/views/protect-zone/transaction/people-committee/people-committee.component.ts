import { filter } from 'rxjs/operators';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EditService, ToolbarService, PageService, GridComponent } from '@syncfusion/ej2-angular-grids';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { Authv2Service } from 'src/app/_core/_service/authv2.service';
import { TranslateService } from '@ngx-translate/core';
import { PeopleCommitteeService } from 'src/app/_core/_service/people-committee.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { StartCampaignService } from 'src/app/_core/_service/start-campaign.service';
import { TrackingProcessService } from 'src/app/_core/_service/tracking-process.service';
import { PeopleCommitteeModalComponent } from './people-committee-modal/people-committee-modal.component';

@Component({
  selector: 'app-people-committee',
  templateUrl: './people-committee.component.html',
  styleUrls: ['./people-committee.component.scss'],
  providers: [ToolbarService, EditService, PageService]
})
export class PeopleCommitteeComponent implements OnInit{

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
    const modalRef = this.modalService.open(PeopleCommitteeModalComponent, { size: 'xxl', backdrop: 'static', keyboard: false });
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
