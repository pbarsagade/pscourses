using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helper;
using WebAPI.Models;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/AuthorsCollection")]
    public class AuthorCollectionController : Controller
    {
        private readonly ILiberaryRepository repository;

        public AuthorCollectionController(ILiberaryRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public IActionResult CreateAuthorCollection([FromBody]IEnumerable<NewAuthorDto> authorCollection)
        {
            if (authorCollection == null)
                return BadRequest();

            var authorEntities = Mapper.Map<IEnumerable<Data.Author>>(authorCollection);
            foreach (var author in authorEntities)
            {
                repository.AddAuthor(author);
            }

            if (!repository.Save())
                throw new Exception("Creating and author collection failed on save");

            var authorsToReturn = Mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

            var idsAsString = string.Join(",", authorsToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorsToReturn);
        }

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<int> ids)
        {
            if (ids == null)
                return BadRequest();

            var authorEntities = repository.GetAuthors(ids);
            if (authorEntities.Count() != ids.Count())
                return NotFound();

            var authorsToReturn = Mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            return Ok(authorsToReturn);

        }
    }
}