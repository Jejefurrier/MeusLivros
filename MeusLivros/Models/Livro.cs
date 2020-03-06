using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MeusLivros.Models
{
    public class Livro
    {
        public Livro()
        {

        }

        [Key]
        public int ID { get; set; }
        [ForeignKey("User")]
        public string NomeDono { get; set; }
        public List<string> Autor { get; set; }
        public string Nome { get; set; }
        public string Link { get; set; }
        public string ImgBase64 { get; set; }

        /// <summary>
        /// Salva imagem na pasta, se não tiver recebido o base64, retorna a url 0
        /// </summary>
        /// <param name="base64image"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string SaveImage(string base64image, string id)
        {
            if (base64image != null)
            {
                var PathWithFolderName = "/images/";

                if (!Directory.Exists(PathWithFolderName))
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(PathWithFolderName);

                }

                byte[] bytes = Convert.FromBase64String(base64image);

                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);

                    image.Save(PathWithFolderName + $"{id}.jpg");
                }

                return id;
            }
            else
            {
                return "0";
            }


        }


    }
}
