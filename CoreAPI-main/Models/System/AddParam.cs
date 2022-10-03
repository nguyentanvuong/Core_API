using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.System
{
    public class AddParam
    {
        [CoreRequired]
        [CoreMaxLength(50)]
        public string PARGRP { get; set; }

        [CoreRequired]
        [CoreMaxLength(50)]
        public string PARNAME { get; set; }

        [CoreRequired]
        [CoreMaxLength(500)]
        public string PARVAL { get; set; }

        [CoreMaxLength(1000)]
        public string MVAL { get; set; }

        [CoreMaxLength(1000)]
        public string DESCR { get; set; }

        [CoreRequired]
        [CoreMaxLength(50)]
        public string PARCODE { get; set; }
    }
}
