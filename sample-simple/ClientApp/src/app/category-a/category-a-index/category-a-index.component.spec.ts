import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryAIndexComponent } from './category-a-index.component';

describe('CategoryAIndexComponent', () => {
  let component: CategoryAIndexComponent;
  let fixture: ComponentFixture<CategoryAIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryAIndexComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryAIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
