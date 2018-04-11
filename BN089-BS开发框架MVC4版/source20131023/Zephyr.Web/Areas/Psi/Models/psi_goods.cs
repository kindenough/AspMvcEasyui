using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Psi")]
    public class psi_goodsService : ServiceBase<psi_goods>
    {

    }

    public class psi_goods : ModelBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }
        public string Brand { get; set; }
        public string Catagory { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
