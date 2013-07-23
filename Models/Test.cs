using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ksg.Models
{
    public class Test
    {
        public int x { get; set; }
    }

    public class TestContainer
    {
        public int Dummy;
        public IList<Test> tests;
    }
}