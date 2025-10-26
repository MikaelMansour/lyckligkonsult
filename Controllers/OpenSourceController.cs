using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace LyckligKonsult.Controllers
{
    [Route("umbraco/api/[controller]/[action]")]
    public class OpenSourceController : UmbracoApiController
    {
        private readonly IContentService _contentService;
        private readonly IUmbracoContextAccessor _contextAccessor;

        public OpenSourceController(IContentService contentService, IUmbracoContextAccessor contextAccessor)
        {
            _contentService = contentService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        public IActionResult SubmitProject([FromBody] ProjectDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid project data.");

            try
            {
                // Change this to the parent node ID where OpenItems are stored
                const int parentId = 1077;

                var content = _contentService.Create(model.Name, parentId, "libraryItem");

                content.SetValue("title", model.Name);
                content.SetValue("description", model.Description);
                content.SetValue("tags", model.Category);
                content.SetValue("author", model.Author);
                content.SetValue("link", model.Github);

                _contentService.Save(content); // Save as UNPUBLISHED

                return Ok(new { success = true, message = "Project submitted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        public class ProjectDto
        {
            public string Name { get; set; } = "";
            public string Description { get; set; } = "";
            public string Category { get; set; } = "";
            public string Author { get; set; } = "";
            public string Github { get; set; } = "";
        }
    }
}
