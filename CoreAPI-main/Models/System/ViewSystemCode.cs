using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.System
{
    public class ViewSystemCode
    {
        [CoreRequired]
        [CoreMaxLength(50)]
        public string cdid { get; set; }
        [CoreRequired]
        [CoreMaxLength(50)]
        public string cdname { get; set; }
        [CoreRequired]
        [CoreMaxLength(1000)]
        public string cdgrp { get; set; }
    }
}
