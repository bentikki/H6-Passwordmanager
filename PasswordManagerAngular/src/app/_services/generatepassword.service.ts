import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GeneratedPassword } from '@app/_models';
import { environment } from '@environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class GeneratepasswordService {

  constructor(private http: HttpClient) { }

  get(includeLetters: boolean, includeNumbers: boolean, includeSigns: boolean) {

    let customPasswordSettings = "?customsettings=";
    if(includeLetters || includeNumbers || includeSigns){
      customPasswordSettings += "true";
    }else{
      customPasswordSettings += "false";
    }
    customPasswordSettings += this.generatePasswordUrlString(includeLetters, includeNumbers, includeSigns);


    return this.http.get<GeneratedPassword>(`${environment.apiUrl}/v1/GeneratePassword${customPasswordSettings}`)
        .pipe(map(passwordResponse => {
            return passwordResponse;
        }));

  }

  private generatePasswordUrlString(includeLetters: boolean, includeNumbers: boolean, includeSigns: boolean) : string{
    let customSettingsUrl = "";
    
    if(includeLetters) customSettingsUrl += "&letters=true"; 
    else customSettingsUrl += "&letters=false"; 

    if(includeNumbers) customSettingsUrl += "&numbers=true"; 
    else customSettingsUrl += "&numbers=false";

    if(includeSigns) customSettingsUrl += "&signs=true"; 
    else customSettingsUrl += "&signs=false";

    return customSettingsUrl;
  }

}
