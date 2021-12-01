import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Sitekey } from '@app/_models';
import { SitekeyService  } from '@app/_services';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-new-sitekey',
  templateUrl: './new-sitekey.component.html',
  styleUrls: ['./new-sitekey.component.less']
})
export class NewSitekeyComponent implements OnInit {
  createNewForm: FormGroup;
  loading = false;
  submitted = false;
  formShown: boolean = false;
  error: string = '';

  @Output() addedNewSitekey: EventEmitter<any> = new EventEmitter();

  // convenience getter for easy access to form fields
  get f() { return this.createNewForm.controls; }

  constructor(
    private formBuilder: FormBuilder,
    private sitekeyService: SitekeyService) 
  { 

  }

  ngOnInit(): void {
      this.createNewForm = this.formBuilder.group({
        sitename: ['',  
        [
          Validators.required, 
          Validators.maxLength(200),
          Validators.minLength(2)
        ]],
        siteusername: ['',  
        [
          Validators.required, 
          Validators.maxLength(200),
          Validators.minLength(2)
        ]],
        sitepassword: ['',  
        [
          Validators.required, 
          Validators.maxLength(200),
          Validators.minLength(2)
        ]],
    });

  }

  onSubmit(){
    this.submitted = true;

    // stop here if form is invalid
    if (this.createNewForm.invalid) {
        return;
    }

    this.loading = true;

    let sitekey : Sitekey;
    sitekey = new Sitekey();
    sitekey.sitename = this.f.sitename.value;
    sitekey.loginName = this.f.siteusername.value;
    sitekey.loginPassword = this.f.sitepassword.value;
    console.log("sitekey from component", sitekey);
    
    this.sitekeyService.createNew(sitekey)
      .pipe(first())
      .subscribe({
          next:  createdSitekey => {
            this.createNewForm.reset()
            this.submitted = false;
            this.loading = false;
            console.log(createdSitekey);
            this.addedNewSitekey.emit(createdSitekey)
          },
          error: error => {
              this.error = error;
              this.loading = false;
          }
      });

  }

  onShowButtonClick(){
    if(this.formShown){
      this.formShown = false;
    }else{
      this.formShown = true;
    }
  }

}
