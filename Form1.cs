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

namespace SerialPort_Comunicacion
{
    public partial class Form1 : Form
    {
        //Creamos las variables necesarias
        string dataOut;
        string dataIN;

        //Creamos un arreglo para guardar el nombre de los puertos
        string[] ports = SerialPort.GetPortNames();
        List<String> portsV = new List<String>() ;
        public Form1()
        {
            
            InitializeComponent();
            ValidacionCOM();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
            //Agregamos el arreglo de ports a nuestra ComboBox para que aparezcan los puertos
           // cBoxCOMPORT.Visible = False;
            cBoxCOMPORT.Items.AddRange(ports);


            rbAddToOldData.Checked = true;
            rbAlwaysUpdate.Checked = false;

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                //seleccionamos los parametros que pusimos en el forms para hacer la conexion copn el PORT seleccionado(COM4)

                serialPort1.PortName = cBoxCOMPORT.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDataBits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxStopBits.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxParityBits.Text);
                //Abrimos la conexion
                serialPort1.Open();
                progressBar1.Value = 100;
            }//Try
            catch(Exception ex)
            {
                //En caso de que nos de error nos abrira un MessageBox con el error 
                MessageBox.Show(ex.Message,"Error ",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Si la conexion esta abierta cierrala
            if (serialPort1.IsOpen)
            {

                serialPort1.Close();
                progressBar1.Value = 0;
            }
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                dataOut=tBoxDataOut.Text;
                serialPort1.WriteLine(dataOut);
            }//if
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            dataIN = serialPort1.ReadExisting();
            //Este metodo nos mostrara los datos en el textbox , para ello es necesario crear un metodo privado 
            //sin este metodo privado no se puede mostrar los datos 
            this.Invoke(new EventHandler(ShowData));
        }
            
        private void ShowData(object sender, EventArgs e)
        {

            if (rbAlwaysUpdate.Checked)
            {
                txbReceive.Text = dataIN;
            }
            else if (rbAddToOldData.Checked)
            {
                txbReceive.Text += dataIN;
            }
        }

        private void rbAlwaysUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if(rbAlwaysUpdate.Checked)
            {
                rbAlwaysUpdate.Checked = true;
                rbAddToOldData.Checked = false;
            }//if
        }

        private void rbAddToOldData_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAddToOldData.Checked)
            {
                rbAlwaysUpdate.Checked = false;
                rbAddToOldData.Checked = true;
            }//if
        }

        private void btnClearDataIn_Click(object sender, EventArgs e)
        {
            if(txbReceive.Text != "")
            {
                txbReceive.Text = "";
            }
        }

        public  async void ValidacionCOM()
        {
            try
            {
                // foreach (string COM in ports)
                // {
                string COM = "COM4";
                    //creamos la conexion 
                    serialPort1.PortName = COM;
                    serialPort1.BaudRate = 115200;
                    serialPort1.DataBits = 8;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Parity = Parity.None;
                    //Abrimos la conexion
                    serialPort1.Open();
                    
                    //leemos el dato de entrada 
                    dataIN += serialPort1.ReadExisting();
                    await Task.Delay(10000);
                    MessageBox.Show(dataIN);
                    if (dataIN.StartsWith("{REQ100"))
                    {
                        portsV.Add(COM);
                        MessageBox.Show("El puerto"+COM+" esta habilitado ");
                    }
                    else
                    {
                        MessageBox.Show("No hay ningun COM Conectado");
                    }
                   
                //}//foreach
                serialPort1.Close();
            }//try
            catch (Exception ex)
            {

                //En caso de que nos de error nos abrira un MessageBox con el error 
                MessageBox.Show(ex.Message, "Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//catch
        }
    }
}
