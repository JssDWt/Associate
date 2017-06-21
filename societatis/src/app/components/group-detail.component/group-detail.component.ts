import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/takeUntil';

import { Group } from '../../models/group';
import { GroupsService } from '../../services/groups.service/groups.service';

@Component({
   templateUrl: './group-detail.component.html'
})
export class GroupDetailComponent implements OnInit, OnDestroy  { 
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  group: Group;
  
  constructor(
    private route: ActivatedRoute,
    private service: GroupsService
  ) { }
  
  ngOnInit() : void {
    
    // TODO: unsubscribe from params
    this.route.params
      // (+) converts string 'id' to a number
      .switchMap((params: Params) => this.service.getGroup(+params['id']))
      .subscribe((group: Group) => this.group = group);
      
  }
  
  ngOnDestroy() : void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}