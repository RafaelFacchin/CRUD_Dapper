//INTERFACE PARA REALIZACAO "CRUD"
//**CRUD = Create, Read, Update, Delete

using eCommerce.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.API.Repositories
{
    interface IUsuarioRepository
    {
        //OPERACOES DE CRUD
        public List<Usuario> Get();//Retorna uma lista para MOSTRAR usuarios via CRUD no BD
        public Usuario Get(int id);//Retorna um resultadopara MOSTRAR APENAS 1 usuario via CRUD no BD
        public void Insert(Usuario usuario);//Insercao de dados para INSERIR usuarios via CRUD no BD
        public void Update(Usuario usuario);//Altera dados para ALTERAR usuarios via CRUD no BD
        public void Delete(int id);//EXCLUI um item USUARIO via DELETE do BD
    }
}

