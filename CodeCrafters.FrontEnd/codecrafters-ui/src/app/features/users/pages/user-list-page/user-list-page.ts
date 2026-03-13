import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { UserService } from '../../../auth/services/user.service';
import { AuthService } from '../../../auth/services/auth.service';
import { UserResponseDto } from '../../../auth/models/auth.models';

@Component({
  selector: 'app-user-list-page',
  standalone: false,
  templateUrl: './user-list-page.html',
  styleUrls: ['./user-list-page.scss']
})
export class UserListPageComponent implements OnInit {
  users: UserResponseDto[] = [];
  currentUser: UserResponseDto | null = null;
  isLoading = true;
  error: string | null = null;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {
    this.currentUser = this.authService.currentUser();
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.isLoading = true;
    this.error = null;
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load users', err);
        this.error = 'Failed to load users. You might not have permission to view this list.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  deactivate(id: string) {
    if (confirm('Are you sure you want to deactivate this user?')) {
      this.userService.deactivateUser(id).subscribe(() => this.loadUsers());
    }
  }
}
