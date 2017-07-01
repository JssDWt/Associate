using System;
using System.Collections.Generic;
using System.IO;

public class User : UserLink
{
    public DateTime BirthDate { get; set; }
    public ICollection<GroupLink> Groups { get; set; }
}

public class UserLink
{
    public long Id { get; set; }
    public string Name { get; set; }
    public Stream ProfileImage { get; set; }
}