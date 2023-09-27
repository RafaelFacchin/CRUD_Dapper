namespace eCommerce.API.Models
{
    public class Contato
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }

        //DEIFINICAO DE RELACIONAMENTOS ENTRE AS TABELAS
        //Tabela contato (classe contato) com tabela usuario (classe Usuario)

        //!!Composicao de um relacionamento entre tabelas e classes
        public Usuario Usuario { get; set; }  //Relacionamento 1:1; Toda vez que a classe Contato for instanciada, trarah tb a classe usuario


    }
}
