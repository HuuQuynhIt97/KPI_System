import { TransactionModule } from './views/protect-zone/transaction/transaction.module';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Import Containers
import { DefaultLayoutComponent } from './containers';

import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';
import { LoginComponent } from './views/login/login.component';
import { RegisterComponent } from './views/register/register.component';
import { AuthGuard } from './_core/_guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: '404',
    component: P404Component,
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    component: P500Component,
    data: {
      title: 'Page 500'
    }
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: {
      title: 'Register Page'
    }
  },

  {
    path: '',
    component: DefaultLayoutComponent,
    runGuardsAndResolvers: 'always',
    // canActivate: [AuthGuard],
    data: {
      title: ''
    },
    children: [
      {
        path: 'system',
        loadChildren: () => import('./views/protect-zone/system/system.module').then(m => m.SystemModule)
      },
      {
        path: 'maintain',
        loadChildren: () => import('./views/protect-zone/maintain/maintain.module').then(m => m.MaintainModule)
      },
      {
        path: 'transaction',
        loadChildren: () => import('./views/protect-zone/transaction/transaction.module').then(m => m.TransactionModule)
      },
      {
        path: 'report',
        loadChildren: () => import('./views/protect-zone/report/report.module').then(m => m.ReportModule)
      },

    ]
  }, // otherwise redirect to home
  { path: '**', redirectTo: '404', pathMatch: 'full' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes,
      { relativeLinkResolution: 'legacy', useHash: true , scrollPositionRestoration: 'enabled'  }
      ) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
