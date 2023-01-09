import { filter } from 'rxjs/operators';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EditService, ToolbarService, PageService, GridComponent } from '@syncfusion/ej2-angular-grids';
import { ModalDismissReasons, NgbModal, NgbModalRef, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { Account } from 'src/app/_core/_model/account';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { Authv2Service } from 'src/app/_core/_service/authv2.service';
import { TranslateService } from '@ngx-translate/core';
import { PeopleCommitteeService } from 'src/app/_core/_service/people-committee.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-people-committee-modal',
  templateUrl: './people-committee-modal.component.html',
  styleUrls: ['./people-committee-modal.component.scss'],
  providers: [ToolbarService, EditService, PageService]
})
export class PeopleCommitteeModalComponent implements OnInit {
  @Input() data: any;
  dataAc: Account[] = [];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  toolbarOptions = [];
  locale = localStorage.getItem('lang');
  name: string;
  frozen: boolean;

  constructor(
    private service: PeopleCommitteeService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private router: Router,
    private translate: TranslateService,
    private spinner: NgxSpinnerService,
    public activeModal: NgbActiveModal,
    public modalService: NgbModal,
  ) { }

  ngOnInit() {
    this.spinner.show();
    this.loadFrozenBtn();
    this.loadData();
  }

  loadData() {
    this.service.getAll(this.locale, this.data.id).subscribe(res => {
      this.dataAc = res;
      this.spinner.hide()
    });
  }

  loadFrozenBtn() {
    this.service.getFrozen(this.data.id).subscribe(res => {
      this.frozen = res;
    });
  }

  lockUpdate() {
    this.service.lockUpdate(this.data.id).subscribe((res: any) => {
      if (res.success === true) {
        this.loadFrozenBtn();
        this.alertify.success(this.translate.instant(res.message))
      } else {
        this.loadFrozenBtn();
        this.alertify.error(this.translate.instant(res.message))
      }
    });
  }

  clickDetail(dataAc) {
    window.open(`#${this.router.url}/people-committee-detail/${dataAc.appraiseeID}/${this.data.id}`,'_blank')
  }

  search(args) {
    this.grid.search(this.name)
  }

  rowDrop(e){
    setTimeout(() => {
      const model = {
        AppraiseeID: e.data[0].appraiseeID,
        FromIndex: e.fromIndex + 1,
        ToIndex: e.dropIndex + 1,
        CampaignID: this.data.id
      }
      this.service.updateCommitteeSequence(model).subscribe(res => {
        this.loadData();
      })
    }, 300);

  }

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
