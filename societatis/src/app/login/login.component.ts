import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
 
import { UserService } from '../_services';
 
@Component({
    moduleId: module.id,
    templateUrl: 'login.component.html'
})
 
export class LoginComponent implements OnInit {
    model: any = {};
    loading = false;
    error = '';
 
    constructor(
        private router: Router,
        private userService: UserService) { }
 
    ngOnInit() {
        // reset login status
        this.userService.currentUser = null;
    }
 
    login() {
        this.loading = true;
        this.userService.getUser(this.model.username, this.model.password)
            .subscribe(user => {
                if(user == null) {
                    this.error = 'Username or password is incorrect';
                    this.loading = false;
                } else {
                    // login successful
                    this.userService.currentUser = user;
                    this.router.navigate(['/']);
                }
            },
            error => this.error = error);
    }
}