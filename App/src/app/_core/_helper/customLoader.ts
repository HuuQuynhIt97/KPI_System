import { TranslateLoader } from "@ngx-translate/core";
import { Observable, of } from "rxjs";

export class CustomLoader implements TranslateLoader {
  getTranslation(lang: string): Observable<any> {
    const languages = JSON.parse(localStorage.getItem('languages'));
   return of(languages);
  }
}
