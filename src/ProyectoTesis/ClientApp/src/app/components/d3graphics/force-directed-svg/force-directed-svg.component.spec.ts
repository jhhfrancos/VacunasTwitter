import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForceDirectedSvgComponent } from './force-directed-svg.component';

describe('ForceDirectedSvgComponent', () => {
  let component: ForceDirectedSvgComponent;
  let fixture: ComponentFixture<ForceDirectedSvgComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForceDirectedSvgComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ForceDirectedSvgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
