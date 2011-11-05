﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDataEntityObjects.SData
{
    public class SDataContextConfiguration : IContextConfiguration
    {
        public string Servername { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long MaxRequestSize { get; set; }
    }
}
