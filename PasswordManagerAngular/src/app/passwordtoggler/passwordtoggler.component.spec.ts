import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordtogglerComponent } from './passwordtoggler.component';

describe('PasswordtogglerComponent', () => {
  let component: PasswordtogglerComponent;
  let fixture: ComponentFixture<PasswordtogglerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PasswordtogglerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PasswordtogglerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
