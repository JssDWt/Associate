import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule }   from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { NgbModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { GroupsComponent } from './groups/groups.component';
import { GroupDetailComponent } from './group-detail/group-detail.component';
import { NewGroupComponent } from './new-group/new-group.component';
import { GroupsService } from './_services/groups.service';

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
