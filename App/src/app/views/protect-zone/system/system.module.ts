import { KeyPointModalComponent } from './campaign/setting-campaign-modal/key-point-modal/key-point-modal.component';
import { SettingCampaignModalComponent } from './campaign/setting-campaign-modal/setting-campaign-modal.component';
import { CampaignComponent } from './campaign/campaign.component';
import { MailingComponent } from './mailing/mailing.component';
import { KpiSequenceComponent } from './kpi-sequence/kpi-sequence.component';
import { VersionAddComponent } from './version/version-add/version-add.component';
import { VersionComponent } from './version/version.component';
import { ViewKPIComponent } from './viewKPI/viewKPI.component';
import { Kpi2nd3rdComponent } from './kpi2nd3rd/kpi2nd3rd.component';
import { KpiCreate2Component } from './kpi-create2/kpi-create2.component';
import { SettingMonthlyComponent } from './setting-monthly/setting-monthly.component';
import { KpiCreateComponent } from './kpi-create/kpi-create.component';
import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { GridAllModule } from '@syncfusion/ej2-angular-grids'
import { DropDownListModule, MultiSelectAllModule, MultiSelectModule } from '@syncfusion/ej2-angular-dropdowns'
import { CheckBoxAllModule, SwitchModule } from '@syncfusion/ej2-angular-buttons'
import { TranslateLoader, TranslateModule } from '@ngx-translate/core'
import { HttpClient } from '@angular/common/http'
import { TranslateHttpLoader } from '@ngx-translate/http-loader'
import { L10n, loadCldr, setCulture } from '@syncfusion/ej2-base'
import { DateInputsModule } from '@progress/kendo-angular-dateinputs'
import { TreeGridAllModule } from '@syncfusion/ej2-angular-treegrid'

import { OcComponent } from './oc/oc.component'
import { AccountGroupPeriodComponent } from './account-group-period/account-group-period.component'
import { AccountComponent } from './account/account.component'
import { AccountGroupComponent } from './account-group/account-group.component'
import { SystemRoutingModule } from './system.routing.module'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { RichTextEditorAllModule } from '@syncfusion/ej2-angular-richtexteditor';
import { CustomLoader } from 'src/app/_core/_helper/customLoader';
import { SystemLanguageComponent } from './system-language/system-language.component';
// import { PeriodComponent } from './period/period.component';
// export function HttpLoaderFactory(http: HttpClient) {
//   return new TranslateHttpLoader(http, './assets/i18n/', '.json');
// }
import { IntlModule } from '@progress/kendo-angular-intl';

import { TooltipModule } from '@syncfusion/ej2-angular-popups';
import { JobTitleComponent } from './job-title/job-title.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
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
    AccountComponent,
    AccountGroupComponent,
    AccountGroupPeriodComponent,
    VersionComponent,
    VersionAddComponent,
    SystemLanguageComponent,
    MailingComponent,
    JobTitleComponent,
    CampaignComponent,
    SettingCampaignModalComponent,
    KeyPointModalComponent
  ],
  imports: [
    TooltipModule,
    IntlModule,
    CKEditorModule,
    RichTextEditorAllModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TreeGridAllModule,
    MultiSelectModule,
    DropDownListModule,
    GridAllModule,
    CheckBoxAllModule,
    SwitchModule,
    SystemRoutingModule,
    DateInputsModule ,
    MultiSelectAllModule,
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
export class SystemModule {
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


