using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.FluentMap;
using eCommerce.API.Models;
using eCommerce.API.Mapper;

//!!!!>>>>  ATENCAO <<<<!!!!
//ESTA EH UMA CLASSE COM DICAS P/ UTILIZACAO DO DAPPER

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipsController : ControllerBase
    {
        private IDbConnection _connection;//Interface p/ comunicacao com qualquer BD

        //!!!CRIAR A CONEXAO COM O Banco de dados com o DAPPER

        public TipsController()
        {
            _connection = new SqlConnection(@"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=eCommerce_DAPPER;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"); //CONNECTION STRING                                                                                                                                                                                                                                                               //Padronizacao de STRING p/ SQL Server (Standart): Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eCommerce_DAPPER;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False; 

        }

        //**OUTRA FORMA MAIS SIMPLES DE FAZER-SE UMA PESQUISA "SQL" SEM UTILIZAR DE "JOIN`s" DO SQL
        //MULTIPLOS "RESULT SETS"
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            //retornara 4 result sets
            string sql = "SELECT * FROM Usuarios WHERE Id = @Id;" +
                            "SELECT * FROM Contatos WHERE UsuarioId = @Id;" +
                            "SELECT * FROM EnderecosEntrega WHERE UsuarioId = @Id;" +
                            "SELECT * FROM UsuariosDepartamento UD INNER JOIN Departamentos D ON DepartamentoId = D.Id WHERE UD UsuarioId = @Id";

            using (var multipleResultSets = _connection.QueryMultiple(sql, new { Id = id }))
            {
                //Ler resultados obtidos "RESULT SETS" 
                var usuario = multipleResultSets.Read<Usuario>().SingleOrDefault();
                var contato = multipleResultSets.Read<Contato>().SingleOrDefault();
                var enderecos = multipleResultSets.Read<EnderecoEntrega>().ToList();
                var departamentos = multipleResultSets.Read<Departamento>().ToList();

                //Transformar os objetos ACIMA (resultados da QUERY) em um unico objeto "Usuario" (afinal, todas as listas estao interligadas pelo OBJ usuario)
                if (usuario != null)
                {
                    usuario.Contato = contato;
                    usuario.EnderecosEntrega = enderecos;
                    usuario.Departamentos = departamentos;

                    return Ok(usuario);
                }
            }

            return NotFound();
        }

        //>>>TRABALHANDO COM STORED PROCEDURES<<<
        [HttpGet("{stored/usuarios}")]//EndPoint da API

        //Retornar TODOS os usuarios
        public IActionResult StoredGet()
        {
            //COMANDO: exec SelecionarUsuario @Id
            var usuarios = _connection.Query<Usuario>("SelecionarUsuarios", commandType : CommandType.StoredProcedure);
            
            return Ok(usuarios);
        }

        [HttpGet("{stored/usuarioS/{id}")]//EndPoint da API

        //Retornar UNICO usuario
        public IActionResult StoredGet(int id)
        {
            //COMANDO: exec SelecionarUsuario @Id
            var usuarios = _connection.Query<Usuario>("SelecionarUsuario", new { Id = id}, commandType: CommandType.StoredProcedure);

            return Ok(usuarios);
        }

        //>>> METODOS PARA MAPEAR NOVAS COLUNAS <<<
        //!!**PROBLEMA**!! => Mapear colunas com nomes diferentes das propriedades do objeto

        //!!!SOLUCAO 1 => Renomear as colunas via SQL
        [HttpGet("mapper1/usuarios")]
        public IActionResult Mapper1()
        {
            string sql = "SELECT Id Cod, Nome NomeCompleto, Email, Sexo, RG, CPF, NomeMae NomeCompletoMae, SituacaoCadastro Situacao, DaTaCadastro FROM Usuarios;";
            
            var usuarios = _connection.Query<Usuario2>("SELECT * FROM Usuarios;");
            return Ok(usuarios);
        }

        //!!!SOLUCAO 2 => Renomear as colunas via C# (POO); Realizar o MAPEAMENTO das colunas  via Biblioteca Dapper.FluentMap
        [HttpGet("mapper2/usuarios")]
        public IActionResult Mapper2()
        {
            //***MAPEADOR DO FLUENT => Determina se uma PROPRIEDADE serah atribuido utilizando uma COLUNA qualquer

            //INICIALIZAR o FluentMap!!!
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new Usuario2map());
            });
            
            string sql = "SELECT Id Cod, Nome NomeCompleto, Email, Sexo, RG, CPF, NomeMae NomeCompletoMae, SituacaoCadastro Situacao, DaTaCadastro FROM Usuarios;";

            var usuarios = _connection.Query<Usuario2>("SELECT * FROM Usuarios;");
            return Ok(usuarios);
        }
    }
}
