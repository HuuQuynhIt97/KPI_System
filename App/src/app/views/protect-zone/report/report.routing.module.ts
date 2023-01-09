import { HqReportComponent } from './hq-report/hq-report.component';
import { TrackingAppaisalProgressComponent } from './tracking-appaisal-progress/tracking-appaisal-progress.component';
import { TrackingProcessComponent } from './tracking-process/tracking-process.component';
import { GhrReportComponent } from './ghr-report/ghr-report.component';
import { HqHrReportComponent } from './hq-hr-report/hq-hr-report.component';
import { H1H2ReportComponent } from './h1-h2-report/h1-h2-report.component';
import { Q1Q3ReportComponent } from './q1-q3-report/q1-q3-report.component';

import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "src/app/_core/_guards/auth.guard";
import { CoreCompetenciesAnalysisComponent } from './core-competencies-analysis/core-competencies-analysis.component';
import { KpiMonthPerfComponent } from './kpi-month-perf/kpi-month-perf.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: '',
      breadcrumb: ''
    },
    children: [
      {
        path: 'q1-q3-report',
        component: Q1Q3ReportComponent,
        data: {
          title: 'Q1,Q3 Report 季報表',
          breadcrumb: 'Q1,Q3 Report 季報表',
          functionCode: 'q1-q3-report'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'h1-h2-report',
        component: H1H2ReportComponent,
        data: {
          title: 'H1 & H2 Report',
          breadcrumb: 'H1 & H2 Report',
          functionCode: 'h1-h2-report'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'hq-hr-report',
        component: HqHrReportComponent,
        data: {
          title: 'HQ HR Report 年中考核名單',
          breadcrumb: 'HQ HR Report 年中考核名單',
          functionCode: 'q1-q3-report'
        },
        // canActivate: [AuthGuard]
      },

      {
        path: 'kpi-month-perf',
        component: KpiMonthPerfComponent,
        data: {
          title: 'KPI Month Perf',
          breadcrumb: 'KPI Month Perf',
          functionCode: 'kpi-month-perf'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'tracking-process',
        component: TrackingProcessComponent,
        data: {
          title: 'Tracking Process',
        }
        // canActivate: [AuthGuard]
      },
      {
        path: 'tracking-appaisal-progress',
        component: TrackingAppaisalProgressComponent,
        data: {
          title: 'Tracking Appaisal Progress',
        },
      },
      {
        path: 'hq-report',
        component: HqReportComponent,
        data: {
          title: 'Tracking Appaisal Progress',
        },
      },
      {
        path: 'core-competencies-analysis',
        component: CoreCompetenciesAnalysisComponent,
        data: {
          title: 'Core Competencies Analysis',
        },
      },
      {
        path: 'ghr-report',
        component: GhrReportComponent,
        data: {
          title: 'GHR Report',
          breadcrumb: 'GHR Report',
          functionCode: 'ghr-report'
        },
        // canActivate: [AuthGuard]
      },
    ]
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
