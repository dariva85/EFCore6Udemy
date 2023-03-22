using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net.WebSockets;

namespace EFCorePeliculas.Entidades.Configuraciones.Conversiones
{
    public class MonedaSimboloConverter: ValueConverter<Moneda,string>
    {
        public MonedaSimboloConverter():base(
            valor => MapeoMonedaString(valor),
            valor => MapeoStringMoneda(valor)
            )
        {
            
        }

        private static string MapeoMonedaString(Moneda value)
        {
            return value switch
            {
                Moneda.PesoDominicano => "RD$",
                Moneda.DolarEstadounidense => "$",
                Moneda.Euro => "€",
                _ => ""
            };
        }
        
        private static Moneda MapeoStringMoneda(string value)
        {

            return value switch
            {
                "RD$" => Moneda.PesoDominicano,
                "$" => Moneda.DolarEstadounidense,
                "€" => Moneda.Euro,
                _ => Moneda.Desconocida
            };
        }
    }
}
