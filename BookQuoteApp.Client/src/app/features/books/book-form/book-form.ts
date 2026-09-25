import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BookService } from '../../../core/services/book.service';

@Component({
  selector: 'app-book-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink ],
  templateUrl: './book-form.html',
  styleUrl: './book-form.scss'
})
export class BookForm implements OnInit {
  bookForm: FormGroup;
  isEditMode = false;
  bookId: number | null = null;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.bookForm = this.fb.group({
      title: ['', Validators.required],
      authorName: ['', Validators.required],
      publisherName: [''],
      publishDate: ['', Validators.required]
    });
  }

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.isEditMode = true;
      this.bookId = Number(idParam);
      this.loadBook(this.bookId);
    }
  }

  loadBook(id: number) {
    this.bookService.getById(id).subscribe({
      next: (book) => {
        this.bookForm.patchValue({
          title: book.title,
          authorName: book.authorName,
          publisherName: book.publisherName,
          publishDate: book.publishDate.substring(0, 10)
        });
      },
      error: () => this.errorMessage = 'Kunde inte hämta boken'
    });
  }

  onSubmit() {
    if (this.bookForm.invalid) return;

    const data = this.bookForm.value;

    if (this.isEditMode && this.bookId) {
      this.bookService.update(this.bookId, data).subscribe({
        next: () => this.router.navigate(['/books']),
        error: () => this.errorMessage = 'Kunde inte uppdatera boken'
      });
    } else {
      this.bookService.create(data).subscribe({
        next: () => this.router.navigate(['/books']),
        error: () => this.errorMessage = 'Kunde inte skapa boken'
      });
    }
  }
}