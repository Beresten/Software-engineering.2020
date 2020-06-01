using Puff.VM.Data;
using System;
using System.ComponentModel;
using System.Windows.Threading;
using OxyPlot;
using Model = Puff.M.Model;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace Puff.VM
{
    public class ViewModel : VMBase, IDataErrorInfo
    {

        private Model model;
        
        //private===================================================

        private readonly DispatcherTimer timer;
        private TimeSpan seconds = TimeSpan.Zero;
        private bool emulation = false;
        private bool pause = false;
        private string fuelConsumption;
        private double tickOfFuelConsumption;

        private InitialConditions initialConditions;
        private ProceduralData proceduralData;
        private DynamicData dynamicData;

        //public===================================================

        public TimeSpan Seconds { get { return seconds; } set { seconds = value; NotifyThatPropertyChanged("Seconds"); } }
        public bool Emulation { get { return emulation; } set { emulation = value; NotifyThatPropertyChanged("Emulation"); } }
        public bool Pause { get { return pause; } set { pause = value; NotifyThatPropertyChanged("Pause"); } }
        public PlotModel Plot { get; set; }

        #region Sliders
        public string FuelConsumption
        {
            get { return fuelConsumption; }
            set { fuelConsumption = value; NotifyThatPropertyChanged("FuelConsumption"); }
        }
        public double TickOfFuelConsumption
        {
            get { return tickOfFuelConsumption; }
            set { tickOfFuelConsumption = value; NotifyThatPropertyChanged("TickOfFuelConsumption"); }
        }


        private int fuelConsumptionFormattingLength;
        private void ConvertFuelConsumptionValue(double value)
        {
            string result = value.ToString();
            FuelConsumption = result.Substring(0, Math.Min(result.Length, fuelConsumptionFormattingLength));
        }
        #endregion

        #region InitialConditions
        private string basketWeight = "0";
        private string seaLevelHeight = "0";
        private string initialVolume = "0";
        private string clorificValue = "0";

        public double InitialFuelConsumptionValue
        {
            get { return initialConditions.FuelConsumption; }
            set { initialConditions.FuelConsumption = value; NotifyThatPropertyChanged("InitialFuelConsumptionValue"); }
        }
        public string BasketWeight
        {
            get { return basketWeight; }
            set { basketWeight = value; NotifyThatPropertyChanged("BasketWeight"); }
        }
        public string ClorificValue
        {
            get { return clorificValue; }
            set { clorificValue = value; NotifyThatPropertyChanged("ClorificValue"); }
        }
        public string SeaLevelHeight
        {
            get { return seaLevelHeight; }
            set { seaLevelHeight = value; NotifyThatPropertyChanged("SeaLevelHeight"); }
        }
        public string InitialVolume
        {
            get { return initialVolume; }
            set { initialVolume = value; NotifyThatPropertyChanged("InitialVolume"); }
        }
        #endregion InitialHeatherConditions
        #region InitialHeatherConditions
        private string initialFuelConsumption = "0";
        private string maxFuelConsumption = "0";
        private string minFuelConsumption = "0";

        public string InitialFuelConsumption
        {
            get { return initialFuelConsumption; }
            set { initialFuelConsumption = value; NotifyThatPropertyChanged("InitialFuelConsumption"); }
        }
        public string MaxFuelConsumption
        {
            get { return maxFuelConsumption; }
            set { maxFuelConsumption = value; NotifyThatPropertyChanged("MaxFuelConsumption"); }
        }
        public string MinFuelConsumption
        {
            get { return minFuelConsumption; }
            set { minFuelConsumption = value; NotifyThatPropertyChanged("MinFuelConsumption"); }
        }

        private double maxFuelConsumptionValue;
        private double minFuelConsumptionValue;

        public double MaxFuelConsumptionValue
        {
            get { return maxFuelConsumptionValue; }
            set { maxFuelConsumptionValue = value; NotifyThatPropertyChanged("MaxFuelConsumptionValue"); }
        }
        public double MinFuelConsumptionValue
        {
            get { return minFuelConsumptionValue; }
            set { minFuelConsumptionValue = value; NotifyThatPropertyChanged("MinFuelConsumptionValue"); }
        }
        #endregion
        #region ProceduralData
        public double Acceleration
        {
            get { return proceduralData.Acceleration; }
            set { proceduralData.Acceleration = value; NotifyThatPropertyChanged("Acceleration"); }
        }
        public double Speed { get { return proceduralData.Speed; } set { proceduralData.Speed = value; NotifyThatPropertyChanged("Speed"); } }
        public double AirTemperature
        {
            get { return proceduralData.AirTemperature; }
            set { proceduralData.AirTemperature = value; NotifyThatPropertyChanged("AirTemperature"); }
        }
        public double RelativeHeight
        {
            get { return proceduralData.RelativeHeight; }
            set { proceduralData.RelativeHeight = value; NotifyThatPropertyChanged("RelativeHeight"); }
        }
        public double Temperature
        {
            get { return proceduralData.Temperature; }
            set { proceduralData.Temperature = value; NotifyThatPropertyChanged("Temperature"); }
        }
        private void NotifyThatProceduralDataChanged()
        {
            NotifyThatPropertyChanged("AirTemperature");
            NotifyThatPropertyChanged("RelativeHeight");
            NotifyThatPropertyChanged("Acceleration");
            NotifyThatPropertyChanged("Temperature");
            NotifyThatPropertyChanged("Volume");
            NotifyThatPropertyChanged("Speed");
        }
        #endregion
        #region DynamicData
        public double FuelConsumptionValue
        {
            get { return dynamicData.FuelConsumption; }
            set { dynamicData.FuelConsumption = value; ConvertFuelConsumptionValue(value);
                NotifyThatPropertyChanged("FuelConsumptionValue"); }
        }
        #endregion


        //some===================================================
        #region

        public ViewModel()
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.0001f) };
            timer.Tick += Emulation_Tick;
            model = new Model();
            initialConditions = new InitialConditions { Volume = 1 };
            proceduralData = new ProceduralData();
            dynamicData = new DynamicData();
            MaxFuelConsumption = "0,2";
            InitialFuelConsumption = "0,05";
            ClorificValue = "45";
            SeaLevelHeight = "1200";
            InitialVolume = "500";
            BasketWeight = "5";
            InitializePlot();
        }
        private void Emulation_Tick(object sender, EventArgs e)
        {
            proceduralData = model.Simulate(dynamicData);
            WritePlot();
            NotifyThatProceduralDataChanged();
            Seconds += TimeSpan.FromSeconds(1);
        }

        #endregion
        //plot===================================================
        #region
        private int plotDelay = 1000;
        private int tick = 0;
        private int secs = 0;

        private void InitializePlot()
        {
            tick = 0; secs = 0;

            Plot = new PlotModel { Title = "Запись симуляции" };
            Plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Время, с" });
            Plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Значение в СИ" });

            Plot.Series.Add(new LineSeries { Title = "Высота", MarkerType = MarkerType.Circle });
            Plot.Series.Add(new LineSeries { Title = "Т воздуха", MarkerType = MarkerType.Circle });
            Plot.Series.Add(new LineSeries { Title = "Т аэростата", MarkerType = MarkerType.Circle });
            Plot.Series.Add(new LineSeries { Title = "Ускорение", MarkerType = MarkerType.Circle });
            Plot.Series.Add(new LineSeries { Title = "Скорость", MarkerType = MarkerType.Circle });

            NotifyThatPropertyChanged("Plot");
        }

        private void WritePlot()
        {
            if (tick != plotDelay)
            {
                tick++;
            }
            else
            {
                secs += plotDelay;
                tick = 0;
                (Plot.Series[0] as LineSeries).Points.Add(new DataPoint(secs / 100, proceduralData.RelativeHeight));
                (Plot.Series[1] as LineSeries).Points.Add(new DataPoint(secs / 100, proceduralData.AirTemperature));
                (Plot.Series[2] as LineSeries).Points.Add(new DataPoint(secs / 100, proceduralData.Temperature));
                (Plot.Series[3] as LineSeries).Points.Add(new DataPoint(secs / 100, proceduralData.Acceleration));
                (Plot.Series[4] as LineSeries).Points.Add(new DataPoint(secs / 100, proceduralData.Speed));
            }
        }
        #endregion
        //commands===================================================
        #region

        private ExecutableCommand changeEmulationStatusCommand;
        public ExecutableCommand ChangeEmulationStatusCommand
        {
            get
            {
                return changeEmulationStatusCommand ?? (changeEmulationStatusCommand = new ExecutableCommand(property =>
                {
                    switch ((string)property)
                    {
                        case "Start":
                            {
                                if (CheckInitialConditionsValidation() == true)
                                {
                                    TickOfFuelConsumption =
                                    (MaxFuelConsumptionValue - MinFuelConsumptionValue) / 10;
                                    fuelConsumptionFormattingLength = Math.Round(MaxFuelConsumptionValue).ToString().Length + 5;
                                    FuelConsumptionValue = initialConditions.FuelConsumption;
                                    Seconds = TimeSpan.Zero;
                                    proceduralData = model.Prepare(initialConditions);
                                    NotifyThatProceduralDataChanged();
                                    InitializePlot();
                                    WritePlot();
                                    timer.Start();
                                    Pause = false;
                                    Emulation = true;
                                }
                                break;
                            }
                        case "Stop":
                            {
                                timer.Stop();
                                Emulation = false;
                                break;
                            }
                        case "Pause":
                            {
                                if (Pause == false)
                                {
                                    timer.Stop();
                                }
                                else
                                {
                                    timer.Start();
                                }
                                Pause = !Pause;
                                break;
                            }
                    }
                }));
            }
        }

        #endregion
        //validations===================================================
        #region

        private bool freezeOfValidation = false;
        private void UpdateTogether(string[] columns)
        {
            if (freezeOfValidation == false)
            {
                freezeOfValidation = true;
                foreach (string column in columns)
                {
                    NotifyThatPropertyChanged(column);
                }
                freezeOfValidation = false;
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case "MaxFuelConsumption":
                        {
                            if (double.TryParse(MaxFuelConsumption, out double convertedValue) == true)
                            {
                                MaxFuelConsumptionValue = convertedValue;
                                if (MaxFuelConsumptionValue < 0)
                                {
                                    error = "Пропускная способность нагревателя не может быть отрицательной.";
                                }
                                if (MaxFuelConsumptionValue < MinFuelConsumptionValue)
                                {
                                    error = "Максимальная пропускная способность нагревателя не может быть меньше минимальной.";
                                }
                                UpdateTogether(new string[] { "MinFuelConsumption", "InitialFuelConsumption" });
                            }
                            else
                            {
                                error = $"Не удалось преобразовать значение \"{MaxFuelConsumption}\".";
                            }
                            break;
                        }
                    case "MinFuelConsumption":
                        {
                            if (double.TryParse(MinFuelConsumption, out double convertedValue) == true)
                            {
                                MinFuelConsumptionValue = convertedValue;
                                if (MinFuelConsumptionValue < 0)
                                {
                                    error = "Пропускная способность нагревателя не может быть отрицательной.";
                                }
                                if (MinFuelConsumptionValue > MaxFuelConsumptionValue)
                                {
                                    error = "Минимальная пропускная способность нагревателя не может быть больше максимальной.";
                                }
                                UpdateTogether(new string[] { "MaxFuelConsumption", "InitialFuelConsumption" });
                            }
                            else
                            {
                                error = $"Не удалось преобразовать значение \"{MinFuelConsumption}\".";
                            }
                            break;
                        }
                    case "InitialFuelConsumption":
                        {
                            if (double.TryParse(InitialFuelConsumption, out double convertedValue) == true)
                            {
                                InitialFuelConsumptionValue = convertedValue;
                                if (InitialFuelConsumptionValue > MaxFuelConsumptionValue ||
                                    InitialFuelConsumptionValue < MinFuelConsumptionValue)
                                {
                                    error = "Начальная пропускная способность нагревателя вне диапазона.";
                                }
                            }
                            else
                            {
                                error = $"Не удалось преобразовать значение \"{InitialFuelConsumption}\".";
                            }
                            break;
                        }
                    case "BasketWeight":
                        {
                            if (double.TryParse(BasketWeight, out double convertedValue) == true)
                            {
                                initialConditions.BasketWeight = convertedValue;
                                if (initialConditions.BasketWeight < 0)
                                {
                                    error = "Вес коризны не может быть отрицательным.";
                                }
                            }
                            else
                            {
                                error = $"Не удалось преобразовать значение \"{BasketWeight}\".";
                            }
                            break;
                        }
                    case "SeaLevelHeight":
                        {
                            if (double.TryParse(SeaLevelHeight, out double convertedValue) == true)
                            {
                                initialConditions.SeaLevelHeight = convertedValue;
                                if (initialConditions.SeaLevelHeight < 0)
                                {
                                    error = "Высота над уровнем моря не может быть отрицательной.";
                                }
                            }
                            else
                            {
                                error = $"Не удалось преобразовать значение \"{SeaLevelHeight}\".";
                            }
                            break;
                        }
                    case "InitialVolume":
                        {
                            if (double.TryParse(InitialVolume, out double convertedValue) == true)
                            {
                                initialConditions.Volume = convertedValue;
                                if (initialConditions.Volume <= 0)
                                {
                                    error = "Объем аэростата не может быть отрицательным или равным нулю.";
                                }
                            }
                            else
                            {
                                error = $"Не удалось преобразовать значение \"{InitialVolume}\".";
                            }
                            break;
                        }
                    case "ClorificValue":
                        {
                            if (double.TryParse(ClorificValue, out double convertedValue) == true)
                            {
                                initialConditions.ClorificValue = convertedValue;
                                if (initialConditions.ClorificValue <= 0)
                                {
                                    error = "Теплота сгорания не может быть отрицательной или равной нулю.";
                                }
                            }
                            else
                            {
                                error = $"Не удалось преобразовать значение \"{ClorificValue}\".";
                            }
                            break;
                        }

                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        private bool CheckInitialConditionsValidation()
        {
            string[] columns = new string[]
            {
                "MaxFuelConsumption",
                "MinFuelConsumption",
                "InitialFuelConsumption",
                "BasketWeight",
                "SeaLevelHeight",
                "InitiialVolume",
                "ClorificValue",
            };
            foreach (string column in columns)
            {
                if (this[column] != string.Empty)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
