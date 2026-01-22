import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { ViewBooksComponent } from './Components/view-books/view-books.component';
import { AddBooksComponent } from './Components/add-books/add-books.component';
import { EditBooksComponent } from './Components/edit-books/edit-books.component';
import { LoginComponent } from './Components/login/login.component';

export const routes: Routes = [ // ✅ const থেকে export const করুন
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'books', component: ViewBooksComponent },
  { path: 'addBook', component: AddBooksComponent },
  { path: 'addBook/edit/:id', component: EditBooksComponent },
  { path: '**', redirectTo: '/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
