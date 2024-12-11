using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Requests.ReadingRequest;
using VkCore.Requests.Word;
using VkInfrastructure.Data;
using VkInfrastructure.DefinitionSources;
using VkWeb.Extensions;

namespace VkWeb.Controllers
{

    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly VkDbContext _context;

        public WordController(
            IMediator mediator,
            VkDbContext context
        )
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet]
        [Route("api/word/{word}")]
        public async Task<IActionResult> Get(string word, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetDefinitionsRequest(word, User.GetId()), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("api/word/definition/{annotationId}")]
        public async Task<IActionResult> DeleteAnnotation(string annotationId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new DeleteDefinitionRequest(annotationId, User.GetId()), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("api/word/sources")]
        public IActionResult Get()
        {
            return Ok(new StaticDefinitionSourcesLister().GetAllSources());
        }



        [HttpGet]
        [Route("api/word/clean")]
        public async Task<IActionResult> CleanWordsAsync()
        {
            var annotations = _context.Annotations.Where(w => string.IsNullOrEmpty(w.Value)).ToList();
            var tasks = new List<Task>();
            foreach (var annotation in annotations)
            {
                var contentItems = _context.ContentItems.Where(c => c.AnnotationId == annotation.Id).ToList();
                foreach (var item in contentItems)
                {
                    var remove = new RemoveDefinitionRequest(item.ReadingId, item.Id);
                    await _mediator.Send(remove);
                }
            }

            return Ok($"{annotations.Count()} definitions cleaned");
        }
    }
}