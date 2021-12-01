import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewSitekeyComponent } from './new-sitekey.component';

describe('NewSitekeyComponent', () => {
  let component: NewSitekeyComponent;
  let fixture: ComponentFixture<NewSitekeyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewSitekeyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewSitekeyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
