import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { TranslateService } from '@ngx-translate/core';
import { fromEvent, Observable } from 'rxjs';
import { delay } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line
  selector: 'body',
  template: '<router-outlet></router-outlet>',
  providers: [IconSetService],
})
export class AppComponent implements OnInit {
  lang = localStorage.getItem('lang');
  source$: Observable<Event>;
  constructor(
    private router: Router,
    public iconSet: IconSetService,
    private translate: TranslateService,
  ) {
    // iconSet singleton
   
    iconSet.icons = { ...freeSet };
  }

  ngOnInit() {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
  }

  
}
