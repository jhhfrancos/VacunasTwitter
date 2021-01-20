import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordCloudSvgComponent } from './word-cloud-svg.component';

describe('WordCloudSvgComponent', () => {
  let component: WordCloudSvgComponent;
  let fixture: ComponentFixture<WordCloudSvgComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WordCloudSvgComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WordCloudSvgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
