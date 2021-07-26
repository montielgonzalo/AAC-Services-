using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AquariumService.Models
{
    public partial class Acuario
    {
        public int IdAcuario { get; set; }
        public int IdUsuario { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public int Color { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
