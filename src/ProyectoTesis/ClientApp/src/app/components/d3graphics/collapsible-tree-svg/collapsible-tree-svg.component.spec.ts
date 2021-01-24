import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollapsibleTreeSvgComponent } from './collapsible-tree-svg.component';

describe('CollapsibleTreeSvgComponent', () => {
  let component: CollapsibleTreeSvgComponent;
  let fixture: ComponentFixture<CollapsibleTreeSvgComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CollapsibleTreeSvgComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CollapsibleTreeSvgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
