using System;
using System.Drawing;

namespace ThermostateV4
{
    class myRegistry
    {
        private Microsoft.Win32.RegistryKey Key = null;
        private const String registryPath = "Software\\Neobe_Thermostate";

        public myRegistry()
        {
            Key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registryPath,true);
            if (Key == null)
            {
                // Default values
                try
                {
                    Key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(registryPath, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } else
            {
            }
        }

        /**
         * 
         */
        public void loadSensors(ref sensorDef[] sensorDefs)
        {
            String keyString;
            Microsoft.Win32.RegistryKey keyValue;
            if (Key != null)
            {
                Microsoft.Win32.RegistryKey subkey = Key.OpenSubKey("sensors");
                if (subkey != null)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        keyString = String.Concat("sensor_definition", x.ToString());
                        keyValue = subkey.OpenSubKey(keyString);

                        sensorDefs[x].sensorText = keyValue.GetValue("text").ToString();
                        sensorDefs[x].sensorColor = Color.FromArgb((int)keyValue.GetValue("color"));
                        sensorDefs[x].sensorType = Convert.ToInt32(keyValue.GetValue("Type"));
                        sensorDefs[x].sensorIpAddress = keyValue.GetValue("ipaddress").ToString();
                        sensorDefs[x].sensorPosition = Convert.ToUInt16(keyValue.GetValue("position"));
                    }
                } else {
                    for (int x = 0; x < 16; x++)
                    {
                        sensorDefs[x].sensorText = "-";
                        sensorDefs[x].sensorColor = Color.Gray;
                        sensorDefs[x].sensorType = 0;
                        sensorDefs[x].sensorIpAddress = "";
                        sensorDefs[x].sensorPosition = 0;
                    }

                }
            }
        }

        /**
         *  Save sensor keys
         */
        private void saveSensors(sensorDef[] sensorDefs, Microsoft.Win32.RegistryKey registry)
        {
            String keyString;
            Microsoft.Win32.RegistryKey keyValue;
            for (int x = 0; x < 16; x++)
            {
                keyString = String.Concat("sensor_definition", x.ToString());
                keyValue = registry.OpenSubKey(keyString,true);
                if (keyValue == null)
                {
                    keyValue = registry.CreateSubKey(keyString,true);
                }
                keyValue.SetValue("text", sensorDefs[x].sensorText);
                keyValue.SetValue("color", sensorDefs[x].sensorColor.ToArgb());
                keyValue.SetValue("type", sensorDefs[x].sensorType.ToString());
                keyValue.SetValue("ipaddress", sensorDefs[x].sensorIpAddress);
                keyValue.SetValue("position", sensorDefs[x].sensorPosition);
            }
        }
        public bool saveSensors(ref sensorDef[] sensorDefs)
        {            
            if (Key != null)
            {
                Microsoft.Win32.RegistryKey subkey = Key.OpenSubKey("sensors",true);
                if (subkey != null)
                {
                    saveSensors(sensorDefs, subkey);
                } else {
                    subkey = Key.CreateSubKey("sensors",true);
                    saveSensors(sensorDefs, subkey);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
