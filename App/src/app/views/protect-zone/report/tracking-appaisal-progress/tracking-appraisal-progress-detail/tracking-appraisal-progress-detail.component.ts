import { DatePipe } from '@angular/common';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { TrackingProcessService } from 'src/app/_core/_service/tracking-process.service';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import { ExcelExportProperties, GridComponent } from '@syncfusion/ej2-angular-grids';
import { DataService } from 'src/app/_core/_service/data.service';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-tracking-appraisal-progress-detail',
  templateUrl: './tracking-appraisal-progress-detail.component.html',
  styleUrls: ['./tracking-appraisal-progress-detail.component.scss'],
  providers: [DatePipe]
})
export class TrackingAppraisalProgressDetailComponent implements OnInit {
  @Input() data: any;
  toolbarOptions = ['Search','ExcelExport'];
  dataDetail: any
  filterSettings = { type: 'Excel' };
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 15 };
  userId: number;
  @ViewChild('grid') public grid: GridComponent;
  constructor(
    private trackingProcessService: TrackingProcessService,
    private spinner: NgxSpinnerService,
    public activeModal: NgbActiveModal,
    private dataService: DataService,
    private translate: TranslateService,
    public modalService: NgbModal
  ) { }

  ngOnInit() {
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.getAll()
    this.dataService.locale.subscribe((res: any)=>{
      this.translate.addLangs([res])
      this.translate.use(res)
    })
  }
  toolbarClick = (args: ClickEventArgs) => { 
    switch (args.item.id) {
      case 'grid_excelexport':
        const excelExportProperties: ExcelExportProperties = {
          fileName: 'Tracking Appraisal Progress.xlsx'
        };
        this.grid.excelExport(excelExportProperties);
        // this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }
  getAll() {
    this.spinner.show()
    this.trackingProcessService.trackingAppraisalProgress(this.userId,this.data.id).subscribe(res => {
      this.dataDetail = res 
      this.spinner.hide()
    })
  }

}
