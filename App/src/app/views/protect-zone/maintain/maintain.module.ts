import { StartCampaignComponent } from './start-campaign/start-campaign.component';
import { ReviseKpiComponent } from './../system/revise-kpi/revise-kpi.component';
import { KpiSequenceComponent } from './../system/kpi-sequence/kpi-sequence.component';
import { CustomLoader } from './../../../_core/_helper/customLoader';
import { ChangePasswordComponent } from './../transaction/change-password/change-password.component';
import { PrivilegeComponent } from './role/privilege/privilege.component';
import { RoleComponent } from './role/role.component';
import { OcComponent } from './../system/oc/oc.component';
import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { GridAllModule } from '@syncfusion/ej2-angular-grids'
import { DropDownListModule, MultiSelectAllModule, MultiSelectModule } from '@syncfusion/ej2-angular-dropdowns'
import {  CheckBoxModule, SwitchModule } from '@syncfusion/ej2-angular-buttons'
import { TranslateLoader, TranslateModule } from '@ngx-translate/core'
import { HttpClient } from '@angular/common/http'
import { L10n, loadCldr, setCulture } from '@syncfusion/ej2-base'
import { DateInputsModule } from '@progress/kendo-angular-dateinputs'
import { TreeGridAllModule } from '@syncfusion/ej2-angular-treegrid'
import { MaintainRoutingModule } from './maintain.routing.module';
import { ViewKPIComponent } from '../system/viewKPI/viewKPI.component';
import { Kpi2nd3rdComponent } from '../system/kpi2nd3rd/kpi2nd3rd.component';
import { KpiCreate2Component } from '../system/kpi-create2/kpi-create2.component';
import { KpiCreateComponent } from '../system/kpi-create/kpi-create.component';
import { SettingMonthlyComponent } from '../system/setting-monthly/setting-monthly.component';
import { TreeViewAllModule, TreeViewModule } from '@syncfusion/ej2-angular-navigations';
import { NgxSpinnerModule } from 'ngx-spinner';
import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';
import { CalendarModule } from '@progress/kendo-angular-dateinputs';
// import { PeriodComponent } from './period/period.component';
import { TooltipModule } from '@syncfusion/ej2-angular-popups';
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
  declarations: [
    OcComponent,
    RoleComponent,
    SettingMonthlyComponent,
    KpiCreateComponent,
    KpiCreate2Component,
    Kpi2nd3rdComponent,
    KpiSequenceComponent,
    ReviseKpiComponent,
    ViewKPIComponent,
    PrivilegeComponent,
    ChangePasswordComponent,
    StartCampaignComponent
  ],
  imports: [
    TooltipModule,
    CalendarModule,
    CommonModule,
    FormsModule,
    NgxSpinnerModule,
    ReactiveFormsModule,
    TreeGridAllModule,
    MultiSelectModule,
    DropDownListModule,
    GridAllModule,
    CheckBoxModule,
    SwitchModule,
    MaintainRoutingModule,
    DateInputsModule ,
    MultiSelectAllModule,
    TreeViewAllModule,
    TreeViewModule ,
    DatePickerModule,
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
export class MaintainModule {
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


