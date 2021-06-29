import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScatterSvgComponent } from './scatter-svg.component';

describe('ScatterSvgComponent', () => {
  let component: ScatterSvgComponent;
  let fixture: ComponentFixture<ScatterSvgComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScatterSvgComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScatterSvgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
