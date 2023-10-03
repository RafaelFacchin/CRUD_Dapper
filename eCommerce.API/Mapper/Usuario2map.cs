using Dapper.FluentMap.Mapping;
using eCommerce.API.Models;

namespace eCommerce.API.Mapper
{
    public class Usuario2map : EntityMap<Usuario2>
    {
        //CONSTRUTOR
        public Usuario2map() 
        {
            //***MAPEAMENTO
            //ESTE CODIGO SERAH FEITO P/ TODAS AS COLUNAS QUE MUDARAM DE NOME!!
            Map(p => p.Cod).ToColumn("Id");
            Map(p => p.NomeCompleto).ToColumn("Nome");
            Map(p => p.NomeCompletoMae).ToColumn("NomeMae");
            Map(p => p.Situacao).ToColumn("SituacaoCadastro");

        }
    }
}
