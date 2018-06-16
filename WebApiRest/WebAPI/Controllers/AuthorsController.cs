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

        [HttpGet("{id}")]
        public IActionResult GetAuthors(int id)
        {
            var author = repository.GetAuthor(id);

            if (author == null)
                return NotFound();

            AuthorDto authorDto = Mapper.Map<AuthorDto>(author);

            return Ok(authorDto);
        }
    }
}