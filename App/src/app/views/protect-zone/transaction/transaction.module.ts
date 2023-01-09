import { CheckboxHeightDirective } from './../../../_core/_directive/checkbox-height.directive';
import { PeopleCommitteeKpiDetailComponent } from './people-committee/people-committee-kpi-detail/people-committee-kpi-detail.component';
import { PeopleCommitteeComponent } from './people-committee/people-committee.component';
import { GmSelfScoreComponent } from './perfomance-evaluation/gm-self-score/gm-self-score.component';
import { L2DownloadFileComponent } from './perfomance-evaluation/l2-self-score/l2-download-file/l2-download-file.component';
import { L2SelfScoreComponent } from './perfomance-evaluation/l2-self-score/l2-self-score.component';
import { L1SelfScoreComponent } from './perfomance-evaluation/l1-self-score/l1-self-score.component';
import { SelfScoreComponent } from './perfomance-evaluation/self-score/self-score.component';
import { ScoreDetailComponent } from './score-attitude/score-detail/score-detail.component';
import { ScoreUploadFileComponent } from './score-attitude/score-upload-file/score-upload-file.component';
import { ScoreAttitudeComponent } from './score-attitude/score-attitude.component';
import { PerfomanceEvaluationComponent } from './perfomance-evaluation/perfomance-evaluation.component';
import { PdcaMeetingComponent } from './meeting-revise/pdcaMeeting/pdcaMeeting.component';
import { PdcaStringTypeMeetingComponent } from './meeting-revise/pdcaStringTypeMeeting/pdcaStringTypeMeeting.component';
import { MeetingReviseComponent } from './meeting-revise/meeting-revise.component';
import { CHMComponent } from './CHM/CHM.component';
import { CustomLoader } from './../../../_core/_helper/customLoader';
import { FluidHeightDirective } from './../../../_core/_directive/fluid-height.directive';
import { PdcaStringTypeComponent } from './todolist2/pdcaStringType/pdcaStringType.component';
import { PlanStringTypeComponent } from './todolist2/planStringType/planStringType.component';
import { MeetingComponent } from './meeting/meeting.component';
import { UploadFileComponent } from './todolist2/upload-file/upload-file.component';
import { PdcaComponent } from './todolist2/pdca/pdca.component';
import { PlanComponent } from './todolist2/plan/plan.component';
import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { CheckBoxAllModule, SwitchModule } from '@syncfusion/ej2-angular-buttons';
import { DropDownListModule,  MultiSelectAllModule } from '@syncfusion/ej2-angular-dropdowns';
import { GridAllModule } from '@syncfusion/ej2-angular-grids';
import { L10n, loadCldr, setCulture } from '@syncfusion/ej2-base';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';
import { TransactionRoutingModule } from './transaction.routing.module';
import { TabModule, ToolbarModule } from '@syncfusion/ej2-angular-navigations';
import { TreeGridAllModule } from '@syncfusion/ej2-angular-treegrid';

import { SpreadsheetAllModule } from '@syncfusion/ej2-angular-spreadsheet';
import { NgTemplateNameDirective } from './ng-template-name.directive';
import { Todolist2Component } from './todolist2/todolist2.component';
import { UploaderModule } from '@syncfusion/ej2-angular-inputs';
import { NgxSpinnerModule } from 'ngx-spinner';
import { TooltipModule } from '@syncfusion/ej2-angular-popups';
import { TextareaAutosizeModule } from 'ngx-textarea-autosize';
import { TargetHeightDirective } from 'src/app/_core/_directive/target-height.directive';
import { TDHeightDirective } from 'src/app/_core/_directive/td-height.directive';
import { ScoreReviseStationComponent } from './score-revise-station/score-revise-station.component';
import { DialogAllModule } from '@syncfusion/ej2-angular-popups';
import { PeopleCommitteeDetailComponent } from './people-committee/people-committee-detail/people-committee-detail.component';
import { PeopleCommitteeModalComponent } from './people-committee/people-committee-modal/people-committee-modal.component';
import { NewAttitudeComponent } from './new-attitude/new-attitude.component';
import { L0ScoreAttitudeComponent } from './new-attitude/l0-score-attitude/l0-score-attitude.component';
import { L1ScoreAttitudeComponent } from './new-attitude/l1-score-attitude/l1-score-attitude.component';
import { L2ScoreAttitudeComponent } from './new-attitude/l2-score-attitude/l2-score-attitude.component';
import { FlScoreAttitudeComponent } from './new-attitude/fl-score-attitude/fl-score-attitude.component';
import { NewAttitudeDetailModalComponent } from './new-attitude/new-attitude-detail-modal/new-attitude-detail-modal.component';
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
  } else if (lang === 'zh'){
    defaultLang = 'zh';
  }

@NgModule({
  declarations: [
    Todolist2Component,
    PeopleCommitteeComponent,
    MeetingComponent,
    NgTemplateNameDirective,
    CHMComponent,
    MeetingReviseComponent,
    PlanComponent,
    FluidHeightDirective,
    CheckboxHeightDirective,
    TargetHeightDirective,
    TDHeightDirective,
    PlanStringTypeComponent,
    PdcaComponent,
    PdcaStringTypeComponent,
    PdcaStringTypeMeetingComponent,
    PdcaMeetingComponent,
    UploadFileComponent,
    PerfomanceEvaluationComponent,
    ScoreAttitudeComponent,
    ScoreUploadFileComponent,
    ScoreDetailComponent,
    SelfScoreComponent,
    L1SelfScoreComponent,
    L2SelfScoreComponent,
    L2DownloadFileComponent,
    GmSelfScoreComponent,
    ScoreReviseStationComponent,
    PeopleCommitteeDetailComponent,
    PeopleCommitteeModalComponent,
    PeopleCommitteeKpiDetailComponent,
    NewAttitudeComponent,
    L0ScoreAttitudeComponent,
    L1ScoreAttitudeComponent,
    L2ScoreAttitudeComponent,
    FlScoreAttitudeComponent,
    NewAttitudeDetailModalComponent
  ],
  imports: [
    NgbModule,
    DialogAllModule,
    TextareaAutosizeModule,
    CommonModule,
    TooltipModule,
    FormsModule,
    ReactiveFormsModule,
    DropDownListModule,
    GridAllModule,
    TreeGridAllModule,
    CheckBoxAllModule,
    SwitchModule,
    TransactionRoutingModule,
    DateInputsModule ,
    ToolbarModule,
    MultiSelectAllModule,
    DatePickerModule,
    TabModule,
    SpreadsheetAllModule,
    UploaderModule,
    NgxSpinnerModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useClass: CustomLoader,
        deps: [HttpClient]
      },
      defaultLanguage: defaultLang
    }),
  ]
})
export class TransactionModule{
  vi: any;
  en: any;
  constructor(public translate: TranslateService) {
    translate.addLangs(['en', 'zh', 'vi']);
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
    translate.use(defaultLang);
  }
}

