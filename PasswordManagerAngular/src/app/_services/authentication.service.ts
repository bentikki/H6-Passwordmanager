import { Injectable, ɵresetJitOptions } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '@environments/environment';
import { User } from '@app/_models';
import { HashingService } from '.';


@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private readonly encryptionKeyName;

    private userSubject: BehaviorSubject<User>;
    public user: Observable<User>;

    constructor(
        private router: Router,
        private http: HttpClient,
        private hashingService: HashingService
    ) {
        this.userSubject = new BehaviorSubject<User>(null);
        this.user = this.userSubject.asObservable();

        this.encryptionKeyName = "userEncryptionKey";
    }

    public get userValue(): User {
        return this.userSubject.value;
    }

    public get getUserEncryptionKey(): string{
        return localStorage.getItem(this.encryptionKeyName)
    }
    private setUserEncryptionKey(key: string){
        localStorage.setItem(this.encryptionKeyName, key);
    }
    private removeUserEncryptionKey(){
        localStorage.removeItem(this.encryptionKeyName);
    }

    login(username: string, password: string) {
        return this.http.post<any>(`${environment.apiUrl}/v1/users/authenticate`, { username, password }, { withCredentials: true })
            .pipe(map(user => {
                console.log("Password before hashing: ", password);
                let hashedPassword = this.hashingService.hashString(password);
                this.setUserEncryptionKey(hashedPassword);
                console.log("Password after hashing: ", this.getUserEncryptionKey);
                this.userSubject.next(user);
                this.startRefreshTokenTimer();
                return user;
            }));
    } 

    createUser(username: string, password: string) {
        return this.http.post<any>(`${environment.apiUrl}/v1/users`, { username, password })
            .pipe(
                map(user => {
                return user;
            }));
    }

    createNewUser(username: string, password: string){
        return this.http.post<any>(`${environment.apiUrl}/v1/users`, { username, password },{observe: 'response'});   
    }


    logout() {
        this.http.post<any>(`${environment.apiUrl}/v1/token/revoke`, {}, { withCredentials: true }).subscribe();
        this.stopRefreshTokenTimer();
        this.userSubject.next(null);
        this.removeUserEncryptionKey()
        this.router.navigate(['/login']);
    }

    refreshToken() {

        return this.http.post<any>(`${environment.apiUrl}/v1/token/refresh`, {}, { withCredentials: true })
            .pipe(map((user) => {
                this.userSubject.next(user);
                this.startRefreshTokenTimer();
                return user;
            }));
    }

    // helper methods

    private refreshTokenTimeout;

    private startRefreshTokenTimer() {
        // parse json object from base64 encoded jwt token
        const jwtToken = JSON.parse(atob(this.userValue.tokenSet.accessToken.split('.')[1]));

        // set a timeout to refresh the token a minute before it expires
        const expires = new Date(jwtToken.exp * 1000);
        const timeout = expires.getTime() - Date.now() - (60 * 1000);
        this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);
    }

    private stopRefreshTokenTimer() {
        clearTimeout(this.refreshTokenTimeout);
    }
}
