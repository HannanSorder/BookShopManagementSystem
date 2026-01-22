import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { HomeComponent } from './Components/home/home.component';
import { ViewBooksComponent } from './Components/view-books/view-books.component';
import { AddBooksComponent } from './Components/add-books/add-books.component';
import { EditBooksComponent } from './Components/edit-books/edit-books.component';
import { LoginComponent } from './Components/login/login.component';
import { NavComponent } from './Components/nav/nav.component';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ViewBooksComponent,    // Standalone component
    AddBooksComponent,     // Standalone component
    EditBooksComponent,    // Standalone component
    LoginComponent,        // Standalone component
    NavComponent          // Standalone component
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
