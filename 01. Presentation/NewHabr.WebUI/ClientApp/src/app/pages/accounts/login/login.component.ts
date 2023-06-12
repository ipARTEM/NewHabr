import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Authorization } from 'src/app/core/models/Authorization';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {

  subscribtions: Subscription[] = [];

  userName: string;
  password: string;
  auth: Authorization;

  path = 'accounts/register'

  constructor(private store: AppStoreProvider, private router: Router) { }
  
  ngOnInit(): void {
    const authSubscribtion = this.store.getAuth().subscribe(auth => {
      if (auth) {
        this.auth = auth;
        this.router.navigate(['accounts', auth.User.Id, 'profile']);
      }
    })

    this.subscribtions.push(authSubscribtion);
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  login() {
    if (!!this.userName && !!this.password) {
      this.store.login({
        UserName: this.userName,
        Password: this.password
      })
    }
  }
}
