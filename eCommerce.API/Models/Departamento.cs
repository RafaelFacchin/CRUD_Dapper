namespace eCommerce.API.Models
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        //DEIFINICAO DE RELACIONAMENTOS ENTRE AS TABELAS
        //Tabela Departamentos (classe Departamento) com tabela usuario (classe Usuario) 
        //!!Composicao de um relacionamento entre tabelas e classes
        public ICollection<Usuario> Usuarios { get; set; } //Relacionamento M:M; Toda vez que a classe EnderecoEntrega for instanciada, trarah tb a classe usuario
    }
}
