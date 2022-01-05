using System;

namespace DevIO.Business.Models
{
    public class Produto : Entity
    {
        //Foreign key
        public Guid FornecedorId { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Imagem { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }

        //EF Releations -> Propriedades de navegação
        public Fornecedor Fornecedor { get; set; }
    }
}
