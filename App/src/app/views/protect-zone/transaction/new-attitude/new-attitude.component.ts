import { ScoreAttitudeComponent } from './../score-attitude/score-attitude.component';
import { EvaluationService } from './../../../../_core/_service/evaluation.service';
import { StartCampaignService } from './../../../../_core/_service/start-campaign.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { EditService, ToolbarService, PageService, GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef,NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { SignalrService } from 'src/app/_core/_service/signalr.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { NewAttitudeService } from 'src/app/_core/_service/new-attitude.service';

@Component({
  selector: 'app-new-attitude',
  templateUrl: './new-attitude.component.html',
  styleUrls: ['./new-attitude.component.scss'],
  providers: [ToolbarService, EditService, PageService,DatePipe]
})
export class NewAttitudeComponent extends BaseComponent implements OnInit {

  modalReference: NgbModalRef;
  // toolbarOptions = ['Search'];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 15 };
  @ViewChild('grid') public grid: GridComponent;
  setFocus: any;
  locale = localStorage.getItem('lang');
  campaignData: Object;
  displayTime: any
  monthTime: any
  yearSelect: number = new Date().getFullYear();
  typeFields: object = { text: 'name', value: 'id' };
  typeFieldsMonth: object = { text: 'name' };
  monthData: any =  [
    {
      start: 1,
      name: 'Jan-June',
      end: 6
    },
    {
      start: 7,
      name: 'July-December',
      end: 12
    }
  ];
  startMonth: number = 0
  endMonth: number = 0
  Monthname: string = ''
  userId: number;
  scoreFrom: number;
  evaluationSelfData: any;
  evaluationFirstLevelData: any;
  evaluationSecondLevelData: any;
  flFeedbackData: any;
  gmData: any;
  constructor(
    public modalService: NgbModal,
    private campaignService: StartCampaignService,
    private evaluationService: EvaluationService,
    public signalRService: SignalrService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private serviceNewAtt: NewAttitudeService,
  ) {
    super();
    this.signalRService.currentMessage.subscribe((res: any) => {
      if(res !== 0) {
        if(this.userId === res.loadUserFrom || this.userId === res.loadUserTo) {
          this.startHttpRequest();
        }else {
          console.log('Signalr nothing refresh');
        }
      }else {
        console.log('Signalr nothing refresh');
      }

    })
  }

  ngOnInit() {
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.scoreFrom = Number(JSON.parse(localStorage.getItem('user')).id);
    this.signalRService.startConnection()
    this.signalRService.loadData()
    this.startHttpRequest();
  }
  ngOnDestroy(): void {
    this.signalRService.close();

  }
  private startHttpRequest = () => {
    this.getAsyncData();
  }
  async getAsyncData() {
    this.spinner.show()
    await this.getSelfAppraisal();
    await this.getFirstLevelAppraisal();
    await this.getSecondLevelAppraisal();
    await this.getFLFeedback();
    this.spinner.hide()
  }

  SelfScore(data) {
    if(data.isGM) {
      window.open(`#${this.router.url}/self-score/GM/${data.campaignID}/${data.userID}`,'_blank')
    }else {
      window.open(`#${this.router.url}/l0-score-attitude/${data.type}/${data.campaignID}/${data.userID}`,'_blank')
    }
  }

  AttitudeNewClick(data) {
    this.serviceNewAtt.generateNewAttitudeScore(data.campaignID, data.userID, this.scoreFrom).subscribe((res: any) => {
      if (res.success) {
        window.open(`#${this.router.url}/new-score-attitude/${data.type}/${data.campaignID}/${data.userID}`,'_blank')
      }
    })
  }

  openScoreAttitudeModalComponent(data) {
    let ngbModalOptions: NgbModalOptions = {
      backdrop : 'static',
      keyboard : false,
      size: 'xxl',
    };
    const modalRef = this.modalService.open(ScoreAttitudeComponent, ngbModalOptions );
      modalRef.componentInstance.data = data;
      modalRef.result.then((result) => {
      }, (reason) => {
        modalRef.componentInstance.closeResult = reason;
        //this.scoreAttitudeModal.getDismissReason(reason);
      });
  }

  generateEvaluation(campaignID) {
    this.campaignService.generateEvaluation(campaignID).subscribe(res => {
    })
  }

  getSelfAppraisal(){
    return new Promise((res, rej) => {
      this.serviceNewAtt.getSelfAppraisal(this.userId).subscribe(
        (result: any) => {
          this.evaluationSelfData = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getFirstLevelAppraisal(){
    return new Promise((res, rej) => {
      this.serviceNewAtt.getFirstLevelAppraisal(this.userId).subscribe(
        (result: any) => {
          this.evaluationFirstLevelData = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getSecondLevelAppraisal(){
    return new Promise((res, rej) => {
      this.serviceNewAtt.getSecondLevelAppraisal(this.userId).subscribe(
        (result: any) => {
          this.evaluationSecondLevelData = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getFLFeedback(){
    return new Promise((res, rej) => {
      this.serviceNewAtt.getFLFeedback(this.userId).subscribe(
        (result: any) => {
          this.flFeedbackData = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getGMData(){
    this.evaluationService.getGM(this.userId).subscribe(res => {
      this.gmData = res
    })
  }

  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }

  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }


}
