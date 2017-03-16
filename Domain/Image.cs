using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp1.Domain
{
    public class Image
    {
        public string Id {get; set;}
        public string Url {get; set;}
        DateTime PublishedWhen {get; set;}
    }
}