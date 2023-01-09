import { SettingCampaignModalComponent } from './setting-campaign-modal/setting-campaign-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { SettingCampaignService } from 'src/app/_core/_service/setting-campaign.service';

@Component({
  selector: 'app-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.scss']
})
export class CampaignComponent extends BaseComponent implements OnInit {
  campaign: any;
  data: any = [];
  @ViewChild('grid') grid: GridComponent;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  filterSettings = { type: 'Excel' };
  toolbarOptions = ['Search'];
  constructor(
    private settingCampaignService: SettingCampaignService,
    private alertify: AlertifyService,
    private modalService: NgbModal,
    private route: ActivatedRoute,
  ) { super(); }

  ngOnInit() {
    this.campaign = {
    };
    this.getAll();
  }
  getAll() {
    this.settingCampaignService.getAll().subscribe(res => {
      this.data = res;
    });
  }

  create() {
    this.settingCampaignService.addCampaign(this.campaign).subscribe(() => {
      this.alertify.success(MessageConstants.CREATED_OK_MSG);
      this.getAll();
      this.campaign = {
      };
    });
  }

  update() {
    this.settingCampaignService.updateCampaign(this.campaign).subscribe(() => {
      this.alertify.success(MessageConstants.UPDATED_OK_MSG);
      this.getAll();
      this.campaign = {
      };
    });
  }
  delete(id) {
    this.alertify.delete("Delete campaign",'Are you sure you want to delete this campaign "' + id + '" ?')
    .then((result) => {
      if (result) {
        this.settingCampaignService.delete(id).subscribe(() => {
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
    if (args.requestType === 'save') {
      if (args.action === 'add') {
        this.campaign.id = 0;
        this.campaign.name = args.data.name;
        this.create();
      }
      if (args.action === 'edit') {
        this.campaign.id = args.data.id;
        this.campaign.name = args.data.name;
        this.update();
      }
    }
    if (args.requestType === 'delete') {
      this.alertify.error('Can not delete this campaign', true);
      this.delete(args.data[0].id);
    }
  }
  actionComplete(e: any): void {
    if (e.requestType === 'add') {
      (e.form.elements.namedItem('name') as HTMLInputElement).focus();
    }
  }
  openSettingCampaignModal(data: any) {
    const modalRef = this.modalService.open(SettingCampaignModalComponent, {
      size: "lg",
    });
    modalRef.componentInstance.title = " Setting campaign: " + data.name;
    modalRef.componentInstance.campaignId = data.id;
    modalRef.result.then(
      (result) => {},
      (reason) => {}
    );
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
