import { Account2Service } from 'src/app/_core/_service/account2.service';
import { Component, OnInit } from '@angular/core';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  confirmPassword: any;
  newPassword: any;
  constructor(
    private alertify: AlertifyService,
    private service: Account2Service,
    private cookieService: CookieService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  ngOnInit() {
  }
  submit() {
    this.spinner.show()
    if (!this.newPassword || !this.confirmPassword) {
      this.alertify.warning("The new password and confirm password are empty! <br>Please try again!", true);
      this.spinner.hide()
      return;
    }
    if (this.newPassword !== this.confirmPassword) {
      this.alertify.warning("The new password and confirm password are not the same! <br> Please try again!", true);
      this.spinner.hide()
      return;
    }
    const request = {
      id: +JSON.parse(localStorage.getItem("user")).id,
      newPassword: this.newPassword,
      confirmPassword: this.confirmPassword
    }
    this.service.changePassword(request).subscribe( res => {
      if (res.success === true) {
        const lang = localStorage.getItem('lang')  ;
        const message = lang == 'vi' ? 'Chỉnh sửa thành công!' : lang === 'en' ? 'Revised Successfully' : '修改成功';
        const close = lang == 'vi' ? 'Đóng' : lang === 'en' ? 'Close' : '關閉';
        const viewProductList = lang == 'vi' ? 'Đăng nhập lại ngay bây giờ' : lang === 'en' ? 'Login again' : '重新登入';
        this.spinner.hide()
        this.alertify.confirm3('',message, close, viewProductList, () => {
          // this.router.navigate(['login']);
        }, () => {
          this.cookieService.deleteAll();
          localStorage.clear();
          const uri = this.router.url;
          this.router.navigate(['login'], { queryParams: { uri }, replaceUrl: true });
          // this.router.navigate(['login']);
          // this.router.navigate(['/consumer/product-list']);
          return;
        })
        this.newPassword = '';
        this.confirmPassword = '';
      } else {
        this.spinner.hide()
         this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    }, err => {
      this.spinner.hide()
      this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    })
  }

}
