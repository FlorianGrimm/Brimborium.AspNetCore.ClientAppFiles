import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryBIndexComponent } from './category-b-index.component';

describe('CategoryBIndexComponent', () => {
  let component: CategoryBIndexComponent;
  let fixture: ComponentFixture<CategoryBIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryBIndexComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryBIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
