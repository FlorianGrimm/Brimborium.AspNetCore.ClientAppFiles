import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CategoryAIndexComponent } from './category-a/category-a-index/category-a-index.component';
import { PageOneComponent } from './category-a/page-one/page-one.component';
import { PageTwoComponent } from './category-a/page-two/page-two.component';
import { CategoryBIndexComponent } from './category-b/category-b-index/category-b-index.component';
import { PageThreeComponent } from './category-b/page-three/page-three.component';
import { PageFourComponent } from './category-b/page-four/page-four.component';
import { HelpIndexComponent } from './help/index/index.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

export const routes: Routes = [
  { path: "", pathMatch: "full", title: "Home", component: HomeComponent },
  { path: "category-a", pathMatch: "full", component: CategoryAIndexComponent },
  { path: "category-a/page-one", component: PageOneComponent },
  { path: "category-a/page-two", component: PageTwoComponent },
  { path: "category-b", pathMatch: "full", component: CategoryBIndexComponent },
  { path: "category-b/page-three", component: PageThreeComponent },
  { path: "category-b/page-four", component: PageFourComponent },
  { path: "help", component: HelpIndexComponent },
  { path: "**", component: PageNotFoundComponent }
];
