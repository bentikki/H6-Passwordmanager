import { Component, Input, OnInit } from '@angular/core';
import { GeneratedPassword } from '@app/_models';
import { GeneratepasswordService } from '@app/_services';
import { ClipboardService } from 'ngx-clipboard';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-password-generator',
  templateUrl: './password-generator.component.html',
  styleUrls: ['./password-generator.component.less']
})
export class PasswordGeneratorComponent implements OnInit {

  formShown:boolean = false;
  generatedPasswordValue: GeneratedPassword;
  showCustomSettings: boolean = false;
  loading: boolean = false;

  passwordCustomIncludeLetters: boolean = false;
  passwordCustomIncludeNumbers: boolean = false;
  passwordCustomIncludeSigns: boolean = false;

  constructor(private generatePasswordService: GeneratepasswordService, private _clipboardService: ClipboardService) { }

  ngOnInit(): void {
    this.generateNewPassword();
  }


  onShowButtonClick(){
    if(this.formShown){
      this.formShown = false;
    }else{
      this.formShown = true;
    }
  }

  onShowCustomSettings(showSettings: boolean){
    this.showCustomSettings = showSettings;
    if(!showSettings){
      this.passwordCustomIncludeNumbers = false;
      this.passwordCustomIncludeLetters = false;
      this.passwordCustomIncludeSigns = false;
    }
  }

  onNewPasswordClick(){
    this.generateNewPassword();
  }

  generateNewPassword(){
    this.loading = true;
    
    this.generatePasswordService.get(this.passwordCustomIncludeLetters, this.passwordCustomIncludeNumbers,this.passwordCustomIncludeSigns)
      .pipe(first()).subscribe(newlyGeneratedPassword => {
          this.loading = false;
          this.generatedPasswordValue = newlyGeneratedPassword;
      });
  }

  changeNewPasswordSettings(passwordSetting: string){
    switch (passwordSetting) {
      case "letters":
        if(this.passwordCustomIncludeLetters){
          this.passwordCustomIncludeLetters = false;
        }else{
          this.passwordCustomIncludeLetters = true;
        }
      break;
        
      case "numbers":
        if(this.passwordCustomIncludeNumbers){
          this.passwordCustomIncludeNumbers = false;
        }else{
          this.passwordCustomIncludeNumbers = true;
        }
      break;

      case "signs":
        if(this.passwordCustomIncludeSigns){
          this.passwordCustomIncludeSigns = false;
        }else{
          this.passwordCustomIncludeSigns = true;
        }
      break;
    }
  }

  copyText(text: string, valueName: string){
    this._clipboardService.copy(text);
    setTimeout(() => { ; }, 1000);
  }

}
