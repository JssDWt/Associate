namespace Societatis.Api.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}