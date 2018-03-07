using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrehistoricLife
{
    public struct Operation
    {
        public OperationType type;
        public int param;
        public Operation(OperationType type,int param)
        {
            this.type = type;
            this.param = param;
        }
        public Operation(OperationType type)
        {
            this.type = type;
            this.param = 0;
        }
    }
}
