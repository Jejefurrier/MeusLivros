using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MeusLivros.Database;
using MeusLivros.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeusLivros.Controllers
{
    public class LivrosController : Controller
    {

        public LivrosController(BancoContext banco)
        {
            _banco = banco;
        }
        public readonly BancoContext _banco;

        [HttpGet]
        [Route("/links")]
        public IActionResult GetLinks([FromBody]User user)
        {
            return Ok(_banco.Livros.Where(l => l.IDDono == user.Id));
        }

        [HttpPost]
        [Authorize]
        [Route("/addlivro")]
        public IActionResult AddLivro([FromBody]Livro livro)
        {
            var nome = ClaimTypes.Name;
            
            //livro.IDDono = user.Id;
            //_banco.
            _banco.Add(livro);
            _banco.SaveChanges();
            return Ok("Adicionado com sucesso!");
        }



    }
}