using SQLite;

namespace Furia_FanHub.MVVM.Models
{
    [Table("fans")]
    public class FanProfile
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        // Endereço
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Pais { get; set; }

        // Interesses (exemplo: separados por vírgula)
        public string Interesses { get; set; }
    }
}
