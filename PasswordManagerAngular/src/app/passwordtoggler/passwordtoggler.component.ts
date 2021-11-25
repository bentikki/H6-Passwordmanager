import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-passwordtoggler',
  templateUrl: './passwordtoggler.component.html',
  styleUrls: ['./passwordtoggler.component.less']
})
export class PasswordtogglerComponent implements OnInit {

  @Input() ShowPassword : boolean = false;
  @Output() PasswordVisibility = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void {
  }

  onShowPasswordClick(){
    if(this.ShowPassword){
        this.ShowPassword = false;
    }else{
        this.ShowPassword = true;  
    }
    this.PasswordVisibility.emit(this.ShowPassword);
}

}
