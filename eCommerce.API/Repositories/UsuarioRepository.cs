//**PADRAO REPOSITORY: Abstrai a forma de realizar a busca de dados; Ao inves de fazermos uma consulta 
//no controlador, essa consulta serah feita pelo repository

using eCommerce.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper; //Adiciona EXTENSOES para trabalhar com o IDBconnection p/ realizar QUERY`s 


namespace eCommerce.API.Repositories
{
    
    public class UsuarioRepository : IUsuarioRepository //IMPLEMETANDO A INTERFACE
    {
        //INSTANCIA A INTERFACE para estabelecer uma conexao com o BD
        private IDbConnection _connection;//Interface p/ comunicacao com qualquer BD

        //!!!CRIAR A CONEXAO COM O Banco de dados com o DAPPER

        //***CONSTRUTOR
        //Este construtor inicializa a conexao com o BD
        public UsuarioRepository()
        {
            //INSERIR O "PROVIDER" para a conexao com o banco de dados (AQUI utiliza-se o SQL Server)
            _connection = new SqlConnection(@"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=eCommerce_DAPPER;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"); //CONNECTION STRING                                                                                                                                                                                                                                                               //Padronizacao de STRING p/ SQL Server (Standart): Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eCommerce_DAPPER;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False; 
        }

        //!!!***MIGRACAO DO ADO.NET P/ O DAPPER.NET ***!!!
        //(MER < - > POO)
        //Abaixo serao refeitas as pesquisas de acesso ao banco de dados via DAPPER
        
        public List<Usuario> Get()
        {
            //METODO QUE RETORNA TODOS OS USUARIOS ATRAVES DE UMA QUERY
            //return _connection.Query<Usuario>("SELECT * FROM Usuarios").ToList(); //Retorna uma consulta (IEnumerable) e a converte para uma lista (ToList())

            //LISTA VAZIA PARA RETORNAR OS DADOS DOA QUARY ABAIXO
            List<Usuario> usuarios = new List<Usuario>();

            //METODO QUE RETORNA TODOS OS USUARIOS E COLUNAS ATRAVES DE UMA QUERY CONJUNTA (TABELA Usuarios + TABELA EnderecosEntrega)
            string sql = "SELECT U.*, C.*, EE.*, D.* FROM Usuarios AS U " +
                "LEFT JOIN Contatos AS C ON C.UsuarioId = U.Id " +
                "LEFT JOIN EnderecosEntrega AS EE ON EE.UsuarioId = U.Id" +
                "LEFT JOIN UsuariosDepartamentos AS UD ON UD.UsuarioId = U.Id" +
                "LEFT JOIN Departamentos AS D ON UD.Departamentos = U.Id";

            _connection.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql, (usuario,contato, enderecoEntrega, departamento) => {
                //EVITA A DUPLICIDADE DE DADOS (Usuarioid) nesta tabela
                if (usuarios.SingleOrDefault(a => a.Id == usuario.Id) == null) //VERIFICA SE POSSUI O MESMO Id
                {
                    usuario.Departamentos = new List<Departamento>();//ADICIONA O USUARIO NA LISTA DE DEPARTAMENTOS
                    usuario.EnderecosEntrega = new List<EnderecoEntrega>();//ADICIONA O USUARIO NA LISTA DE ENTREGAS
                    usuario.Contato = contato;//ADICIONA TAMBEM O USUARIO AO CONTATO
                    usuarios.Add(usuario);//SENAO TIVER DUPLICIDADE, Adiciona um novo usuario
                }
                else //SE FOR DIFERENTE DE NULO, ATRIBUI-SE
                {
                    usuario = usuarios.SingleOrDefault(a => a.Id == usuario.Id);
                }

                //VERIFICACAO DO ENDERECO DE ENTREGA
                if (usuario.EnderecosEntrega.SingleOrDefault(a => a.Id == enderecoEntrega.Id) == null)
                {
                    usuario.EnderecosEntrega.Add(enderecoEntrega);
                }

                //VERIFICACAO DO DEPARTAMENTO (de compra)
                if (usuario.Departamentos.SingleOrDefault(a => a.Id == departamento.Id) == null)
                {
                    usuario.Departamentos.Add(departamento);
                }

                return usuario;
            });//MAPEAMENTO DE OBJETOS DA QUERY ACIMA

            return usuarios;//RETORN UMA LISTA DE USUARIOS JAH TRATADOS, isto eh, sem duplicidades

        }

        public Usuario Get(int id)
        {
            //!!METODO QUE RETORNA APENAS O USUARIO SELECIONADO ATRAVES DE UMA QUERY

            //LISTA VAZIA PARA RETORNAR OS DADOS DOA QUARY ABAIXO
            List<Usuario> usuarios = new List<Usuario>();

            //METODO QUE RETORNA TODOS OS USUARIOS E COLUNAS ATRAVES DE UMA QUERY CONJUNTA (TABELA Usuarios + TABELA EnderecosEntrega)
            string sql = "SELECT U.*, C.*, EE.*, D.* FROM Usuarios AS U " +
                "LEFT JOIN Contatos AS C ON C.UsuarioId = U.Id " +
                "LEFT JOIN EnderecosEntrega AS EE ON EE.UsuarioId = U.Id" +
                "LEFT JOIN UsuariosDepartamentos AS UD ON UD.UsuarioId = U.Id" +
                "LEFT JOIN Departamentos AS D ON UD.Departamentos = U.Id WHERE U.Id = @Id";

            _connection.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql, (usuario, contato, enderecoEntrega, departamento) => {
                //EVITA A DUPLICIDADE DE DADOS (Usuarioid) nesta tabela
                if (usuarios.SingleOrDefault(a => a.Id == usuario.Id) == null) //VERIFICA SE POSSUI O MESMO Id
                {
                    usuario.Departamentos = new List<Departamento>();//ADICIONA O USUARIO NA LISTA DE DEPARTAMENTOS
                    usuario.EnderecosEntrega = new List<EnderecoEntrega>();//ADICIONA O USUARIO NA LISTA DE ENTREGAS
                    usuario.Contato = contato;//ADICIONA TAMBEM O USUARIO AO CONTATO
                    usuarios.Add(usuario);//SENAO TIVER DUPLICIDADE, Adiciona um novo usuario
                }
                else //SE FOR DIFERENTE DE NULO, ATRIBUI-SE
                {
                    usuario = usuarios.SingleOrDefault(a => a.Id == usuario.Id);
                }

                //VERIFICACAO DO ENDERECO DE ENTREGA
                if (usuario.EnderecosEntrega.SingleOrDefault(a => a.Id == enderecoEntrega.Id) == null)
                {
                    usuario.EnderecosEntrega.Add(enderecoEntrega);
                }

                //VERIFICACAO DO DEPARTAMENTO (de compra)
                if (usuario.Departamentos.SingleOrDefault(a => a.Id == departamento.Id) == null)
                {
                    usuario.Departamentos.Add(departamento);
                }

                return usuario;
            }, new { Id = id});//MAPEAMENTO DE OBJETOS DA QUERY ACIMA

            return usuarios.SingleOrDefault();//RETORN APENAS UM "OBJETO(Usuario)" JAH TRATADO, isto eh, sem duplicidades
        }

        public void Insert(Usuario usuario)
        {
            //INSERIR UM USUARIO NA LISTA
            //Controle do ID do usuario

            //**ABRIR CONEXAO COM O BD (com TRY e FINALLY)
            _connection.Open();

            //**TRANSACTION: Estabelece um grupo de operacoes de transacao, POREM
            // se alguma falhar, todas as anteriores sao canceladas!
            var transaction = _connection.BeginTransaction();//INICIA UMA TRANSACAO E A ARMAZENA DENTRO DE UMA VARIAVEL "transaction"

            try
            {
                string sql = "INSERT INTO Usuarios(Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) VALUES (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro); SELECT CAST (SCOPE_IDENTITY() AS INT);";//CRIA QUERY P/ INSERCAO DE DADOS ; retorno o Id do usuario inserido (como um numero inteiro)
                usuario.Id = _connection.Query(sql, usuario, transaction).Single();//.Single retorna a primeira Query INSERIDA

                //INSERE UM CONTATO NA LISTA DE USUARIOS
                //TESTE DE VERIFICACAO DE CONTATO "O bloco abaixo somente irah funcionar se houver um CONTATO
                if (usuario.Contato != null)
                {
                    usuario.Contato.UsuarioId = usuario.Id;
                    string sqlContato = "INSERT INTO Contatos(UsuarioId, Telefone, Celular) VALUES (@UsuarioId, @Telefone, @Celular)";
                    usuario.Contato.Id = _connection.Query<int>(sqlContato, usuario.Contato, transaction).Single();
                }

                //VERIFICA SE O USUARIO POSSUI ENDERECO DE ENTREGA
                if(usuario.EnderecosEntrega != null && usuario.EnderecosEntrega.Count > 0)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;
                        string sqlEndereco = "INSERT INTO EnderecosEntrega (UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento) VALUES (@UsuarioId, @NomeEndereco, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento); SELECT CAST(SCOPE_IDENTITY() AS INT); ";
                        enderecoEntrega.Id = _connection.Query<int>(sqlEndereco, enderecoEntrega, transaction).Single();

                    }
                }

                //DEFINICAO DE VINCULOS DE PESQUISA NO BD
                if (usuario.Departamentos != null && usuario.Departamentos.Count > 0)
                {
                    foreach (var departamento in usuario.Departamentos)
                    {

                        string sqlUsuariosDepartamentos = "INSERT INTO UsuariosDepartamentos (UsuarioId, DepartamentoId) VALUES (@UsuarioId, @DepartamentoId);";
                        _connection.Execute(sqlUsuariosDepartamentos, new { UsuarioId = usuario.Id, DepartamentoId = departamento.Id}, transaction);
                    }
                }

                //**FINALIZA A TRANSACTION
                transaction.Commit();//SALVA OS DADOS NO BANCO
            }
            catch (Exception) //tratamento de EXCEPTION (ERROS DE TRANSMISSAO DE DADOS)
            {
                //EVITA UM ERRO DE TRANSACTION
                try
                {
                    transaction.Rollback();//DESFAZ A TRANSACAO E RETORNA PARA O PONTO ANTERIOR
                }
                catch(Exception e)
                {
                    //RETORNAR PARA usuarioController ALGUMA MENSAGEM ou LANCAR UMA EXCECAO
                    Console.WriteLine(" ERRO de Transacao!" + e.Message);
                }
                
            }
            finally
            {
                // //**FECHAR CONEXAO COM O BD
                _connection.Close();
            } 
        }

        public void Update(Usuario usuario)
        {
            //**ABRIR CONEXAO COM O BD
            _connection.Open();

            //**INICIAR TRANSACAO DE DADOS
            var transaction = _connection.BeginTransaction();
            try
            {
                //ALTERAR UM USUARIO NA LISTA
                string sql = "UPDATE Usuarios SET (Nome =@Nome, Email = @Email, Sexo = @Sexo, RG = @RG, CPF = @CPF, NomeMae = @NomeMae, SituacaoCadastro = @SituacaoCadastro, DataCadastro = @DataCadastro) WHERE Id = @Id";//Query de atualizacao de usuario

                _connection.Execute(sql, usuario, transaction); //o (.Execute(sql, usuario) EXECUTA o comando SQL sem retornra alguma informacao

                //ATUALIZA O CONTATO MEDIANTE ATUALIZACAO DO MESMO
                if(usuario.Contato != null)
                {
                    //ATUALIZACAO DE CONTATO
                    string sqlContato = "UPDATE Contatos SET UsuarioId = @UsuarioId, Telefone = @Telefone, Celular = @Celular WHERE Id = @Id";
                    _connection.Execute(sqlContato, usuario.Contato, transaction);
                }

                //DELETAR TODOS OS DADOS PARA REINSERILOS (AO INVES DE ATUALIZAR, oque pode gerar problemas de compatibilidade)
                string sqlDeletarEnderecosEntrega = "DELETE FROM EnderecosEntrega WHERE UsuarioId = @Id";
                _connection.Execute(sqlDeletarEnderecosEntrega, usuario, transaction);

                //ADICIONAR NOVAMENTE OS DADOS
                if (usuario.EnderecosEntrega != null && usuario.EnderecosEntrega.Count > 0)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;
                        string sqlEndereco = "INSERT INTO EnderecosEntrega (UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento) VALUES (@UsuarioId, @NomeEndereco, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento); ";
                        enderecoEntrega.Id = _connection.Query<int>(sqlEndereco, enderecoEntrega, transaction).Single();

                    }
                }

                //**FINALIZA A TRANSACTION
                transaction.Commit();//SALVA OS DADOS NO BANCO
            }
            //EVITA UM ERRO DE TRANSACTION
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();//DESFAZ A TRANSACAO E RETORNA PARA O PONTO ANTERIOR
                }
                catch (Exception)
                {

                }
                
                //RETORNAR PARA usuarioController ALGUMA MENSAGEM ou LANCAR UMA EXCECAO
                Console.WriteLine(" ERRO de Transacao!" + ex.Message);
            }
            finally
            {
                //FECHA A CONEXAO COM O BD
                _connection.Close();
            }
        }

        public void Delete(int id)
        {
            //RELACIONAMENTOS DESTE BANCO DE DADOS eCommerce_DAPPER
            //Contatos        < = > Usuario  1:1
            //EnderecoEntrega < = > Usuario M:1
            //Usuarios        < = > DEPARTAMENTOS M:M (Tabela intermediaria)
            
            //DELETAR UM USUARIO NA LISTA
            _connection.Execute("DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });

            //DELETAR TODOS OS DADOS ATRIBUIDOS A UM ID (ON DELETE CASCADE)
            //**TODA CHAVE ESTRANGEIRA EXCLUIRAH TODOS OS SEUS DADOS EM CASCATA
            //!!!O comando "ON DELETE CASCADE" Jah esta incluso no codigo SQL
        
        }

        //!!!DESENVOLVIMENTO!!!
        //Implementacao de PROPRIEDADES ESTATICAS P/ SIMULACAO E TESTE DE BD

        //!!!***BLOCO DESATIVADO***!!!
        /*private static List<Usuario> _db = new List<Usuario>() //***OBS: O uso do "static" EVITA que toda a vez que o programa for iniciado, sejam criadas novas listas com os dados abaixo!
        {
            //!!!PERSISTENCIA DE DADOS ESTATICOS PARA TESTE!!!
            //*** Estes dados inseridos abaixo servem para "inicializar" o BD estatico populado com dados ***
            new Usuario(){Id=1, Nome="Felipe Rodrigues", Email="felipe.rodrigues@gmail.com"},
            new Usuario(){Id=2, Nome="Marcelo Rodrigues", Email="marcelo.rodrigues@gmail.com"},
            new Usuario(){Id=3, Nome="Jessica Rodrigues", Email="jessica,rodrigues@gmail.com"}
        };*/
        //!!!***BLOCO DESATIVADO***!!!
    }
}
