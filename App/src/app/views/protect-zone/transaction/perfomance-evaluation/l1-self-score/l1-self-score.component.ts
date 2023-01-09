import { SpecialScoreService } from './../../../../../_core/_service/special-score.service';
import { KpiScoreService } from './../../../../../_core/_service/kpi-score.service';
import { Component, OnInit, ViewChild, } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { AttitudeScoreService } from 'src/app/_core/_service/attitude-score.service';
import { DropDownListComponent, MultiSelectComponent } from '@syncfusion/ej2-angular-dropdowns';
import { environment } from 'src/environments/environment';
import { EnvService } from 'src/app/_core/_service/env.service';
import { FileInfo, RemovingEventArgs, SelectedEventArgs, UploaderComponent } from '@syncfusion/ej2-angular-inputs';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { DataService } from 'src/app/_core/_service/data.service';

@Component({
  selector: 'app-l1-self-score',
  templateUrl: './l1-self-score.component.html',
  styleUrls: ['./l1-self-score.component.scss']
})
export class L1SelfScoreComponent implements OnInit {

  campaignID: any;
  public type: string = 'L1';
  userID: any;
  dataDefault: any
  dataString: any
  point: string = null
  comment: string = null
  commentl1: string = null
  pointl1: string = null
  pointFields: object = { text: 'value', value: 'value' };
  pointData: any
  pointDataL1: any = []

  @ViewChild('ddlelement')
  public dropDownListObject: DropDownListComponent;

  @ViewChild('ddlelementType')
  public dropDownListType: MultiSelectComponent;

  @ViewChild('ddlelementImpact')
  public dropDownListImpact: MultiSelectComponent;

  @ViewChild('ddlelementRatio')
  public dropDownListRatio: DropDownListComponent;

  @ViewChild('ddlelementScore')
  public dropDownListScore: DropDownListComponent;

  userID_System: number;
  @ViewChild('message') message: NgbModalRef;

  base = environment.apiUrl;
  totalSize: number = 0;
  size: string;
  public allowExtensions: string = '.doc, .docx, .xls, .xlsx, .pdf, .png, .jpg, .msg';
  files = [
  ]
  public dropElement: HTMLElement = document.getElementsByClassName('control_wrapper')[0] as HTMLElement;
  public path: Object = {
    saveUrl: ''
  };
  @ViewChild('defaultupload')
  public uploadObj: UploaderComponent;
  lang = localStorage.getItem('lang')
  // typel1: number = 0
  // compactl1: number = 0
  typel1: any = []
  compactl1: any = []
  ratiol1: string = null
  scorel1: string = null
  typeDataL1: any
  scoreDataL1: any
  compactDataL1: any
  ratioDataL1: any
  subjectl1: string = null
  contentl1: string = null
  typeFields: object = { text: 'name', value: 'id' };
  compactFields: object = { text: 'name', value: 'id' };
  ratioFields: object = { text: 'point', value: 'point' };
  scoreFields: object = { text: 'point', value: 'point' };
  isSubmit: boolean = false

  needSpecialScore: boolean = false

  specialScoreID_default = 0
  constructor(
    private attitudeScoreService: AttitudeScoreService,
    private kpiScoreService: KpiScoreService,
    private specialScoreService: SpecialScoreService,
    private alertify: AlertifyService,
    private modalService: NgbModal,
    private translate: TranslateService,
    private route: ActivatedRoute,
    public env: EnvService,
    private dataService: DataService,
    public service: Todolist2Service,
    private spinner: NgxSpinnerService
  ) {
    this.dataService.locale.subscribe((res: any)=>{
      this.translate.addLangs([res])
      this.translate.use(res)
    })
  }

  ngOnInit() {
    this.campaignID = this.route.snapshot.params.campaignID
    this.userID = this.route.snapshot.params.userID
    this.userID_System = Number(JSON.parse(localStorage.getItem('user')).id);
    // this.type = this.route.snapshot.params.type
    this.loadData()
    this.loadDataString()
    this.getKPIScoreDetaill0()
    this.getKPIScoreDetailL1()
    this.path = {
      saveUrl: `${this.env.apiUrl}UploadFile/SaveSpecialScore?campaignID=${this.campaignID}&scoreFrom=${this.userID_System}&scoreTo=${this.userID}&scoreType=${this.type}`,
      removeUrl: `${this.env.apiUrl}UploadFile/removeSpecialScore?campaignID=${this.campaignID}&scoreFrom=${this.userID_System}&scoreTo=${this.userID}&scoreType=${this.type}`
    }
    this.dropElement = document.getElementsByClassName('control_wrapper')[0] as HTMLElement;
    this.getSpecialType()
    this.getSpecialCompact()
    this.getSpecialScore()
    this.getSpecialRatio()
    this.loadKPIAttackment()
    this.getMultiType()
    this.getMultiImpact()
    this.getSpecialScoreDetailL1()
    this.disableOrEnableSpecialScore();
    if(this.pointDataL1.length === 0) {
      this.pointDataL1.unshift({ id: 0, value: this.translate.instant('NO_RECORDS_FOUND_KPI_DROPDOWN') });
    }
  }

  onChangeScoreType() {
    // this.model.multiSites = {
    //   sites: this.sites
    // }
    this.dropDownListImpact.enabled = true
    this.dropDownListRatio.enabled = true
    this.dropDownListScore.enabled = true
  }

  changeSpecialType(args) {
    this.needSpecialScore = true
    if (this.dropDownListType.value.length == 0) {
      this.needSpecialScore = false
    }
  }
  disableOrEnableSpecialScore() {
    setTimeout(() => {
      this.dropDownListImpact.enabled = false
      this.dropDownListRatio.enabled = false
      this.dropDownListScore.enabled = false
    }, 300);
  }
  selected(args) {
    if(args.isInteracted) {
      this.needSpecialScore = true
      this.dropDownListImpact.enabled = true
      this.dropDownListRatio.enabled = true
      this.dropDownListScore.enabled = true
    }
  }
  ngOnDestroy(): void {
    this.service.changeUploadMessage(false);
  }
  success(args) {
    this.service.changeUploadMessage(true);

  }
  refresh() {
    this.needSpecialScore = false
    this.dropDownListType.value = null
    this.dropDownListImpact.value = null
    this.dropDownListRatio.value = null
    this.dropDownListScore.value = null
    this.typel1 = []
    this.compactl1 = []

    this.subjectl1 = null
    this.contentl1 = null

  }
  getSpecialScore() {
    this.specialScoreService.getSpecialScore().subscribe(res => {
      this.scoreDataL1 = res
    })
  }

  getSpecialRatio() {
    this.specialScoreService.getSpecialRatio().subscribe(res => {
      this.ratioDataL1 = res
    })
  }

  getSpecialCompact() {
    this.specialScoreService.getSpecialCompact(this.lang).subscribe(res => {
      this.compactDataL1 = res
    })
  }

  getSpecialType() {
    this.specialScoreService.getSpecialType(this.lang).subscribe(res => {
      this.typeDataL1 = res
    })
  }

  loadKPIAttackment() {
    this.files = [];
    this.service.getSpecialFilesScore(this.campaignID,this.userID_System,this.userID, this.type).subscribe(res => {
      this.files = res as any || [];
    });
  }

  beforeUpload(args) {
    if(args.response.statusCode == 400) {
      args.statusText = "File already exists ! 此檔案已存在，請更改檔案名稱、重新上傳！";
    }else {
      args.statusText = args.response.statusText;
    }
  }

  public onSelected(args : SelectedEventArgs) : void {
    args.filesData.splice(5);
    let filesData : FileInfo[] = this.uploadObj.getFilesData();
    let allFiles : FileInfo[] = filesData.concat(args.filesData);
    if (allFiles.length > 5) {
      for (let i : number = 0; i < allFiles.length; i++) {
        if (allFiles.length > 5) {
          allFiles.shift();
        }
      }
      args.filesData = allFiles;
      args.modifiedFilesData = args.filesData;
    }
    args.isModified = true;

    for (let file of args.filesData) {
      this.totalSize = this.totalSize + file.size;
      this.size = this.uploadObj.bytesToSize(this.totalSize);
    }

  }

  public onFileRemove(args: RemovingEventArgs): void {
    args.postRawFile = false;
  }

  getKPIScoreDetaill0() {
    this.kpiScoreService.getKPIScoreDetail(this.campaignID,this.userID ,'L0').subscribe((res: any) => {
      if(res !== null) {
        this.point = res.point
        this.comment = res.comment
      }
    })
  }

  getKPIScoreDetailL1() {
    this.kpiScoreService.getKPIScoreDetailL1L0(this.campaignID, this.userID_System, this.userID , this.type).subscribe((res: any) => {
      if(res !== null) {
        this.loadPoint(res.point)
        setTimeout(() => {
          this.pointl1 = res.point
        }, 500);
        this.commentl1 = res.comment
        this.isSubmit = res.isSubmit
      }
    })
  }
  getMultiType() {
    this.specialScoreService.getMultiType(this.campaignID, this.userID , this.type).subscribe((res: any) => {
      this.typel1 = res
    })
  }
  getMultiImpact() {
    this.specialScoreService.getMultiImpact(this.campaignID,  this.userID , this.type).subscribe((res: any) => {
      this.compactl1 = res
    })
  }
  getSpecialScoreDetailL1() {
    this.specialScoreService.getSpecialScoreDetail(this.campaignID, this.userID_System, this.userID , this.type).subscribe((res: any) => {
      if(res !== null) {
        this.specialScoreID_default = res.id
        this.subjectl1 = res.subject
        this.contentl1 = res.content
        this.ratiol1 = res.ratio
        this.isSubmit = res.isSubmit
        // this.typel1 = res.typeID
        // this.compactl1 = res.compactID

        this.scorel1 = res.point
        // this.needSpecialScore = true
      }
    })
  }


  loadPoint(key) {
    const key_c = parseFloat(key);
    const key_5 = parseFloat('5');
    const key_4_5 = parseFloat('4.5');
    const key_4 = parseFloat('4');
    const key_3 = parseFloat('3');
    const key_2 = parseFloat('2');
    const key_0 = parseFloat('0');

    if (key_5 >= key_c && key_c >=  key_4_5) {
      this.getPoint(4.5,5)
    } else if(key_4_5 > key_c && key_c >= key_4) {
      this.getPoint(4,4.4)
    }
    else if(key_4 > key_c && key_c >= key_3) {
      this.getPoint(3,3.9)
    }
    else if(key_3 > key_c && key_c >= key_2) {
      this.getPoint(2,2.9)
    }
    else {
      this.getPoint(0,1.9)
    }
  }
  checkEmptyOrNull(str)
  {
    if (typeof str == 'undefined' || !str || str.length === 0 || str === "" || !/[^\s]/.test(str) || /^\s*$/.test(str) || str.replace(/\s/g,"") === "")
      return true;
    else
      return false;
  }
  validation() {
    var comment = this.checkEmptyOrNull(this.commentl1)
    if(this.pointl1 === null) {
      this.alertify.error(this.translate.instant('POINT_VALIDATION_MESSAGE'))
      return false;
    }

    if(comment) {
      this.alertify.error(this.translate.instant('COMMENT_VALIDATION_MESSAGE'))
      return false;
    }

    if(this.needSpecialScore ) {
      //special validation
      if(this.checkEmptyOrNull(this.subjectl1)) {
        this.alertify.error(this.translate.instant('SUBJECT_VALIDATION_MESSAGE'))
        return false;
      }

      if(this.checkEmptyOrNull(this.contentl1)) {
        this.alertify.error(this.translate.instant('CONTENT_VALIDATION_MESSAGE'))
        return false;
      }

      if(this.typel1.length === 0) {
        this.alertify.error(this.translate.instant('TYPE_VALIDATION_MESSAGE'))
        return false;
      }

      if(this.compactl1.length === 0) {
        this.alertify.error(this.translate.instant('COMPACT_VALIDATION_MESSAGE'))
        return false;
      }

      if(this.ratiol1 === null) {
        this.alertify.error(this.translate.instant('RATIO_VALIDATION_MESSAGE'))
        return false;
      }

      if(this.scorel1=== null) {
        this.alertify.error(this.translate.instant('SCORE_VALIDATION_MESSAGE'))
        return false;
      }

      //end validation special score
    }
    else {
      if((!this.checkEmptyOrNull(this.subjectl1)) || (!this.checkEmptyOrNull(this.contentl1)) ||
         (this.typel1.length !== 0 && this.typel1.length !== null) ||
         (this.compactl1.length !== 0 && this.compactl1.length !== null) ||
         (this.ratiol1 !== null ) || (this.scorel1 !== null)) {
        this.alertify.error(this.translate.instant('SUBJECT_VALIDATION_MESSAGE'))
        return false;
      }

    }

    return true;
  }

  openVerticallyCentered(content) {
    this.modalService.open(content, { centered: true });
  }

  save(submit) {
    if(submit) {
      this.openVerticallyCentered(this.message)
      return;
    }
    if (this.validation() == false) return;
    const modelKPIScore = {
      point: this.pointl1,
      comment: this.commentl1,
      campaignID: this.campaignID,
      ScoreBy: this.userID_System,
      ScoreFrom: this.userID_System,
      ScoreTo: this.userID,
      ScoreType: 'L1',
      IsSubmit: submit
    }

    const modelSpecialScore = {
      point: this.scorel1,
      campaignID: this.campaignID,
      ScoreBy: this.userID_System,
      ScoreFrom: this.userID_System,
      ScoreTo: this.userID,
      ScoreType: 'L1',
      IsSubmit: submit,
      ratio: this.ratiol1,
      subject: this.subjectl1,
      content: this.contentl1,
      specialScore: 0,
      TypeID: 0,
      compactID: 0,
      TypeListID: this.typel1,
      CompactListID: this.compactl1
    }
    // this.addKPIScore(modelKPIScore);
    // this.addSpecialScore(modelSpecialScore);
    // this.alertify.success('Successfully')
    // setTimeout(() => {
    //   this.getKPIScoreDetailL1()
    // }, 500);
  }

  addKPIScore(model) {
    this.kpiScoreService.add(model).subscribe(res => {
      this.dataService.changeMessage('RefreshData')

    })
  }

  addSpecialScore(model) {
    this.specialScoreService.add(model).subscribe(res => {
      this.dataService.changeMessage('RefreshData')
    })
  }

  yesConfirm() {
    if (this.validation() == false) return;
    const modelKPIScore = {
      point: this.pointl1,
      comment: this.commentl1,
      campaignID: this.campaignID,
      ScoreBy: this.userID_System,
      ScoreFrom: this.userID_System,
      ScoreTo: this.userID,
      ScoreType: 'L1',
      IsSubmit: true
    }

    const modelSpecialScore = {
      point: this.scorel1 === null ? "1000" : this.scorel1,
      campaignID: this.campaignID,
      ScoreBy: this.userID_System,
      ScoreFrom: this.userID_System,
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

    this.addKPIScore(modelKPIScore);
    this.addSpecialScore(modelSpecialScore);
    this.alertify.success('Successfully')
    this.getKPIScoreDetailL1()
    setTimeout(() => {
      this.getSpecialScoreDetailL1()
    }, 150);
    this.modalService.dismissAll();


  }

  getPoint(from, to) {
    this.dropDownListObject.value = null;
    this.attitudeScoreService.getPoint(from,to).subscribe(res => {
      this.pointDataL1 = res
    })
  }

  loadData() {
    this.spinner.show()
    this.attitudeScoreService.getKPISelfScoreDefault(this.campaignID,this.userID,this.type).subscribe(res => {
      this.dataDefault = res
      this.spinner.hide();
    })
  }

  loadDataString() {
    this.spinner.show()
    this.attitudeScoreService.getKPISelfScoreString(this.campaignID,this.userID,this.type).subscribe(res => {
      this.dataString = res
      this.spinner.hide();
    })
  }

}
