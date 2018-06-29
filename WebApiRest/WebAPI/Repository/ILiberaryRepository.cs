using System.Collections.Generic;
using WebAPI.Data;
using WebAPI.Helper;

namespace WebAPI.Repository
{
    public interface ILiberaryRepository
    {
        PagedList<Author> GetAuthors(AuthorsResourceParameters parameters);
        Author GetAuthor(int authorId);
        bool AuthorExists(int authorId);
        IEnumerable<Book> GetBooksForAuthor(int authorId);
        Book GetBookForAuthor(int authorId, int bookId);
        void AddAuthor(Author author);
        bool Save();
        void AddBookForAuthor(int authorId, Book book);
        IEnumerable<Author> GetAuthors(IEnumerable<int> authorIds);
        void DeleteBook(Book book);
        void DeleteAuthor(Author author);
        void UpdateBookForAuthor(Book book);

    }
}
