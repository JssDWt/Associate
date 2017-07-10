namespace Societatis.Api.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Societatis.Api.Models;
    using Societatis.Misc;

    public class Repository : IGroupRepository
    {
        private object padlock = new object();

        private ICollection<Group> groups;
        private ICollection<User> users;

        public Repository()
        {
            this.groups = new List<Group>();
            this.users = new List<User>();
        }

        /// <summary>
        /// Gets all groups for the specifid user id.
        /// </summary>
        /// <param name="userId">The id of the user to get the groups for.</param>
        /// <returns>All groups the specified user is a member of.</returns>
        public IEnumerable<Group> GetGroups(long userId)
        {
            // NOTE: No need to materialize this.
            var groups = this.groups; // .Where(g => g.Members.Any(m => m.Id == userId));

            return groups;
        }

        public Group GetGroup(long id)
        {
            var group = this.groups.SingleOrDefault(g => g.Id == id);
            if (group == null)
            {
                throw new DataNotFoundException($"Cannot find group with ID {id}.");
            }

            return group;
        }

        /// <summary>
        /// Adds a root group to storage.
        /// </summary>
        /// <param name="group">The group to store.</param>
        public void AddGroup(Group group)
        {
            group.ThrowIfNull(nameof(group));
            this.AddGroup(group, parent: null);
        }

        /// <summary>
        /// Adds a group to the storage with a parent, specified with a parent id.
        /// </summary>
        /// <param name="group">The group to store.</param>
        /// <param name="parentId">THe id of the parent group.</param>
        public void AddGroup(Group group, long parentId)
        {
            group.ThrowIfNull(nameof(group));
            if (parentId <= 0) throw new ArgumentOutOfRangeException(nameof(parentId), "ID must be positive.");

            var parent = this.groups.SingleOrDefault(g => g.Id == parentId);
            if (parent == null)
            {
                throw new InvalidOperationException($"Parent group with id {parentId} does not exist.");
            }

            this.AddGroup(group, parent);
        }

        private void AddGroup(Group group, Group parent)
        {
            if (string.IsNullOrWhiteSpace(group.Name)) throw new ArgumentException("Group must have a name.", "group");

            // group.Members = new List<UserLink>();

            // // TODO: Check whether this will be a circular reference.
            // group.ParentGroup = parent;
            // lock (this.padlock)
            // {
            //     long id;
            //     if (this.groups.Any())
            //     {
            //         id = this.groups.Max(g => g.Id) + 1;
            //     }
            //     else
            //     {
            //         id = 1;
            //     }

            //     group.Id = id;
            // }

            // group.SubGroups = new List<GroupLink>();

            // TODO: Check whether the group already exists.
            this.groups.Add(group);
        }

        public void AddUser(User user)
        {
            user.ThrowIfNull(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Name)) throw new ArgumentException("User must have a name.", nameof(user));

            //user.Groups = new List<GroupLink>();
            lock (this.padlock)
            {
                long id;
                if (this.users.Any())
                {
                    id = this.users.Max(u => u.Id) + 1;
                }
                else
                {
                    id = 1;
                }

                user.Id = id;
            }

            this.users.Add(user);
        }

        public void AddUserToGroup(long userId, long groupId)
        {
            var user = this.users.SingleOrDefault(u => u.Id == userId);
            if (user == null) throw new InvalidOperationException($"User with ID {userId} does not exist.");
            var group = this.groups.SingleOrDefault(g => g.Id == groupId);
            if (group == null) throw new InvalidOperationException($"Group with ID {groupId} does not exist.");

            // TODO: create logic to add the member to parent groups, or 'inherit' members from child groups.
            // if (!user.Groups.Contains(group))
            // {
            //     user.Groups.Add(group);
            // }

            // if (!group.Members.Contains(user))
            // {
            //     group.Members.Add(user);
            // }
        }
        private void InitializeData()
        {
            // NOTE: This method only works because the groups being stored are the same references as the groups passed in to repository methods.
            var ksv = new Group();
            ksv.Name = "K.S.V. st. Franciscus Xaverius";
            this.AddGroup(ksv);

            var knights = new Group();
            knights.Name = "W.H.D. The Knights of the Round Table";
            this.AddGroup(knights, ksv);

            var be = new Group();
            be.Name = "W.H.D. La Brasserie Elitaire";
            this.AddGroup(be, ksv);

            var jarenPlan = new Group();
            jarenPlan.Name = "Borrelpartij het Vijfjarenplan";
            this.AddGroup(jarenPlan, ksv);

            var jesse = new User();
            jesse.Name = "Jesse de Wit";
            jesse.BirthDate = new DateTime(1988, 5, 27);
            this.AddUser(jesse);
            this.AddUserToGroup(jesse.Id, ksv.Id);
            this.AddUserToGroup(jesse.Id, knights.Id);
            this.AddUserToGroup(jesse.Id, jarenPlan.Id);

            var cass = new User();
            cass.Name = "Cass Gooskens";
            cass.BirthDate = new DateTime(1989, 6, 28);
            this.AddUser(cass);
            this.AddUserToGroup(cass.Id, ksv.Id);
            this.AddUserToGroup(cass.Id, knights.Id);
            this.AddUserToGroup(cass.Id, jarenPlan.Id);

            var jan = new User();
            jan.Name = "Jan Jansen";
            this.AddUser(jan);
            this.AddUserToGroup(jan.Id, ksv.Id);
            this.AddUserToGroup(jan.Id, knights.Id);
            this.AddUserToGroup(jan.Id, jarenPlan.Id);
        }
    }
}