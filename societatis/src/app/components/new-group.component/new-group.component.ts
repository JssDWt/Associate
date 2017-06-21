import { Component, ViewChild, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

import { Group } from '../../models/group';
import { GroupsService } from '../../services/groups.service/groups.service';

@Component({
  templateUrl: './new-group.component.html'
})
export class NewGroupComponent implements OnInit { 
  imageUri: any;
  group: Group;
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private groupsService: GroupsService,
    private modalService: NgbModal
  ) { }
  
  ngOnInit(): void {
    this.group = new Group();
    // TODO: Unsubscribe from route
    this.route.queryParams
    .subscribe(params => {
      console.log('subscribe param. params: ', params, 'parentId', params['parentId']);
      if (params['parentId'] != null) {
        this.groupsService.getGroup(+params['parentId'])
          .subscribe(group => this.group.parentGroup = group);
      }
    });
  }

  onSubmit(event: any): void {
    this.groupsService.createGroup(this.group)
      .subscribe((group: Group) => this.router.navigate(['/groups', group.id]));
  }
  
  onFileChange($event) : void {
    let inputValue: any = $event.target;
    let file:File = inputValue.files[0];
    let reader:FileReader = new FileReader();

    reader.onloadend = (e) => {
      this.imageUri = reader.result;
    }
    reader.readAsDataURL(file);
  }

  open(content) {
    this.modalService.open(content).result.then((result) => {
      this.group.profileImage = result; 
    }, (reason) => {
      // Do something if cross was clicked.
    });
  }
}