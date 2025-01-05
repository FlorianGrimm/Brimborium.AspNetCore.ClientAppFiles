import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpIndexComponent } from './index.component';

describe('IndexComponent', () => {
  let component: HelpIndexComponent;
  let fixture: ComponentFixture<HelpIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HelpIndexComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HelpIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
