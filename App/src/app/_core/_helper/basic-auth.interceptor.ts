
import { Injectable } from '@angular/core';
import { HttpRequest, HttpErrorResponse, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { AlertifyService } from '../_service/alertify.service';
import { Router } from '@angular/router';
import { Authv2Service } from '../_service/authv2.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable()
export class BasicAuthInterceptor implements HttpInterceptor {
    constructor(
        private alertify: AlertifyService
        , private authService: Authv2Service,
        private spinner: NgxSpinnerService,
        private router: Router
    ) { }
    private handleError(error: HttpErrorResponse) {
      switch (error.status) {
        case 0:
          this.spinner.hide()
          alert(`The server is down or disconnected. Please check again ! 服務器已關閉或斷開連接。請再次檢查!`);
          return throwError(
            'Something bad happened; please try again later.');
        case 200:

            break;
        case 401:
            // this.authService.clearLocalStorage();
            // this.router.navigate(['login'], {
            //     queryParams: { returnUrl: this.router.routerState.snapshot.url },
            // });
            // alert( 'Unauthorized');
            break;
        case 400:
            console.error(error.error);
            this.spinner.hide()
            break;
        case 500:
            alert(`The server error. Please try again after sometime!`);
            this.spinner.hide()
            break;
      }

        // Return an observable with a user-facing error message.
        // return throwError(
        //     'Something bad happened; please try again later.');
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      const accessToken = localStorage.getItem('token');
        // add authorization header with basic auth credentials if available
        if (localStorage.getItem('token')) {
            request = request.clone({
              setHeaders: { Authorization: `Bearer ${accessToken}` },
            });
        }
        if(accessToken === null) {
          this.router.navigate(['login']);
        }
        // return next.handle(request);
        return next.handle(request).pipe(
            retry(1),
            catchError(this.handleError)

        );
    }
}
