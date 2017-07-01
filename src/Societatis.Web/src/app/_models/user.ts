import { UserLink } from './user-link';
import { GroupLink } from './group-link';

export class User extends UserLink {
    birthDate?: Date;
    groups: GroupLink[];
}