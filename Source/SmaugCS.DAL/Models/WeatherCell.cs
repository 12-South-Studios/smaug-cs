using SmaugCS.Common.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("WeatherCells")]
    public class WeatherCell : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public int CellXCoordinate { get; set; }

        public int CellYCoordinate { get; set; }

        public ClimateTypes ClimateType { get; set; }

        public HemisphereTypes HemisphereType { get; set; }

        public int CloudCover { get; set; }

        public int Energy { get; set; }

        public int Humidity { get; set; }

        public int Precipitation { get; set; }

        public int Pressure { get; set; }

        public int Temperature { get; set; }

        public int WindSpeedX { get; set; }

        public int WindSpeedY { get; set; }
    }
}
