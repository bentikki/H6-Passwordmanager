import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


import { environment } from '@environments/environment';
import { Sitekey, User } from '@app/_models';
import { map } from 'rxjs/operators';
import { AuthenticationService, EncryptionService } from '.';

@Injectable({ providedIn: 'root' })
export class SitekeyService {

    constructor(
        private http: HttpClient,
        private encryptionService: EncryptionService
        ) { }

    getAllOld() {
        return this.http.get<Sitekey[]>(`${environment.apiUrl}/v1/Sitekeys`);
    }

    getAll() {
        let decryptedSitekeys : Sitekey[] = [];
        return this.http.get<Sitekey[]>(`${environment.apiUrl}/v1/Sitekeys`)
            .pipe(map(encryptedSitekeys => {
                console.log("encrypted sitekeys", encryptedSitekeys);
                encryptedSitekeys.forEach(element => {
                   decryptedSitekeys.push(this.encryptionService.decryptSitekey(element)); 
                });

                return decryptedSitekeys;
            }));

    }


    createNew(sitekey : Sitekey){
        console.log("Sitekey before encrypt", sitekey);
        sitekey = this.encryptionService.encryptSitekey(sitekey);
        console.log("Sitekey after encrypt", sitekey);
        return this.http.post<any>(`${environment.apiUrl}/v1/Sitekeys`, sitekey)
            .pipe(
                map(sitekeyResponse => {
                return sitekeyResponse;
            }));
    }
}