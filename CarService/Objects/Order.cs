using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Objects
{
    internal class Order
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int Client { get; set; }
        public int Car { get; set; }
        public int Employee { get; set; }
        public int Master { get; set; }
        public float TotalCost { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
    }
}
