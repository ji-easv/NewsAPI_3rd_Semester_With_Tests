import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ArticleDisplayComponent } from './article-display/article-display.component';
import {HttpClient, HttpClientModule} from "@angular/common/http";
import {DataViewModule} from "primeng/dataview";

@NgModule({
  declarations: [
    AppComponent,
    ArticleDisplayComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DataViewModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
