namespace Societatis.Api.Data
{
    using System.Collections.Generic;
    using Societatis.Api.Models;

    public interface IGroupRepository
    {
        Group GetGroup(long id);
        IEnumerable<Group> GetGroups(long userId);
        void AddGroup(Group group);
        void AddGroup(Group group, long parentId);
        void AddUserToGroup(long userId, long groupId);
    }
}