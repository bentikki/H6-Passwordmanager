import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthenticationService } from '@app/_services';
import { first } from 'rxjs/operators';
import { MustMatch } from '../_validators/mustmatch.validator';


@Component({
  selector: 'app-createuser',
  templateUrl: './createuser.component.html',
  styleUrls: ['./createuser.component.less']
})
export class CreateuserComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error = '';
  passwordShown : boolean = false;

  constructor(
      private formBuilder: FormBuilder,
      private route: ActivatedRoute,
      private router: Router,
      private authenticationService: AuthenticationService
  ) { 
      // redirect to home if already logged in
      if (this.authenticationService.userValue) { 
          this.router.navigate(['/']);
      }
  }

  ngOnInit() {
      this.loginForm = this.formBuilder.group({
          username: ['', Validators.required],
          password: ['', Validators.required],
          retypepassword: ['', Validators.required]
      },
      {
          validator: MustMatch('password', 'retypepassword')
      });

      // get return url from route parameters or default to '/'
      this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

    onShowPasswordClick(){
        if(this.passwordShown){
            this.passwordShown = false;
        }else{
            this.passwordShown = true;  
        }
    }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }

  onSubmit() {
      this.submitted = true;

      // stop here if form is invalid
      if (this.loginForm.invalid) {
          return;
      }

      this.loading = true;
      this.authenticationService.createUser(this.f.username.value, this.f.password.value)
          .pipe(first())
          .subscribe({
              next: () => {
                this.router.navigate(['/login']);
              },
              error: error => {
                  this.error = error;
                  this.loading = false;
              }
          });
  }


}
