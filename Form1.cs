using C3D_Pascal_AirMax.Analisis;
using C3D_Pascal_AirMax.Optimizacion.Gramatica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C3D_Pascal_AirMax
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string entrada = richTextBox1.Text;

            Sintactico sintactico = new Sintactico();
            bool salida = sintactico.Analizar(entrada);

            if (salida)
            {
                var result = MessageBox.Show("Analisis correcto");
                string txt = Manejador.Master.getInstancia.getSalida();
                richTextBox2.Text = txt;
            }
            else
            {
                var result = MessageBox.Show("Analisis incorrecto");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string entrada = richTextBox1.Text;

            SintacticoC3D sintacticoC3D = new SintacticoC3D();
            bool salida = sintacticoC3D.Analizar(entrada);
            if (salida)
            {
                var result = MessageBox.Show("Analisis correcto");
            }
            else
            {
                var result = MessageBox.Show("Analisis incorrecto");
            }
            
        }
    }
}
