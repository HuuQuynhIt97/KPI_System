import { Host, Image } from './../../_core/enum/FactoryHost';
import { PermissionService } from 'src/app/_core/_service/permission.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { AlertifyService } from '../../_core/_service/alertify.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { UserForLogin } from 'src/app/_core/_model/user';
import { environment } from 'src/environments/environment';
import { RoleService } from 'src/app/_core/_service/role.service';
import { IRole, IUserRole } from 'src/app/_core/_model/role';
import { IBuilding } from 'src/app/_core/_model/building';
import { AuthenticationService } from 'src/app/_core/_service/authentication.service';
import { Subscription } from 'rxjs';
import { FunctionSystem } from 'src/app/_core/_model/application-user';
import { NgxSpinnerService } from 'ngx-spinner';
import { Authv2Service } from 'src/app/_core/_service/authv2.service';
import { MessageConstants } from 'src/app/_core/_constants/system';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
  busy = false;
  username = '';
  password = '';
  loginError = false;
  host: any

  SHC_host = Host.SHC
  SHC_Image = Image.SHC

  CB_host = Host.CB
  CB_Image = Image.CB

  SPC_host = Host.SPC
  SPC_Image = Image.SPC

  TSH_host = Host.TSH
  TSH_Image= Image.TSH

  private subscription: Subscription;

  user: UserForLogin = {
    username: '',
    password: '',
    systemCode: environment.systemCode
  };
  uri: any;
  level: number;

  remember = false;
  functions: FunctionSystem[];
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: Authv2Service,
    private permisisonService: PermissionService,
    private roleService: RoleService,
    private spinner: NgxSpinnerService,
    private cookieService: CookieService,
    private permissionService: PermissionService,
    private alertifyService: AlertifyService
  ) {
    if (this.cookieService.get('remember') !== undefined) {
      if (this.cookieService.get('remember') === 'Yes') {
        this.user = {
          username: this.cookieService.get('username'),
          password: this.cookieService.get('password'),
          systemCode: environment.systemCode
        };
        this.username = this.cookieService.get('username');
        this.password = this.cookieService.get('password');
        this.remember = true;
        this.login();
      }
    }
    this.uri = this.route.snapshot.queryParams.uri || '/transaction/todolist2';
    this.host = window.location.hostname
  }
  role: number;
  ngOnInit(): void {
    const accessToken = localStorage.getItem('token');
    const refreshToken = localStorage.getItem('refresh_token');
    if (accessToken && refreshToken && this.route.routeConfig.path === 'login') {
      const uri = decodeURI(this.uri) || '/transaction/todolist2';
      this.router.navigate([uri]);
    }
  }
  onChangeRemember(args) {
    this.remember = args.target.checked;
  }
  authentication() {
    return this.authService
      .login(this.username, this.password).toPromise();
  }
  async login() {
    if (!this.username || !this.password) {
      return;
    }
    this.spinner.show();
    this.busy = true;
    try {
      await this.authentication();
      const userId = JSON.parse(localStorage.getItem('user')).id;

      const roleUser = await this.roleService.getRoleByUserID(userId).toPromise();
      if(roleUser === null)
        this.alertifyService.warning("User role empty! Please using System Admin account add role for user");
      localStorage.setItem('level', JSON.stringify(roleUser));
      this.authService.setRoleValue(roleUser as IRole);
      const currentLang = localStorage.getItem('lang');

      if (currentLang) {
        localStorage.setItem('lang', currentLang);
        await this.permissionService.getMenuByLangID(userId, currentLang).toPromise();
      } else {
        localStorage.setItem('lang', 'zh');
        await this.permissionService.getMenuByLangID(userId, 'zh').toPromise();

      }

      const functions = await this.permisisonService.getActionInFunctionByRoleID(roleUser.id).toPromise();
      this.functions = functions as FunctionSystem[];
      localStorage.setItem("functions", JSON.stringify(functions));
      this.authService.setFunctions(functions as any);

      if (this.remember) {
        this.cookieService.set('remember', 'Yes');
        this.cookieService.set('username', this.user.username);
        this.cookieService.set('password', this.user.password);
        this.cookieService.set('systemCode', this.user.systemCode.toString());
      } else {
        this.cookieService.set('remember', 'No');
        this.cookieService.set('username', '');
        this.cookieService.set('password', '');
        this.cookieService.set('systemCode', '');
      }

      setTimeout(() => {
        const check = this.checkRole();
        if (check) {
          const uri = decodeURI(this.uri);
          this.router.navigate([uri]);
        } else {
          this.router.navigate([this.functions[0].url]);
        }

      });
      // localStorage.setItem('user', JSON.stringify(data.user));
      // localStorage.setItem('token', data.token);

      this.alertifyService.success('Login Success!!');
      this.busy = false;
      this.spinner.hide();
    } catch (error) {
      this.spinner.hide();
      this.busy = true;
      this.loginError = true;
    }
  }
  getMenu(userid) {
    this.permissionService.getMenuByUserPermission(userid).toPromise();
  }
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  checkRole(): boolean {
    const uri = decodeURI(this.uri);
    const permissions = this.functions.map(x => x.url);
    for (const url of permissions) {
      if (uri.includes(url)) {
        return true;
      }
    }
    return false;

  }
}
