import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

export interface Quote {
  id: number;
  text: string;
  authorName: string;
  bookTitle: string;
  uploadTime: string;
}

@Injectable({ providedIn: 'root' })
export class QuoteService {
  private readonly apiUrl = 'http://localhost:5199/api/quotes';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Quote[]>(this.apiUrl);
  }

  getById(id: number) {
    return this.http.get<Quote>(`${this.apiUrl}/${id}`);
  }

  create(data: { text: string; authorName: string; bookId: number }) {
    return this.http.post<Quote>(this.apiUrl, data);
  }

  update(id: number, data: { text: string; authorName: string; bookId: number }) {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}