using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using MeusLivros.Database;
using MeusLivros.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

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
        [Authorize]
        public IActionResult GetLinks()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var primeiraclaim = claims.FirstOrDefault();
            var Name = primeiraclaim.Value;

            using (MySqlConnection conn = new MySqlConnection(ConnectionString.conn))
            {
                List<Livro> obj = (List<Livro>)conn.Query<Livro>($"SELECT * FROM Livro WHERE NomeDono = '{Name}'");
                foreach (var item in obj)
                {
                    item.Autor = conn.Query<string>($"SELECT Nome FROM Autores WHERE IDLivro = {item.ID}").ToList();
                }
                return Ok(obj);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("/addlivro")]
        public IActionResult AddLivro([FromBody]Livro livro)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var primeiraclaim = claims.FirstOrDefault();
            var Name = primeiraclaim.Value;

            if (livro.ImgBase64 == null)
                livro.ImgBase64 = "0";

            livro.NomeDono = Name;
            
            //Livro.SaveImage

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString.conn))
                {
                    var a = conn.Query<int>("SELECT MAX(ID) AS ID FROM Livro");//|Pegam o maior
                    int id = int.Parse(a.FirstOrDefault().ToString());//         |ID existente
                    id++;//                                                      |e adiciona +1 (pra ver qual vai ser o ID do proximo item a ser inserido

                    var imgUrl = Livro.SaveImage(livro.ImgBase64, id.ToString()); 

                    conn.Query($"INSERT INTO Livro (NomeDono, Nome, Link, UrlImagem) VALUES ('{livro.NomeDono}', '{livro.Nome}', '{livro.Link}', '{imgUrl}');");

                    foreach (var item in livro.Autor)//pra cada autor na lista, ele salva na tabela Autores
                    {
                        conn.Query($"INSERT INTO Autores (IDLivro, Nome) VALUES ({id}, '{item}')");
                    }
                }
                return Ok("Adicionado com sucesso!");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString()); 
            }
        }

    }
}