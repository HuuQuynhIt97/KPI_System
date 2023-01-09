import { MailingComponent } from './mailing/mailing.component';
import { KpiSequenceComponent } from './kpi-sequence/kpi-sequence.component';
import { VersionComponent } from './version/version.component';
import { ViewKPIComponent } from './viewKPI/viewKPI.component';
import { Kpi2nd3rdComponent } from './kpi2nd3rd/kpi2nd3rd.component';
import { KpiCreate2Component } from './kpi-create2/kpi-create2.component';
import { SettingMonthlyComponent } from './setting-monthly/setting-monthly.component';
import { KpiCreateComponent } from './kpi-create/kpi-create.component';
import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { AuthGuard } from 'src/app/_core/_guards/auth.guard'

import { OcComponent } from './oc/oc.component'
import { AccountGroupPeriodComponent } from './account-group-period/account-group-period.component'
import { AccountGroupComponent } from './account-group/account-group.component'
import { AccountComponent } from './account/account.component'
import { VersionAddComponent } from './version/version-add/version-add.component';
import { SystemLanguageComponent } from './system-language/system-language.component';
import { JobTitleComponent } from './job-title/job-title.component';
import { CampaignComponent } from './campaign/campaign.component';

// import { PeriodComponent } from './period/period.component';
const routes: Routes = [
  {
    path: '',
    data: {
      title: '',
      breadcrumb: ''
    },
    children: [
      {
        path: 'account',
        component: AccountComponent,
        data: {
          title: 'Account',
          breadcrumb: 'Account',
          functionCode: 'Account'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'account-group',
        component: AccountGroupComponent,
        data: {
          title: 'Account Group',
          breadcrumb: 'Account Group',
          functionCode: 'Account Group'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'setting-monthly',
        component: SettingMonthlyComponent,
        data: {
          title: 'Setting Monthly',
          breadcrumb: 'Setting Monthly',
          functionCode: 'setting-monthly'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'account-group-period',
        component: AccountGroupPeriodComponent,
        data: {
          title: 'Account Group Period',
          breadcrumb: 'Account Group Period',
          functionCode: 'account-group-period'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'oc',
        component: OcComponent,
        data: {
          title: 'OC',
          breadcrumb: 'OC',
          functionCode: 'oc'
        },
        // canActivate: [AuthGuard]
      },

      {
        path: 'kpi-create',
        component: KpiCreateComponent,
        data: {
          title: 'KPI Create',
          breadcrumb: 'KPI Create',
          functionCode: 'kpi-create'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'kpi-create2',
        component: KpiCreate2Component,
        data: {
          title: 'KPI Create',
          breadcrumb: 'KPI Create',
          functionCode: 'kpi-create'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'kpi-2nd3rd',
        component: Kpi2nd3rdComponent,
        data: {
          title: 'KPI 2nd & 3rd Create',
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'view-kpi',
        component: ViewKPIComponent,
        data: {
          title: 'KPI View',
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'system-language',
        component: SystemLanguageComponent,
        data: {
          title: 'System Language'
        }
      },

      {
        path: 'mailing',
        component: MailingComponent,
        data: {
          title: 'Mailing'
        }
      },
      {
        path: 'job-title',
        component: JobTitleComponent,
        data: {
          title: 'Job Title',
          breadcrumb: 'Job Title',
          functionCode: 'job title'
        }
      },
      {
        path: 'setting-campaign',
        component: CampaignComponent,
        data: {
          title: 'Setting Campaign',
          breadcrumb: 'Setting Campaign',
          functionCode: 'setting-campaign'
        }
      },
      {
        path: 'version',
        data: {
          title: 'Verison',
          breadcrumb: 'Verison',
          functionCode: 'Version'
        },
        children: [
          {
            path: '',
            component: VersionComponent
          },
         {
            path: 'add',
            component: VersionAddComponent,
            data: {
              title: 'Add'
            }
         },
          {
            path: 'edit/:id',
            component: VersionAddComponent,
            data: {
              title: 'Edit'
            }
          }
        ]
      }

    ]
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule { }
