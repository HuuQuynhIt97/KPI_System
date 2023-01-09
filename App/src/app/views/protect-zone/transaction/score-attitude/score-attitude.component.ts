import { AttitudeScoreService } from './../../../../_core/_service/attitude-score.service';
import { Component, ElementRef, HostListener, Input, OnInit, QueryList, TemplateRef, ViewChild, ViewChildren } from '@angular/core';
import { ModalDismissReasons, NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { SystemCode_Heading } from 'src/app/_core/enum/system';
import { NgTemplateNameDirective } from '../ng-template-name.directive';
import { DataService } from 'src/app/_core/_service/data.service';
import { ScoreUploadFileComponent } from './score-upload-file/score-upload-file.component';
import { TranslateService } from '@ngx-translate/core';
import { SelectEventArgs, TabComponent } from '@syncfusion/ej2-angular-navigations';
import { DialogComponent } from '@syncfusion/ej2-angular-popups';
@Component({
  selector: 'app-score-attitude',
  templateUrl: './score-attitude.component.html',
  styleUrls: ['./score-attitude.component.scss']
})
export class ScoreAttitudeComponent implements OnInit {
  backToPreviousPage: boolean = false;
  checkbackToPreviousPage: boolean = true;
  @Input() data: any;
  @Input() closeResult: any;
  gridData: any
  BTN_NEXT: string = "Next"
  BTN_BACK: string = "Back"
  BTN_CLOSE: string = "Close"
  BTN_LOAD_DEFAULT: string = "Default"
  passionData: any
  accountbilityData: any
  attentionData: any
  continuousData: any
  effectiveData: any
  resilienceData: any

  checked: boolean = false
  scoreAdd: any = []
  listBehaviorCheck: any = []
  dataAdd = {
    score: "",
    ScoreBy: 0,
    ScoreTo: 0,
    campaignID: 0,
    headingID: 0,
    comment: '',
    type: '',
    data: [],
    l0: false,
    l1: false,
    l2: false,
    fl: false,
    typeBtn: '',
  }
  score: number = 0
  L0: boolean = false
  L1: boolean = false
  L2: boolean = false
  FL: boolean = false
  changeScore: boolean = false
  changeComment: boolean = false
  checkComment: boolean = false
  alertCheckComment: string = null
  comment: string = null
  userId: number;
  pageData = [
    {
      id: 1,
      name: 'Page 1',
      position: '1'
    },
  ]
  public SystemCode_Heading = SystemCode_Heading
  index: any;
  content: any;
  @ViewChild(NgTemplateNameDirective) public Gridtemplates: QueryList<NgTemplateNameDirective>;
  @ViewChild('DatePickertemplateRef') public DatePickertemplate: TemplateRef<any>;
  changeEl = [];
  dataRemove: any = [];
  changeLocalHome = [];
  @ViewChild('message') message: NgbModalRef;
  @ViewChild('tab') tabInstance: TabComponent;
  scoreData: object[] = [
    {
      id:2,
      name: "2"
    },
    {
      id:3,
      name: "3"
    },
    {
      id:4,
      name: "4"
    },
    {
      id:5,
      name: "5"
    }
  ]
  public value: number = 3;
  fieldsScore: object = { text: 'name', value: 'name'};
  scoreItem: any;
  @ViewChild('alertDialog') alertDlg: DialogComponent;
  dlgButtons: { buttonModel: { content: string; isPrimary: boolean; }; click: () => void; }[];
  public animation: object = {
    previous: { effect: "", duration: 0, easing: "" },
    next: { effect: "", duration: 0, easing: "" }
  };
  constructor(
    public activeModal: NgbActiveModal,
    private attitudeScoreService: AttitudeScoreService,
    private alertify: AlertifyService,
    public modalService: NgbModal,
    private dataService: DataService,
    private translate: TranslateService,
    private spinner: NgxSpinnerService,
    private eRef: ElementRef
  ) {
  }

  ngOnInit() {
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);

    this.getAsyncData()
    switch (this.data.type) {
      case 'L0':
        this.dataAdd.l0 = true
        break;
      case 'L1':
        this.dataAdd.l1 = true
        break;
      case 'L2':
        this.dataAdd.l2 = true
        break;
      case 'FL':
        this.dataAdd.fl = true
        break;
      default:
        break;
    }

    if (this.closeResult == 0) {
      this.getDismissReason(this.closeResult);
    }

    this.dataService.locale.subscribe((res: any)=>{
      this.translate.addLangs([res])
      this.translate.use(res)
    })

  }
  public select(e: SelectEventArgs) {
    if (e.isSwiped) {
      e.cancel = true; // Prevents the content swipe selection
    }
  }

  ngAfterViewInit(): void {
    // this.tabInstance.element.classList.add('e-fill');
    // let items: any = this.tabInstance.items;
    //   for(let i: number = 0; i < items.length; i++) {
    //      items[i].header.iconPosition = 'e-facebook';
    //   }
  }
  switchValue(heading) {
    switch (heading) {
      case SystemCode_Heading.PASSION:
        this.dataAdd.headingID = this.passionData.headingID
        this.dataAdd.comment = this.changeComment ? this.dataAdd.comment : this.passionData.comment
        this.dataAdd.score = this.changeScore ? this.dataAdd.score : this.passionData.score
        break;
      case SystemCode_Heading.ACCOUNTABILITY:
        this.dataAdd.headingID = this.accountbilityData.headingID
        this.dataAdd.comment = this.changeComment ? this.dataAdd.comment : this.accountbilityData.comment
        this.dataAdd.score = this.changeScore ? this.dataAdd.score : this.accountbilityData.score
        break;
      case SystemCode_Heading.ATTENTION_TO_DETAIL:
        this.dataAdd.headingID = this.attentionData.headingID
        this.dataAdd.comment = this.changeComment ? this.dataAdd.comment : this.attentionData.comment
        this.dataAdd.score = this.changeScore ? this.dataAdd.score : this.attentionData.score
        break;
      case SystemCode_Heading.EFFECTIVE_COMUNICATION:
        this.dataAdd.headingID = this.effectiveData.headingID
        this.dataAdd.comment = this.changeComment ? this.dataAdd.comment : this.effectiveData.comment
        this.dataAdd.score = this.changeScore ? this.dataAdd.score : this.effectiveData.score
        break;
      case SystemCode_Heading.RESILIENCE:
        this.dataAdd.headingID = this.resilienceData.headingID
        this.dataAdd.comment = this.changeComment ? this.dataAdd.comment : this.resilienceData.comment
        this.dataAdd.score = this.changeScore ? this.dataAdd.score : this.resilienceData.score
        break;
      case SystemCode_Heading.CONTINUOUS_LEARNING:
        this.dataAdd.headingID = this.continuousData.headingID
        this.dataAdd.comment = this.changeComment ? this.dataAdd.comment : this.continuousData.comment
        this.dataAdd.score = this.changeScore ? this.dataAdd.score : this.continuousData.score
        break;
      default:
        break;
    }
  }

  public async btnClicked(e: any): Promise<void> {
    switch (e.target.id) {
      case 'nextPage2':
        await this.AsyncSaveScore(SystemCode_Heading.PASSION,0, 1, this.BTN_NEXT)
        break;
      case 'nextPage3':
        await this.AsyncSaveScore(SystemCode_Heading.ACCOUNTABILITY,1, 2, this.BTN_NEXT)
        break;
      case 'nextPage4':
        await this.AsyncSaveScore(SystemCode_Heading.ATTENTION_TO_DETAIL,2, 3, this.BTN_NEXT)
        break;
      case 'nextPage5':
        await this.AsyncSaveScore(SystemCode_Heading.EFFECTIVE_COMUNICATION,3, 4, this.BTN_NEXT)
        break;
      case 'nextPage6':
        await this.AsyncSaveScore(SystemCode_Heading.RESILIENCE,4, 5, this.BTN_NEXT)
        break;

      case 'backPage1':
        await this.AsyncSaveScore(SystemCode_Heading.ACCOUNTABILITY,1, 0, this.BTN_BACK)
        // this.tabInstance.enableTab(1, false);
        // this.tabInstance.enableTab(0, true);
        // this.tabInstance.select(0);
        this.changeComment = false
        this.changeScore = false
        break;
      case 'backPage2':
        await this.AsyncSaveScore(SystemCode_Heading.ATTENTION_TO_DETAIL,2, 1, this.BTN_BACK)
        // this.tabInstance.enableTab(2, false);
        // this.tabInstance.enableTab(1, true);
        // this.tabInstance.select(1);
        this.changeComment = false
        this.changeScore = false
        break;
      case 'backPage3':
        await this.AsyncSaveScore(SystemCode_Heading.EFFECTIVE_COMUNICATION,3, 2, this.BTN_BACK)
        // this.tabInstance.enableTab(3, false);
        // this.tabInstance.enableTab(2, true);
        // this.tabInstance.select(2);
        this.changeComment = false
        this.changeScore = false
        break;
      case 'backPage4':
        await this.AsyncSaveScore(SystemCode_Heading.RESILIENCE,4, 3, this.BTN_BACK)
        // this.tabInstance.enableTab(4, false);
        // this.tabInstance.enableTab(3, true);
        // this.tabInstance.select(3);
        this.changeComment = false
        this.changeScore = false
        break;
      case 'backPage5':
        await this.AsyncSaveScore(SystemCode_Heading.CONTINUOUS_LEARNING,5, 4, this.BTN_BACK)
        // this.tabInstance.enableTab(5, false);
        // this.tabInstance.enableTab(4, true);
        // this.tabInstance.select(4);
        this.changeComment = false
        this.changeScore = false
        break;

      default:
        break;
    }
  }

  detailScore() {
    window.open(`#/transaction/score-detail/${this.data.campaignID}/${this.data.flid}/${this.data.l0ID}/${this.data.l1ID}/${this.data.l2ID}/${this.data.type}`,'_blank')
  }

  ngOnDestroy() {
    this.dataRemove = []
    this.changeLocalHome.forEach(item => item.unsubscribe());
  }
  onclick() {

  }
  public dlgCreated(): void {
    this.alertDlg.hide();
}
  getGridTemplate(name): TemplateRef<any> {
    const dir = this.Gridtemplates.find(dir => dir.name === name + '');
    return dir ? dir.template : null
  }

  async getAsyncData(){
    await this.getAll(this.BTN_LOAD_DEFAULT)
    await this.getListCheckBehavior()
    this.enableOrdisableTab()
  }

  public changeHandler(args,item_behavior, item_keypoint) {
    const checked = args.checked
    const data = {
      behaviorID: item_behavior.id,
      checked: checked,
      campaignID: this.data.campaignID,
      attitudeHeadingID: item_behavior.headingID
    }
    if (this.listBehaviorCheck.length === 0) {
      this.listBehaviorCheck.push(data)
    } else {
      if(checked) {
        for (var i = 0; i < this.listBehaviorCheck.length; i++) {
          if (this.listBehaviorCheck[i].campaignID == this.data.campaignID && this.listBehaviorCheck[i].behaviorID == item_behavior.id) {
            this.listBehaviorCheck[i].checked = checked;
            break;
          } else {
            this.listBehaviorCheck.push(data)
            break;
          }
        }
      }else {
        for (var i = 0; i < this.listBehaviorCheck.length; i++) {
          if (this.listBehaviorCheck[i].campaignID == this.data.campaignID && this.listBehaviorCheck[i].behaviorID == item_behavior.id) {
            this.listBehaviorCheck[i].checked = checked;
            break;
          }
        }
      }
    }
    this.dataAdd.data = this.listBehaviorCheck

    // this.checkbox.label = 'CheckBox: ' + this.checkbox.checked;
  }
  removeTd() {
    const el =  document.querySelectorAll(".upload");
  }

  getAll(typeBtn) {
    if(typeBtn !== this.BTN_CLOSE) {
      this.spinner.show()
    }
    return new Promise((res, rej) => {
      this.attitudeScoreService.getAll(this.data.campaignID, this.userId, this.data.userID, this.data.type).subscribe(
        (result: any) => {
          this.passionData = result.passion
          this.accountbilityData = result.accountbility
          this.attentionData = result.attention
          this.continuousData = result.continuous
          this.effectiveData = result.effective
          this.resilienceData = result.resilience
          if(typeBtn !== this.BTN_CLOSE) {
            this.spinner.hide()
          }
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getListCheckBehavior() {
    return new Promise((res, rej) => {
      this.attitudeScoreService.getListCheckBehavior(this.data.campaignID , this.userId, this.data.userID, this.data.type).subscribe(
        (result: any) => {
          this.listBehaviorCheck = result

          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });

  }

  enableOrdisableTab() {
    this.tabInstance.enableTab(0, true);
    this.tabInstance.enableTab(1, false);
    this.tabInstance.enableTab(2, false);
    this.tabInstance.enableTab(3, false);
    this.tabInstance.enableTab(4, false);
    this.tabInstance.enableTab(5, false);
    this.tabInstance.select(0);
  }

  onChangeScore(args) {
    if(args.isInteracted) {
      this.changeScore = true
      this.dataAdd.score = args.itemData.id;
    }
  }

  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;
  }

  onChangeComment(args) {
    this.changeComment = true
    this.dataAdd.comment = args
  }

  onTabSelecting(args: any) {
    // debugger
    if (this.checkbackToPreviousPage === true) {

    }
    this.backToPreviousPage = !this.backToPreviousPage;
  }

  getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return ;
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return ;
    } else {
      return ;
    }
  }

  async onTabSelect(args: any){


  }

  async AsyncSaveScore(heading,keyPre,keyNext, typeBtn){
    await this.closeAndSaveScore(heading,keyPre,keyNext, typeBtn)
  }

  checkTickBehavior(previousIndex: any, selectedIndex: any) {
    this.switchValue(SystemCode_Heading.ACCOUNTABILITY);
    const listBehaviorCheckPage2 = this.listBehaviorCheck.filter(x => x.checked && x.attitudeHeadingID === this.dataAdd.headingID);

    this.switchValue(SystemCode_Heading.ATTENTION_TO_DETAIL);
    const listBehaviorCheckPage3 = this.listBehaviorCheck.filter(x => x.checked && x.attitudeHeadingID === this.dataAdd.headingID);

    this.switchValue(SystemCode_Heading.EFFECTIVE_COMUNICATION);
    const listBehaviorCheckPage4 = this.listBehaviorCheck.filter(x => x.checked && x.attitudeHeadingID === this.dataAdd.headingID);

    this.switchValue(SystemCode_Heading.RESILIENCE);
    const listBehaviorCheckPage5 = this.listBehaviorCheck.filter(x => x.checked && x.attitudeHeadingID === this.dataAdd.headingID);

    switch (selectedIndex) {
      case 2:
        if (listBehaviorCheckPage2.length == 0) {
          this.alertify.validation(
            "Warnig!",
            this.translate.instant('CHECK_PAGES_IN_ORDER'));
          this.tabInstance.selectedItem = previousIndex;
          this.backToPreviousPage = true;
          return;
        }
        break;
      case 3:
        if (listBehaviorCheckPage2.length == 0 || listBehaviorCheckPage3.length == 0) {
          this.alertify.validation(
            "Warnig!",
            this.translate.instant('CHECK_PAGES_IN_ORDER'));
          this.tabInstance.selectedItem = previousIndex;
          this.backToPreviousPage = true;
          return;
        }
        break;
      case 4:
        if (listBehaviorCheckPage2.length == 0 || listBehaviorCheckPage3.length == 0 || listBehaviorCheckPage4.length == 0) {
          this.alertify.validation(
            "Warnig!",
            this.translate.instant('CHECK_PAGES_IN_ORDER'));
          this.tabInstance.selectedItem = previousIndex;
          this.backToPreviousPage = true;
          return;
        }
        break;
      case 5:
        if (listBehaviorCheckPage2.length == 0 || listBehaviorCheckPage3.length == 0 || listBehaviorCheckPage4.length == 0 || listBehaviorCheckPage5.length == 0) {
          this.alertify.validation(
            "Warnig!",
            this.translate.instant('CHECK_PAGES_IN_ORDER'));
          this.tabInstance.selectedItem = previousIndex;
          this.backToPreviousPage = true;
          return;
        }
        break;
        break;
      default:
        break;
    }
  }

  onClickClose(heading) {
    this.switchValue(heading);
    const result = this.listBehaviorCheck.filter(x => x.checked && x.attitudeHeadingID === this.dataAdd.headingID);
    if (result.length > 0) {
      this.saveScore(heading);
    }
    this.activeModal.dismiss();
  }
  async onCloseModal(heading) {
    this.modalService.dismissAll();
    await this.AsyncSaveScore(heading,0,0,this.BTN_CLOSE)
  }
  openUploadModalComponent(item) {
    const modalRef = this.modalService.open(ScoreUploadFileComponent, { size: 'md', backdrop: 'static', keyboard: false });
    modalRef.componentInstance.data = item;
    modalRef.componentInstance.scoreBy = this.userId;
    modalRef.componentInstance.scoreTo = this.data.userID;
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }
  saveScore(heading) {
    this.switchValue(heading)
    const result = this.listBehaviorCheck.filter(x => x.checked && x.attitudeHeadingID === this.dataAdd.headingID)
    if(result.length === 0) {
      this.alertify.warning('Please choose more than behavior')
      return;
    }
    this.dataAdd.campaignID = this.data.campaignID
    this.dataAdd.ScoreBy = this.userId
    this.dataAdd.ScoreTo = this.data.userID
    this.dataAdd.type = this.data.type
    if (this.dataAdd.score === null || this.dataAdd.score === "") {
      this.dataAdd.score = "3";
    }
    this.attitudeScoreService.saveScore(this.dataAdd).subscribe((res: any) => {
      if(res.success) {
        this.changeComment = false
        this.changeScore = false
        this.getListCheckBehavior()
        this.alertify.success('Successfully')
      }else {
        this.alertify.error(this.translate.instant(res.message))
      }
    })
  }

  closeAndSaveScore(heading,keyPre, keyNext, typeBtn) {
    return new Promise((res, rej) => {
      this.switchValue(heading)
      if (typeBtn === this.BTN_NEXT) {
        const result = this.listBehaviorCheck.filter(x => x.checked && x.attitudeHeadingID === this.dataAdd.headingID)
        if(result.length === 0) {
          this.alertify.validation(
            "Warning!",
            this.translate.instant('CHECK_TICK_BEHAVIOR'));
          return;
        }
      }
      this.dataAdd.campaignID = this.data.campaignID
      this.dataAdd.ScoreBy = this.userId
      this.dataAdd.ScoreTo = this.data.userID
      this.dataAdd.type = this.data.type
      this.dataAdd.typeBtn = typeBtn
      if (this.dataAdd.score === null || this.dataAdd.score === "") {
        this.dataAdd.score = "3";
      }
      this.attitudeScoreService.saveScore(this.dataAdd).subscribe(
        async (result: any) => {
          this.checkComment = result.success;
          if(result.success) {
            this.changeComment = false
            this.changeScore = false
            await this.getAll(typeBtn)
            await this.getListCheckBehavior()
           
            this.tabInstance.enableTab(keyPre, false);
            this.tabInstance.enableTab(keyNext, true);
            this.tabInstance.select(keyNext);
          }
          else {
            this.alertify.error(this.translate.instant(result.message))
            this.alertCheckComment = result.message
          }
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }





  submit() {
    this.openVerticallyCentered(this.message)
  }
  openVerticallyCentered(content) {
    this.modalService.open(content, { centered: true });
  }
  yesConfirm() {
      this.switchValue(SystemCode_Heading.CONTINUOUS_LEARNING)
      const result = this.listBehaviorCheck.filter(x => x.checked && x.attitudeHeadingID === this.dataAdd.headingID)
      if(result.length === 0) {
        this.alertify.validation(
          "Warning!",
          this.translate.instant('CHECK_TICK_BEHAVIOR'));
        return;
      }
      this.dataAdd.campaignID = this.data.campaignID
      this.dataAdd.ScoreBy = this.userId
      this.dataAdd.ScoreTo = this.data.userID
      this.dataAdd.type = this.data.type
      if (this.dataAdd.score === null || this.dataAdd.score === "") {
        this.dataAdd.score = "3";
      }
      this.attitudeScoreService.saveScore(this.dataAdd).subscribe(
        (result: any) => {
          this.checkComment = result.success;
          if(result.success) {
            this.changeComment = false
            this.changeScore = false
            // this.getListCheckBehavior()s
            this.attitudeScoreService.submitAttitude(this.dataAdd).subscribe((res: any) => {
              this.alertify.success(this.translate.instant('SCORE_MESSAGE_SUCCESS'));
              this.modalService.dismissAll();
            })
          }
          else {
            this.alertify.error(this.translate.instant(result.message))
            this.alertCheckComment = result.message
          }
        },
        (error) => {
        }
      );

  }

}
