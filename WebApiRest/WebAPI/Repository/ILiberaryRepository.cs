using System.Collections.Generic;
using WebAPI.Data;

namespace WebAPI.Repository
{
    public interface ILiberaryRepository
    {
        IEnumerable<Author> GetAuthors();
        Author GetAuthor(int authorId);
        bool AuthorExists(int authorId);
        IEnumerable<Book> GetBooksForAuthor(int authorId);
        Book GetBookForAuthor(int authorId, int bookId);
    }
}
