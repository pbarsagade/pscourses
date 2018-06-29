using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Helper;
using WebAPI.Models;

namespace WebAPI.Repository
{
    public class LiberaryRepository : ILiberaryRepository
    {
        private readonly DataContext context;

        public LiberaryRepository(DataContext context)
        {
            this.context = context;
        }

        public void AddAuthor(Author author)
        {
            this.context.Authors.Add(author);
        }

        public void AddBookForAuthor(int authorId, Book book)
        {
            var author = GetAuthor(authorId);
            if (author != null)
            {
                author.Books.Add(book);
            }
        }

        public bool AuthorExists(int authorId)
        {
            return this.context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            this.context.Authors.Remove(author);
        }

        public void DeleteBook(Book book)
        {
            this.context.Books.Remove(book);
        }

        public Author GetAuthor(int authorId)
        {
            return this.context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public PagedList<Author> GetAuthors(AuthorsResourceParameters resourceParameters)
        {
            var collectionBeforePaging = this.context.Authors
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName).AsQueryable();

            if (!string.IsNullOrWhiteSpace(resourceParameters.Genre))
            {
                string genreForWhereClause = resourceParameters.Genre.Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging.Where(a => a.Genre.Trim().ToLowerInvariant().Contains(genreForWhereClause));
            }

            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = resourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }


            return PagedList<Author>.Create(collectionBeforePaging, resourceParameters.PageNumber, resourceParameters.PageSize);
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<int> authorIds)
        {
            return this.context.Authors.Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public Book GetBookForAuthor(int authorId, int bookId)
        {
            return this.context.Books
               .Where(b => b.AuthorId == authorId && b.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> GetBooksForAuthor(int authorId)
        {
            return this.context.Books.Where(b => b.AuthorId == authorId);
        }

        public bool Save()
        {
            return (this.context.SaveChanges() >= 0);
        }

        public void UpdateBookForAuthor(Book book)
        {
            // Method not implemented as EF can handle with db context.
            // But it is  good practice to add update method in repository to maintain consistency
        }
    }
}
