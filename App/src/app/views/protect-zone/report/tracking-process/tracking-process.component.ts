import { Workbook } from '@syncfusion/ej2-excel-export';
import { TrackingProcessService } from './../../../../_core/_service/tracking-process.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ExcelExportProperties, GridComponent } from '@syncfusion/ej2-angular-grids';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';

@Component({
  selector: 'app-tracking-process',
  templateUrl: './tracking-process.component.html',
  styleUrls: ['./tracking-process.component.scss'],
  providers: [DatePipe]
})
export class TrackingProcessComponent  implements OnInit {
  toolbarOptions = ['ExcelExport', 'Search'];
  dataTracking: any = [];
  dataKPI: any = [];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid1') public fGrid: GridComponent;
  @ViewChild('grid2') public sGrid: GridComponent;
  fieldsRoleType: object = { text: 'name', value: 'name' };
  filterSettings = { type: 'Excel' };
  currentTimeRequest: any;
  currentTime: any;
  userId: number;
  todoPending: any;
  public aggreagtes: Object;
  todoTotal: any;
  percentage: any;
  public objGrid: any = [];
  constructor(
    private trackingProcessService: TrackingProcessService,
    private spinner: NgxSpinnerService,
    private datePipe: DatePipe
  ) {  }

  ngOnInit() {
    // this.Permission(this.route);
    this.currentTime = new Date();
    this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    this.trackingProcess();
  }
  onChangeReportTime(args) {
    this.trackingProcess();
  }
  // api
  trackingProcess() {
    this.spinner.show();
    this.currentTimeRequest = this.datePipe.transform(this.currentTime, "yyyy-MM-dd");
    this.trackingProcessService.trackingProcess(this.currentTimeRequest,this.userId).subscribe((res: any) => {
      this.dataTracking = res.totalTracking
      this.todoPending = res.todoPending
      this.todoTotal = res.todoTotal
      this.percentage = res.percentage
      this.aggreagtes = [
        {
          columns: [
            {
              type: 'Sum',
              field: 'fullName',
              footerTemplate: 'Total',
            },
            {
              type: 'Sum',
              field: 'todoPending',
              footerTemplate: '${Sum}',
            },
            {
              type: 'Sum',
              field: 'todoTotal',
              footerTemplate: '${Sum}',
            },
            {
              type: 'Sum',
              field: 'percentage',
              footerTemplate: `${this.percentage}%`,
            },
          ],
        },
      ];
      this.trackingProcessKPI();
      this.spinner.hide()
    })
  }
  trackingProcessKPI() {
    this.currentTimeRequest = this.datePipe.transform(this.currentTime, "yyyy-MM-dd");
    this.trackingProcessService.trackingProcessKPI(this.currentTimeRequest,this.userId).subscribe((res: any) => {
      this.dataKPI = res
    })
  }

  toolbarClick = (args: ClickEventArgs) => {
    var names = ['Complete rate', 'Delay detail'];
    for (var i = 0; i < document.querySelectorAll('.e-grid').length; i++) {
      // you can find all grid controls using this.
      var grid = (
        document.getElementById(
          document.querySelectorAll('.e-grid')[i].id
        ) as any
      ).ej2_instances[0];
      this.objGrid.push(grid);
    }
    if (args.item.id === 'FirstGrid_excelexport') {
      // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
      var exportData;
      const appendExcelExportProperties: ExcelExportProperties = {
        multipleExport: { type: 'NewSheet' },
        //  header: {
        //     headerRows: 1,
        //     rows: [
        //         {
        //           cells: [
        //             {
        //              value: '中心',
        //           },
        //           {
        //             value: '部門',
        //           },
        //           {
        //             value: '名字',
        //           },
        //           {
        //             value: '尚未提交',
        //           },
        //           {
        //             value: '全部',
        //           },
        //           {
        //             value: '完成率',
        //           }
        //         ]
        //         },
                
        //     ]
        // },
      };
      if (this.objGrid.length > 1) {
        var firstGridExport = this.objGrid[0].excelExport(appendExcelExportProperties, true)
          .then(
            function (fData) {
              fData.worksheets[0].name = names[0];
              exportData = fData;
              for (var j = 1; j < this.objGrid.length - 1; j++) {
                // iterate grids here
                this.objGrid[j].excelExport(appendExcelExportProperties, true, exportData)
                  .then(function (wb) {
                    exportData = wb;
                    if (
                      exportData.worksheets.length === this.objGrid.length - 1
                    ) {
                      for (var k = 0; k < exportData.worksheets.length; k++) {
                        if (!exportData.worksheets[k].name) {
                          exportData.worksheets[k].name = names[k];
                        }
                      }
                    }
                  });
              }
              var lastGridExport = this.objGrid[this.objGrid.length - 1].excelExport(appendExcelExportProperties, true, exportData)
                .then(
                  function (wb) {
                    wb.worksheets[wb.worksheets.length - 1].name = names[this.objGrid.length - 1];
                    const book = new Workbook(wb, 'xlsx');
                    book.save('Tracking Progress.xlsx');
                    this.objGrid = [];
                  }.bind(this)
                );
            }.bind(this)
          );
      }
    }
  };

}
