import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ArticleDisplayComponent } from './article-display/article-display.component';
import {HttpClient, HttpClientModule} from "@angular/common/http";
import {DataViewModule} from "primeng/dataview";
import {ButtonModule} from "primeng/button";
import {ArticleDialogComponent} from "./article-dialog/aricle-dialog.component";
import {ArticleService} from "./ArticleService";
import {DialogService} from "primeng/dynamicdialog";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import { ArticleInputComponent } from './article-input/article-input.component';
import {ReactiveFormsModule} from "@angular/forms";
import {InputTextModule} from "primeng/inputtext";
import {InputTextareaModule} from "primeng/inputtextarea";

@NgModule({
  declarations: [
    AppComponent,
    ArticleDisplayComponent,
    ArticleDialogComponent,
    ArticleInputComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DataViewModule,
    HttpClientModule,
    ButtonModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    InputTextModule,
    InputTextareaModule
  ],
  providers: [ArticleService, DialogService],
  bootstrap: [AppComponent]
})
export class AppModule { }
