using AquariumService.Models;
using AquariumService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AquariumService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AquariumController : Controller
    {
        #region Properties
        private readonly AcuarioRepository _aquariumRepository;
        private readonly UsuarioRepository _userRepository;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public AquariumController(
            AcuarioRepository aquariumRepository,
            UsuarioRepository userRepository,
            IConfiguration configuration)
        {
            _aquariumRepository = aquariumRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }
        #endregion

        #region Public Methods
        [Description("Login validator")]
        [HttpGet("LoginValidator/{user},{pass}")]
        public Usuario LoginValidator(string user, string pass)
        {
            try
            {
                var usuario = _userRepository.Find(x => x.Email == user
                                                     && x.Password == pass)
                                             .FirstOrDefault();
                return usuario;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Login validator for test")]
        [HttpGet("LoginValidatorTest/{user},{pass}")]
        public IActionResult LoginValidatorTest(string user, string pass)
        {
            try
            {
                IActionResult response = Unauthorized();

                var usuario = _userRepository.Find(x => x.Email == user
                                                     && x.Password == pass)
                                             .FirstOrDefault();

                if (usuario != null)
                {
                    var tokenString = GenerateJSONWebToken(usuario);
                    response = Ok(
                        new
                        {
                            token = tokenString,
                            type = usuario.Type
                        });
                }

                return response;
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
                List<Usuario> usuarios = _userRepository.GetAll()
                                                        .OrderBy(x => x.IdUsuario)
                                                        .ToList();
                return usuarios;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Get a specified user")]
        [HttpGet("GetUserByEmail/{email}")]
        public Usuario GetUserByEmail(string email)
        {
            try
            {
                var user = _userRepository.Find(x => x.Email == email)
                                          .FirstOrDefault();
                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("GetUserId/{userToken}")]
        public Usuario GetUserId(string userToken)
        {
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.ReadJwtToken(userToken);

                List<Claim> tokenList = token.Claims.ToList();

                int idUser = Convert.ToInt32(tokenList[0].Value);

                Usuario user = _userRepository.Find(x => x.IdUsuario == idUser)
                                              .FirstOrDefault();

                return user;
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
                List<Acuario> acuarios = _aquariumRepository.GetAll()
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
        [HttpGet("GetAquariumsByUser/{userToken}")]
        public List<Acuario> GetAquariumsByUser(string userToken)
        {
            try
            {
                var user = GetUserId(userToken);

                List<Acuario> acuariosByUser = _aquariumRepository.Find(x => x.IdUsuarioNavigation == user)
                                                                  .Include(x => x.IdUsuarioNavigation)
                                                                  .ToList();

                return acuariosByUser;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Get an aquarium by id")]
        [HttpGet("GetAcuarioById/{id}")]
        public Acuario GetAcuarioById(string id)
        {
            int idAcuario = int.Parse(id);

            try
            {
                var acuario = _aquariumRepository.Find(x => x.IdAcuario == idAcuario)
                                                 .Include(x => x.IdUsuarioNavigation)
                                                 .FirstOrDefault();
                return acuario;
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

                        usuario.ValidationCode = ValidatorNumber();
                        _userRepository.Add(usuario);
                    }
                    else
                    {
                        _userRepository.Edit(usuario);
                    }

                    _userRepository.Save();
                }

                return Ok(usuario);
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
                        _aquariumRepository.Add(acuario);
                    }
                    else
                    {
                        _aquariumRepository.Edit(acuario);
                    }

                    _aquariumRepository.Save();
                }

                return Ok(acuario);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Deletes an user")]
        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser(Usuario usuario)
        {
            try
            {
                _userRepository.Delete(usuario);
                _userRepository.Save();

                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Deletes an aquarium")]
        [HttpPost("DeleteAquarium")]
        public IActionResult DeleteAquarium(Acuario acuario)
        {
            try
            {
                _aquariumRepository.Delete(acuario);
                _aquariumRepository.Save();

                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region Private Methods
        [Description("Randomize a number wich will be a validator number.")]
        private int ValidatorNumber()
        {
            try
            {
                Random nroValidacion = new Random();

                return nroValidacion.Next(100000, 999999);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Description("Generate a JWT Token for an user.")]
        private string GenerateJSONWebToken(Usuario userInfo)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("userId", userInfo.IdUsuario.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, userInfo.Email)
                };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                  _configuration["Jwt:Issuer"],
                  claims,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
