import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-page',
  standalone: false,
  templateUrl: './register-page.html',
  styleUrls: ['../login/login-page.scss'] 
})
export class RegisterPageComponent {
  registerForm: FormGroup;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password')?.value === g.get('confirmPassword')?.value
      ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const { confirmPassword, ...data } = this.registerForm.value;
      this.authService.register(data).subscribe({
        next: () => {
          this.authService.login({ email: data.email, password: data.password }).subscribe(
            () => this.router.navigate(['/dashboard'])
          );
        },
        error: (err) => {
          this.errorMessage = err?.error?.message || 'Registration failed';
        }
      });
    }
  }
}
