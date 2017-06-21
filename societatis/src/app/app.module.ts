import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule }   from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { HttpModule } from '@angular/http';

import { NgbModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar';
import { GroupsComponent } from './groups';
import { GroupDetailComponent } from './group-detail';
import { NewGroupComponent } from './new-group';
import { LoginComponent } from './login';
import { GroupService } from './_services';
import { UserService } from './_services';
import { AuthGuard } from './_guards';

const appRoutes: Routes = [
  { path: 'groups', component: GroupsComponent, canActivate: [AuthGuard] },
  { path: 'groups/:id', component: GroupDetailComponent, canActivate: [AuthGuard] },
  { path: 'new-group', component: NewGroupComponent, canActivate: [AuthGuard] },
  { path: '',   redirectTo: '/groups', pathMatch: 'full' },
  { path: 'login', component: LoginComponent }
]; 

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    GroupsComponent,
    GroupDetailComponent,
    NewGroupComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot(appRoutes),
    NgbModule.forRoot(),
    HttpModule
  ],
  providers: [
    AuthGuard,
    UserService,
    GroupService, 
    NgbModal],
  bootstrap: [AppComponent],
})
export class AppModule { }
