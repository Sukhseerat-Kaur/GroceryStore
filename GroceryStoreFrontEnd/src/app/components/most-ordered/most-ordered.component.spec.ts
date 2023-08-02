import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MostOrderedComponent } from './most-ordered.component';

describe('MostOrderedComponent', () => {
  let component: MostOrderedComponent;
  let fixture: ComponentFixture<MostOrderedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MostOrderedComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MostOrderedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
