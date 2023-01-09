import { UnauthorizedInterceptor } from './_helper/unauthorized.interceptor';
import { NgModule, APP_INITIALIZER, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthenticationService } from './_service/authentication.service';
import { appInitializer } from './_helper/appInitializer';
import { JwtInterceptor } from '@auth0/angular-jwt';
import { SystemLanguageService } from './_service/systemLanguage.service';
import { sysLoadLanguageInitializer } from './_helper/sysLoadLanguageInitializer';
import { DataService } from './_service/data.service';
import { TranslateService } from '@ngx-translate/core';

function languagesInitializer(service: SystemLanguageService) {
  return () =>
    new Promise((resolve, reject) => {
      service.getLanguages(localStorage.getItem('lang') || 'zh').subscribe(data => {
        localStorage.setItem('languages', JSON.stringify(data));
      }).add(resolve);
  });
}

@NgModule({
  declarations: [],
  imports: [CommonModule],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: languagesInitializer,
      multi: true,
      deps: [SystemLanguageService],
    },

    // {
    //   provide: APP_INITIALIZER,
    //   useFactory: sysLoadLanguageInitializer,
    //   multi: true,
    //   deps: [DataService,TranslateService],
    // },

  ],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new Error('Core Module can only be imported to AppModule.');
    }
  }
}
