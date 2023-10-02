using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.API.Models;
using eCommerce.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper.Contrib.Extensions;

namespace eCommerce.API.Repositories
{
    public class ContribUsuarioRepository : IUsuarioRepository
    {
        //INSTANCIA A INTERFACE para estabelecer uma conexao com o BD
        private IDbConnection _connection;//Interface p/ comunicacao com qualquer BD

        //!!!CRIAR A CONEXAO COM O Banco de dados com o DAPPER

        //***CONSTRUTOR
        //Este construtor inicializa a conexao com o BD
        public ContribUsuarioRepository()
        {
            //INSERIR O "PROVIDER" para a conexao com o banco de dados (AQUI utiliza-se o SQL Server)
            _connection = new SqlConnection(@"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=eCommerce_DAPPER;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"); //CONNECTION STRING                                                                                                                                                                                                                                                               //Padronizacao de STRING p/ SQL Server (Standart): Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eCommerce_DAPPER;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False; 
        }
        public List<Usuario> Get()
        {
            return _connection.GetAll<Usuario>().ToList();//Getall: essa funcao retorna TODOS os dados da tabela de usuarios
                                            //NAO NECESSITA criar a query com linguagem SQL
                                            //***OBS: o GetAll retornarah dados no formato "IEnumerable", portanto, utiliza-se o ".ToList()" para retorna-los em uma LISTA

        }

        public Usuario Get(int id)
        {
            return _connection.Get<Usuario>(id);
        }

        public void Insert(Usuario usuario)
        {
            usuario.Id = Convert.ToInt32(_connection.Insert(usuario));//eh NECESSARIO converter os dados de insercao p/ 32bits(int), pois o formato de implementacao eh um tipo "long" ao inves de "int"
        }

        public void Update(Usuario usuario)
        {
            _connection.Update(usuario);
        }

        public void Delete(int id)
        {
            //***NA EXCLUSAO NAO pode-se passar o Id (do usuario), porem informa-se o objeto relacionado para sua exclusao!
            Get(id);//informa o OBJETO para realizar o delete
            _connection.Delete(Get(id));
        }
    }
}
