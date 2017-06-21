import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule }   from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { NgbModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar';
import { GroupsComponent } from './groups';
import { GroupDetailComponent } from './group-detail';
import { NewGroupComponent } from './new-group';
import { GroupsService } from './_services';

const appRoutes: Routes = [
  { path: 'groups', component: GroupsComponent },
  { path: 'groups/:id', component: GroupDetailComponent },
  { path: 'new-group', component: NewGroupComponent },
  { path: '',   redirectTo: '/groups', pathMatch: 'full' },
]; 

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    GroupsComponent,
    GroupDetailComponent,
    NewGroupComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot(appRoutes),
    NgbModule.forRoot()
  ],
  providers: [GroupsService, NgbModal],
  bootstrap: [AppComponent],
})
export class AppModule { }
