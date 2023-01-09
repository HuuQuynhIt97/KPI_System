import { TrackingAppraisalProgressDetailComponent } from './tracking-appaisal-progress/tracking-appraisal-progress-detail/tracking-appraisal-progress-detail.component';
import { TrackingAppaisalProgressComponent } from './tracking-appaisal-progress/tracking-appaisal-progress.component';
import { TrackingProcessComponent } from './tracking-process/tracking-process.component';
import { CustomLoader } from './../../../_core/_helper/customLoader';
import { GhrReportComponent } from './ghr-report/ghr-report.component';
import { HqHrReportComponent } from './hq-hr-report/hq-hr-report.component';
import { H1H2ReportComponent } from './h1-h2-report/h1-h2-report.component';
import { Q1Q3ReportComponent } from './q1-q3-report/q1-q3-report.component';
import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { CheckBoxAllModule, SwitchModule } from '@syncfusion/ej2-angular-buttons';
import { DropDownListModule,  MultiSelectAllModule } from '@syncfusion/ej2-angular-dropdowns';
import { GridAllModule } from '@syncfusion/ej2-angular-grids';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { L10n, loadCldr, setCulture } from '@syncfusion/ej2-base';
import { ReportRoutingModule } from '../report/report.routing.module';

import { AggregateService } from '@syncfusion/ej2-angular-grids';
import { HttpClient } from '@angular/common/http';
import { TabModule, ToolbarModule } from '@syncfusion/ej2-angular-navigations';

import { TreeGridAllModule } from '@syncfusion/ej2-angular-treegrid';

import { NgxSpinnerModule } from 'ngx-spinner';
import { SpreadsheetAllModule } from '@syncfusion/ej2-angular-spreadsheet';
import { TooltipAllModule } from '@syncfusion/ej2-angular-popups';
import { NgxPrettyCheckboxModule } from 'ngx-pretty-checkbox';
import { HqReportComponent } from './hq-report/hq-report.component';
import { HqReportModalComponent } from './hq-report/hq-report-modal/hq-report-modal.component';
import { CoreCompetenciesAnalysisComponent } from './core-competencies-analysis/core-competencies-analysis.component';
import { CoreCompetenciesAnalysisModalComponent } from './core-competencies-analysis/core-competencies-analysis-modal/core-competencies-analysis-modal.component';
import { KpiMonthPerfComponent } from './kpi-month-perf/kpi-month-perf.component';

declare var require: any;
let defaultLang: string;
const lang = localStorage.getItem('lang');
loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/en/ca-gregorian.json'),
  require('cldr-data/main/en/numbers.json'),
  require('cldr-data/main/en/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); // To load the culture based first day of week

loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/vi/ca-gregorian.json'),
  require('cldr-data/main/vi/numbers.json'),
  require('cldr-data/main/vi/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); // To load the culture based first day of week


  loadCldr(
    require('cldr-data/supplemental/numberingSystems.json'),
    require('cldr-data/main/zh/ca-gregorian.json'),
    require('cldr-data/main/zh/numbers.json'),
    require('cldr-data/main/zh/timeZoneNames.json'),
    require('cldr-data/supplemental/weekdata.json')); // To load the culture based first day of week

    if (lang === 'vi') {
      defaultLang = lang;
    } else if (lang === 'en') {
      defaultLang = 'en';
    } else {
      defaultLang = 'zh';
    }
@NgModule({
  imports: [
    NgxPrettyCheckboxModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    DropDownListModule,
    GridAllModule,
    TreeGridAllModule,
    CheckBoxAllModule,
    SwitchModule,
    ReportRoutingModule,
    DateInputsModule ,
    ToolbarModule,
    TooltipAllModule,
    MultiSelectAllModule,
    DatePickerModule,
    TabModule,
    SpreadsheetAllModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useClass: CustomLoader,
        deps: [HttpClient]
      },
      defaultLanguage: defaultLang
    }),
  ],
  declarations: [
    Q1Q3ReportComponent,
    H1H2ReportComponent,
    HqHrReportComponent,
    GhrReportComponent,
    TrackingProcessComponent,
    TrackingAppaisalProgressComponent,
    TrackingAppraisalProgressDetailComponent,
    HqReportComponent,
    HqReportModalComponent,
    KpiMonthPerfComponent,
    CoreCompetenciesAnalysisComponent,
    CoreCompetenciesAnalysisModalComponent

  ],
  providers: [AggregateService]
})
export class ReportModule {
  vi: any;
  en: any;
  constructor() {
    if (lang === 'vi') {
      defaultLang = 'vi';
      setTimeout(() => {
        L10n.load(require('../../../../assets/ej2-lang/vi.json'));
        setCulture('vi');
      });
    } else if (lang === 'en') {
      defaultLang = 'en';
      setTimeout(() => {
        L10n.load(require('../../../../assets/ej2-lang/en.json'));
        setCulture('en');
      });
    }else{
      defaultLang = 'zh';
      setTimeout(() => {
        L10n.load(require('../../../../assets/ej2-lang/zh.json'));
        setCulture('zh');
      });
    }
  }
}

