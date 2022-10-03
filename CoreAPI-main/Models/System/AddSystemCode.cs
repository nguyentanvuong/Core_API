using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.System
{
    public class AddSystemCode
    {
        [CoreRequired]
        [CoreMaxLength(50)]
        public string cdid { get; set; }

        [CoreRequired]
        [CoreMaxLength(50)]
        public string cdname { get; set; }

        [CoreRequired]
        [CoreMaxLength(500)]
        public string caption { get; set; }

        [CoreRequired]
        [CoreMaxLength(1000)]
        public string cdgrp { get; set; }

        [CoreRequired]
        public int cdidx { get; set; }

        [CoreRequired]
        [CoreMaxLength(50)]
        public string isvisible { get; set; }
    }
}
