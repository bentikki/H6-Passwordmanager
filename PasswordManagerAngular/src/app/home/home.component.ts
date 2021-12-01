import { Component } from '@angular/core';
import { Sitekey } from '@app/_models';
import { SitekeyService } from '@app/_services';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.less']
  })
export class HomeComponent {
    usersSiteKeys: Sitekey[];
    loading: boolean = false;

    constructor(
      private sitekeyService: SitekeyService) 
    { 

    }

    ngOnInit() {
      this.loadSitekeys();
    }

    newSiteKeyCreated(sitekey: Sitekey){
      this.loadSitekeys();
    }

    private loadSitekeys(){
      this.loading = true;
      this.sitekeyService.getAll().pipe(first()).subscribe(sitekeys => {
          this.loading = false;
          this.usersSiteKeys = sitekeys;
      });
    }
}