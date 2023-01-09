import { KpiScoreService } from './../../../../../_core/_service/kpi-score.service';
import { Component, OnInit, ViewChild, } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { DataService } from 'src/app/_core/_service/data.service';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AttitudeScoreService } from 'src/app/_core/_service/attitude-score.service';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';

@Component({
  selector: 'app-self-score',
  templateUrl: './self-score.component.html',
  styleUrls: ['./self-score.component.scss']
})
export class SelfScoreComponent implements OnInit {
  campaignID: any;
  type: any = 'L0';
  userID: any;
  dataDefault: any
  dataString: any
  point: string = null
  comment: string = null
  pointFields: object = { text: 'value', value: 'value' };
  pointData: any = [] 
  @ViewChild('ddlelement')
  public dropDownListObject: DropDownListComponent;
  userID_System: number;
  @ViewChild('message') message: NgbModalRef;
  isSubmit: boolean = false;
  constructor(
    private attitudeScoreService: AttitudeScoreService,
    private kpiScoreService: KpiScoreService,
    private alertify: AlertifyService,
    private modalService: NgbModal,
    private translate: TranslateService,
    private route: ActivatedRoute,
    private dataService: DataService,
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
    
    this.loadData()
    this.loadDataString()
    this.getKPIScoreDetail()
    if(this.pointData.length === 0) {
      this.pointData.unshift({ id: 0, value: this.translate.instant('NO_RECORDS_FOUND_KPI_DROPDOWN') });
    }
  }

  getKPIScoreDetail() {
    this.kpiScoreService.getKPIScoreDetail(this.campaignID, this.userID_System , this.type).subscribe((res: any) => {
      if(res !== null) {
        this.loadPoint(res.point)
        setTimeout(() => {
          this.point = res.point
        }, 500);
        this.comment = res.comment
        this.isSubmit = res.isSubmit
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
    var comment = this.checkEmptyOrNull(this.comment)
    if(this.point === null) {
      this.alertify.error(this.translate.instant('POINT_VALIDATION_MESSAGE'))
      return false;
    }

    if(comment) {
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
    const model = {
      point: this.point,
      comment: this.comment,
      campaignID: this.campaignID,
      ScoreBy: this.userID_System,
      ScoreFrom: this.userID_System,
      ScoreTo: this.userID,
      ScoreType: this.type,
      IsSubmit: submit
    }
    this.kpiScoreService.add(model).subscribe(res => {
      if(res) {
        this.alertify.success('Successfully')
        this.getKPIScoreDetail()
      }
      else  
        this.alertify.error('Error')
    })
  }
  
  yesConfirm() {
    if (this.validation() == false) return;
    const model = {
      point: this.point,
      comment: this.comment,
      campaignID: this.campaignID,
      ScoreBy: this.userID_System,
      ScoreFrom: this.userID_System,
      ScoreTo: this.userID,
      ScoreType: this.type,
      IsSubmit: true
    }
    this.kpiScoreService.add(model).subscribe(res => {
      if(res) {
        this.alertify.success('Successfully')
        this.getKPIScoreDetail()
        this.modalService.dismissAll();
      }
      else  
        this.alertify.error('Error')
    })
  }

  getPoint(from, to) {
    this.dropDownListObject.value = null;
    this.attitudeScoreService.getPoint(from,to).subscribe(res => {
      this.pointData = res
    })
  }

  loadData() {
    this.spinner.show()
    this.attitudeScoreService.getKPISelfScoreDefault(this.campaignID,this.userID_System,this.type).subscribe(res => {
      console.log(res);
      this.dataDefault = res
      this.spinner.hide();
    })
  }

  loadDataString() {
    this.spinner.show()
    this.attitudeScoreService.getKPISelfScoreString(this.campaignID,this.userID_System,this.type).subscribe(res => {
      this.dataString = res
      this.spinner.hide();
    })
  }

}
