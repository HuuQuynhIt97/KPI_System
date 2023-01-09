import { async } from '@angular/core/testing';
import { isNumeric } from 'rxjs/util/isNumeric';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PeopleCommitteeService } from 'src/app/_core/_service/people-committee.service';
import { EditService, ToolbarService, PageService, GridComponent } from '@syncfusion/ej2-angular-grids';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { EnvService } from 'src/app/_core/_service/env.service';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { SpecialScoreService } from 'src/app/_core/_service/special-score.service';
import { DropDownListComponent, MultiSelectComponent } from '@syncfusion/ej2-angular-dropdowns';
import { toInteger } from '@ng-bootstrap/ng-bootstrap/util/util';
import { keyframes } from '@angular/animations';
import { QueryCellInfoEventArgs } from '@syncfusion/ej2-angular-grids';
import { EmitType } from '@syncfusion/ej2-base';
import { isNumber } from '@syncfusion/ej2-angular-spreadsheet';

@Component({
  selector: 'app-people-committee-detail',
  templateUrl: './people-committee-detail.component.html',
  styleUrls: ['./people-committee-detail.component.scss']
})
export class PeopleCommitteeDetailComponent implements OnInit {

  textareaHr: string = null
  textareaKpiComment: string = null
  edittingKpiComment: boolean = false
  lang = localStorage.getItem('lang');
  userLogin = JSON.parse(localStorage.getItem('user')).id;

  frozen: boolean;
  contentTooltipKpi: any = '';
  contentTooltipAttitude: any = '';

  kpiScore: any;
  attitudeScore: any;
  specialScore: any;
  kpiScoreL2: any;
  committeeScore: any;
  base_download: any
  base: any
  userID: any
  userID_System: any
  campaignID: any
  center: any
  dept: any
  appraisee: any

  isDataSpecial: boolean = false

  flID: number = 0
  l1ID: number = 0
  l2ID: number = 0

  locale = localStorage.getItem('lang');
  dataPersonal: [] = [];
  dataMulti: [] = [];
  dataKpiScore: [] = [];
  hrComment: string;
  dataAttitudeScore: [] = [];
  dataSumAttitudeScore: [] = [];
  dataAttEvaluation: any;
  dataKpiScoreL2: [] = [];
  kpiSpecialScoreL2: number = 0;
  kpiSpecialScoreL2Tamp: number = 0;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };

  public type: string = 'L1';
  typel1: any = []
  compactl1: any = []
  ratiol1: string = null
  scorel1: string = null
  scorel2: string = null
  filel1: any = []
  typeDataL1: any
  scoreDataL1: any
  scoreDataL2: any
  compactDataL1: any
  ratioDataL1: any
  subjectl1: string = null
  contentl1: string = null
  typeFields: object = { text: 'name', value: 'id' };
  compactFields: object = { text: 'name', value: 'id' };
  ratioFields: object = { text: 'point', value: 'point' };
  scoreFields: object = { text: 'point', value: 'point' };

  kpiScoreCalL0: number = 0
  kpiScoreCalL1: number = 0
  kpiScoreCalL2: number = 0

  kpiScoreByL0: string = "N/A"
  kpiScoreByL1: string = "N/A"
  kpiScoreByL2: string = "N/A"

  atitudeScoreCalFL: number = 0
  atitudeScoreCalL0: number = 0
  atitudeScoreCalL1: number = 0
  atitudeScoreCalL2: number = 0

  atitudeScoreByFL: string = "N/A"
  atitudeScoreByL0: string = "N/A"
  atitudeScoreByL1: string = "N/A"
  atitudeScoreByL2: string = "N/A"

  kpiScoreCal: number = 0
  attitudeScoreCal: number = 0
  specialScoreCal: any
  h1ScoreCal: number = 0

  @ViewChild('grid') public grid: GridComponent;
  @ViewChild('gridKPIScore') public gridKPIScore: GridComponent;
  @ViewChild('gridAttitudeScore') public gridAttitudeScore: GridComponent;
  @ViewChild('gridKpiScoreL2') public gridKpiScoreL2: GridComponent;

  // @ViewChild('ddlelementType')
  // public dropDownListType: DropDownListComponent;
  @ViewChild('ddlelementType')
  public dropDownListType: MultiSelectComponent;

  // @ViewChild('ddlelementImpact')
  // public dropDownListImpact: DropDownListComponent;

  @ViewChild('ddlelementImpact')
  public dropDownListImpact: MultiSelectComponent;

  @ViewChild('ddlelementRatio')
  public dropDownListRatio: DropDownListComponent;

  @ViewChild('ddlelementScore')
  public dropDownListScore: DropDownListComponent;



  toolbarOptions = [""];
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  specialID: number = 0;
  editSettingsKpiScore = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Batch' };
  constructor(
    private service: PeopleCommitteeService,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private env: EnvService,
    private alertify: AlertifyService,
    private router: Router,
    private specialScoreService: SpecialScoreService,
  ) {
    this.base = this.env.apiUrl.replace('/api/', '')
    this.base_download = this.env.apiUrl
   }

  ngOnInit() {

    this.kpiScore = {
    };
    this.attitudeScore = {
    };
    this.specialScore = {
    };
    this.kpiScoreL2 = {
    };
    this.committeeScore = {
    };
    this.dataAttEvaluation = {
    }
    this.userID = this.route.snapshot.params.appraiseeID
    this.campaignID = this.route.snapshot.params.campaignID
    this.userID_System = Number(JSON.parse(localStorage.getItem('user')).id);
    this.getAsyncData();
  }

  public queryCellInfoEvent: EmitType<QueryCellInfoEventArgs> = (args: QueryCellInfoEventArgs) => {
    let data = args.data;
    if (args.column.field === 'hrComment') {
       args.rowSpan = this.dataKpiScore.length;
    }
  };

  async getAsyncData() {
    this.spinner.show();
    await this.loadFrozenBtn();
    this.editSettings = { showDeleteConfirmDialog: false, allowEditing: !this.frozen, allowAdding: true, allowDeleting: true, mode: 'Normal' };
    this.editSettingsKpiScore = { showDeleteConfirmDialog: false, allowEditing: !this.frozen, allowAdding: true, allowDeleting: true, mode: 'Batch' };
    await this.loadDataKpiNew();
    await this.loadDataHeader();
    await this.loadDataKpiScore();
    await this.loadDataAttitudeScore();
    // await this.loadDataSumAttitudeScore();
    await this.loadDataSumNewAttitudeScore();
    await this.getDetailNewAttitudeEvaluation();
    await this.getSpecialScoreL2();
    await this.loadDataKpiScoreL2();
    await this.getSpecialScore();
    await this.getSpecialRatio();
    await this.getSpecialCompact();
    await this.getSpecialType();
    await this.getCommitteeScore();
    await this.loadDataSpecialScoreDetail();

    this.loadkpiSpecialScoreL2Tamp();
    this.calculatorH1();

    this.spinner.hide();

  }
  detailKPI() {
    window.open(`#/transaction/people-committee/people-committee-detail/kpi-detail/${this.userID}/${this.campaignID}`,'_blank')
  }
  loadFrozenBtn() {
    return new Promise((res, rej) => {
      this.service.getFrozen(this.campaignID).subscribe(
        (result: any) => {
          this.frozen = result;

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }
  loadDataHeader() {
    // this.service.getPeopleCommittee(this.userID).subscribe((res: any) => {
    //   this.center = res.center
    //   this.dept = res.dept
    //   this.appraisee = res.appraisee
    // });

    return new Promise((res, rej) => {
      this.service.getPeopleCommittee(this.userID).subscribe(
        (result: any) => {
          this.center = result.center
          this.dept = result.dept
          this.appraisee = result.appraisee

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadDataKpiNew() {
    // this.service.getKpi(this.userID).subscribe((res: any) => {
    //   this.dataPersonal = res.personal
    //   this.dataMulti = res.multi
    // });

    return new Promise((res, rej) => {
      this.service.getKpi(this.userID).subscribe(
        (result: any) => {
          this.dataPersonal = result.personal
          this.dataMulti = result.multi

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadDataKpiScore() {
    return new Promise((res, rej) => {
      this.service.getKpiScore(this.userID, this.campaignID).subscribe(
        (result: any) => {
          this.dataKpiScore = result
          this.hrComment = result.hrComment
          if (result[0] != undefined) {
            this.kpiScoreCalL0 = result[0].point
            this.kpiScoreByL0 = result[0].scoreFromName
            this.textareaHr = result[0].hrComment
          }
          if (result[1] != undefined) {
            this.kpiScoreCalL1 = result[1].point
            this.kpiScoreByL1 = result[1].scoreFromName
          }
          if (result[2] != undefined) {
            this.kpiScoreCalL2 = result[2].point
            this.kpiScoreByL2 = result[2].scoreFromName
          }


          if (!isNumber(this.kpiScoreCalL0)) {
            this.kpiScoreCalL0 = 0
          }
          if (!isNumber(this.kpiScoreCalL1)) {
            this.kpiScoreCalL1 = 0
          }
          if (!isNumber(this.kpiScoreCalL2)) {
            this.kpiScoreCalL2 = 0
          }

          // if (!isNumeric(this.kpiScoreCalL0)) {
          //   this.kpiScoreCalL0 = 0
          // }
          // if (!isNumeric(this.kpiScoreCalL1)) {
          //   this.kpiScoreCalL1 = 0
          // }
          // if (!isNumeric(this.kpiScoreCalL2)) {
          //   this.kpiScoreCalL2 = 0
          // }

          if ((Number(this.kpiScoreCalL0) != 0 && Number(this.kpiScoreCalL1) == 0 && Number(this.kpiScoreCalL2) == 0) ||
              (Number(this.kpiScoreCalL0) == 0 && Number(this.kpiScoreCalL1) != 0 && Number(this.kpiScoreCalL2) == 0) ||
              (Number(this.kpiScoreCalL0) == 0 && Number(this.kpiScoreCalL1) == 0 && Number(this.kpiScoreCalL2) != 0)) {

              this.kpiScoreCal = (Number(this.kpiScoreCalL0) + Number(this.kpiScoreCalL1) + Number(this.kpiScoreCalL2))/5*70
          }
          else if ((Number(this.kpiScoreCalL0) != 0 && Number(this.kpiScoreCalL1) != 0 && Number(this.kpiScoreCalL2) == 0) ||
              (Number(this.kpiScoreCalL0) != 0 && Number(this.kpiScoreCalL1) == 0 && Number(this.kpiScoreCalL2) != 0) ||
              (Number(this.kpiScoreCalL0) == 0 && Number(this.kpiScoreCalL1) != 0 && Number(this.kpiScoreCalL2) != 0)) {

              this.kpiScoreCal = ((Number(this.kpiScoreCalL0) + Number(this.kpiScoreCalL1) + Number(this.kpiScoreCalL2))/2)/5*70
          }
          else {
            this.kpiScoreCal = ((Number(this.kpiScoreCalL0) + Number(this.kpiScoreCalL1) + Number(this.kpiScoreCalL2))/3)/5*70
          }

          this.kpiScoreCal = Math.round((this.kpiScoreCal + Number.EPSILON) * 10) / 10

          this.calculatorH1();

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });

  }

  loadDataAttitudeScore() {
    return new Promise((res, rej) => {
      this.service.getAttitudeScore(this.userID, this.campaignID).subscribe(
        (result: any) => {
          this.dataAttitudeScore = result

          // this.dataAttitudeScore = result.filter(x => x.comment === this.userID);

          // if (result[0] != undefined) {
          //   this.atitudeScoreCalFL = result[0].point
          // }
          // if (result[1] != undefined) {
          //   this.atitudeScoreCalL0 = result[1].point
          // }
          // if (result[2] != undefined) {
          //   this.atitudeScoreCalL1 = result[2].point
          // }
          // if (result[3] != undefined) {
          //   this.atitudeScoreCalL2 = result[3].point
          // }

          // if (!isNumeric(this.atitudeScoreCalFL)) {
          //   this.atitudeScoreCalFL = 0
          // }
          // if (!isNumeric(this.atitudeScoreCalL0)) {
          //   this.atitudeScoreCalL0 = 0
          // }
          // if (!isNumeric(this.atitudeScoreCalL1)) {
          //   this.atitudeScoreCalL1 = 0
          // }
          // if (!isNumeric(this.atitudeScoreCalL2)) {
          //   this.atitudeScoreCalL2 = 0
          // }

          // this.attitudeScoreCal = ((Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL0) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2))/4)/30*30
          // this.attitudeScoreCal = Math.round((this.attitudeScoreCal + Number.EPSILON) * 10) / 10

          // this.calculatorH1();

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadDataSumAttitudeScore() {
    return new Promise((res, rej) => {
      this.service.getSumAttitudeScore(this.userID, this.campaignID).subscribe(
        (result: any) => {
          this.dataSumAttitudeScore = result

          if (result[0] != undefined) {
            this.atitudeScoreCalFL = result[0].sumPoint
            this.atitudeScoreByFL = result[0].scoreFromName
            this.flID = result[0].id
          }
          if (result[1] != undefined) {
            this.atitudeScoreCalL0 = result[1].sumPoint
            this.atitudeScoreByL0 = result[1].scoreFromName
          }
          if (result[2] != undefined) {
            this.atitudeScoreCalL1 = result[2].sumPoint
            this.atitudeScoreByL1 = result[2].scoreFromName
            this.l1ID = result[2].id
          }
          if (result[3] != undefined) {
            this.atitudeScoreCalL2 = result[3].sumPoint
            this.atitudeScoreByL2 = result[3].scoreFromName
            this.l2ID = result[3].id
          }

          if (!isNumber(this.atitudeScoreCalFL)) {
            this.atitudeScoreCalFL = 0
          }
          if (!isNumber(this.atitudeScoreCalL0)) {
            this.atitudeScoreCalL0 = 0
          }
          if (!isNumber(this.atitudeScoreCalL1)) {
            this.atitudeScoreCalL1 = 0
          }
          if (!isNumber(this.atitudeScoreCalL2)) {
            this.atitudeScoreCalL2 = 0
          }

          // if ((Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL0) == 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) == 0) ||
          //     (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL0) != 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) == 0) ||
          //     (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL0) == 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) == 0) ||
          //     (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL0) == 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) != 0)) {

          //       this.attitudeScoreCal = (Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL0) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2))/30*30
          // }
          // else if ((Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL0) != 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) == 0) ||
          //           (Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL0) == 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) == 0) ||
          //           (Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL0) == 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) != 0) ||
          //           (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL0) != 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) == 0) ||
          //           (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL0) != 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) != 0) ||
          //           (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL0) == 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) != 0)) {

          //             this.attitudeScoreCal = ((Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL0) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2))/2)/30*30
          // }
          // else if ((Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL0) != 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) == 0) ||
          //           (Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL0) != 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) != 0) ||
          //           (Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL0) == 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) != 0) ||
          //           (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL0) != 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) != 0)) {

          //             this.attitudeScoreCal = ((Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL0) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2))/3)/30*30
          // }
          // else {
          //   this.attitudeScoreCal = ((Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL0) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2))/4)/30*30
          // }

          if ((Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) == 0) ||
              (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) == 0) ||
              (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) != 0)) {

                this.attitudeScoreCal = (Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2))/30*30
          }
          else if ((Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) == 0) ||
                    (Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) != 0) ||
                    (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) != 0)) {

                      this.attitudeScoreCal = ((Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2))/2)/30*30
          }
          else {
            this.attitudeScoreCal = ((Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2))/3)/30*30
          }

          this.attitudeScoreCal = Math.round((this.attitudeScoreCal + Number.EPSILON) * 10) / 10

          this.calculatorH1();

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadDataSumNewAttitudeScore() {
    return new Promise((res, rej) => {
      this.service.getSumNewAttitudeScore(this.userID, this.campaignID).subscribe(
        (result: any) => {
          this.dataSumAttitudeScore = result

          if (result[0] != undefined) {
            this.atitudeScoreCalFL = result[0].sumPoint
            this.atitudeScoreByFL = result[0].scoreFromName
            this.flID = result[0].id
          }
          if (result[1] != undefined) {
            this.atitudeScoreCalL0 = result[1].sumPoint
            this.atitudeScoreByL0 = result[1].scoreFromName
          }
          if (result[2] != undefined) {
            this.atitudeScoreCalL1 = result[2].sumPoint
            this.atitudeScoreByL1 = result[2].scoreFromName
            this.l1ID = result[2].id
          }
          if (result[3] != undefined) {
            this.atitudeScoreCalL2 = result[3].sumPoint
            this.atitudeScoreByL2 = result[3].scoreFromName
            this.l2ID = result[3].id
          }

          if (!isNumber(this.atitudeScoreCalFL)) {
            this.atitudeScoreCalFL = 0
          }
          if (!isNumber(this.atitudeScoreCalL0)) {
            this.atitudeScoreCalL0 = 0
          }
          if (!isNumber(this.atitudeScoreCalL1)) {
            this.atitudeScoreCalL1 = 0
          }
          if (!isNumber(this.atitudeScoreCalL2)) {
            this.atitudeScoreCalL2 = 0
          }

          if ((Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) == 0) ||
              (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) == 0) ||
              (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) != 0)) {

                this.attitudeScoreCal = Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2)
          }
          else if ((Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) == 0) ||
                    (Number(this.atitudeScoreCalFL) != 0 && Number(this.atitudeScoreCalL1) == 0 && Number(this.atitudeScoreCalL2) != 0) ||
                    (Number(this.atitudeScoreCalFL) == 0 && Number(this.atitudeScoreCalL1) != 0 && Number(this.atitudeScoreCalL2) != 0)) {

                      this.attitudeScoreCal = (Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2))/2
          }
          else {
            this.attitudeScoreCal = (Number(this.atitudeScoreCalFL) + Number(this.atitudeScoreCalL1) + Number(this.atitudeScoreCalL2))/3
          }

          this.attitudeScoreCal = Math.round((this.attitudeScoreCal + Number.EPSILON) * 100) / 100

          this.calculatorH1();

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadDataSpecialScoreDetail() {
    return new Promise((res, rej) => {
      this.service.getSpecialScoreDetail(this.userID, this.campaignID).subscribe(
        (result: any) => {
          if(result !== null) {

            this.specialID = result.id
            this.subjectl1 = result.subject
            this.contentl1 = result.content
            this.ratiol1 = result.ratio
            // this.typel1 = result.typeID
            this.getMultiType()
            // this.compactl1 = result.compactID
            this.getMultiImpact()
            this.scorel1 = result.point
            this.filel1 = result.files
            this.specialScore.id = result.id
            this.isDataSpecial = true
          }

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  clearSpecialScoreDetail() {
    this.subjectl1 = null
    this.contentl1 = null
    this.ratiol1 = null
    this.scorel1 = null
    this.dropDownListType.value = null
    this.dropDownListImpact.value = null
  }

  saveSpecial() {
    // if (this.typel1 == null || this.compactl1 == null) {
    //   this.typel1 = 0
    //   this.compactl1 = 0
    // }
    // const modelSpecialScore = {
    //   id: this.specialID,
    //   point: this.scorel1,
    //   campaignID: this.campaignID,
    //   ScoreTo: this.userID,
    //   ScoreType: 'L1',
    //   TypeID: this.typel1,
    //   ratio: this.ratiol1,
    //   subject: this.subjectl1,
    //   content: this.contentl1,
    //   specialScore: 0,
    //   compactID: this.compactl1
    // }

    const modelSpecialScore = {
      point: this.scorel1 === null ? "1000" : this.scorel1,
      campaignID: this.campaignID,
      ScoreBy: 0,
      ScoreFrom: 0,
      ScoreTo: this.userID,
      ScoreType: 'L1',
      IsSubmit: true,
      ratio: this.ratiol1 === null ? "1000" : this.ratiol1,
      subject: this.subjectl1,
      content: this.contentl1,
      TypeID: 0,
      compactID: 0,
      TypeListID: this.typel1,
      CompactListID: this.compactl1,
      specialScore: 0
    }

    // this.service.updateSpecialScore(modelSpecialScore).subscribe((res: any) => {
    //   this.alertify.success(MessageConstants.UPDATED_OK_MSG);
    //   this.updateCommitteeScore();
    //   this.loadDataSpecialScoreDetail();
    // })

    this.service.updateSpecialContribution(modelSpecialScore).subscribe((res: any) => {
        this.alertify.success(MessageConstants.UPDATED_OK_MSG);
        this.updateCommitteeScore();
        this.updateNewAttitudeEvaluation();
        this.loadDataSpecialScoreDetail();
      })
  }

  getSpecialScore() {
    // this.specialScoreService.getSpecialScore().subscribe(res => {
    //   this.scoreDataL1 = res
    // })

    return new Promise((res, rej) => {
      this.specialScoreService.getSpecialScore().subscribe(
        (result: any) => {
          this.scoreDataL1 = result

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getSpecialRatio() {
    // this.specialScoreService.getSpecialRatio().subscribe(res => {
    //   this.ratioDataL1 = res
    // })

    return new Promise((res, rej) => {
      this.specialScoreService.getSpecialRatio().subscribe(
        (result: any) => {
          this.ratioDataL1 = result

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getSpecialCompact() {
    // this.specialScoreService.getSpecialCompact(this.lang).subscribe(res => {
    //   this.compactDataL1 = res
    // })

    return new Promise((res, rej) => {
      this.specialScoreService.getSpecialCompact(this.lang).subscribe(
        (result: any) => {
          this.compactDataL1 = result

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getSpecialType() {
    // this.specialScoreService.getSpecialType(this.lang).subscribe(res => {
    //   this.typeDataL1 = res
    // })

    return new Promise((res, rej) => {
      this.specialScoreService.getSpecialType(this.lang).subscribe(
        (result: any) => {
          this.typeDataL1 = result

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getMultiType() {
    this.specialScoreService.getMultiType(this.campaignID, this.userID , this.type).subscribe((res: any) => {
      console.log('getMultiType', res);
      this.typel1 = res
    })
  }

  getMultiImpact() {
    this.specialScoreService.getMultiImpact(this.campaignID,  this.userID , this.type).subscribe((res: any) => {
      console.log('getMultiImpact', res);
      this.compactl1 = res
    })
  }

  getSpecialScoreL2() {
    // this.specialScoreService.getSpecialScore().subscribe(res => {
    //   this.scoreDataL2 = res
    //   this.scoreDataL2.unshift({ id: 'N/A', point: 'N/A'});
    // })

    return new Promise((res, rej) => {
      this.specialScoreService.getSpecialScore().subscribe(
        (result: any) => {
          this.scoreDataL2 = result
          this.scoreDataL2.unshift({ id: 'N/A', point: 'N/A'});

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getDetailNewAttitudeEvaluation() {
    return new Promise((res, rej) => {
      this.service.getDetailNewAttitudeEvaluation(this.userID, this.campaignID).subscribe((result: any) => {
        if (result !== null) {
          this.dataAttEvaluation = result
          console.log(this.dataAttEvaluation)
        }

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });

  }

  onSpecialScoreL2(args) {
    this.scorel2 = args.itemData.point;
  }

  loadDataKpiScoreL2() {
    return new Promise((res, rej) => {
      this.service.getScoreL2(this.userID, this.campaignID).subscribe(
        (result: any) => {
          this.dataKpiScoreL2 = result
          if (result[0] != undefined) {
            this.kpiSpecialScoreL2 = result[0].point
            this.scorel2 = result[0].point
          }
          if (!isNumber(this.kpiSpecialScoreL2)) {
            this.kpiSpecialScoreL2 = 0
          }

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getCommitteeScore() {
    return new Promise((res, rej) => {
      this.service.getCommitteeScore(this.userID, this.userLogin, this.campaignID).subscribe(
        (result: any) => {
          if (result != null) {
            this.specialScoreCal = result.score
          }
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadkpiSpecialScoreL2Tamp(){
    this.kpiSpecialScoreL2Tamp = this.kpiSpecialScoreL2
    // this.specialScoreCal = this.kpiSpecialScoreL2Tamp
  }

  calculatorH1(){
    let ipnutScoreCal
    ipnutScoreCal = this.specialScoreCal
    if(!isNumber(ipnutScoreCal)) {
      ipnutScoreCal = 0
    }
    // if (ipnutScoreCal == 0) {
    //   ipnutScoreCal = this.kpiSpecialScoreL2Tamp
    // }
    if (this.specialScoreCal == null || this.specialScoreCal == undefined || this.specialScoreCal == "") {
      ipnutScoreCal = this.kpiSpecialScoreL2Tamp
      this.specialScoreCal = this.kpiSpecialScoreL2Tamp
    }

    this.h1ScoreCal = Number(this.kpiScoreCal) + Number(this.attitudeScoreCal) + Number(ipnutScoreCal);
    this.h1ScoreCal = Math.round((this.h1ScoreCal + Number.EPSILON) * 10) / 10

    this.contentTooltipKpi = '<div><span>Score By L0: ' + this.kpiScoreByL0 + ' - ' + this.kpiScoreCalL0 + '</span><br><span>Score By L1: ' + this.kpiScoreByL1 + ' - ' + this.kpiScoreCalL1 + '</span><br><span>Score By L2: ' + this.kpiScoreByL2 + ' - ' + this.kpiScoreCalL2 + '</span></div>'
    this.contentTooltipAttitude = '<div><span>Score By FL: ' + this.atitudeScoreByFL + ' - ' + this.atitudeScoreCalFL + '</span><br><span>Score By L1: ' + this.atitudeScoreByL1 + ' - ' + this.atitudeScoreCalL1 + '</span><br><span>Score By L2: ' + this.atitudeScoreByL2 + ' - ' + this.atitudeScoreCalL2 + '</span></div>'

    //this.updateCommitteeScore()
  }

  detailScore() {
    // window.open(`#/transaction/score-detail/${this.campaignID}/${this.flID}/${this.userID}/${this.l1ID}/${this.l2ID}/${"L0"}`,'_blank')
    window.open(`#/transaction/new-score-attitude-detail/${this.campaignID}/${this.userID}`,'_blank')
  }

  downloadFile(item) {
    const file_open_brower = ['png', 'jpg','pdf']
    var ext =  item.name.split('.').pop();
    if(file_open_brower.includes(ext)) {
      window.open(this.base + item.path,'_blank')
    } else {
      const url = `${this.base_download}UploadFile/DownloadFileSpecialScore/${item.name}`
      this.service.download(url).subscribe(data =>{
        const blob = new Blob([data]);
        const downloadURL = window.URL.createObjectURL(data);
        const link = document.createElement('a');
        link.href = downloadURL;
        const ct = new Date();
        link.download = `${item.name}`;
        link.click();
      })
    }
  }
  updateKpiScore() {
    this.service.updateKpiScore(this.kpiScore).subscribe(() => {
      this.alertify.success(MessageConstants.UPDATED_OK_MSG);
      this.loadDataKpiScore();
      this.kpiScore = {
      };
    });
  }

  updateAttitudeScore() {
    this.service.updateAttitudeScore(this.attitudeScore).subscribe((res: any) => {
        this.alertify.success(MessageConstants.UPDATED_OK_MSG);
        this.loadDataAttitudeScore();
        this.attitudeScore = {
        };
    });
  }

  updateKpiScoreL2() {
    this.service.updateKpiScoreL2(this.kpiScoreL2).subscribe(() => {
      this.alertify.success(MessageConstants.UPDATED_OK_MSG);
      this.loadDataKpiScoreL2();
      this.kpiScoreL2 = {
      };
    });
  }

  updateCommitteeScore() {
    this.committeeScore.scoreFrom = this.userLogin;
    this.committeeScore.scoreTo = this.userID;
    this.committeeScore.campaignID = this.campaignID;
    this.committeeScore.score = this.specialScoreCal;
    this.service.updateCommitteeScore(this.committeeScore).subscribe(() => {
      //this.alertify.success(MessageConstants.UPDATED_OK_MSG);
      this.calculatorH1();
      this.committeeScore = {
      };
    });
  }

  updateNewAttitudeEvaluation() {
    this.service.updateNewAttitudeEvaluation(this.dataAttEvaluation).subscribe((res: any) => {
      this.getDetailNewAttitudeEvaluation()

    });
  }

  actionBeginEditKpiScore(args) {
    //console.log(args)
    // if (args.requestType === 'save') {
    //   if (args.action === 'edit') {
    //     this.kpiScore.id = args.data.id;
    //     this.kpiScore.comment = args.data.comment;
    //     this.kpiScore.point = args.data.point;
    //     //this.updateKpiScore();
    //   }
    // }
  }

  cellEdit(args) {
  //   if (args.rowData.status === false) {
  //     this.alertify.warning("Please select the ink/chemical ! <br> Vui lòng chọn mực/hóa chất", true);
  //     args.cancel = true;
  //     return;
  //   }
  //   if (args.rowData.status) {
  //     if (args.rowData.modify === false) {
  //       this.alertify.warning("Can not modify this chemical <br> Không thể sửa đổi hóa chất này", true);
  //       args.cancel = true;
  //       return;
  //     }
  //   }

    if (args.columnName == 'comment') {
      this.edittingKpiComment = true
      this.textareaKpiComment = args.value
    }
  }
  keyPressed(args) {
    if (args.shiftKey){
      if(args.key === 'Enter')
      {
        args.cancel = true;
      }
    }
    else if (args.key === 'Enter') {
      this.gridKPIScore.editModule.saveCell();
    }

  }
  //   if (args.key == 'Enter') {
  //     args.cancel = true;
  //   }
  // }
  public keyHandler(e) {
    if (e.keyCode === 13 && e.shiftKey && this.edittingKpiComment) {
      this.textareaKpiComment = this.textareaKpiComment + '\n'
      return
    }
    else if (e.keyCode === 16 && e.shiftKey) {
      return;
    }
    else if (e.keyCode === 13 && e.shiftKey) {
      //this.gridKPIScore.editModule.saveCell();

      // var text = document.getElementById("textarea1");
      //     text.value += '\n';
      this.textareaHr = this.textareaHr + '\n'
    }
    // else if (e.keyCode === 13) {
    //   debugger
    //   this.gridKPIScore.editModule.saveCell();
    // }
  }
  rowDeselected(args) {
    this.edittingKpiComment = false
  }

  public textareaaa(e) {
    // if (e.keyCode === 13 && e.shiftKey) {
    //   //this.gridKPIScore.editModule.saveCell();
    //   console.log("11111")
    //   // var text = document.getElementById("textarea1");
    //   //     text.value += '\n';
    //   console.log(e.data.hrComment)
    // }
    this.textareaHr = e
  }
  dataBound(args) {
    // this.flag = true;
  }

  cellSave(args) {
    if (args.columnName === 'point') {
      if (args.previousValue === args.value) {
        return
      }
      else {
        this.kpiScore.id = args.rowData.id;
        this.kpiScore.comment = args.rowData.comment;
        this.kpiScore.point = args.value;
        // this.kpiScore.hrComment = args.rowData.hrComment;
        this.kpiScore.hrComment = this.textareaHr;
        this.kpiScore.hrCommentCreatedBy = this.userLogin;
        this.kpiScore.hrCommentUpdateBy = this.userLogin;
        this.updateKpiScore();
      }
    }

    if (args.columnName === 'comment') {
      this.edittingKpiComment = false
      if (args.previousValue === this.textareaKpiComment) {
        return
      }
      else {
        this.kpiScore.id = args.rowData.id;
        this.kpiScore.comment = this.textareaKpiComment;
        this.kpiScore.point = args.rowData.point;
        this.kpiScore.hrComment = args.rowData.hrComment;
        this.kpiScore.hrCommentCreatedBy = this.userLogin;
        this.kpiScore.hrCommentUpdateBy = this.userLogin;
        this.updateKpiScore();
      }
    }

    if (args.columnName === 'hrComment') {
      if (args.previousValue === args.value) {
        return
      }
      else {
        this.kpiScore.id = args.rowData.id;
        this.kpiScore.comment = args.rowData.comment;
        this.kpiScore.point = args.rowData.point;
        this.kpiScore.hrComment = args.value;
        this.kpiScore.hrCommentCreatedBy = this.userLogin;
        this.kpiScore.hrCommentUpdateBy = this.userLogin;
        this.updateKpiScore();
      }
    }

    // if (args.columnName === 'percentage') {

    //   let details = this.getLocalStore("details");
    //     for (let i in this.chemicalData) {
    //       if (this.chemicalData[i].id == args.rowData.id && this.chemicalData[i].subname == args.rowData.subname) {
    //         this.chemicalData[i].percentage = args.value;
    //         this.chemicalData[i].consumption = args.rowData.consumption;
    //         break;
    //       }
    //     }
    //     for (let i in details) {
    //       if (details[i].id == args.rowData.id && details[i].subname == args.rowData.subname) {
    //         details[i].percentage = args.value;
    //         details[i].consumption = args.rowData.consumption;
    //         break;
    //       }
    //     }
    //   this.setLocalStore("details", details);
    // }

    // if (args.columnName === 'consumption') {

    //   let details = this.getLocalStore("details");
    //     for (let i in this.chemicalData) {
    //       if (this.chemicalData[i].id == args.rowData.id && this.chemicalData[i].subname == args.rowData.subname) {
    //         this.chemicalData[i].percentage = args.rowData.percentage;
    //         this.chemicalData[i].consumption = args.value;
    //         break;
    //       }
    //     }
    //     for (let i in details) {
    //       if (details[i].id == args.rowData.id && details[i].subname == args.rowData.subname) {
    //         details[i].percentage = args.rowData.percentage;
    //         details[i].consumption = args.value;
    //       }
    //     }
    //   this.setLocalStore("details", details);
    // }

  }

  keyPressedKpi(args) {
    if (args.shiftKey){
      if(args.key === 'Enter')
      {
        args.cancel = true;
      }
    }
    else
    if (args.key === 'Enter') {
      this.gridAttitudeScore.editModule.endEdit();
    }
  }

  actionBeginEditAttitudeScore(args) {
    if (args.requestType === 'save') {
      if (args.action === 'edit') {
        if (args.previousData.comment === args.data.comment) {
          this.loadDataAttitudeScore();
          this.attitudeScore = {
          };
          return;
        }
        this.attitudeScore.id = args.data.id;
        this.attitudeScore.comment = args.data.comment;
        this.attitudeScore.point = args.data.point;
        this.attitudeScore.type = args.data.type;
        this.updateAttitudeScore();
      }
    }
  }

  keyPressedKpiScoreL2(args) {
    if (args.shiftKey){
      if(args.key === 'Enter')
      {
        args.cancel = true;
      }
    }
    else
    if (args.key === 'Enter') {
      this.gridKpiScoreL2.editModule.endEdit();
    }
  }

  actionBeginEditKpiScoreL2(args) {
    if (args.requestType === 'beginEdit') {
      this.scorel2 = args.rowData.point
    }
    if (args.requestType === 'save') {
      if (args.action === 'edit') {
        this.kpiScoreL2.id = args.data.id;
        this.kpiScoreL2.content = args.data.content;
        this.kpiScoreL2.point = this.scorel2;
        this.updateKpiScoreL2();
      }
    }
  }

  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 47 && charCode < 58 || charCode === 46  || charCode === 45) {
      return true;
    }
    return false;
  }

  // actionComplete(args) {
  //   // if (e.requestType === 'add') {
  //   //   //(e.form.elements.namedItem('slPage') as HTMLInputElement).focus();
  //   //   // (e.form.elements.namedItem('id') as HTMLInputElement).disabled = true;
  //   // }
  // }

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
