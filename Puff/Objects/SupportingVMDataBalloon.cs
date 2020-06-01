using Puff.VM.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Puff.Objects
{
    public class SupportingVMDataBalloon : Balloon
    {
        public double StartHeight { get; set; }

        public ProceduralData Prepare(InitialConditions conditions)
        {
            Temperature = Physics.GetAirTemperature(conditions.SeaLevelHeight);
            Height = conditions.SeaLevelHeight;
            Speed = 0;
            BasketWeight = conditions.BasketWeight;
            Volume = conditions.Volume;
            ClorificValue = conditions.ClorificValue * 1000;
            FuelConsumption = conditions.FuelConsumption / 1000;
            StartHeight = conditions.SeaLevelHeight;
            SurfaceArea = 4 * Math.PI * Math.Pow((3 * Volume) / (4 * Math.PI), 1.0f / 3);
            return new ProceduralData { Temperature = this.AirTemperature, AirTemperature = this.AirTemperature };
        } 

        public ProceduralData SimulateByData(DynamicData data)
        {
            FuelConsumption = data.FuelConsumption;
            Simulate();
            if (Height < StartHeight)
            {
                Height = StartHeight;
                Acceleration = 0;
                Speed = 0;
            }
            return new ProceduralData
            {
                Temperature = this.Temperature,
                AirTemperature = this.AirTemperature,
                RelativeHeight = this.Height - StartHeight,
                Acceleration = this.Acceleration,
                Speed = this.Speed
            };
        }
    }
}
