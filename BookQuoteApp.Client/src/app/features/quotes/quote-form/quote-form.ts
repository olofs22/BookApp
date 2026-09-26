import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { QuoteService } from '../../../core/services/quote.service';
import { BookService, Book } from '../../../core/services/book.service';

@Component({
  selector: 'app-quote-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './quote-form.html',
  styleUrl: './quote-form.scss'
})
export class QuoteForm implements OnInit {
  quoteForm: FormGroup;
  isEditMode = false;
  quoteId: number | null = null;
  errorMessage = '';
  books: Book[] = [];

  constructor(
    private fb: FormBuilder,
    private quoteService: QuoteService,
    private bookService: BookService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.quoteForm = this.fb.group({
      text: ['', Validators.required],
      authorName: ['', Validators.required],
      bookId: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.loadBooks();

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEditMode = true;
      this.quoteId = Number(idParam);
      this.loadQuote(this.quoteId);
    }
  }

  loadBooks() {
    this.bookService.getAll().subscribe({
      next: (data) => this.books = data,
      error: (err) => console.error('Failed to load books', err)
    });
  }

  loadQuote(id: number) {
    this.quoteService.getById(id).subscribe({
      next: (quote) => {
        this.quoteForm.patchValue({
          text: quote.text,
          authorName: quote.authorName,
          bookId: quote.bookid
        });
      },
      error: () => this.errorMessage = 'Kunde inte hämta citatet'
    });
  }

  onSubmit() {
    if (this.quoteForm.invalid) return;

    const data = {
      ...this.quoteForm.value,
      bookId: Number(this.quoteForm.value.bookId)
    };

    if (this.isEditMode && this.quoteId) {
      this.quoteService.update(this.quoteId, data).subscribe({
        next: () => this.router.navigate(['/quotes']),
        error: () => this.errorMessage = 'Kunde inte uppdatera citatet'
      });
    } else {
      this.quoteService.create(data).subscribe({
        next: () => this.router.navigate(['/quotes']),
        error: () => this.errorMessage = 'Kunde inte skapa citatet'
      });
    }
  }
}