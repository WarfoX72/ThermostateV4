using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThermostateV4
{
    public class sensorValue
    {
        public int sensorIndex { get; set; }
        public double value { get; set; }
    }
    class sensorHandle
    {
        List<sensorValue> sensorData = new List<sensorValue>();
        public sensorHandle(int sensorNumber, int sensorSize)
        {
            for (int x=0;x<sensorSize; x++)
            {
                
            }
        }
    }
}
