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

        public bool AuthorExists(int authorId)
        {
            return this.context.Authors.Any(a => a.Id == authorId);
        }

        public Author GetAuthor(int authorId)
        {
            return this.context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return this.context.Authors.OrderBy(a => a.FirstName).ThenBy(a => a.LastName);
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
    }
}
