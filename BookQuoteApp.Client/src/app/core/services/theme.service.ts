import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly storageKey = 'theme';

  constructor() {
    const savedTheme = localStorage.getItem(this.storageKey) || 'light';
    this.setTheme(savedTheme);
  }

  toggleTheme() {
    const current = document.documentElement.getAttribute('data-bs-theme');
    const newTheme = current === 'dark' ? 'light' : 'dark';
    this.setTheme(newTheme);
  }

  setTheme(theme: string) {
    document.documentElement.setAttribute('data-bs-theme', theme);
    localStorage.setItem(this.storageKey, theme);
  }

  getCurrentTheme(): string {
    return document.documentElement.getAttribute('data-bs-theme') || 'light';
  }
}