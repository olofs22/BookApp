import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { BookList } from './features/books/book-list/book-list';
import { authGuard } from './core/guards/auth.Guard';

export const routes: Routes = [
    {path: 'login', component: Login},
    {path: 'register', component: Register},
    {path: 'books', component: BookList, canActivate: [authGuard]},
    {path: '', redirectTo:'/login', pathMatch: 'full'}
];
