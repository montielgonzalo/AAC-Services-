using System;
using System.Collections.Generic;

#nullable disable

namespace AquariumService.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int ValidationCode { get; set; }
        public int State { get; set; }
        public string Type { get; set; }
    }
}
