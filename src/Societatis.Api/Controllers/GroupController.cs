namespace Societatis.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Societatis.Api.Data;
    using Societatis.Api.Models;
    using Societatis.HAL;
    using Societatis.Misc;

    [Route("api/[controller]")]
    public class GroupController : Controller
    {
        private long userId;
        private IGroupRepository repository;

        public GroupController(
            IGroupRepository repository, 
            long userId)
        {
            repository.ThrowIfNull(nameof(repository));
            this.repository = repository;
            this.userId = userId;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Group> Get()
        {
            var groups = this.repository.GetGroups(this.userId);
            return groups;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Group), (int)System.Net.HttpStatusCode.OK)]
        public IActionResult Get(long id)
        {
            var group = this.repository.GetGroup(id);
            var selfLink = new Link(new Uri(Url.Action("Get", "Groups", new {id = id})));
            group.Links.Set("self", selfLink);
            return this.Ok(group);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
