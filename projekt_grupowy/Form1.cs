﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace projekt_grupowy
{
    /// <summary>
    /// The main form of the application.
    /// </summary>
    public partial class Form1 : Form
    {
        int[] temperature = new int[20];
        int[] maxTemperature = new int[20];
        int[] minTemperature = new int[20]; 
        double[] averageTemperature = new double[20];
        int[] X_Array = new int[20];
        char[] buff = new char[1];
        int samples = 0;

        double meanTemp = 0;
        double sumTemp = 0;
        public Form1()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            for (int i = 0; i < 20; i++)
            {
                X_Array[i] = 0;
                temperature[i] = 0;
            }

            chart1.ChartAreas[0].AxisY.Maximum = 10;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
        }

        ~Form1()
        {
            serialPort.Close();
        }

        void get_availablePorts()
        {
            String[] availablePorts = SerialPort.GetPortNames();
            ports_comboBox.Items.AddRange(availablePorts);

        }



        private void connect_button_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                connect_button.Text = "Connect";

            }
            else
            {
                try
                {
                    if (ports_comboBox.Text != "")
                    {
                        serialPort.PortName = ports_comboBox.Text;
                        serialPort.Open();
                        connect_button.Text = "Disconnect";
                    }
                    else
                    {

                    }
                }
                catch (UnauthorizedAccessException)
                {

                }
            }

        }


        private void tableBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tableBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.databaseDataSet);

        }

        private void tableBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.tableBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.databaseDataSet);

        }

        private void tableBindingNavigatorSaveItem_Click_2(object sender, EventArgs e)
        {
            this.Validate();
            this.tableBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.databaseDataSet);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.Table' table. You can move, or remove it, as needed.
            this.tableTableAdapter.Fill(this.databaseDataSet.Table);

        }

        private void searchSurnameToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.tableTableAdapter.SearchSurname(this.databaseDataSet.Table, surnameToolStripTextBox.Text);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void tableDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void findPorts_button_Click(object sender, EventArgs e)
        {
            get_availablePorts();
        }

        private void plot_button_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                if (timer1.Enabled)
                {
                    timer1.Enabled = false;
                    plot_button.Text = "PLOT";
                }
                else {
                    timer1.Enabled = true;
                    plot_button.Text = "STOP";
                }
            }
            else {

                MessageBox.Show("You have to connect device first!");

                

            }
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 19; i += 1)
            {
                X_Array[i] = X_Array[i + 1];
                temperature[i] = temperature[i + 1];
            }
            X_Array[19] = X_Array[19] + 1;
            Random rand = new Random();
            serialPort.Write(rand.Next(10).ToString());


        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialPort.Read(buff, 0, 1);
            samples++;
            temperature[19] = buff[0]-48;
            calculateCharacteristicValues("Temperature");
            minTextBox.Text = minTemperature[19].ToString();
            maxTextBox.Text = maxTemperature[19].ToString();
            averageTextBox.Text = averageTemperature[19].ToString("0.####");
            chart1.Series["Series1"].Points.DataBindXY(X_Array, temperature);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                meanTemp += temperature[i];
            }
            meanTemp /= 20;
           textBox2.Text = meanTemp.ToString();
        }

        /// <summary>
        /// Method calculates values like minimal, maximal and average.
        /// </summary>
        /// <param name="valueType">Informs method about type of values. It can be temperature or another value.</param>
        private void calculateCharacteristicValues(string valueType) {
            if (valueType.ToLower() == "temperature") {
                if (temperature[19] < minTemperature[19]) minTemperature[19] = temperature[19];
                if (temperature[19] > maxTemperature[19]) maxTemperature[19] = temperature[19];
                if (samples == 1)
                {
                    averageTemperature[19] = temperature[19];
                }
                else
                {
                    averageTemperature[19] = (((double)averageTemperature[19] * (samples - 1)) + (double)temperature[19]) / samples;
                }
            }
        }
    }
}
		