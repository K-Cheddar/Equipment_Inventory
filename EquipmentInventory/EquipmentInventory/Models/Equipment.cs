using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentInventory.Models
{
    public class Equipment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
    }
}
