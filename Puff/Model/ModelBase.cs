using Puff.VM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puff.M
{
    abstract public class ModelBase
    {
        abstract public ProceduralData Prepare(InitialConditions conditions);
        abstract public ProceduralData Simulate(DynamicData data);
    }
}
