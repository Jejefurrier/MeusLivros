using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public List<string?> Autor { get; set; }
        public string Nome { get; set; }
        public string Link { get; set; }

    }
}
