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

            livro.NomeDono = Name;
            //livro.IDDono = user.Id;
            //_banco.
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString.conn))
                {
                    conn.Query($"INSERT INTO Livro (NomeDono, Nome, Link) VALUES ('{livro.NomeDono}', '{livro.Nome}', '{livro.Link}');");
                    var a = conn.Query<int>("SELECT MAX(ID) AS ID FROM Livro");
                    int id = int.Parse(a.FirstOrDefault().ToString());

                    foreach (var item in livro.Autor)
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

        [HttpPost]
        public async Task<string> UploadImage(IFormFile files)
        {
            if (files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                    }
                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + files.files.FileName))
                    {
                        files.CopyTo(filestream);
                        filestream.Flush();
                        return "\\uploads\\" + files.FileName;
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            else
            {
                return "Unsuccessful";
            }
        }

    }
}