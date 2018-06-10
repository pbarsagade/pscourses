using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPICore2.Services;

namespace RestAPICore2.Controllers
{
    [Produces("application/json")]
    [Route("api/Author")]
    public class AuthorsController : Controller
    {
        private ILibraryRepository _liberaryRepository;

        public AuthorsController(ILibraryRepository libraryRepository)
        {
            this._liberaryRepository = libraryRepository;
        }
        public IActionResult GetAuthors()
        {
            var authorsFromRepo = _liberaryRepository.GetAuthors();
            return new JsonResult(authorsFromRepo);
        }
    }
}