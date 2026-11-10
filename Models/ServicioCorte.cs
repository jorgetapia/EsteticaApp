namespace EsteticaApp.Models
{
    public class ServicioCorte : Servicio
    {
        public override decimal CalcularComision(decimal porcentajeEstilista)
        {
            return Precio * porcentajeEstilista;
        }
    }
}
