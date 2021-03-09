using System;
using System.Collections.Generic;

#nullable disable

namespace AquariumService.Models
{
    public partial class Acuario
    {
        public int IdAcuario { get; set; }
        public int IdUsuario { get; set; }
        public string Name { get; set; }
        public string Alimentarprog { get; set; }
        public bool AlimentarAhora { get; set; }
        public TimeSpan AlimentarFrec { get; set; }
        public TimeSpan AlimentarIni { get; set; }
        public string IluminacionEstado { get; set; }
        public bool IluminacionAhora { get; set; }
        public TimeSpan IluminacionFrec { get; set; }
        public TimeSpan IluminacionIni { get; set; }
        public int TemperActual { get; set; }
        public bool TemperCalentar { get; set; }
        public bool TemperEnfriar { get; set; }
        public int PhActual { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
