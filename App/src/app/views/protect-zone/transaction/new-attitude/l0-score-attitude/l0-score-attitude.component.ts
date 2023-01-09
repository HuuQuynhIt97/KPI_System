import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { PermissionService } from 'src/app/_core/_service/permission.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { TranslateService } from '@ngx-translate/core';
import { NewAttitudeService } from 'src/app/_core/_service/new-attitude.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { NewAttitudeDetailModalComponent } from '../new-attitude-detail-modal/new-attitude-detail-modal.component';

@Component({
  selector: 'app-l0-score-attitude',
  templateUrl: './l0-score-attitude.component.html',
  styleUrls: ['./l0-score-attitude.component.scss']
})
export class L0ScoreAttitudeComponent implements OnInit {
  submitCheck = false;

  campaignID = 0;
  scoreTo = 0;
  scoreFrom = 0;
  type = 'L0';
  dataList: any = [];
  dataAttEvaluation: any = {
    id: 0,
    campaignID: this.campaignID,
    scoreTo: this.scoreTo,
    scoreFrom: this.scoreFrom,
    type: this.type,
    firstQuestion1: false,
    firstQuestion2: false,
    firstQuestion3: false,
    firstQuestion4: false,
    firstQuestion5: false,
    firstQuestion6: false,

    secondQuestion1: false,
    secondQuestion2: false,
    secondQuestion3: false,
    secondQuestion4: false,
    secondQuestion5: false,
    secondQuestion6: false,

    thirdQuestion: '',
    fourthQuestion: '',
  };
  attAttchment: any;
  commentEdit: string = "";
  commentID: any = 0;
  @ViewChild('editComment') editComment: NgbModalRef;
  @ViewChild('message') message: NgbModalRef;
  constructor(
    private service: NewAttitudeService,
    private alertify: AlertifyService,
    public modalService: NgbModal,
    private translate: TranslateService,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private router: Router,
    ) { }

  ngOnInit() {
    this.spinner.show()
    this.campaignID = this.route.snapshot.params.campaignID
    this.scoreTo = this.route.snapshot.params.scoreTo
    this.scoreFrom = Number(JSON.parse(localStorage.getItem('user')).id);
    this.dataAttEvaluation = {
      id: 0,
      campaignID: this.campaignID,
      scoreTo: this.scoreTo,
      scoreFrom: this.scoreFrom,
      type: this.type,
      firstQuestion1: false,
      firstQuestion2: false,
      firstQuestion3: false,
      firstQuestion4: false,
      firstQuestion5: false,
      firstQuestion6: false,

      secondQuestion1: false,
      secondQuestion2: false,
      secondQuestion3: false,
      secondQuestion4: false,
      secondQuestion5: false,
      secondQuestion6: false,

      thirdQuestion: '',
      fourthQuestion: '',
    };
    this.getAll()
    this.getAllAttEvaluation()
    this.getNewAttitudeSubmit()
    this.attAttchment = {
    };
  }

  getAll() {
    this.service.getAll(this.campaignID, this.scoreTo, this.scoreFrom).subscribe(res => {
      this.dataList = res
      this.spinner.hide()
    })
  }
  getAllAttEvaluation() {
    this.service.getAttEvaluation(this.campaignID, this.scoreTo, this.scoreFrom).subscribe(res => {
      if (res !== null) {
        this.dataAttEvaluation = res
      }
    })
    // return new Promise((res, rej) => {
    //   this.service.getAttEvaluation(this.campaignID, this.scoreTo, this.scoreFrom).subscribe(
    //     result => {

    //       if (result !== null) {
    //             this.dataAttEvaluation = res
    //           }

    //       res(result);
    //     },
    //     (error) => {
    //       rej(error);
    //     }
    //   );
    // });
  }

  getNewAttitudeSubmit() {
    this.service.getNewAttitudeSubmit(this.campaignID, this.scoreTo).subscribe((res :any) => {
      if (res !== null) {
        this.submitCheck = res.isSubmitAttitudeL0
      }
    })
  }

  onCheck(id: any, point: string) {
    this.service.updatePoint(id, point).subscribe(res => {
      this.getAll()
    })
  }

  openEditComment(data: any) {
    // this.modalService.open(this.message, { centered: true });
    this.commentID = data.newAttitudeAttchmentID;
    this.commentEdit = data.comment;
    const campaignID = data.newAttitudeScore[0].campaignID;
    this.attAttchment.id = data.newAttitudeAttchmentID;
    this.attAttchment.comment = data.comment;
    this.attAttchment.orderNumber = data.orderNumber;
    this.attAttchment.campaignID = data.newAttitudeScore[0].campaignID;
    this.attAttchment.scoreTo = data.newAttitudeScore[0].scoreTo;
    this.attAttchment.scoreFrom = data.newAttitudeScore[0].scoreFrom;

    this.modalService.open(this.editComment, { size: 'md', backdrop: 'static', keyboard: false, centered: true });
  }

  updateComment() {
    this.attAttchment.comment = this.commentEdit.trim();
    this.service.updateComment(this.attAttchment).subscribe((res: any) => {
      this.attAttchment =  {};
      this.commentEdit = '';
      if (res.success) {
        this.getAll();
        this.modalService.dismissAll();
      } else {
        this.modalService.dismissAll();
      }
    })
  }

  save(clickSave: boolean) {
    // return new Promise((res, rej) => {
    //   this.service.getAttEvaluation(this.campaignID, this.scoreTo, this.scoreFrom).subscribe(
    //     (result: any) => {

    //       if (result !== null) {
    //             this.dataAttEvaluation.id = result.id
    //           }

    //           return new Promise((res, rej) => {
    //             this.service.updateAttEvaluation(this.dataAttEvaluation).subscribe(
    //               (result: any) => {

    //                 if (result.success) {
    //                       this.getAllAttEvaluation();
    //                       if (clickSave) {
    //                         this.alertify.success('Successfully')
    //                       }
    //                     }
    //                     else {
    //                       // this.alertify.error(this.translate.instant(res.message))
    //                       this.alertify.error('Error')
    //                     }

    //                 res(result);
    //               },
    //               (error) => {
    //                 rej(error);
    //               }
    //             );
    //           });

    //       res(result);
    //     },
    //     (error) => {
    //       rej(error);
    //     }
    //   );
    // });

    this.spinner.show()
    this.service.updateAttEvaluation(this.dataAttEvaluation).subscribe((res: any) => {
      if (res.success) {
        this.getAllAttEvaluation();
        if (clickSave) {
          this.alertify.success('Successfully')
        }
      }
      else {
        // this.alertify.error(this.translate.instant(res.message))
        this.alertify.error('Error')
      }
      this.spinner.hide()
    })


    // return new Promise((res, rej) => {
    //   this.service.updateAttEvaluation(this.dataAttEvaluation).subscribe(
    //     (result: any) => {

    //       if (result.success) {
    //             this.getAllAttEvaluation();
    //             if (clickSave) {
    //               this.alertify.success('Successfully')
    //             }
    //           }
    //           else {
    //             // this.alertify.error(this.translate.instant(res.message))
    //             this.alertify.error('Error')
    //           }

    //       res(result);
    //     },
    //     (error) => {
    //       rej(error);
    //     }
    //   );
    // });
  }

  submit() {
    this.modalService.open(this.message, { centered: true });
  }

  yesConfirm() {
    // this.save(false)
    this.spinner.show()
    this.service.updateAttEvaluation(this.dataAttEvaluation).subscribe((res: any) => {
      if (res.success) {
        this.getAllAttEvaluation();
        this.service.checkSubmitNewAtt(this.campaignID, this.scoreTo, this.scoreFrom, this.type).subscribe((res : any) => {
          if (res.success) {
            this.alertify.success(this.translate.instant(res.message));
            this.getNewAttitudeSubmit()
          } else {
            this.alertify.error(this.translate.instant(res.message));
          }
          this.modalService.dismissAll();
        })
      }
      else {
        // this.alertify.error(this.translate.instant(res.message))
        this.alertify.error('Error')
      }
    })
    this.spinner.hide()

  }

  openDetail() {
    // const modalRef = this.modalService.open(NewAttitudeDetailModalComponent, { size: 'xxl', backdrop: true, keyboard: true });
    //   modalRef.componentInstance.campaignID = this.campaignID;
    //   modalRef.componentInstance.scoreTo = this.scoreTo;
    //   modalRef.result.then((result) => {
    //   }, (reason) => {

    // });

    window.open(`#/transaction/new-score-attitude-detail/${this.campaignID}/${this.scoreTo}`,'_blank')
    // window.open(`#${this.router.url}/new-score-attitude-detail`,'_blank')
  }

}
