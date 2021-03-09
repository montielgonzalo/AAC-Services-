using AquariumService.Models;
using AquariumService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AquariumService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AquariumController : Controller
    {
        #region Properties
        private AcuarioRepository _acuarioRepository;
        private UsuarioRepository _usuarioRepository;
        #endregion

        #region Constructor
        public AquariumController(
            AcuarioRepository acuarioRepository,
            UsuarioRepository usuarioRepository)
        {
            _acuarioRepository = acuarioRepository;
            _usuarioRepository = usuarioRepository;
        }
        #endregion

        #region Public Methods
        [Description("Login validator")]
        [HttpGet("LoginValidator/{user},{pass}")]
        public bool LoginValidator(string user, string pass)
        {
            try
            {
                var usuario = _usuarioRepository.Find(x => x.Email == user
                                                        && x.Password == pass)
                                                .SingleOrDefault();
                bool isTrue;

                if (usuario != null)
                {
                    isTrue = true;
                }
                else
                {
                    isTrue = false;
                }

                return isTrue;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        [Description("Get all aquariums")]
        [HttpGet("GetAcuarios")]
        public List<Acuario> GetAcuarios()
        {
            try
            {
                List<Acuario> acuarios = _acuarioRepository.GetAll()
                                                           .Include(x => x.IdUsuarioNavigation)
                                                           .ToList();
                return acuarios;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Get an aquarium by user")]
        [HttpGet("GetAquariumsByUser/{user}")]
        public List<Acuario> GetAquariumsByUser(string user)
        {
            try
            {
                List<Acuario> acuariosByUser = _acuarioRepository.Find(x => x.IdUsuarioNavigation.Email == user)
                                                                 .Include(x => x.IdUsuarioNavigation)
                                                                 .ToList();

                return acuariosByUser;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Get all users")]
        [HttpGet("GetUsers")]
        public List<Usuario> GetUsuarios()
        {
            try
            {
                List<Usuario> usuarios = _usuarioRepository.GetAll()
                                                           .ToList();
                return usuarios;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Create or modify an aquarium")]
        [HttpPost("AddOrEditAquarium")]
        public IActionResult AddOrEditAquarium(Acuario acuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (acuario.IdAcuario == 0)
                    {
                        _acuarioRepository.Add(acuario);
                    }
                    else
                    {
                        _acuarioRepository.Edit(acuario);
                    }

                    _acuarioRepository.Save();
                }

                return Ok(acuario);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Create or modify an user")]
        [HttpPost("AddOrEditUser")]
        public IActionResult AddOrEditUser(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (usuario.IdUsuario == 0)
                    {
                        _usuarioRepository.Add(usuario);
                    }
                    else
                    {
                        _usuarioRepository.Edit(usuario);
                    }

                    _usuarioRepository.Save();
                }

                return Ok(usuario);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Deletes an aquarium")]
        [HttpPost("DeleteAquarium/{idUsuario}")]
        public bool DeleteAquarium(int idAcuario)
        {
            try
            {
                bool deleted;

                if (ModelState.IsValid)
                {
                    Acuario acuario = _acuarioRepository.Find(x => x.IdAcuario == idAcuario)
                                                        .FirstOrDefault();
                    if (acuario != null)
                    {
                        _acuarioRepository.Delete(acuario);
                        _acuarioRepository.Save();

                        deleted = true;
                    }
                    else
                    {
                        deleted = false;
                    }
                }
                else
                {
                    deleted = false;
                }

                return deleted;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Deletes an user")]
        [HttpPost("DeleteUser/{idUsuario}")]
        public bool DeleteUser(int idUsuario)
        {
            try
            {
                bool deleted;

                if (ModelState.IsValid)
                {
                    Usuario usuario = _usuarioRepository.Find(x => x.IdUsuario == idUsuario)
                                                        .FirstOrDefault();
                    if (usuario != null)
                    {
                        _usuarioRepository.Delete(usuario);
                        _usuarioRepository.Save();

                        deleted = true;
                    }
                    else
                    {
                        deleted = false;
                    }
                }
                else
                {
                    deleted = false;
                }

                return deleted;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
