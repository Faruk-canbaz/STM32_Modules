using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using EasyTabs;

namespace WindowsFormsApp2_1
{

    public partial class Form1 : Form
    {
        Form2 form2SinifGosterim = new Form2();


        // USER CODE BEGİN DEFİNE VARİABLES

        int switchIntSayi;
        int saniye = 1;
        int mousePressControl = 0;
        int haritaVeriKontrol = 0;
        int KayitSayisi = 0;
        double graphicalValue2, graphicalValue3, graphicalValue4;

        // USER CODE END VARİABLES

        // USER CODE BEGİN TYPEDEF

        private SerialPort ComPort = new SerialPort();
        GMapOverlay markers = new GMapOverlay(" markers ");
        DateTime myDateValue = DateTime.Now;

        // USER CODE END TYPEDEF

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // bağlan butonu
        {
            checkConnection();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            setGUISettings();
            // BURANIN ARDUİNO İLE DENENMESİ LAZIM // 
            textBox1.Text = " sa 1";
            form2SinifGosterim.textBox1.Text = textBox1.Text;
            form2SinifGosterim.Show();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rxtxDataArea.Clear();
            txtSend.Clear();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            sendData();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ComPort.IsOpen)
            {
                ComPort.Close();
            }

            Application.Exit();
        }

        delegate void SetTextCallback(string text);

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            updatePorts();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            saniye++;
        }
        private void kaydetButon_Click(object sender, EventArgs e)
        {
            KayitSayisi++;
            metinBelgesiKaydet();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            mousePressControl++;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            clearCharts();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadMap();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            haritaVeriKontrol++;
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            map.Overlays.Remove(markers);
        }

        //***************************************************//**************************************************************************************//
        // User Functions Begin
        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;
            var data = serialPort.ReadLine();
            SetText(data);

            // data = data.Replace(',', '/');
            string[] vals = data.Split(',');
            int veri = vals.Length;
            //            

            int saniyeX = Convert.ToInt32(saniye);

            if (veri < 3)  // RAMPA PAKETİ 
            {
            }

            else
            {
                switchIntSayi = Convert.ToInt32(vals[1]);
                switch (switchIntSayi)
                {
                    case 1:
                        textBox3.Text = " Rampadayız";
                        break;
                    case 2:
                        textBox3.Text = " kalkis ";
                        break;
                    case 3:
                        textBox3.Text = " burnout tespit ";
                        break;
                }

                foreach (string value in vals)
                {

                    graphicalValue2 = (Convert.ToDouble(vals[2] = vals[2].Replace(".", ",")));
                    graphicalValue3 = (Convert.ToDouble(vals[5] = vals[5].Replace(".", ",")));
                    graphicalValue4 = (Convert.ToDouble(vals[4] = vals[4].Replace(".", ",")));

                    // if (!double.TryParse(vals[3] = vals[3].Replace(".", ","), out graphicalValue3)) // Hız 
                    //{

                    //}
                    //if (!double.TryParse(vals[4] = vals[4].Replace(".", ","), out graphicalValue4)) // İVME
                    //{

                    //}
                }
                int mod = mousePressControl % 2;
                int HaritaMod = haritaVeriKontrol % 2;

                if (mod == 1)
                {
                    rxtxDataArea.ScrollToCaret();
                }
                else
                {

                }
                if (HaritaMod == 1)
                {
                    vals[8] = vals[8].Replace(".", ",");
                    vals[9] = vals[9].Replace(".", ",");
                    textBox1.Text = Convert.ToString(vals[8]);
                    textBox2.Text = Convert.ToString(vals[9]);
                }
                else
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                }

                // hız - zaman
                this.chart1.Series[0].Points.AddXY(saniyeX, graphicalValue4);
                // ivme - zaman
                this.chart2.Series[0].Points.AddXY(saniyeX, graphicalValue3);
                // irtifa - zaman 
                this.chart3.Series[0].Points.AddXY(saniyeX, graphicalValue2);
            }
        }
        private void clearCharts()
        {
            foreach (var series in chart1.Series)
                series.Points.Clear();
            foreach (var series in chart2.Series)
                series.Points.Clear();
            foreach (var series in chart3.Series)
                series.Points.Clear();
            saniye = 1;
        }
        private void loadMap()
        {
            map.DragButton = MouseButtons.Right;
            map.MapProvider = GMapProviders.GoogleSatelliteMap;
            double enlem = Convert.ToDouble(textBox1.Text);
            double boylam = Convert.ToDouble(textBox2.Text);
            map.Position = new PointLatLng(enlem, boylam);

            PointLatLng point = new PointLatLng(enlem, boylam);
            Bitmap bmpMarker = (Bitmap)Image.FromFile("img/logo3.png"); // logo 3 = roket, logo 19 = tehlike;
            GMapMarker marker = new GMarkerGoogle(point, bmpMarker);

            markers.Markers.Add(marker);
            map.Overlays.Add(markers);
        }

        private void setGUISettings()
        {
            timer1.Interval = 1000;
            updatePorts(); // Portlar Görmüyorsa sıkıntı burada olabilir
            CheckForIllegalCrossThreadCalls = false;
            cmbBaudeRate.SelectedIndex = 1; // Baud Rate

            // 1.Grafik Kismi 
            chart1.Titles.Add(" Ivme / Zaman Grafiği ");
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 5;
            chart1.ChartAreas[0].AxisY.Interval = 0.75;
            chart1.ChartAreas[0].AxisX.Interval = 20;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            //chart1.ChartAreas[0].AxisX.LabelStyle.Format = ("%d saniye ", saniye);

            //2.Grafik Kismi
            //chart2.Titles.Add(" Hiz / Zaman Grafiği ");
            chart2.ChartAreas[0].AxisY.Minimum = 0;
            chart2.ChartAreas[0].AxisY.Maximum = 5;
            chart2.ChartAreas[0].AxisY.Interval = 0.75;
            chart2.ChartAreas[0].AxisX.Interval = 20;
            chart2.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;

            //3.Grafik Kismi
            chart3.Titles.Add(" İrtifa / Zaman Grafiği ");
            chart3.ChartAreas[0].AxisY.Minimum = 0;
            chart3.ChartAreas[0].AxisY.Maximum = 5;
            chart3.ChartAreas[0].AxisY.Interval = 1;
            chart3.ChartAreas[0].AxisX.Interval = 20;
            chart3.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;

            // Harita Ayarlar
            map.MinZoom = 5;
            map.MaxZoom = 100;
            map.Zoom = 10;

            // Sekme Ayarları EASYTABS
        }

        private void metinBelgesiKaydet()
        {
            using (var writer = new StreamWriter(@"C:\Users\OmerFarukCanbaz\Desktop\MetinDenemE\veri_1.txt", true)) // using.IO dahil etmeyi unutma.
            {
                writer.WriteLine(rxtxDataArea.Text + " ****** kaydedildi ****** ", KayitSayisi, Environment.NewLine, Environment.NewLine);
                string KayitSayisiString = Convert.ToString(KayitSayisi);
                label5.Text = (KayitSayisiString + " . kez başarıyla kaydedildi");
                label5.BackColor = Color.White;
                writer.Close();
            }
        }
        private void checkConnection()
        {
            if (ComPort.IsOpen)
            {
                timer1.Stop();
                saniye = 0;
                disconect();
            }
            else
            {
                timer1.Start();
                connect();
            }
        }
        private void updatePorts()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                cmbPortName.Items.Add(port);
            }
        }
        private void connect()
        {
            bool error = false;
            if (cmbPortName.SelectedIndex != -1 & cmbBaudeRate.SelectedIndex != -1)
            {
                ComPort.PortName = cmbPortName.Text;
                ComPort.BaudRate = int.Parse(cmbBaudeRate.Text);

                try
                {
                    ComPort.Open();
                    ComPort.DataReceived += SerialPortDataReceived;

                }
                catch (UnauthorizedAccessException) { error = true; }
                catch (ArgumentException) { error = true; }
                catch (System.IO.IOException) { error = true; }
                if (error) MessageBox.Show(this, " portu acamiyorum ");
            }
            else
            {
                MessageBox.Show(" Portu secmedin galibaa ");
            }

            if (ComPort.IsOpen)
            {
                btnConnect.Text = " disconnect ";
                btnConnect.ForeColor = Color.Blue;
                btnSend.Enabled = true;

                if (!rbText.Checked & !rbHex.Checked)
                {
                    rbText.Checked = true;
                }
                groupBox1.Enabled = false;
            }
        }

        private void disconect()
        {
            ComPort.Close();
            btnConnect.Text = "connect";
            btnSend.Enabled = false;
            groupBox1.Enabled = true;
        }
        private void SetText(string text)
        {
            if (this.rxtxDataArea.InvokeRequired)
            {
                rxtxDataArea.ForeColor = Color.Black; // Yazı Rengi Değiştirme
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.rxtxDataArea.AppendText(text);
            }
        }
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }
            return buffer;
        }

        private void sendData()
        {
            bool error = false;
            if (rbText.Checked == true)
            {
                ComPort.Write(txtSend.Text);
                rxtxDataArea.ForeColor = Color.Green;
                txtSend.Clear();
            }
            else
            {
                try
                {
                    byte[] data = HexStringToByteArray(txtSend.Text);
                    ComPort.Write(data, 0, data.Length);
                    rxtxDataArea.ForeColor = Color.Blue;
                    rxtxDataArea.AppendText(txtSend.Text.ToUpper() + "\n");
                    txtSend.Clear();
                }
                catch (FormatException) { error = true; }
                catch (ArgumentException) { error = true; }
                if (error) MessageBox.Show(this, " olmadi");
            }
        }

        //********************************************//************************************************************************************************//
        // Unnecessary Functions
        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void rxtxDataArea_TextChanged(object sender, EventArgs e)
        { 
        }
        private void chart2_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void label10_Click(object sender, EventArgs e)
        {

        }
        private void txtSend_TextChanged(object sender, EventArgs e)
        {

        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

        }

    } // Partical CLASS
}// NameSpace
