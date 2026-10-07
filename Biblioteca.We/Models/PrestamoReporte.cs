using System.ComponentModel.DataAnnotations;

namespace Biblioteca.We.Models
{
    public class PrestamoReporte
    {
        [Display(Name = "N° Préstamo")]
        public int PrestamoId { get; set; }

        [Display(Name = "Socio")]
        public string Socio { get; set; } = string.Empty;

        [Display(Name = "DNI")]
        public string DNI { get; set; } = string.Empty;

        [Display(Name = "Libros")]
        public string Libros { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de préstamo")]
        public DateTime FechaPrestamo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha límite")]
        public DateTime FechaLimite { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = string.Empty;
    }
}