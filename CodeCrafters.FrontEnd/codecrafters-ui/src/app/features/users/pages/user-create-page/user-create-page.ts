import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../auth/services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-create-page',
  standalone: false,
  templateUrl: './user-create-page.html',
  styleUrls: ['../../../auth/pages/login/login-page.scss']
})
export class UserCreatePageComponent {
  createForm: FormGroup;
  errorMessage = '';
  
  roles = ['PlatformAdmin', 'ProgramOfficer', 'GrantReviewer', 'FinanceOfficer', 'Applicant'];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {
    this.createForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['Applicant', Validators.required]
    });
  }

  onSubmit() {
    if (this.createForm.valid) {
      this.userService.createUser(this.createForm.value).subscribe({
        next: () => {
          this.router.navigate(['/users']);
        },
        error: (err) => {
          this.errorMessage = err?.error?.message || 'Failed to create user';
        }
      });
    }
  }
}
