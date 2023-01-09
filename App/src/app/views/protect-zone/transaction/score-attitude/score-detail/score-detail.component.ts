import { Component, HostListener, Input, OnInit, QueryList, TemplateRef, ViewChild, ViewChildren } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { SystemCode_Heading } from 'src/app/_core/enum/system';
import { DataService } from 'src/app/_core/_service/data.service';
import { TranslateService } from '@ngx-translate/core';
import { AttitudeScoreService } from 'src/app/_core/_service/attitude-score.service';
import { NgTemplateNameDirective } from '../../ng-template-name.directive';
import { EnvService } from 'src/app/_core/_service/env.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-score-detail',
  templateUrl: './score-detail.component.html',
  styleUrls: ['./score-detail.component.scss']
})
export class ScoreDetailComponent implements OnInit {
  @Input() data: any;
  gridData: any

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
    score: 0,
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
  }
  score: number = 0
  L0: boolean = false
  L1: boolean = false
  L2: boolean = false
  FL: boolean = false
  changeScore: boolean = false
  changeComment: boolean = false
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
  public tabItemsHeaderText: Object = [
    { 'text': 'DatePicker' }
  ];
  dataRemove: any = [];
  changeLocalHome = [];
  @ViewChild('message') message: NgbModalRef;
  filesLeft = [];
  filesRight = [];
  base_download: any
  base: any
  campaignID: any;
  type: any;
  userId_System: number;
  userIdFL: any;
  userIdL0: any;
  userIdL1: any;
  userIdL2: any;
  changeHeghtCheckbox = [];
  numbers: number[];
  constructor(
    private attitudeScoreService: AttitudeScoreService,
    private alertify: AlertifyService,
    private dataService: DataService,
    public modalService: NgbModal,
    private translate: TranslateService,
    private spinner: NgxSpinnerService,
    private route: ActivatedRoute,
    private env: EnvService
  ) {
    this.numbers = Array(5).fill(1).map((x, i) => i + 1); 
    this.base = this.env.apiUrl.replace('/api/', '')
    this.base_download = this.env.apiUrl
  }

  ngOnInit() {
    this.userId_System = Number(JSON.parse(localStorage.getItem('user')).id);
    this.campaignID = this.route.snapshot.params.campaignID
    this.type = this.route.snapshot.params.Type
    this.userId = this.route.snapshot.params.userID
    this.userIdFL = this.route.snapshot.params.FlID
    this.userIdL0 = this.route.snapshot.params.L0ID
    this.userIdL1 = this.route.snapshot.params.L1ID
    this.userIdL2 = this.route.snapshot.params.L2ID
    this.getAsyncData()
    this.dataService.locale.subscribe((res: any)=>{
      this.translate.addLangs([res])
      this.translate.use(res)
    })

  }
  downloadFile(item) {
    const file_open_brower = ['png', 'jpg','pdf']
    var ext =  item.name.split('.').pop();
    if(file_open_brower.includes(ext)) {
      window.open(this.base + item.path,'_blank')
    } else {
      const url = `${this.base_download}UploadFile/DownloadFileScore/${item.name}`
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

  ngOnDestroy() {
    this.dataRemove = []
    this.changeLocalHome.forEach(item => item.unsubscribe());
    this.changeHeghtCheckbox.forEach(item => item.unsubscribe());

  }
  getGridTemplate(name): TemplateRef<any> {
    const dir = this.Gridtemplates.find(dir => dir.name === name + '');
    return dir ? dir.template : null
  }
  selected(args) {

  }
  async getAsyncData(){
    // await this.getAll()
    await this.getDetailPassion()
    // await this.getListCheckBehavior()
  }

  getDetailPassion() {
    this.spinner.show()
    return new Promise((res, rej) => {
      this.attitudeScoreService.getDetailPassion(this.campaignID,this.userIdFL, this.userIdL0,this.userIdL1, this.userIdL2, this.type).subscribe(
        (result: any) => {
          this.passionData = result
          const data = result
          this.changeHeghtCheckbox.push(this.dataService.currentMessagesCheckbox.subscribe((res: any)=>{
            if(res === 0)
              return
            if(res.value > 0 || res.value !== undefined)
              for (let item of data.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }
          }))
          setTimeout(() => {
            this.spinner.hide()
          }, 500);
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getDetailAccountbility() {
    this.spinner.show()
    return new Promise((res, rej) => {
      this.attitudeScoreService.getDetailAccountbility(this.campaignID,this.userIdFL, this.userIdL0,this.userIdL1, this.userIdL2, this.type).subscribe(
        (result: any) => {
          this.accountbilityData = result
          const data = result
          this.changeHeghtCheckbox.push(this.dataService.currentMessagesCheckbox.subscribe((res: any)=>{
            if(res === 0)
              return
            if(res.value > 0 || res.value !== undefined)
              for (let item of data.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }
          }))
          setTimeout(() => {
            this.spinner.hide()
          }, 500);
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getDetailAttention() {
    this.spinner.show()
    return new Promise((res, rej) => {
      this.attitudeScoreService.getDetailAttention(this.campaignID,this.userIdFL, this.userIdL0,this.userIdL1, this.userIdL2, this.type).subscribe(
        (result: any) => {
          this.attentionData = result
          const data = result
          this.changeHeghtCheckbox.push(this.dataService.currentMessagesCheckbox.subscribe((res: any)=>{
            if(res === 0)
              return
            if(res.value > 0 || res.value !== undefined)
              for (let item of data.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }
          }))
          setTimeout(() => {
            this.spinner.hide()
          }, 500);
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getDetailContinuous() {
    this.spinner.show()
    return new Promise((res, rej) => {
      this.attitudeScoreService.getDetailContinuous(this.campaignID,this.userIdFL, this.userIdL0,this.userIdL1, this.userIdL2, this.type).subscribe(
        (result: any) => {
          const data = result
          this.changeHeghtCheckbox.push(this.dataService.currentMessagesCheckbox.subscribe((res: any)=>{
            if(res === 0)
              return
            if(res.value > 0 || res.value !== undefined)
              for (let item of data.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                  }
                }
              }
          }))
          this.continuousData = data
          setTimeout(() => {
            this.spinner.hide()
          }, 500);
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

 
  getDetailEffective() {
    this.spinner.show()
    return new Promise((res, rej) => {
      this.attitudeScoreService.getDetailEffective(this.campaignID,this.userIdFL, this.userIdL0,this.userIdL1, this.userIdL2, this.type).subscribe(
        (result: any) => {
          this.effectiveData = result
          const data = result
          this.changeHeghtCheckbox.push(this.dataService.currentMessagesCheckbox.subscribe((res: any)=>{
            if(res === 0)
              return
            if(res.value > 0 || res.value !== undefined)
              for (let item of data.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }
          }))
          setTimeout(() => {
            this.spinner.hide()
          }, 500);
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  getDetailResilience() {
    this.spinner.show()
    return new Promise((res, rej) => {
      this.attitudeScoreService.getDetailResilience(this.campaignID,this.userIdFL, this.userIdL0,this.userIdL1, this.userIdL2, this.type).subscribe(
        (result: any) => {
          this.resilienceData = result
          const data = result
          this.changeHeghtCheckbox.push(this.dataService.currentMessagesCheckbox.subscribe((res: any)=>{
            if(res === 0)
              return
            if(res.value > 0 || res.value !== undefined)
              for (let item of data.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }
          }))
          setTimeout(() => {
            this.spinner.hide()
          }, 500);
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  public changeHandler(args,item_behavior, item_keypoint) {
    const checked = args.checked
    const data = {
      behaviorID: item_behavior.id,
      checked: checked,
      campaignID: this.campaignID,
    }
    if (this.listBehaviorCheck.length === 0) {
      this.listBehaviorCheck.push(data)
    } else {
      if(checked) {
        for (var i = 0; i < this.listBehaviorCheck.length; i++) {
          if (this.listBehaviorCheck[i].campaignID == this.campaignID && this.listBehaviorCheck[i].behaviorID == item_behavior.id) {
            this.listBehaviorCheck[i].checked = checked;
            break;
          } else {
            this.listBehaviorCheck.push(data)
            break;
          }
        }
      }else {
        for (var i = 0; i < this.listBehaviorCheck.length; i++) {
          if (this.listBehaviorCheck[i].campaignID == this.campaignID && this.listBehaviorCheck[i].behaviorID == item_behavior.id) {
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
  select(args) {
    const index = args.selectingIndex + 1
    switch (index) {
      case 1:
        if(this.passionData === undefined)
          this.getDetailPassion()
        break;
      case 2:
        if(this.accountbilityData === undefined)
          this.getDetailAccountbility()
        break;
      case 3:
        if(this.attentionData === undefined)
          this.getDetailAttention()
        break;
      case 4:
        if(this.effectiveData === undefined)
          this.getDetailEffective()
        break;
      case 5:
        if(this.resilienceData === undefined)
          this.getDetailResilience()
        break;
      case 6:
        if(this.continuousData === undefined)
          this.getDetailContinuous()
        break;
    
      default:
        break;
    }
    // this.getAll()
  }
  getAll() {
    this.spinner.show()
    return new Promise((res, rej) => {
      this.attitudeScoreService.getDetail(this.campaignID,this.userIdFL, this.userIdL0,this.userIdL1, this.userIdL2, this.type).subscribe(
        (result: any) => {
          this.passionData = result.passion
          this.accountbilityData = result.accountbility
          this.attentionData = result.attention
          this.continuousData = result.continuous
          this.effectiveData = result.effective
          this.resilienceData = result.resilience

          const passionData = result.passion
          const accountbilityData = result.accountbility
          const resilienceData = result.resilience
          const attentionData = result.attention
          const continuousData = result.continuous
          const effectiveData = result.effective
          this.changeHeghtCheckbox.push(this.dataService.currentMessagesCheckbox.subscribe((res: any)=>{
            if(res === 0)
              return
            if(res.value > 0 || res.value !== undefined)
              for (let item of passionData.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }

              for (let item of accountbilityData.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }

              for (let item of attentionData.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }

              for (let item of continuousData.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }

              for (let item of effectiveData.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }

              for (let item of resilienceData.data) {
                for (let items of item.keypoint) {
                  for (let itemss in items.behavior ) { 
                    let i = Number(itemss);
                    if(items.behavior[i].id === res.behaviorId) {
                      items.behavior[i].height = res.value
                      break;
                    }
                    
                  }
                }
              }
          }))
          this.spinner.hide()
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  onChangeScore(args) {
    this.changeScore = true
    this.dataAdd.score = args
  }

  onChangeComment(args) {
    this.changeComment = true
    this.dataAdd.comment = args
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
  saveScore(heading) {
    this.switchValue(heading)
    this.dataAdd.campaignID = this.campaignID
    this.dataAdd.ScoreBy = this.userId
    this.dataAdd.ScoreTo = this.data.userID
    this.dataAdd.type = this.type
    this.attitudeScoreService.saveScore(this.dataAdd).subscribe((res: any) => {
      if(res.success) {
        this.alertify.success('Successfully')
      }
    })
  }

  submit() {
    this.openVerticallyCentered(this.message)
  }
  openVerticallyCentered(content) {
    this.modalService.open(content, { centered: true });
  }
  yesConfirm() {
    this.alertify.success(this.translate.instant('SCORE_MESSAGE_SUCCESS'));
    this.modalService.dismissAll();
  }

}
