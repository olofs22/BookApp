import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { QuoteService, Quote } from '../../../core/services/quote.service';

@Component({
  selector: 'app-quote-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './quote-list.html',
  styleUrl: './quote-list.scss'
})
export class QuoteList implements OnInit {
  quotes: Quote[] = [];

  constructor(private quoteService: QuoteService) {}

  ngOnInit() {
    this.loadQuotes();
  }

  loadQuotes() {
    this.quoteService.getAll().subscribe({
      next: (data) => this.quotes = data,
      error: (err) => console.error('Failed to load quotes', err)
    });
  }

  deleteQuote(id: number) {
    if (!confirm('Är du säker på att du vill radera citatet?')) return;

    this.quoteService.delete(id).subscribe({
      next: () => this.loadQuotes(),
      error: (err) => console.error('Failed to delete quote', err)
    });
  }
}