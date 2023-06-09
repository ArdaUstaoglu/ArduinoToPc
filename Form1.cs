﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;
namespace ArduinoToPc
{
    public partial class Form1 : Form
    {
        public ArduinoCommand ac;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ac = new ArduinoCommand();
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ac.setSerialPort(
                    System.Convert.ToInt32(portBautRate.Text)
                    , portName.Text
                );
                connectStatus.Text = "Connected.....";
                timer1.Enabled = true;
                timer1.Interval = 1000;
            }
            catch (Exception ex) {
                connectStatus.Text = "Connection Error...";
            }
        }

        private void disconnect_Click(object sender, EventArgs e)
        {
            try
            {
                ac.serialPort.Close();
                connectStatus.Text = "Disconnected...";
            }
            catch (Exception ex)
            {

            }
            timer1.Enabled = false;

        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                ac.send(modul.Text, action.Text, data1.Text, data2.Text, data3.Text);
            }
            catch (Exception ex)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                gelenveri.Text = ac.serialPort.ReadExisting();
            }
            catch (Exception ex) {

            }
        }
        public class LightData {
            public int id { get; set; }
            public string code { get; set; }
            public string info { get; set; }
            public int status { get; set; }
        }
        public class ApiError { 
            public int code { get; set; }
            public string context { get; set; }
            public string itemName { get; set; }

        }
        public class LightsApiData {
            public Boolean success { get; set; }
            public ApiError[] errors { get; set; }
            public LightData[] data { get; set; }
        }
        public LightsApiData lights;
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void lightsTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string url = "https://onlineshop.ibmtal.com/api/index.php?api=smartHomeLight_getAll";
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(receiveStream);
                    string data = reader.ReadToEnd();
                    lights = JsonConvert.DeserializeObject<LightsApiData>(data);
                    if (lights.success) {
                        lightsListBax.Items.Clear();
                        for (int i = 0; i < lights.data.Length; i++) {
                            lightsListBax.Items.Add(lights.data[1].code + " -> " + lights.data[1].status);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
