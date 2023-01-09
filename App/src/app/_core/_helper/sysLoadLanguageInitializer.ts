import { TranslateService } from '@ngx-translate/core';
import { Authv2Service } from 'src/app/_core/_service/authv2.service';
import { DataService } from '../_service/data.service';


export function sysLoadLanguageInitializer(
  dataService: DataService,
  translate: TranslateService
  ) {
    return () =>
     {
        const lang = localStorage.getItem('lang')
        console.log(lang);
        if(lang === null) {
          translate.addLangs(['zh'])
          translate.use('zh')
        }else {
          translate.addLangs([lang])
          translate.use(lang)
        }
        // dataService.locale.subscribe((res: any)=>{
        // }).add(resolve);
    };
}