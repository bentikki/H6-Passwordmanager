import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ClipboardModule } from 'ngx-clipboard';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { JwtInterceptor, ErrorInterceptor, appInitializer } from './_helpers';
import { AuthenticationService } from './_services';
import { HomeComponent } from './home';
import { LoginComponent } from './login';;
import { PasswordGeneratorComponent } from './password-generator/password-generator.component';
import { CreateuserComponent } from './createuser/createuser.component';
import { PasswordtogglerComponent } from './passwordtoggler/passwordtoggler.component';;
import { NewSitekeyComponent } from './new-sitekey/new-sitekey.component'
;
import { GeneratePasswordComponent } from './generate-password/generate-password.component'
@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpClientModule,
        AppRoutingModule,
        ClipboardModule
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        PasswordGeneratorComponent,
        CreateuserComponent,
        PasswordtogglerComponent,
        NewSitekeyComponent
,
        GeneratePasswordComponent    ],
    providers: [
        { provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [AuthenticationService] },
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }