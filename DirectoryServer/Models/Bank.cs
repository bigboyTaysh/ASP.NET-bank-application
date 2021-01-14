﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirectoryServer.Models
{
    public class Bank
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Path { get; set; }
        public string ApiKey { get; set; }
    }
}