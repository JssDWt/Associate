import { GroupLink } from './group-link';
import { UserLink } from './user-link';

export class Group extends GroupLink {
  subGroups: GroupLink[];
  parentGroup: GroupLink;
  members: UserLink[];
}