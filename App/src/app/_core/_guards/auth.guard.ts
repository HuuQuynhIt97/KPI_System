import { filter } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { CanActivate, Router , ActivatedRoute, ActivatedRouteSnapshot, RouterStateSnapshot  } from '@angular/router';
import { ActionConstant } from '../_constants';
import { AlertifyService } from '../_service/alertify.service';
import { AuthenticationService } from '../_service/authentication.service';
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthenticationService,
    private alertify: AlertifyService,
    private router: Router,
    private route: ActivatedRoute) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.loggedIn()) {
      if (this.checkRole(route) === false) {
        const functions = JSON.parse(localStorage.getItem('functions'));
        this.router.navigate([functions[0].url]);
        return true;
      }
      return true;
    }
    this.router.navigate(['login'], { queryParams: { uri: state.url }, replaceUrl: true });
    return false;
  }
  checkRole(route: ActivatedRouteSnapshot): boolean {
    const functionCode = route.data.functionCode;
    if (functionCode) {
      const functions = JSON.parse(localStorage.getItem('functions'));
      const childrenTemp = functions.map(el => {
        return el?.childrens;
      }).filter(Boolean);
      const permissions = [].concat.apply([], childrenTemp).filter(x => x.functionCode === functionCode);
      for (const item of permissions) {
        if (functionCode.includes(item.functionCode) && item.code === ActionConstant.VIEW ) {
          return true;
        }
      }
      return false;
    }
    return true;

  }
}

