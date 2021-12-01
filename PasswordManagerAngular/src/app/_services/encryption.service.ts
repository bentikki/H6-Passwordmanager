import { Injectable } from '@angular/core';
import { Sitekey } from '@app/_models';
import * as CryptoJS from 'crypto-js';  
import { AuthenticationService } from '.';

@Injectable({
  providedIn: 'root'
})
export class EncryptionService {

  private encryptionMethod;

  constructor(private authenticationService : AuthenticationService) 
  { 
    this.encryptionMethod = CryptoJS.AES;
  }

  private get getEncryptionKey(){
    return this.authenticationService.getUserEncryptionKey;
  }

  public encryptString(input: string): string{
    let encryptedString = this.encryptionMethod.encrypt(input, this.getEncryptionKey);
    return encryptedString.toString();
  }

  public decryptString(input: string): string{
    let decryptedString = this.encryptionMethod.decrypt(input, this.getEncryptionKey);
    return decryptedString.toString(CryptoJS.enc.Utf8);;
  }

  public encryptSitekey(sitekey: Sitekey) : Sitekey{
    sitekey.sitename = this.encryptString(sitekey.sitename);
    sitekey.loginName = this.encryptString(sitekey.loginName);
    sitekey.loginPassword = this.encryptString(sitekey.loginPassword);

    return sitekey;
  }

  public decryptSitekey(sitekey: Sitekey) : Sitekey{
    sitekey.sitename = this.decryptString(sitekey.sitename);
    sitekey.loginName = this.decryptString(sitekey.loginName);
    sitekey.loginPassword = this.decryptString(sitekey.loginPassword);

    return sitekey;
  }

}
