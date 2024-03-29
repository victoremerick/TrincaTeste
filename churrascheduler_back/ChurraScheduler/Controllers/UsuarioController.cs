﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using BancoDeDados.Impl;
using Microsoft.AspNetCore.Cors;

namespace ChurraScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        // POST api/Usuario/List
        [HttpPost]
        [Route("Authenticate")]
        public JsonResult Authenticate(string login, string senha)
        {

            Usuario usuario = new Usuario(login,senha,"","");

            JsonResult j;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string local = appSettings["DatabasePath"] ?? "Not Found";
                UsuarioDAO dao = new UsuarioDAO(local);
                try
                {
                    List<dynamic> logado = dao.FindAll_Custom("" +
                        "Select " +
                        " * " +
                        "from " +
                        "usuario " +
                        "where " +
                        "login = '" + usuario.Login + "' " +
                        "AND senha = '" + usuario.Senha + "';");
                    if (logado.Count > 0)
                    {
                        Usuario u = dao.Find(Convert.ToInt32(logado[0].id));
                        u.AuthToken = Utils.Utils.alfanumericoAleatorio(25);
                        dao.Update(u);
                        dao.Close();
                        j = new JsonResult( new object[] { true, u.AuthToken });
                    }
                    else
                    {
                        dao.Close();
                        j = new JsonResult(new object[] { false, "Usuário não encontrado.",
                        "" +
                        "Select " +
                        " * " +
                        "from " +
                        "usuario " +
                        "where " +
                        "login = '" + usuario.Login + "' " +
                        "AND senha = '" + usuario.Senha + "';", login, senha});
                    }
                }
                finally
                {
                    dao.Close();
                }
            }
            catch (Exception e)
            {
                j = new JsonResult(new object[] { false, "Houve uma falha ao executar, contate o administrador.", e.Message, e.StackTrace });
            }
            return j;
        }

        // POST api/Usuario/Create
        [HttpPost]
        [Route("Create")]
        public JsonResult Create(string login, string senha, string nome)
        {
            Usuario usuario = new Usuario(login, senha, nome, "");
            JsonResult j;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string local = appSettings["DatabasePath"] ?? "Not Found";
                UsuarioDAO dao = new UsuarioDAO(local);
                try { 
                    List<dynamic> logado = dao.FindAll_Custom("" +
                        "Select " +
                        "   * " +
                        "from " +
                        "   usuario " +
                        "where " +
                        "   login = \"" + usuario.Login + "\";");
                    if (logado.Count == 0)
                    {
                        Usuario u = usuario;
                        u.AuthToken = Utils.Utils.alfanumericoAleatorio(25);
                        dao.Insert(u);
                        j = new JsonResult(new object[] { true, u.AuthToken });
                    }
                    else
                    {
                        dao.Close();
                        j = new JsonResult(new object[] { false, "Este login já está sendo utilizado." });
                    }
                }
                finally
                {
                    dao.Close();
                }
            }
            catch (Exception e)
            {
                j = new JsonResult(new object[] { false, "Houve uma falha ao executar, contate o administrador.", e.StackTrace });
            }
            return j;
        }

        // POST api/Usuario/CreateGet
        [HttpGet]
        [Route("CreateGet")]
        public JsonResult CreateGet(string nome, string login, string senha)
        {
            JsonResult j;
            Usuario usuario = new Usuario(login, senha, nome, "");

            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string local = appSettings["DatabasePath"] ?? "Not Found";
                UsuarioDAO dao = new UsuarioDAO(local);
                try
                {
                    List<dynamic> logado = dao.FindAll_Custom("" +
                        "Select " +
                        "   * " +
                        "from " +
                        "   usuario " +
                        "where " +
                        "   login = \"" + usuario.Login + "\";");
                    if (logado.Count == 0)
                    {
                        Usuario u = usuario;
                        u.AuthToken = Utils.Utils.alfanumericoAleatorio(25);
                        dao.Insert(u);
                        dao.Close();
                        j = new JsonResult(new object[] { true, u.Login, u.AuthToken, u.Nome });
                    }
                    else
                    {
                        dao.Close();
                        j = new JsonResult(new object[] { false, "Este login já está sendo utilizado." });
                    }
                }
                finally
                {
                    dao.Close();
                }
            }
            catch (Exception e)
            {
                j = new JsonResult(new object[] { false, "Houve uma falha ao executar, contate o administrador.", e.StackTrace });
            }
            return j;
        }

        // GET api/Usuario/List
        [HttpGet]
        [Route("List")]
        public ActionResult<IEnumerable<string>> List()
        {
            JsonResult j;

            var appSettings = ConfigurationManager.AppSettings;
            string local = appSettings["DatabasePath"] ?? "Not Found";
            try
            {
                UsuarioDAO dao = new UsuarioDAO(local);
                try
                {
                    List<dynamic> usuarios = dao.FindAll_Custom("select * from usuario");
                    dao.Close();
                    j = new JsonResult(new object[] {true, usuarios });
                }
                finally { dao.Close(); }
            }
            catch (Exception e)
            {
                j = new JsonResult(new object[] { false, "Houve uma falha ao executar, contate o administrador.", local, e.StackTrace });
            }
            return j;
        }
    }
}