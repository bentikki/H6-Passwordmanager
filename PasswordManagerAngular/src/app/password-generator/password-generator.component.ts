import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-password-generator',
  templateUrl: './password-generator.component.html',
  styleUrls: ['./password-generator.component.less']
})
export class PasswordGeneratorComponent implements OnInit {

  formShown:boolean = false;
  generatedPasswordValue: string;
  showCustomSettings: boolean = false;

  constructor() { }

  ngOnInit(): void {
    // Test
    this.formShown = true;
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
  }

}
