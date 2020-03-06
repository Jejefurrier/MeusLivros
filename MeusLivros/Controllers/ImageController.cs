using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MeusLivros.Models;
using MeusLivros.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace MeusLivros.Controllers
{
    public class ImageController : Controller
    {
        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
            int id;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.conn))
            {
                var a = conn.Query<int>("SELECT MAX(ID) AS ID FROM Livro");
                id = int.Parse(a.FirstOrDefault().ToString());
            }



            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = ConnectionString.imagePath + $"{id}";
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            //ImageService.PostToImgur(ConnectionString.imagePath, )

            return Ok(new { count = files.Count, size, filePaths });
        }
    }
}