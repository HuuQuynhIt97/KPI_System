import { PeopleCommitteeKpiDetailComponent } from './people-committee/people-committee-kpi-detail/people-committee-kpi-detail.component';
import { PeopleCommitteeComponent } from './people-committee/people-committee.component';
import { ScoreReviseStationComponent } from './score-revise-station/score-revise-station.component';
import { GmSelfScoreComponent } from './perfomance-evaluation/gm-self-score/gm-self-score.component';
import { L2SelfScoreComponent } from './perfomance-evaluation/l2-self-score/l2-self-score.component';
import { L1SelfScoreComponent } from './perfomance-evaluation/l1-self-score/l1-self-score.component';
import { SelfScoreComponent } from './perfomance-evaluation/self-score/self-score.component';
import { ScoreAttitudeComponent } from './score-attitude/score-attitude.component';
import { PerfomanceEvaluationComponent } from './perfomance-evaluation/perfomance-evaluation.component';
import { MeetingReviseComponent } from './meeting-revise/meeting-revise.component';
import { CHMComponent } from './CHM/CHM.component';
import { MeetingComponent } from './meeting/meeting.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { Todolist2Component } from "./todolist2/todolist2.component";
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';
import { P404Component } from '../../error/404.component';
import { ScoreDetailComponent } from './score-attitude/score-detail/score-detail.component';
import { PeopleCommitteeDetailComponent } from './people-committee/people-committee-detail/people-committee-detail.component';
import { NewAttitudeComponent } from './new-attitude/new-attitude.component';
import { L0ScoreAttitudeComponent } from './new-attitude/l0-score-attitude/l0-score-attitude.component';
import { L1ScoreAttitudeComponent } from './new-attitude/l1-score-attitude/l1-score-attitude.component';
import { L2ScoreAttitudeComponent } from './new-attitude/l2-score-attitude/l2-score-attitude.component';
import { FlScoreAttitudeComponent } from './new-attitude/fl-score-attitude/fl-score-attitude.component';
import { NewAttitudeDetailModalComponent } from './new-attitude/new-attitude-detail-modal/new-attitude-detail-modal.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: '',
      breadcrumb: ''
    },
    children: [
      {
        path: '404',
        component: P404Component,
        data: {
          title: 'Page 404'
        }
      },

      {
        path: 'todolist2',
        component: Todolist2Component,
        data: {
          title: 'To Do List 2',
          breadcrumb: 'To Do List 2',
          functionCode: 'To Do List'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'meeting',
        component: MeetingComponent,
        data: {
          title: 'Meeting',
          breadcrumb: 'Meeting',
          functionCode: 'Meeting'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'chm',
        component: CHMComponent,
        data: {
          title: 'CHM',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'meeting-revise',
        component: MeetingReviseComponent,
        data: {
          title: 'Meeting Revise',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'perfomance-evaluation',
        component: PerfomanceEvaluationComponent,
        data: {
          title: 'Perfomance Evaluation',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'score-attitude',
        component: ScoreAttitudeComponent,
        data: {
          title: 'Score Attitude',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'score-revise-station',
        component: ScoreReviseStationComponent,
        data: {
          title: 'Score Revise Station',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'people-committee',
        component: PeopleCommitteeComponent,
        data: {
          title: 'People Committee',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'score-detail/:campaignID/:FlID/:L0ID/:L1ID/:L2ID/:Type',
        component: ScoreDetailComponent,
        data: {
          title: 'Score Detail',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'perfomance-evaluation/self-score/L0/:campaignID/:userID',
        component: SelfScoreComponent,
        data: {
          title: 'Self Score',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'perfomance-evaluation/self-score/L1/:campaignID/:userID',
        component: L1SelfScoreComponent,
        data: {
          title: 'L1 Self Score',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'perfomance-evaluation/self-score/L2/:campaignID/:userID',
        component: L2SelfScoreComponent,
        data: {
          title: 'L2 Self Score',
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'perfomance-evaluation/self-score/GM/:campaignID/:userID',
        component: GmSelfScoreComponent,
        data: {
          title: 'GM Self Score',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'people-committee/people-committee-detail/:appraiseeID/:campaignID',
        component: PeopleCommitteeDetailComponent,
        data: {
          title: 'People Committee Detail',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'people-committee/people-committee-detail/kpi-detail/:appraiseeID/:campaignID',
        component: PeopleCommitteeKpiDetailComponent,
        data: {
          title: 'People Committee KPI Detail',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'new-attitude',
        component: NewAttitudeComponent,
        data: {
          title: 'New Attitude',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'new-attitude/new-score-attitude/L0/:campaignID/:scoreTo',
        component: L0ScoreAttitudeComponent,
        data: {
          title: 'L0 Score Attitude',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'new-attitude/new-score-attitude/L1/:campaignID/:scoreTo',
        component: L1ScoreAttitudeComponent,
        data: {
          title: 'L1 Score Attitude',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'new-attitude/new-score-attitude/L2/:campaignID/:scoreTo',
        component: L2ScoreAttitudeComponent,
        data: {
          title: 'L2 Score Attitude',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'new-attitude/new-score-attitude/FL/:campaignID/:scoreTo',
        component: FlScoreAttitudeComponent,
        data: {
          title: 'FL Score Attitude',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'perfomance-evaluation/new-score-attitude/L0/:campaignID/:scoreTo',
        component: L0ScoreAttitudeComponent,
        data: {
          title: 'L0 Score Attitude',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'perfomance-evaluation/new-score-attitude/L1/:campaignID/:scoreTo',
        component: L1ScoreAttitudeComponent,
        data: {
          title: 'L1 Score Attitude',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'perfomance-evaluation/new-score-attitude/L2/:campaignID/:scoreTo',
        component: L2ScoreAttitudeComponent,
        data: {
          title: 'L2 Score Attitude',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'perfomance-evaluation/new-score-attitude/FL/:campaignID/:scoreTo',
        component: FlScoreAttitudeComponent,
        data: {
          title: 'FL Score Attitude',
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'new-score-attitude-detail/:campaignID/:scoreTo',
        component: NewAttitudeDetailModalComponent,
        data: {
          title: 'Score Attitude Detail',
        },
        canActivate: [AuthGuard]
      },
    ]
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransactionRoutingModule { }
