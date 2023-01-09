import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { AttitudeScoreService } from 'src/app/_core/_service/attitude-score.service';
import { DataService } from 'src/app/_core/_service/data.service';
import { KpiScoreService } from 'src/app/_core/_service/kpi-score.service';
import { PeopleCommitteeService } from 'src/app/_core/_service/people-committee.service';

@Component({
  selector: 'app-people-committee-kpi-detail',
  templateUrl: './people-committee-kpi-detail.component.html',
  styleUrls: ['./people-committee-kpi-detail.component.scss']
})
export class PeopleCommitteeKpiDetailComponent implements OnInit {
  dataDefaultPerson: any
  dataStringPerson: any
  dataDefaultMuti: any
  dataStringMuti: any
  userID: any;
  campaignID: any;
  type: any = 'L0';
  constructor(
    private attitudeScoreService: AttitudeScoreService,
    private route: ActivatedRoute,
    private service: PeopleCommitteeService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.userID = this.route.snapshot.params.appraiseeID
    this.campaignID = this.route.snapshot.params.campaignID
    this.getAsyncData()
  }
  async getAsyncData() {
    this.spinner.show()
    await this.loadDataPerson()
    await this.loadDataStringPerson()
    await this.loadDataMuti()
    await this.loadDataStringMuti()
    this.spinner.hide()
  }
  loadDataPerson() {
    return new Promise((res, rej) => {
      this.service.getKPIDefaultPerson(this.campaignID,this.userID).subscribe(
        (result: any) => {
          this.dataDefaultPerson = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadDataStringPerson() {
    return new Promise((res, rej) => {
      this.service.getKPIStringPerson(this.campaignID,this.userID).subscribe(
        (result: any) => {
          this.dataStringPerson = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadDataMuti() {
    return new Promise((res, rej) => {
      this.service.getKPIDefaultMuti(this.campaignID,this.userID).subscribe(
        (result: any) => {
          this.dataDefaultMuti = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadDataStringMuti() {
    return new Promise((res, rej) => {
      this.service.getKPIStringMuti(this.campaignID,this.userID).subscribe(
        (result: any) => {
          this.dataStringMuti = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }
}
