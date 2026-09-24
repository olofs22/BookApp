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

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private router: Router
    ) {
        this.registerForm = this.fb.group({
            name: ['', Validators.required],
            email: ['', Validators.required, Validators.email],
            password: ['', Validators.required, Validators.minLength(6)]
        });
    }
    onSubmit() {
        if (this.registerForm.invalid) return;

        this.authService.register(this.registerForm.value as {name: string; email: string; password: string;}).subscribe({
            next: () => this.router.navigate(['./login']),
            error: () => this.errorMessage = 'Regsitration failed'
        });
    }
}