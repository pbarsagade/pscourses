using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repository;
using WebAPI.Helper;
using AutoMapper;

namespace WebAPI.Controllers
{
    [Route("api/Authors")]
    public class AuthorsController : Controller
    {
        private readonly ILiberaryRepository repository;
        private readonly IUrlHelper urlHelper;

        public AuthorsController(ILiberaryRepository repository,
            IUrlHelper urlHelper)
        {
            this.repository = repository;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetAuthors")]
        public IActionResult GetAuthors(AuthorsResourceParameters resourceParameters)
        {
            var authors = repository.GetAuthors(resourceParameters);
            IEnumerable<AuthorDto> authorsDto = Mapper.Map<IEnumerable<AuthorDto>>(authors);

            var previousPageLink = authors.HasPrevious ?
            CreateAuthorsResourceUri(resourceParameters,
            ResourceUriType.PreviousPage) : null;

            var nextPageLink = authors.HasNext ?
                CreateAuthorsResourceUri(resourceParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = authors.TotalCount,
                pageSize = authors.PageSize,
                currentPage = authors.CurrentPage,
                totalPages = authors.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(authorsDto);
        }

        private string CreateAuthorsResourceUri(
           AuthorsResourceParameters authorsResourceParameters,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("GetAuthors",
                      new
                      {
                          searchQuery = authorsResourceParameters.SearchQuery,
                          genre = authorsResourceParameters.Genre,
                          pageNumber = authorsResourceParameters.PageNumber - 1,
                          pageSize = authorsResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("GetAuthors",
                      new
                      {
                          searchQuery = authorsResourceParameters.SearchQuery,
                          genre = authorsResourceParameters.Genre,
                          pageNumber = authorsResourceParameters.PageNumber + 1,
                          pageSize = authorsResourceParameters.PageSize
                      });

                default:
                    return urlHelper.Link("GetAuthors",
                    new
                    {
                        searchQuery = authorsResourceParameters.SearchQuery,
                        genre = authorsResourceParameters.Genre,
                        pageNumber = authorsResourceParameters.PageNumber,
                        pageSize = authorsResourceParameters.PageSize
                    });
            }
        }


        [HttpGet("{id}", Name = "GetAuthor")]
        public IActionResult GetAuthors(int id)
        {
            var author = repository.GetAuthor(id);

            if (author == null)
                return NotFound();

            AuthorDto authorDto = Mapper.Map<AuthorDto>(author);

            return Ok(authorDto);
        }


        [HttpPost]
        public IActionResult CreateAuthor([FromBody] NewAuthorDto author)
        {
            if (author == null)
                return BadRequest();

            var authorEntity = Mapper.Map<Data.Author>(author);
            this.repository.AddAuthor(authorEntity);

            if (!repository.Save())
                throw new Exception("Creating an author failed on save");

            var newAuthor = Mapper.Map<AuthorDto>(authorEntity);

            return CreatedAtRoute("GetAuthor", new { id = newAuthor.Id }, newAuthor);
        }

        [HttpPost("{id}")]
        public IActionResult BlockAuthorCreation(int id)
        {
            if (repository.AuthorExists(id))
                return new StatusCodeResult(StatusCodes.Status409Conflict);

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var authorToDelete = repository.GetAuthor(id);
            if (authorToDelete == null)
                return NotFound();

            repository.DeleteAuthor(authorToDelete);
            if (!repository.Save())
                throw new Exception($"Deleting author {id} failed on save");

            return NoContent();
        }
    }
}