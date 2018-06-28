using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Helper;
using WebAPI.Models;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    [Route("api/authors/{authorId}/books")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> logger;
        private readonly ILiberaryRepository repository;

        public BooksController(ILiberaryRepository repository, ILogger<BooksController> logger)
        {
            this.logger = logger;
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

        [HttpGet("{bookId}", Name = "GetBook")]
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

        [HttpPost]
        public IActionResult CreateBookForAuthor(int authorId, [FromBody] NewBookDto book)
        {
            if (book == null)
                return BadRequest();

            if (book.Title.Equals(book.Description))
                ModelState.AddModelError(nameof(BookDto), "Title and description of book can't be same");

            if (!ModelState.IsValid)
            {
                return new UnProcessableEntityObjectResult(ModelState);
            }

            var bookEntity = Mapper.Map<Data.Book>(book);
            this.repository.AddBookForAuthor(authorId, bookEntity);

            if (!repository.Save())
                throw new Exception("Adding book for an author failed on save");

            var newBook = Mapper.Map<BookDto>(bookEntity);

            return CreatedAtRoute("GetBook", new { authorId = authorId, bookId = newBook.Id }, newBook);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookForAuthor(int authorId, int id)
        {
            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookForAuthorFromRepo = repository.GetBookForAuthor(authorId, id);
            if (bookForAuthorFromRepo == null)
            {
                return NotFound();
            }

            repository.DeleteBook(bookForAuthorFromRepo);

            if (!repository.Save())
            {
                throw new Exception($"Deleting book {id} for author {authorId} failed on save.");
            }

            logger.LogInformation(100, $"Book {id} Deleted for author {authorId}");

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBookForAuthor(int authorId, int id, [FromBody] EditBookDto book)
        {
            if (book == null)
                return BadRequest();

            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookForAuthorFromRepo = repository.GetBookForAuthor(authorId, id);
            if (bookForAuthorFromRepo == null)
            {
                return NotFound();
            }

            Mapper.Map(book, bookForAuthorFromRepo);

            repository.UpdateBookForAuthor(bookForAuthorFromRepo);

            if (!repository.Save())
            {
                throw new Exception($"Updating book {id} for author {authorId} failed on save.");
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateBookForAuthor(int authorId, int id,
           [FromBody] JsonPatchDocument<EditBookDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookForAuthorFromRepo = repository.GetBookForAuthor(authorId, id);

            if (bookForAuthorFromRepo == null)
            {
                return NotFound();
            }

            var bookToPatch = Mapper.Map<EditBookDto>(bookForAuthorFromRepo);
            patchDoc.ApplyTo(bookToPatch);
            // patchDoc.ApplyTo(bookToPatch,ModelState);
            if (bookToPatch.Description == bookToPatch.Title)
            {
                ModelState.AddModelError(nameof(EditBookDto),
                    "The provided description should be different from the title.");
            }

            TryValidateModel(bookToPatch);
            if (!ModelState.IsValid)
                return new UnProcessableEntityObjectResult(ModelState);

            Mapper.Map(bookToPatch, bookForAuthorFromRepo);

            repository.UpdateBookForAuthor(bookForAuthorFromRepo);

            if (!repository.Save())
            {
                throw new Exception($"Patching book {id} for author {authorId} failed on save.");
            }

            return NoContent();
        }
    }
}