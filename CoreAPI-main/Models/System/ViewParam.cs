using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.System
{
    public class ViewParam
    {
        [CoreRequired]
        [CoreMaxLength(50)]
        public string PARNAME { get; set; }
    }
}
