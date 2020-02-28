using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MeusLivros.Database;
using MeusLivros.Models;
using MeusLivros.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeusLivros.Controllers
{
    public class LoginController : Controller
    {
        public LoginController(BancoContext banco)
        {
            _banco = banco;
        }

        public readonly BancoContext _banco;
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody]User model)
        {
            model.Password = CryptographyService.ComputeSha256Hash(model.Password);
            // Recupera o usuário
            var user = _banco.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            // Verifica se o usuário existe
            if (user == null)
            {
                return NotFound(new { message = "Usuário ou senha inválidos" });

            }

            // Gera o Token
            var token = TokenService.GenerateToken(user);

            // Oculta a senha
            user.Password = "";

            // Retorna os dados
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("create")]
        public async Task<ActionResult<dynamic>> Create([FromBody]User model)
        {
            try
            {
                model.Password = CryptographyService.ComputeSha256Hash(model.Password);
                model.DataIncricao = DateTime.Now;
                model.Role = "User";
                _banco.Add(model);
                _banco.SaveChanges();
                return Ok("Inserido com sucesso!");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}