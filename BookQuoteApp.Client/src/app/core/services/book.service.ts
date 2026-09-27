import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environments";

export interface Book {
    id: number;
    title: string;
    authorName: string;
    publisherName: string | null;
    publishDate: string;
}

@Injectable({providedIn: 'root'})
export class BookService {
    private readonly apiUrl = `${environment.apiUrl}/auth`;

    constructor(private http: HttpClient) {}

    getAll(){
        return this.http.get<Book[]>(this.apiUrl);
    }

    getById(id: number) {
        return this.http.get<Book>(`${this.apiUrl}/${id}`);
    }

    create(data: { title: string; authorName: string; publisherName: string | null; publishDate: string;}) {
        return this.http.post<Book>(this.apiUrl, data);
    }

    update(id: number, data: {title: string; authorName: string; publisherName: string | null; publishDate: string}) {
        return this.http.put(`${this.apiUrl}/${id}`, data);
    }

    delete(id: number) {
        return this.http.delete(`${this.apiUrl}/${id}`);
    }

}