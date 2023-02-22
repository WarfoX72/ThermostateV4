#define DEBUG

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace ThermostateV4
{
    public partial class thermostateMode : Form
    {
        const int SENSORNUMBER = 16;
        #region Variables

        fullScreen myFullScreen = new fullScreen();
        sensorDef[] sensors = new sensorDef[16];
        myRegistry register = new myRegistry();

        lineGraph temperaturesGraph;
        lineGraph humidityGraph;

        sensorHandle sensorHandlingFunction;

        int preferenceKey = 0;

        #region formElement

        Panel currentPanel;
        Button[] tempViewButtonArray = new Button[16];
        Button[] prefsButtonArray = new Button[16];

        #endregion
        #endregion
        public thermostateMode()
        {
            InitializeComponent();
        }
        /**
         * Initialize components in array and variables
         */
        private void initComponent()
        {
            #region registryLoad
            register.loadSensors(ref sensors);


            #endregion
        }
        /**
         *  Function to place element in correct position
         *  
         */
        private void adjustElements(int ScreenWidth,int screeHeight)
        {
            menuPanel.BackColor = Color.FromArgb(160, Color.White);
            thermostateStatus.BackColor = Color.FromArgb(160, Color.White);
            buttonsPanel.BackColor = Color.FromArgb(160, Color.White);
            thermoMenuPanel.BackColor = Color.FromArgb(160, Color.White);
            //tempGraph.BackColor = Color.FromArgb(160, Color.White);

            #region preferences
            preferencesContainer.BackColor = Color.FromArgb(120, Color.White);
            #endregion

            #region WIDGET IN VARIABLES
            tempViewButtonArray[0] = tempViewButton1;
            tempViewButtonArray[1] = tempViewButton2;
            tempViewButtonArray[2] = tempViewButton3;
            tempViewButtonArray[3] = tempViewButton4;
            tempViewButtonArray[4] = tempViewButton5;
            tempViewButtonArray[5] = tempViewButton6;
            tempViewButtonArray[6] = tempViewButton7;
            tempViewButtonArray[7] = tempViewButton8;
            tempViewButtonArray[8] = tempViewButton9;
            tempViewButtonArray[9] = tempViewButton10;
            tempViewButtonArray[10] = tempViewButton11;
            tempViewButtonArray[11] = tempViewButton12;
            tempViewButtonArray[12] = tempViewButton13;
            tempViewButtonArray[13] = tempViewButton14;
            tempViewButtonArray[14] = tempViewButton15;
            tempViewButtonArray[15] = tempViewButton16;

            prefsButtonArray[0] = preferenceButton1;
            prefsButtonArray[1] = preferenceButton2;
            prefsButtonArray[2] = preferenceButton3;
            prefsButtonArray[3] = preferenceButton4;
            prefsButtonArray[4] = preferenceButton5;
            prefsButtonArray[5] = preferenceButton6;
            prefsButtonArray[6] = preferenceButton7;
            prefsButtonArray[7] = preferenceButton8;
            prefsButtonArray[8] = preferenceButton9;
            prefsButtonArray[9] = preferenceButton10;
            prefsButtonArray[10] = preferenceButton11;
            prefsButtonArray[11] = preferenceButton12;
            prefsButtonArray[12] = preferenceButton13;
            prefsButtonArray[13] = preferenceButton14;
            prefsButtonArray[14] = preferenceButton15;
            prefsButtonArray[15] = preferenceButton16;


            #endregion

            setColorValues();
        }
        /**
         * Update widget text and colors
         */
        private void setColorValues()
        {
            #region set_values
            for (int x = 0; x < SENSORNUMBER; x++)
            {
                //Main button
                tempViewButtonArray[x].Text = sensors[x].sensorText;
                tempViewButtonArray[x].BackColor = sensors[x].sensorColor;
                tempViewButtonArray[x].FlatAppearance.BorderColor = sensors[x].sensorColor;
                // Prefs Button
                prefsButtonArray[x].Text = sensors[x].sensorText;
                prefsButtonArray[x].BackColor = sensors[x].sensorColor;
                prefsButtonArray[x].FlatAppearance.BorderColor = sensors[x].sensorColor;

            }
            #endregion

        }

        /**
         *  Set Graph colors
         */
        private void setTempColors(lineGraph temperatureGraph, sensorDef[] sensors)
        {
            temperatureGraph.resetColors();
            for (int x=0; x<sensors.Length; x++)
            {
                temperatureGraph.setColors(sensors[x].sensorColor);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.initComponent();
            currentPanel = mainPanel;
            // Position element based on display Size.
            Rectangle screenSize = Screen.FromControl(this).Bounds;
            int height = screenSize.Height;
            int width = screenSize.Width;
            this.adjustElements(width, height);

            temperaturesGraph = new lineGraph(temperaturePictureBox);
            temperaturePictureBox.Paint += new System.Windows.Forms.PaintEventHandler(temperaturesGraph.copyImage);
            setTempColors(temperaturesGraph, sensors);

            temperaturesGraph.drawGraphAxis();
            temperaturesGraph.blitGraph();
            temperaturesGraph.setColors(Color.Red);
            temperaturesGraph.setColors(Color.Blue);
            sensorHandlingFunction = new sensorHandle(SENSORNUMBER, temperaturesGraph.Width);
            Random rnd = new Random();
            for (int x = 0; x < 1200; x++)
            {
                temperaturesGraph.pushTemp(0, rnd.Next(-4, 12));
                temperaturesGraph.pushTemp(1, rnd.Next(-4, 12));
            }
            temperaturesGraph.drawGraphAxis();
            temperaturesGraph.drawGraphData();
            temperaturesGraph.blitGraph();

#if !DEBUG
            myFullScreen.Maximize(this);
#else
            Console.WriteLine("Ciao");
#endif

        }

        /*
         *  One Second Clock event
         */
        private void clockTimer_Tick(object sender, EventArgs e)
        {
            CultureInfo cultureIT = CultureInfo.CreateSpecificCulture("it-IT"); 
            DateTime now = DateTime.Now;
            labelOra.Text = now.ToString("T", cultureIT);
#if DEBUG
            Random rnd = new Random();
            temperaturesGraph.pushTemp(0, rnd.Next(-4, 12));
            temperaturesGraph.pushTemp(1, rnd.Next(-4, 12));
            temperaturesGraph.drawGraphAxis();
            temperaturesGraph.drawGraphData();
            temperaturesGraph.blitGraph();

#endif
        }

        private void changeTab(Panel inPanel, Panel outPanel)
        {
            if (inPanel != outPanel)
            {
                inPanel.Visible = true;
                inPanel.Top = 0;
                inPanel.Left = 0;
                outPanel.Left = 1280;
                outPanel.Top = 0;
                currentPanel = inPanel;
            }
        }
        #region menuButton
        /**
         * 
         */
        private void electricityButton_Click(object sender, EventArgs e)
        {
            changeTab(electricityPanel, currentPanel);
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            changeTab(mainPanel, currentPanel);
        }
        private void preferencesButton_Click(object sender, EventArgs e)
        {
            changeTab(preferencesPanel, currentPanel);

        }
        #endregion

        private void label12_Click(object sender, EventArgs e)
        {
        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void panel17_Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine("Ciao");
        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void panel17_Paint_1(object sender, PaintEventArgs e)
        {
            

        }

        private void panel17_Paint_1(object sender, EventArgs e)
        {
            Console.WriteLine("Ciao");
        }
        /**
         * 
         * Function to edit preferences
         * 
         */
        private void editPreference(int Key)
        {
            // Enable controls
            preferenceSetColor.Enabled = true;
            preferenceSetText.Enabled = true;
            preferencesSetType.Enabled = true;
            confirmButtonPrefs.Enabled = true;
            sensorPosition.Enabled = true;
            preferenceKey = Key;

            // Set elements....
            preferenceSetColor.BackColor = sensors[preferenceKey].sensorColor;
            preferenceSetText.Text = sensors[preferenceKey].sensorText;
            preferencesSetType.SelectedIndex = sensors[preferenceKey].sensorType;
            if (sensors[preferenceKey].sensorType == 2)
            {
                sensorIPAddress.Text = sensors[preferenceKey].sensorIpAddress;
            }

        }
        
        private void confirmButtonprefs_Click(object sender, EventArgs e)
        {
            bool proceed = true;
            string selectedEmployee = (string)preferencesSetType.SelectedItem;
            int resultIndex = preferencesSetType.FindStringExact(selectedEmployee);
            if (resultIndex == 2)
            {
                String sensorIpAddressValue = sensorIPAddress.Text;
                if (sensorIpAddressValue.Trim() == "") {
                    var alertDialog = MessageBox.Show("Attenzione, inserire l'indirizzo IP del sensore","Errore",MessageBoxButtons.OK);
                    proceed = false;
                }
            }
            if (proceed)
            {
                var confirmResult = MessageBox.Show("Vuoi salvare le nuove preferenze?",
                                         "Conferma salvataggio!",
                                         MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    sensors[preferenceKey].sensorColor = preferenceSetColor.BackColor;
                    sensors[preferenceKey].sensorText = preferenceSetText.Text;

                    sensors[preferenceKey].sensorType = resultIndex;
                    if (resultIndex == 2)
                    {
                        sensors[preferenceKey].sensorIpAddress = sensorIPAddress.Text;
                    }
                    else
                    {
                        sensors[preferenceKey].sensorIpAddress = "";
                    }
                    register.saveSensors(ref sensors);
                    register.loadSensors(ref sensors);
                    setColorValues();
                    preferenceSetColor.BackColor = Color.Gray;
                    preferenceSetText.Text = "";
                    sensorIPAddress.Text = "";
                    preferencesSetType.SelectedIndex = -1;
                    preferenceSetColor.Enabled = false;
                    preferenceSetText.Enabled = false;
                    preferencesSetType.Enabled = false;
                    confirmButtonPrefs.Enabled = false;
                    sensorIPAddress.Visible = false;
                    sensorIPAddress.Enabled = false;
                    setTempColors(temperaturesGraph, sensors);
                }
                else
                {
                    // If 'No', do something here.
                }
            }
        }

        private void preferenceChangeCombo(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedEmployee = (string)comboBox.SelectedItem;
            int resultIndex = comboBox.FindStringExact(selectedEmployee);
            if (resultIndex == 2)
            {
                sensorIPAddress.Visible = true;
                sensorIPAddress.Enabled = true;
            } else
            {
                sensorIPAddress.Visible = false;
                sensorIPAddress.Enabled = false;
            }
        }
        #region preferencesButton
        private void preferenceButton1_Click(object sender, EventArgs e)
        {
            editPreference(0);
        }
        private void preferenceButton2_Click(object sender, EventArgs e)
        {
            editPreference(1);
        }

        private void preferenceButton3_Click(object sender, EventArgs e)
        {
            editPreference(2);
        }

        private void preferenceButton4_Click(object sender, EventArgs e)
        {
            editPreference(3);
        }

        private void preferenceButton5_Click(object sender, EventArgs e)
        {
            editPreference(4);
        }

        private void preferenceButton6_Click(object sender, EventArgs e)
        {
            editPreference(5);
        }

        private void preferenceButton7_Click(object sender, EventArgs e)
        {
            editPreference(6);
        }

        private void preferenceButton8_Click(object sender, EventArgs e)
        {
            editPreference(7);
        }
        #endregion

        private void preferenceSetColor_Paint(object sender, EventArgs e)
        {
            if (buttonColorDialog.ShowDialog() == DialogResult.OK)
            {
                preferenceSetColor.BackColor = buttonColorDialog.Color;
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void thermoFunctionButton_Click(object sender, EventArgs e)
        {
            Point panel = thermoMenuPanel.Location;
            if (panel.X ==1280)
            {
                panel.X = 1280 - (menuPanel.Size.Width *2) - 6;
                panel.Y = 2;
            } else
            {
                panel.X = 1280;
                panel.Y = 2;
            }
            thermoMenuPanel.Location = panel;

        }
    }
}
