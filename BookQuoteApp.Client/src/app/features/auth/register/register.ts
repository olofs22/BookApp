import { Component } from "@angular/core";
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { RouterLink, Router, RouterConfigOptions } from "@angular/router";
import { AuthService } from "../../../core/services/auth.service";

@Component ({
    selector: 'app-register',
    standalone: true,
    imports: [ReactiveFormsModule, CommonModule, RouterLink],
    templateUrl: './register.html',
    styleUrl: './register.scss'
})

export class Register {
    registerForm: FormGroup;
    errorMessage = '';

    get pw(): string {
  return this.registerForm.get('password')?.value ?? '';
    }
    get hasLength(): boolean { return this.pw.length >= 8; }
    get hasUpper(): boolean { return /[A-Z]/.test(this.pw); }
    get hasDigit(): boolean { return /\d/.test(this.pw); }

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private router: Router
    ) {
        this.registerForm = this.fb.group({
            name: ['', Validators.required],
            email: ['', Validators.required, Validators.email],
            password: ['', Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Z])(?=.*\d).+$/)]
        });
    }
    onSubmit() {
        if (this.registerForm.invalid) return;

        this.authService.register(this.registerForm.value as {name: string; email: string; password: string;}).subscribe({
            next: () => this.router.navigate(['./login']),
            error: (err) => this.errorMessage = Array.isArray(err.error) ? err.error.map((e: any) => e.description).join('') : 'Regsitration failed'
        });
    }
}