import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';  

@Injectable({
  providedIn: 'root'
})
export class HashingService {

  private hashingMethod;

  constructor() 
  { 
    this.hashingMethod = CryptoJS.sha256;
  }

  hashString(input: string) : string {
    let hashedString = CryptoJS.SHA256(input);
    return hashedString.toString(CryptoJS.enc.Hex);
  }

}
