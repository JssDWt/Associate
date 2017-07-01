import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';

import { User } from '../_models';

@Injectable()
export class UserService {
    public currentUser: User;

    public getUser(userName: string, password: string) : Observable<User> {
        if (userName !== 'Jesse') {
            return Observable.of(null);
        }
        
        let user: User = new User();
        user.id = 1;
        user.name = userName;
        user.groups = [];
        return Observable.of(user);
    }
}