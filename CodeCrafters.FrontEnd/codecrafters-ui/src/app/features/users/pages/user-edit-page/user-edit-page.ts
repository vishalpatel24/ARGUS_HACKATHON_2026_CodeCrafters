import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../auth/services/user.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-edit-page',
  standalone: false,
  templateUrl: './user-edit-page.html',
  styleUrls: ['../../../auth/pages/login/login-page.scss']
})
export class UserEditPageComponent implements OnInit {
  editForm: FormGroup;
  errorMessage = '';
  userId: string = '';
  
  roles = ['PlatformAdmin', 'ProgramOfficer', 'GrantReviewer', 'FinanceOfficer', 'Applicant'];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.editForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      role: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('id') || '';
    if (this.userId) {
      this.userService.getUsers().subscribe(users => {
        const user = users.find(u => u.id === this.userId);
        if (user) {
          this.editForm.patchValue({
            name: user.name,
            email: user.email,
            phone: user.phone,
            role: user.role
          });
        }
      });
    }
  }

  onSubmit() {
    if (this.editForm.valid) {
      this.userService.updateUser(this.userId, this.editForm.value).subscribe({
        next: () => {
          this.router.navigate(['/users']);
        },
        error: (err) => {
          this.errorMessage = err?.error?.message || 'Failed to update user';
        }
      });
    }
  }
}
