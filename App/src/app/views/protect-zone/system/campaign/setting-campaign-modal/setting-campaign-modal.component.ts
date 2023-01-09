import { KeyPointModalComponent } from './key-point-modal/key-point-modal.component';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { AttitudeHeadingService } from 'src/app/_core/_service/attitude-heading.service';
import { AttitudeScoreService } from 'src/app/_core/_service/attitude-score.service';

@Component({
  selector: 'app-setting-campaign-modal',
  templateUrl: './setting-campaign-modal.component.html',
  styleUrls: ['./setting-campaign-modal.component.scss']
})
export class SettingCampaignModalComponent extends BaseComponent implements OnInit {
  @Input() title: string;
  @Input() campaignId: number;
  attitudeScore: any;
  data: any = [];
  @ViewChild('grid') grid: GridComponent;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  filterSettings = { type: 'Excel' };
  attitudeHeadings: any[] = [];
  attitudeHeadingId: number = 0;
  fieldsAttitudeHeading: object = { text: 'name', value: 'id'};
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  toolbarOptions = ['Add', 'Update', 'Delete', 'Cancel', 'Search'];
  constructor(
    private attitudeHeadingService: AttitudeHeadingService,
    private attitudeScoreService: AttitudeScoreService,
    private alertify: AlertifyService,
    private modalService: NgbModal,
    private route: ActivatedRoute,
    public activeModal: NgbActiveModal,
  ) { super(); }

  ngOnInit() {
    this.attitudeScore = {
    };
    this.getAll();
    this.getAllAttitudeHeadings();
  }
  getAll() {
    this.attitudeScoreService.getAllByCampaign(this.campaignId).subscribe(res => {
      this.data = res;
    });
  }
  onChangeAttitudeHeadings(args) {
    this.attitudeHeadingId = args.itemData.id;
  }
  getAllAttitudeHeadings() {
    this.attitudeHeadingService.getAll().subscribe(res => {
      this.attitudeHeadings = res
    })
  }

  create() {
    this.attitudeScoreService.addAttitudeScore(this.attitudeScore).subscribe((res: any) => {
      if (res.status) {
        this.alertify.success(res.message);
      } else {
        this.alertify.error(res.message);
      }
      this.getAll();
      this.attitudeScore = {
      };
    });
    this.attitudeHeadingId = 0;
  }

  update() {
    this.attitudeScoreService.updateAttitudeScore(this.attitudeScore).subscribe((res: any) => {
      if (res.status) {
        this.alertify.success(res.message);
      } else {
        this.alertify.error(res.message);
      }
      this.getAll();
      this.attitudeScore = {
      };
    });
    this.attitudeHeadingId = 0;
  }
  delete(id) {
    this.alertify.delete("Delete attitude score",'Are you sure you want to delete this attitude score "' + id + '" ?')
    .then((result) => {
      if (result) {
        this.attitudeScoreService.deleteAttitudeScore(id).subscribe(() => {
          this.getAll();
          this.alertify.success(MessageConstants.DELETED_OK_MSG);
        }, error => {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        });
      }
    })
    .catch((err) => {
      this.getAll();
      this.grid.refresh();
    });
  }
  toolbarClick(args): void {
    switch (args.item.text) {
      case 'Excel Export':
        this.grid.excelExport();
        break;
      default:
        break;
    }
  }

  actionBegin(args) {
    if (args.requestType === "beginEdit") {
      this.attitudeScore.id = args.rowData.id;
      this.attitudeScore.attitudeHeadingID = args.rowData.attitudeHeadingID;
      this.attitudeScore.campaignID = args.rowData.campaignID;
      this.attitudeScore.comment = args.rowData.comment;
      this.attitudeScore.score = args.rowData.score;
      this.attitudeScore.scoreBy = args.rowData.scoreBy;
      this.attitudeScore.scoreTime = args.rowData.scoreTime;
    }
    if (args.requestType === 'save') {
      if (args.action === 'add') {
        if (this.attitudeHeadingId > 0) {
          this.attitudeScore.id = 0;
          this.attitudeScore.attitudeHeadingID = this.attitudeHeadingId;
          this.attitudeScore.campaignID = this.campaignId;
          this.create();
        }
        else{
          this.getAll();
        }
      }
      if (args.action === 'edit') {
        if (this.attitudeHeadingId > 0 && this.attitudeHeadingId != args.previousData.attitudeHeadingID) {
          this.attitudeScore.id = args.data.id;
          this.attitudeScore.attitudeHeadingID = this.attitudeHeadingId;
          this.attitudeScore.campaignID = this.campaignId;
          this.update();
        }
      }
    }
    if (args.requestType === 'delete') {
      this.alertify.error('Can not delete this attitude score', true);
      this.delete(args.data[0].id);
    }
  }

  openKeypointModal(data: any) {
    const modalRef = this.modalService.open(KeyPointModalComponent, {
      size: "xl",
    });
    modalRef.componentInstance.title = " Heading name: " + data.attitudeHeadingName;
    modalRef.componentInstance.attitudeHeadingId = data.attitudeHeadingID;
    modalRef.componentInstance.campaignId = data.campaignID;
    modalRef.result.then(
      (result) => {},
      (reason) => {}
    );
  }

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
