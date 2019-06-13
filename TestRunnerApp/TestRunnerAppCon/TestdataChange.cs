using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunnerAppCon
{
    public class TestdataChange
    {
        public string id { get; set; }
        public int index { get; set; }
        public string data { get; set; }

        public TestdataChange(string id, int index, string data)
        {
            this.id = id;
            this.index = index;
            this.data = data;
        }
    }
}
