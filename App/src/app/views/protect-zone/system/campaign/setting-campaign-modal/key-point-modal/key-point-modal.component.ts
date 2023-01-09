import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { AttitudeHeadingService } from 'src/app/_core/_service/attitude-heading.service';
import { AttitudeKeypointService } from 'src/app/_core/_service/attitude-keypoint.service';

@Component({
  selector: 'app-key-point-modal',
  templateUrl: './key-point-modal.component.html',
  styleUrls: ['./key-point-modal.component.scss']
})
export class KeyPointModalComponent extends BaseComponent implements OnInit {
  @Input() title: string;
  @Input() attitudeHeadingId: number;
  @Input() campaignId: number;
  editing: boolean = false;
  attitudeCategory: any = { attitudeKeypoint: {}};
  data: any = [];
  @ViewChild('grid') grid: GridComponent;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  filterSettings = { type: 'Excel' };
  attitudeHeadingName: string;
  attitudeCategories: any[] = [{name: "We expect team members to"}, {name: "We don’t expect team members to"}];
  attitudeTest: any[] = ["We expect team members to", "We don’t expect team members to"];
  fieldsAttitudeCategory: object = { text: 'name', value: 'name'};
  attitudeCategoryName: string = '';
  constructor(
    private attitudeHeadingService: AttitudeHeadingService,
    private attitudeKeypointService: AttitudeKeypointService,
    private alertify: AlertifyService,
    private modalService: NgbModal,
    private route: ActivatedRoute,
    public activeModal: NgbActiveModal,
  ) { super(); }

  ngOnInit() {

    this.attitudeCategory = {
      attitudeKeypoint: {
      }
    };
    this.getAll();
  }
  getAll() {
    this.attitudeKeypointService.getAllByAttitudeScore(this.attitudeHeadingId, this.campaignId).subscribe(res => {
      this.data = res;
    });
  }
  onChangeAttitudeCategories(args) {
    this.attitudeCategoryName = args.itemData.value;
  }

  create() {
    this.attitudeKeypointService.addAttitudeKeypoint(this.attitudeCategory).subscribe((res: any) => {
      if (res.status) {
        this.alertify.success(res.message);
      } else {
        this.alertify.error(res.message);
      }
      this.getAll();
      this.attitudeCategory = {
      };
    });
    this.attitudeCategoryName = '';
  }

  // update() {
  //   this.attitudeScoreService.updateAttitudeScore(this.attitudeScore).subscribe((res: any) => {
  //     if (res.status) {
  //       this.alertify.success(res.message);
  //     } else {
  //       this.alertify.error(res.message);
  //     }
  //     this.getAll();
  //     this.attitudeScore = {
  //     };
  //   });
  //   this.attitudeHeadingId = 0;
  // }
  // delete(id) {
  //   this.alertify.delete("Delete attitude score",'Are you sure you want to delete this attitude score "' + id + '" ?')
  //   .then((result) => {
  //     if (result) {
  //       this.attitudeScoreService.deleteAttitudeScore(id).subscribe(() => {
  //         this.getAll();
  //         this.alertify.success(MessageConstants.DELETED_OK_MSG);
  //       }, error => {
  //         this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
  //       });
  //     }
  //   })
  //   .catch((err) => {
  //     this.getAll();
  //     this.grid.refresh();
  //   });
  // }
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
      this.editing = true;

      // this.attitudeKeypoint.id = args.rowData.id;
      // this.attitudeKeypoint.attitudeHeadingID = args.rowData.attitudeHeadingID;
      // this.attitudeKeypoint.campaignID = args.rowData.campaignID;
      // this.attitudeKeypoint.comment = args.rowData.comment;
      // this.attitudeKeypoint.score = args.rowData.score;
      // this.attitudeKeypoint.scoreBy = args.rowData.scoreBy;
      // this.attitudeKeypoint.scoreTime = args.rowData.scoreTime;
    }
    if (args.requestType === 'save') {
      if (args.action === 'add') {
        if (this.attitudeHeadingId > 0) {
          // this.attitudeKeypoint.id = 0;
          // this.attitudeKeypoint.attitudeHeadingID = this.attitudeHeadingId;
          // this.attitudeKeypoint.campaignID = this.campaignId;
          //this.create();
        }
        else{
          this.getAll();
        }

        if (this.attitudeCategoryName === '') {
          this.attitudeCategoryName = "We expect team members to";
        }
        this.attitudeCategory.id = 0;
        this.attitudeCategory.name = this.attitudeCategoryName;
        this.attitudeCategory.attitudeHeadingID = this.attitudeHeadingId;
        this.attitudeCategory.campaignID = this.campaignId;
        this.attitudeCategory.attitudeKeypoint.id = 0;
        this.attitudeCategory.attitudeKeypoint.name = args.data.name;
        this.attitudeCategory.attitudeKeypoint.level = args.data.level;
        this.attitudeCategory.attitudeKeypoint.attitudeHeadingID = this.attitudeHeadingId;
        this.create();
      }
      if (args.action === 'edit') {
        if (this.attitudeHeadingId > 0 && this.attitudeHeadingId != args.previousData.attitudeHeadingID) {
          // this.attitudeKeypoint.id = args.data.id;
          // this.attitudeKeypoint.attitudeHeadingID = this.attitudeHeadingId;
          // this.attitudeKeypoint.campaignID = this.campaignId;
          //this.update();
        }
      }

    }
    if (args.requestType === 'delete') {
      this.alertify.error('Can not delete this attitude score', true);
      //this.delete(args.data[0].id);
    }
  }

  actionComplete(args) {
    if (args.requestType === 'add') {
      this.editing = false;
    }
    if (args.requestType === 'beginEdit') {
      this.editing = true;
    }
    if (args.requestType === 'cancel') {
      this.editing = false;
    }

  }

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
