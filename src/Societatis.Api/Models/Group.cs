namespace Societatis.Api.Models
{
    using System.Collections.Generic;
    using System.IO;
    using Societatis.HAL;

    public class Group : Resource
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Stream ProfileImage { get; set; }
    }
}