using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialPort_Comunicacion
{
    internal class Validacion
    {
        public string dataIN;
        private string[] ports = SerialPort.GetPortNames();
        public List<String> portsV = new List<String>();

        private async void ValidacionT(SerialPort SP, string COM)
        {
            //Abrimos la conexion
            SP.Open();
            //leemos el dato de entrada 
            dataIN += SP.ReadExisting();
            await Task.Delay(10000);
            if (dataIN.StartsWith("{REQ100"))
            {
                portsV.Add(COM);  
                MessageBox.Show("El puerto" + COM + " esta habilitado ");
            }
            else
            {
                MessageBox.Show("El puerto" + COM + " no mando REQ");
            }
            SP.Close();
        }
        public void ValidacionCOM()
        {
            try
            { 
                foreach (string COM in ports)
                {
                    var port = new SerialPort()
                    {
                        //string COM = "COM4";
                        //creamos la conexion 
                        PortName = COM,
                        BaudRate = 115200,
                        DataBits = 8,
                        StopBits = StopBits.One,
                        Parity = Parity.None
                    };
                    ValidacionT(port, COM);
                }//foreach

                MessageBox.Show("Puertos Habilitados");
               
            }//try
            catch (Exception ex)
            {
                //En caso de que nos de error nos abrira un MessageBox con el error 
                MessageBox.Show(ex.Message, "Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//catch*/
        }
    }
}
