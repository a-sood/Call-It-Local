using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.DataTypes.Database.Weather
{
    [Serializable]
    public class WeatherInstance
    {
        public string LocalObservationDateTime { get; set; }
        public int EpochTime { get; set; }
        public string WeatherText { get; set; }
        public int WeatherIcon { get; set; }
        public bool IsDayTime { get; set; }
        public Temperature Temperature { get; set; }

        public RealFeelTemperature RealFeelTemperature { get; set; }
        public string MobileLink { get; set; }
        public string Link { get; set; }
    }

    [Serializable]
    public class Temperature
    {
        public Metric Metric { get; set; }
        public Imperial Imperial { get; set; }
    }

    [Serializable]
    public class RealFeelTemperature
    {
        public Metric Metric { get; set; }
        public Imperial Imperial { get; set; }
    }

    [Serializable]
    public class Metric
    {
        public double Value { get; set; }
        public string Unit { get; set; } 
        public int UnitType { get; set; }
    }

    [Serializable]
    public class Imperial
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }
}
