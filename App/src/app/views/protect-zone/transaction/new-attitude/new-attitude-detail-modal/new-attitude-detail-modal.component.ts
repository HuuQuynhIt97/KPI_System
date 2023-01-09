import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NewAttitudeService } from 'src/app/_core/_service/new-attitude.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute } from '@angular/router';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { SignalrService } from 'src/app/_core/_service/signalr.service';


@Component({
  selector: 'app-new-attitude-detail-modal',
  templateUrl: './new-attitude-detail-modal.component.html',
  styleUrls: ['./new-attitude-detail-modal.component.scss']
})
export class NewAttitudeDetailModalComponent extends BaseComponent implements OnInit {
  // @Input() campaignID: 0;
  // @Input() scoreTo: 0;
  campaignID: 0;
  scoreTo: 0;
  userId: number;

  dataDetail: any = [];
  dataAttEvaluation: any;
  // dataAttEvaluation: any = {
  //   commentFL: "",
  //   commentL2
  //   firstQuestion1L0
  //   firstQuestion1L1
  //   firstQuestion2L0
  //   firstQuestion2L1
  //   firstQuestion3L0
  //   firstQuestion3L1
  //   firstQuestion4L0
  //   firstQuestion4L1
  //   firstQuestion5L0
  //   firstQuestion5L1
  //   firstQuestion6L0
  //   firstQuestion6L1
  //   fourthQuestionL0

  //   secondQuestion1L0
  //   secondQuestion1L1
  //   secondQuestion2L0
  //   secondQuestion2L1
  //   secondQuestion3L0
  //   secondQuestion3L1
  //   secondQuestion4L0
  //   secondQuestion4L1
  //   secondQuestion5L0
  //   secondQuestion5L1
  //   secondQuestion6L0
  //   secondQuestion6L1
  //   thirdQuestionL0
  //   thirdQuestionL1
  // };

  constructor(
    // public activeModal: NgbActiveModal,
    private service: NewAttitudeService,
    public modalService: NgbModal,
    private translate: TranslateService,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    public signalRService: SignalrService,
  ) { super();
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
    this.campaignID = this.route.snapshot.params.campaignID
    this.scoreTo = this.route.snapshot.params.scoreTo
    this.dataAttEvaluation = {
    }
    this.spinner.show()
    // this.getDetailNewAttitude()
    // this.getDetailNewAttitudeEvaluation()

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
    await this.getDetailNewAttitude();
    await this.getDetailNewAttitudeEvaluation();
    // this.spinner.hide()
  }

  getDetailNewAttitude() {
    this.service.getDetailNewAttitude(this.campaignID, this.scoreTo).subscribe(res => {
      this.dataDetail = res
    })
  }

  getDetailNewAttitudeEvaluation() {
    this.service.getDetailNewAttitudeEvaluation(this.campaignID, this.scoreTo).subscribe((res: any) => {
      if (res !== null) {
        this.dataAttEvaluation = res
      }
      this.spinner.hide()
    })
  }

}
