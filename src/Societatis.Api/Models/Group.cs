using System.Collections.Generic;
using System.IO;

public class Group : Resource
{
    public long Id { get; set; }
    public string Name { get; set; }
    public Stream ProfileImage { get; set; }
    public GroupLink ParentGroup { get; set; }
    public ICollection<GroupLink> SubGroups { get; set; }
    public ICollection<UserLink> Members { get; set; }
}