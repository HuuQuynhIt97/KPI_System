import { SpecialScoreService } from './../../../../../_core/_service/special-score.service';
import { KpiScoreService } from './../../../../../_core/_service/kpi-score.service';
import { Component, OnInit, ViewChild, } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { AttitudeScoreService } from 'src/app/_core/_service/attitude-score.service';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { environment } from 'src/environments/environment';
import { EnvService } from 'src/app/_core/_service/env.service';
import { FileInfo, RemovingEventArgs, SelectedEventArgs, UploaderComponent } from '@syncfusion/ej2-angular-inputs';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { DataService } from 'src/app/_core/_service/data.service';

@Component({
  selector: 'app-gm-self-score',
  templateUrl: './gm-self-score.component.html',
  styleUrls: ['./gm-self-score.component.scss']
})
export class GmSelfScoreComponent implements OnInit {

  campaignID: any;
  public type: string = 'L1';
  userID: any;
  dataDefault: any
  dataString: any
  point: string = null
  comment: string = null
  commentl1: string = null
  commentl2: string = null
  commentGM: string = null
  pointl1: string = null
  pointl2: string = null
  pointGM: string = null
  pointFields: object = { text: 'value', value: 'value' };
  pointData: any 
  pointDataL1: any 
  pointDataL2: any 
  pointDataGM: any = []
  @ViewChild('ddlelement')
  public dropDownListObject: DropDownListComponent;
  userID_System: number;
  @ViewChild('message') message: NgbModalRef;

  base: any;
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
  typel1: number = 0
  compactl1: number = 0
  ratiol1: string = null
  scorel1: string = null
  scorel2: string = null
  typeDataL1: any 
  scoreDataL1: any 
  scoreDataL2: any 
  compactDataL1: any 
  ratioDataL1: any 
  subjectl1: string = null
  contentl1: string = null
  contentl2: string = null
  typeFields: object = { text: 'name', value: 'id' };
  compactFields: object = { text: 'name', value: 'id' };
  ratioFields: object = { text: 'point', value: 'point' };
  scoreFields: object = { text: 'point', value: 'point' };
  base_download: any
  constructor(
    private attitudeScoreService: AttitudeScoreService,
    private kpiScoreService: KpiScoreService,
    private specialScoreService: SpecialScoreService,
    private alertify: AlertifyService,
    private modalService: NgbModal,
    private translate: TranslateService,
    private dataService: DataService,
    private route: ActivatedRoute,
    public env: EnvService,
    public service: Todolist2Service,
    private spinner: NgxSpinnerService
  ) {
    this.base = this.env.apiUrl.replace('/api/', '')
    this.base_download = this.env.apiUrl
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
    this.getKPIScoreDetailL2()
    this.getKPIScoreDetailGM()
    this.path = {
      saveUrl: `${this.env.apiUrl}UploadFile/SaveSpecialScore?campaignID=${this.campaignID}&scoreType=${this.type}`,
      removeUrl: `${this.env.apiUrl}UploadFile/removeSpecialScore?campaignID=${this.campaignID}&scoreType=${this.type}`
    }
    this.dropElement = document.getElementsByClassName('control_wrapper')[0] as HTMLElement;
    this.getSpecialType()
    this.getSpecialCompact()
    this.getSpecialScore()
    this.getSpecialRatio()
    this.loadKPIAttackment()
    this.getSpecialScoreDetailL1()
    this.getSpecialScoreDetailL2()

    if(this.pointDataGM.length === 0) {
      this.pointDataGM.unshift({ id: 0, value: this.translate.instant('NO_RECORDS_FOUND_KPI_DROPDOWN') });
    }
  }

  ngOnDestroy(): void {
    this.service.changeUploadMessage(false);
  }
  success(args) {
    this.service.changeUploadMessage(true);

  }
  getSpecialScore() {
    this.specialScoreService.getSpecialScore().subscribe(res => {
      this.scoreDataL1 = res
      this.scoreDataL2 = res
    })
  }
  downloadFile(item) {
    const file_open_brower = ['png', 'jpg','pdf']
    var ext =  item.name.split('.').pop();
    if(file_open_brower.includes(ext)) {
      window.open(this.base + item.path,'_blank')
    } else {
      const url = `${this.base_download}UploadFile/DownloadFileSpecialScore/${item.name}`
      this.attitudeScoreService.download(url).subscribe(data =>{
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
    this.kpiScoreService.getKPIScoreDetail(this.campaignID, this.userID ,'L0').subscribe((res: any) => {
      if(res !== null) {
        this.point = res.point
        this.comment = res.comment
      }
    })
  }

  getKPIScoreDetailL1() {
    this.kpiScoreService.getKPIScoreDetailL2L1(this.campaignID, this.userID , 'L1').subscribe((res: any) => {
      if(res !== null) {
        this.pointl1 = res.point
        this.commentl1 = res.comment
      }
    })
  }

  getKPIScoreDetailGM() {
    this.kpiScoreService.getKPIScoreDetailGM(this.campaignID, this.userID_System , this.userID , 'GM').subscribe((res: any) => {
      if(res !== null) {
        this.loadPoint(res.point)
        setTimeout(() => {
          this.pointGM = res.point
        }, 500);
        this.commentGM = res.comment
      }
    })
  }

  getKPIScoreDetailL2() {
    this.kpiScoreService.getKPIScoreDetail(this.campaignID, this.userID_System , 'L2').subscribe((res: any) => {
      if(res !== null) {
        this.pointl2 = res.point
        this.commentl2 = res.comment
      }
    })
  }

  getSpecialScoreDetailL1() {
    this.specialScoreService.getSpecialL1ScoreDetail(this.campaignID, this.userID , this.type).subscribe((res: any) => {
      if(res !== null) {
        this.subjectl1 = res.subject
        this.contentl1 = res.content
        this.ratiol1 = res.ratio
        this.typel1 = res.typeID
        this.compactl1 = res.compactID
        this.scorel1 = res.point
      }
    })
  }

  getSpecialScoreDetailL2() {
    this.specialScoreService.getSpecialScoreDetail(this.campaignID, this.userID_System , this.userID, 'L2').subscribe((res: any) => {
      if(res !== null) {
        this.contentl2 = res.content
        this.scorel2 = res.point
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
    var commentGM = this.checkEmptyOrNull(this.commentGM)
    if(this.pointGM === null) {
      this.alertify.error(this.translate.instant('POINT_VALIDATION_MESSAGE'))
      return false;
    }

    if(commentGM) {
      this.alertify.error(this.translate.instant('COMMENT_VALIDATION_MESSAGE'))
      return false;
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
      point: this.pointGM,
      comment: this.commentGM,
      campaignID: this.campaignID,
      ScoreBy: this.userID_System,
      ScoreFrom: this.userID_System,
      ScoreTo: this.userID,
      ScoreType: 'GM',
      IsSubmit: submit,
    }
    this.addKPIScore(modelKPIScore);
    
  }

  addKPIScore(model) {
    this.kpiScoreService.add(model).subscribe(res => {
      if(res) {
        this.alertify.success('Successfully')
        this.modalService.dismissAll();
        this.dataService.changeMessage('RefreshData')
        this.getKPIScoreDetailGM()

      }else {
        this.alertify.error('Error')
      }
     })
  }

  addSpecialScore(model) {
    this.specialScoreService.add(model).subscribe(res => {})
  }
  
  yesConfirm() {
    if (this.validation() == false) return;
    const modelKPIScore = {
      point: this.pointGM,
      comment: this.commentGM,
      campaignID: this.campaignID,
      ScoreBy: this.userID_System,
      ScoreFrom: this.userID_System,
      ScoreTo: this.userID,
      ScoreType: 'L2',
      IsSubmit: true,
      
    }
    this.addKPIScore(modelKPIScore);
  }

  getPoint(from, to) {
    this.dropDownListObject.value = null;
    this.attitudeScoreService.getPoint(from,to).subscribe(res => {
      this.pointDataGM = res
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
