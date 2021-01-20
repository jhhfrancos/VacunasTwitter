import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BarSvgComponent } from './bar-svg.component';

describe('BarSvgComponent', () => {
  let component: BarSvgComponent;
  let fixture: ComponentFixture<BarSvgComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BarSvgComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BarSvgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
