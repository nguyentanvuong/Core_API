using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class SSystemcode
    {
        public string cdid { get; set; }
        public string cdname { get; set; }
        public string caption { get; set; }
        public string cdgrp { get; set; }
        public int cdidx { get; set; }
        public bool isvisible { get; set; }

    }
}
