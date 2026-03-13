import { Component, OnInit } from '@angular/core';
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

  constructor(
    private userService: UserService,
    private authService: AuthService
  ) {
    this.currentUser = this.authService.currentUser();
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers().subscribe(users => {
      this.users = users;
    });
  }

  deactivate(id: string) {
    if (confirm('Are you sure you want to deactivate this user?')) {
      this.userService.deactivateUser(id).subscribe(() => this.loadUsers());
    }
  }
}
