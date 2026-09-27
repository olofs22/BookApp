import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BookService, Book } from '../../../core/services/book.service';
@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './book-list.html',
  styleUrl: './book-list.scss',
})
export class BookList implements OnInit{
  books: Book[] = [];

  constructor(private bookService: BookService) {}

  ngOnInit() {
    this.loadBooks();
  }

  loadBooks() {
    this.bookService.getAll().subscribe({
      next: (data) => this.books = data,
      error: (err) => console.error('Failed to load books', err)
    });
  }

  deleteBooks(id: number) {
    if(!confirm('Are you sure you want to delete this book?')) return; 

    this.bookService.delete(id).subscribe({
      next: () => this.loadBooks(),
      error: (err) => console.error('Failed to delete book', err)
    });
  }

}
