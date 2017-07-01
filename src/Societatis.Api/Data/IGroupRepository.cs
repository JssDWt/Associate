using System.Collections.Generic;

public interface IGroupRepository
{
    Group GetGroup(long id);
    IEnumerable<GroupLink> GetGroups(long userId);
    void AddGroup(Group group);
    void AddGroup(Group group, long parentId);
    void AddUserToGroup(long userId, long groupId);
}