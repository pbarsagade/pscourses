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

        public AuthorsController(ILiberaryRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult GetAuthors()
        {
            var authors = repository.GetAuthors();
            IEnumerable<AuthorDto> authorsDto = Mapper.Map<IEnumerable<AuthorDto>>(authors);

            return Ok(authorsDto);
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