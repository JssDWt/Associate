import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule }   from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { NgbModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './components/app.component/app.component';
import { NavbarComponent } from './components/navbar.component/navbar.component';
import { GroupsComponent } from './components/groups.component/groups.component';
import { GroupDetailComponent } from './components/group-detail.component/group-detail.component';
import { NewGroupComponent } from './components/new-group.component/new-group.component';
import { GroupsService } from './services/groups.service/groups.service';

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
