using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeGeneratorSystem.Domain.Models.Base
{
    public class BaseModel
    {
        public DateTime? Created { get; set; }
        public string? Creator { get; set; }
        public DateTime? Changed { get; set; }
        public string? Changer { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
