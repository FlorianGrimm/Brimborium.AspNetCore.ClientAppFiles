import { Component, OnDestroy, OnInit, LOCALE_ID, inject } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { WebApiService } from './service/webapi.service';
import { Subscription, take, tap } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit, OnDestroy {
  subscription: Subscription = new Subscription();
  title = 'ClientApp sample-i18n';
  username = '';
  culture = '';
  constructor(
    private webApiService: WebApiService
  ) {
    this.onCultureChange = this.onCultureChange.bind(this);
    this.culture = inject(LOCALE_ID);
  }

  ngOnInit(): void {
    const localSubscription = new Subscription();
    this.subscription.add(localSubscription);
    localSubscription.add(
      this.webApiService.getUsername().pipe(take(1)).subscribe({
        next: (value) => {
          console.log("getUsername-value");
          this.username = value;
        },
        complete: () => {
          localSubscription.unsubscribe();
          console.log("getUsername-done");
        },
        error: (err) => {
          localSubscription.unsubscribe();
          console.error("getUsername-error", err);
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onCultureChange(event: Event) {
    const target = event.target as (HTMLSelectElement|null);
    if (target){
      const culture = target.value;
      if (culture){
        window.location.assign(`/${culture}`);
      }
    }
    return false;
  }

}
