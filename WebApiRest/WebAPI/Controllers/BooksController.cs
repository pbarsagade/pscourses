using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    [Route("api/authors/{authorId}/books")]
    public class BooksController : Controller
    {
        private readonly ILiberaryRepository repository;

        public BooksController(ILiberaryRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult GetBooksForAuthor(int authorId)
        {
            if (!this.repository.AuthorExists(authorId))
                return NotFound();

            var books = this.repository.GetBooksForAuthor(authorId);
            IEnumerable<BookDto> booksDto = Mapper.Map<IEnumerable<BookDto>>(books);
            return Ok(booksDto);
        }

        [HttpGet("{bookId}")]
        public IActionResult GetBookForAuthor(int authorId, int bookId)
        {
            if (!this.repository.AuthorExists(authorId))
                return NotFound();

            var book = this.repository.GetBookForAuthor(authorId, bookId);
            if (book == null)
                return NotFound();

            BookDto bookDto = Mapper.Map<BookDto>(book);
            return Ok(bookDto);
        }
    }
}