using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePeliculas.Entidades
{
    public class Actor
    {
        public int Id { get; set; }
        private string _nombre;
        public string Nombre {
            get { return _nombre; }  
            set {
                _nombre = string.Join(" ",
                    value.Split(" ")
                    .Select(x => x[0].ToString().ToUpper() + x.Substring(1).ToLower()));
            } }
        public string Biografia { get; set; }
        //[Column(TypeName = "Date")]
        public DateTime? FechaNacimiento { get; set; }
        public HashSet<PeliculaActor> PeliculasActores { get; set; }
        [NotMapped]
        public int? Edad { 
            get
            {
                if(!FechaNacimiento.HasValue)
                {
                    return null;
                }

                var fechaDeNacimiento = FechaNacimiento.Value;

                var edad = DateTime.Now.Year - fechaDeNacimiento.Year;

                if(new DateTime(DateTime.Now.Year, fechaDeNacimiento.Month, fechaDeNacimiento.Day) > DateTime.Now)
                {
                    edad--;
                }
                return edad;
            }
        }
        [NotMapped]
        public Direccion Direccion { get; set; }

        public string FotoURL { get; set; }
    }
}
