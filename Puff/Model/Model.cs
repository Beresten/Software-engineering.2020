using Puff.Objects;
using Puff.VM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puff.M
{
    public class Model : ModelBase
    {
        private SupportingVMDataBalloon balloon;

        public Model()
        {
            balloon = new SupportingVMDataBalloon();
        }

        public override ProceduralData Prepare(InitialConditions conditions)
        {
            return balloon.Prepare(conditions);
        }

        public override ProceduralData Simulate(DynamicData data)
        {
            return balloon.SimulateByData(data);
        }
    }
}
