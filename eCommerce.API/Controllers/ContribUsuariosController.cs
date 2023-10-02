﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.API.Models;
using eCommerce.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//***!!! ESTA CLASSE DE DEMONSTRACAO FUNCIONA PARA MOSTRAR A FUNCIONALIDADE DO "DAPPER CONTRIB."
//QUE TORNA MAIS FACIL A CONSTRUCAO DE UM "CRUD" PARA PEQUENIS PROJETOS DE DADOS !!!***

namespace eCommerce.API.Controllers
{
    [Route("api/Contrib/Usuarios")]
    [ApiController]
    public class ContribUsuariosController : ControllerBase
    {
        //AQUI NESTE BLOCO TEREMOS TODAS AS OPERACOES DO "USUARIOcONTROLLER"
        private IUsuarioRepository _repository;
        //***CONSTRUTOR DA CLASSE
        public ContribUsuariosController()
        {
            _repository = new ContribUsuarioRepository();
        }
        
        [HttpGet]
        public IActionResult Get() //TIP: A interface "IActionResult" retorna dados de varias formas; especifica do Asp.Net
        {
            //Obter todos os usuarios da lista:
            return Ok(_repository.Get()); //O param "Ok" representa o codigo 200 do HTTP (isto eh, esta tudo Ok)
        }

        //CRUD GET -> Obter APENAS um usuario por ID -- Para acessar esse metodo eh necessario especificar o numero id (Ex. 2):www.minhaapi.com.br/api/Usuarios/2
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            //Variavel para receber o dado informado (ID)
            var usuario = _repository.Get(id);

            //VERIFICACAO DA VARIAVEL (avalia se o numero ID informado esta cadastrado
            if (usuario == null)
            {
                return NotFound(); //Envia um erro HTTP da serie 400 (nao encontrado); 404 - not found
            }

            return Ok(usuario);
        }

        //CRUD POST -> Inserir/Cadastrar novos usuarios (METODO HTTP)
        [HttpPost]
        public IActionResult Insert([FromBody] Usuario usuario) //DATA ANOTATION: [FromBody]: retornarah um dado diretamente do corpo
        {
            _repository.Insert(usuario);
            return Ok(usuario);
        }

        //CRUD PUT -> Atualizar o cadastro de um usuario (METODO HTTP)
        [HttpPut]
        public IActionResult Update([FromBody] Usuario usuario)
        {
            _repository.Update(usuario);
            return Ok(usuario);
        }

        //CRUD DELETE -> Remover um usuario (METODO HTTP)
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return Ok();
        }
    }
}
