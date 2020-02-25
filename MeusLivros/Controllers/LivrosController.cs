using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeusLivros.Database;
using MeusLivros.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeusLivros.Controllers
{
    public class LivrosController : Controller
    {
        public readonly BancoContext _banco;

        [HttpGet]
        [Route("/links")]
        public IActionResult GetLinks([FromBody]User user)
        {
            return Ok(_banco.Livros.Where(l => l.IDDono == user.Id));
        }

        [HttpPost]
        [Route("/addlivro")]
        public IActionResult AddLivro([FromBody]Livro livro, [FromBody]User user)
        {
            livro.IDDono = user.Id;
            _banco.Add(livro);
            _banco.SaveChanges();
            return Ok("Adicionado com sucesso!");
        }



    }
}