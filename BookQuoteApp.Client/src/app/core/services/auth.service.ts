import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { tap } from 'rxjs';
import { environment } from "../../../environments/environments";

@Injectable({providedIn: 'root'})
export class AuthService {
    private readonly apiUrl = `${environment.apiUrl}/auth`;
    constructor(private http: HttpClient) {}

    register(data: {name: string; email: string; password: string;}) {
    return this.http.post(`${this.apiUrl}/register`, data)
    }

    login(data: {email: string; password: string;}){
        return this.http.post<{token: string}>(`${this.apiUrl}/login`, data).pipe(
            tap(response => {
                localStorage.setItem('token', response.token);
            })
        )
    }

    logout(){
        localStorage.removeItem('token');
    }

    getToken(): string | null {return localStorage.getItem('token')
    }

    getUserId(): string | null {
        const token = this.getToken();
        if (!token) return null;

        const payload = JSON.parse(atob(token.split('.')[1]));
        return payload.sub;
    }

    isLoggedIn(): boolean {return !!this.getToken();
    }

}

