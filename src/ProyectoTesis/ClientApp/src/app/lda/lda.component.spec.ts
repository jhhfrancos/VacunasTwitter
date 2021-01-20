import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LdaComponent } from './lda.component';

describe('LdaComponent', () => {
  let component: LdaComponent;
  let fixture: ComponentFixture<LdaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LdaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LdaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
