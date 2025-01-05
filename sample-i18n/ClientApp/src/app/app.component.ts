import { Component, OnDestroy, OnInit } from '@angular/core';
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

  constructor(
    private webApiService: WebApiService
  ) {
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
}
