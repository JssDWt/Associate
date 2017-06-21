import { GroupLink } from './group-link';

export class Group extends GroupLink {
  subGroups: GroupLink[];
  parentGroup: GroupLink;
}