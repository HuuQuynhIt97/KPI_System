
import { AfterViewInit, Component, HostListener, OnInit } from '@angular/core';
import { AuthService } from '../../_core/_service/auth.service';
import { AlertifyService } from '../../_core/_service/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HeaderService } from 'src/app/_core/_service/header.service';
import { DomSanitizer } from '@angular/platform-browser';
import { CalendarsService } from 'src/app/_core/_service/calendars.service';
import * as moment from 'moment';
import { Nav } from 'src/app/_core/_model/nav';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { RoleService } from 'src/app/_core/_service/role.service';
import { CookieService } from 'ngx-cookie-service';
import { DataService } from 'src/app/_core/_service/data.service';
import { PermissionService } from 'src/app/_core/_service/permission.service';
import { AuthenticationService } from 'src/app/_core/_service/authentication.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { VersionService } from 'src/app/_core/_service/version.service';
declare var require: any;
import * as signalr from '../../../assets/js/ec-client.js';
import { HubConnectionState } from '@microsoft/signalr';
import { navItems, navItemsEN, navItemsVI } from 'src/app/_nav';
import { Authv2Service } from 'src/app/_core/_service/authv2.service';
import { Account2Service } from 'src/app/_core/_service/account2.service';
import { Account } from 'src/app/_core/_model/account';
import { IRole } from 'src/app/_core/_model/role';
import { FunctionSystem } from 'src/app/_core/_model/application-user';
import { SystemCode } from 'src/app/_core/enum/system';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
  styleUrls: ['default-layout.component.css']
})
export class DefaultLayoutComponent implements OnInit, AfterViewInit {
  public sidebarMinimized = false;
  public navItems = navItems;
  public navAdmin: any;
  public navClient: any;
  navEc: any;
  public total: number;
  public totalCount: number;
  public page: number;
  public pageSize: number;
  public currentUser: string;
  public currentTime: any;
  userid: number;
  level: number;
  roleName: string;
  role: any;
  avatar: any;
  vi: any;
  en: any;
  langsData: object[];
  public fields = { text: 'name', value: 'id' };
  public value: string;
  zh: any;
  menus: any;
  modalReference: any;
  remember: any;
  @HostListener('window:scroll', ['$event'])
  onScroll(e) {
  }
  online: number;
  userID: number;
  userName: any;
  data: [] = [];
  firstItem: any;
  userData: any;
  functions: FunctionSystem[];
  fieldsRole: object = { text: 'fullName', value: 'username' };
  uri: any;
  values: string
  SYSTEMADMIN = SystemCode.SYSTEMADMIN
  public currentYear: number = new Date().getFullYear()
  constructor(
    private versionService: VersionService,
    private authService: Authv2Service,
    private authenticationService: Authv2Service,
    private roleService: RoleService,
    private alertify: AlertifyService,
    private permissionService: PermissionService,
    private headerService: HeaderService,
    private calendarsService: CalendarsService,
    private sanitizer: DomSanitizer,
    private router: Router,
    private dataService: DataService,
    private spinner: NgxSpinnerService,
    private cookieService: CookieService,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private service: Account2Service,

    public translate: TranslateService

  ) {


    this.vi = require('../../../assets/ej2-lang/vi.json');
    this.en = require('../../../assets/ej2-lang/en.json');
    this.role = JSON.parse(localStorage.getItem('level'));
    this.value = localStorage.getItem('lang');
    const user = JSON.parse(localStorage.getItem("user"));
    const level = JSON.parse(localStorage.getItem("level"));
    if(localStorage.getItem('anonymous') === "yes") {
      this.userName = "admin";
      this.roleName = level?.code;
    }
    else {
      this.roleName = level?.code;
      this.userName = user?.fullName;
    }
    // this.values = user?.username;
    this.userID = user?.id;
    this.uri = this.route.snapshot.queryParams.uri || '/transaction/todolist2';
  }
  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }
  onActivate(event) {
    window.scroll(0,0);
    //or document.body.scrollTop = 0;
    //or document.querySelector('body').scrollTo(0,0)
  }
  ngOnInit(): void {

    this.getAccounts()

    this.versionService.getAllVersion().subscribe((item: any) => {
      this.data = item;
      this.firstItem = item[0] || {};
    });

    this.langsData = [, { id: 'zh', name: '中文' },
     { id: 'en', name: 'EN' },
    //  { id: 'vi', name: 'VI' }
    ];

    // this.getAvatar();
    this.currentUser = JSON.parse(localStorage.getItem('user')).fullName;
    this.values = JSON.parse(localStorage.getItem('user')).username;
    this.page = 1;
    this.pageSize = 10;

    this.userid = JSON.parse(localStorage.getItem('user')).id;
    this.getMenu();
    this.onService();
    this.currentTime = moment().format('hh:mm:ss A');
    setInterval(() => this.updateCurrentTime(), 1 * 1000);
  }
  ngAfterViewInit() {
    // this.getBuilding();
  }
  onChangeRole(args) {
    // this.userID = data.id;
    this.userName = args.itemData.username;
    this.loginAnonymous()
  }
  authentication() {
    return this.authService
      .loginAnonymous(this.userName).toPromise();
  }
  async loginAnonymous() {
    // this.authenticationService.logOut();
    this.spinner.show();

    try {
      await this.authentication();
      const userId = JSON.parse(localStorage.getItem('user')).id;
      const currentLang = localStorage.getItem('lang');

      const roleUser = await this.roleService.getRoleByUserID(userId).toPromise();
      localStorage.setItem('level', JSON.stringify(roleUser));
      this.authService.setRoleValue(roleUser as IRole);

      if (currentLang) {
        localStorage.setItem('lang', currentLang);
        await this.permissionService.getMenuByLangID(userId, currentLang).toPromise();
      } else {
        localStorage.setItem('lang', 'zh');
        await this.permissionService.getMenuByLangID(userId, 'zh').toPromise();

      }

      const functions = await this.permissionService.getActionInFunctionByRoleID(roleUser.id).toPromise();
      this.functions = functions as FunctionSystem[];
      localStorage.setItem("functions", JSON.stringify(functions));
      this.authService.setFunctions(functions as any);

      if (this.remember) {
        this.cookieService.set('remember', 'Yes');
        this.cookieService.set('username', this.userName);
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
      window.location.reload()
      this.spinner.hide();

    } catch (error) {
      this.spinner.hide();
    }
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
  getAccounts() {
    this.service.getAll().subscribe(data => {
      this.userData = data;
      this.userData.unshift({ username: "admin", fullName: "Default(Admin)" });
      this.userData.unshift({ username: "supremeadmin", fullName: "Supreme Admin" });
    });

  }
  getMenu() {
    const navs = JSON.parse(localStorage.getItem('navs'));
    if (navs === null) {
      this.spinner.show();
      const langID = localStorage.getItem('lang');
      this.permissionService.getMenuByLangID(this.userid, langID).subscribe((navsData: []) => {
        // localStorage.setItem('navs', JSON.stringify(navsData));
        this.navItems = navsData;
        this.spinner.hide();

      }, (err) => {
        this.spinner.hide();
      });
    } else {
      this.navItems = navs;
    }
  }
  home() {
    return '/ec/execution/todolist2';
  }
  onChange(args) {
    this.spinner.show();
    const lang = args.itemData.id;
    localStorage.removeItem('lang');
    localStorage.setItem('lang', lang);
    this.dataService.setValueLocale(lang);
    // this.translate.addLangs([lang])
    // this.translate.use(lang)
    this.permissionService.getMenuByLangID(this.userid, lang).subscribe((navs: []) => {
      // localStorage.setItem('navs', JSON.stringify(navs));
      this.navItems = navs;
      this.spinner.hide();
      window.location.reload();

    }, (err) => {
      this.spinner.hide();
    });
  }
  getBuilding() {
    const userID = JSON.parse(localStorage.getItem('user')).user.id;
    this.roleService.getRoleByUserID(userID).subscribe((res: any) => {
      res = res || {};
      if (res !== {}) {
        this.level = res.id;
      }
    });
  }
  onService() {
    this.headerService.currentImage
      .subscribe(arg => {
        if (arg) {
          this.changeAvatar(arg);
        }
      });
  }
  changeAvatar(avt) {
    let avatar;
    if (avt) {
      avatar = avt.replace('data:image/png;base64,', '').trim();
      localStorage.removeItem('avatar');
      localStorage.setItem('avatar', avatar);
      this.getAvatar();
    } else {
      this.avatar = this.defaultImage();
    }

  }

  updateCurrentTime() {
    this.currentTime = moment().format('hh:mm:ss A');
  }
  logout() {
    this.cookieService.deleteAll();
    localStorage.clear();
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.authenticationService.logOut();
    const uri = this.router.url;
    this.router.navigate(['login'], { queryParams: { uri }, replaceUrl: true });
    this.alertify.message('Logged out');

  }

  defaultImage() {
    return this.sanitizer.bypassSecurityTrustResourceUrl(`data:image/png;base64, iVBORw0KGgoAAAANSUhEUgAAAJYAA
      ACWBAMAAADOL2zRAAAAG1BMVEVsdX3////Hy86jqK1+ho2Ql521ur7a3N7s7e5Yhi
      PTAAAACXBIWXMAAA7EAAAOxAGVKw4bAAABAElEQVRoge3SMW+DMBiE4YsxJqMJtH
      OTITPeOsLQnaodGImEUMZEkZhRUqn92f0MaTubtfeMh/QGHANEREREREREREREtIJ
      J0xbH299kp8l8FaGtLdTQ19HjofxZlJ0m1+eBKZcikd9PWtXC5DoDotRO04B9YOvF
      IXmXLy2jEbiqE6Df7DTleA5socLqvEFVxtJyrpZFWz/pHM2CVte0lS8g2eDe6prOy
      qPglhzROL+Xye4tmT4WvRcQ2/m81p+/rdguOi8Hc5L/8Qk4vhZzy08DduGt9eVQyP
      2qoTM1zi0/uf4hvBWf5c77e69Gf798y08L7j0RERERERERERH9P99ZpSVRivB/rgAAAABJRU5ErkJggg==`);
  }

  getAvatar() {
    const img = localStorage.getItem('avatar');
    if (img === 'null') {
      this.avatar = this.defaultImage();
    } else {
      this.avatar = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/png;base64, ' + img);
    }
  }

  imageBase64(img) {
    if (img === 'null') {
      return this.defaultImage();
    } else {
      return this.sanitizer.bypassSecurityTrustResourceUrl('data:image/png;base64, ' + img);
    }
  }

  datetime(d) {
    return this.calendarsService.JSONDateWithTime(d);
  }

  openModal(ref) {
    this.modalReference = this.modalService.open(ref, { size: 'xl', backdrop: 'static', keyboard: false });

  }
}
