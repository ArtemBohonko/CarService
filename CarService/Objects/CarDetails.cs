using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Objects
{
    internal class CarDetails
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int Year { get; set; }
        public int Engine { get; set; }
        public string Value { get; set; }
        public string VIN {  get; set; }
        public int Mileage { get; set; }
    }
}
