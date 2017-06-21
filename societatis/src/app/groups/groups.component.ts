import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Rx';

import { Group } from '../_models';
import { GroupService } from '../_services';


@Component({
  templateUrl: './groups.component.html'
})
export class GroupsComponent implements OnInit { 
  
  groups: Group[];
  
  constructor(
    private service: GroupService){ }
  
  ngOnInit() : void {
    this.getGroups();
  }
  
  getGroups() : void {
    // TODO: Do something useful with the error.
    this.service.getGroups()
    .subscribe(
      groups => this.groups = groups,
      error =>  console.log(error));
  }
}