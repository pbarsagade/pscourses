using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
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

        public IEnumerable<Author> GetAuthors()
        {
            return this.context.Authors.OrderBy(a => a.FirstName).ThenBy(a => a.LastName);
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
    }
}
