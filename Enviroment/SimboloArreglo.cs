using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Manejador;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class SimboloArreglo
    {
        public string id;
        public LinkedList<Dimension> dimensiones;
        public Objeto objeto;
        public int cantidad;

        private Dimension[] aux_dim;

        public SimboloArreglo(string id, LinkedList<Dimension> dimensions, Objeto objeto)
        {
            this.id = id;
            this.dimensiones = dimensions;
            this.cantidad = dimensiones.Count;
            this.aux_dim = new Dimension[this.dimensiones.Count];
            this.dimensiones.CopyTo(this.aux_dim, 0);
            this.objeto = objeto;
        }

        public void setObjeto(Objeto objeto)
        {
            this.objeto = objeto;
        }

        public string get_cantidad_dimensiones()
        {
            return this.cantidad.ToString();
        }

        public string Espacios_Utilizar()
        {
            if(this.cantidad == 1)
            {
                return Cantidad_Una_Dimension();
            }else if(this.cantidad == 2)
            {
                return Cantidad_Dos_Dimensiones();
            }else if(this.cantidad == 3)
            {
                return Cantidad_Tres_dimensiones();
            }
            return "0";
        }

        public string Cantidad_Una_Dimension()
        {

            string tem1 = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(tem1, this.aux_dim[0].superior.ToString(), this.aux_dim[0].inferior.ToString(), "-");

            string salida = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(salida, tem1, "1", "+");

            return salida;

        }

        public string Cantidad_Dos_Dimensiones()
        {
            string tem1 = Cantidad_Una_Dimension();

            string tem2 = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(tem2, this.aux_dim[1].superior.ToString(), this.aux_dim[1].inferior.ToString(), "-");

            string aux = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(aux, tem2, "1", "+");


            string multi = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(multi, tem1, aux, "*");

            return multi;

        }

        public string Cantidad_Tres_dimensiones()
        {
            string tem2 = Cantidad_Dos_Dimensiones();
            
            string tem3 = Master.getInstancia.newTemporal();

            Master.getInstancia.addBinaria(tem3, this.aux_dim[2].superior.ToString(), this.aux_dim[2].inferior.ToString(), "-");

            string aux = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(aux, tem3, "1", "+");


            string multi = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(multi, tem2, aux, "*");

            return multi;
        }
       

        public string Posicion_Una_dimension(string pos1)
        {
            string tem1 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem1, pos1, this.aux_dim[0].inferior.ToString(), "-");

            return tem1;
        }

        public string Posicion_Dos_Dimensiones(string pos1, string pos2)
        {
            
            string tem1 = Posicion_Una_dimension(pos1);

            string tem2 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem2, pos2, this.aux_dim[1].inferior.ToString(), "-");

            string auxiliar = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addUnaria(auxiliar, this.aux_dim[1].superior.ToString());


            string multi = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(multi, tem1, auxiliar, "*");

            string sum = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(sum, multi, tem2, "+");

            return sum;

        }

        public string Posicion_Tres_dimensiones(string pos1, string pos2, string pos3)
        {

            string tem1 = Posicion_Una_dimension(pos1);

            string n2 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(n2, this.aux_dim[1].superior.ToString(), "1", "+");

            Master.getInstancia.addBinaria(n2, tem1, n2, "*");

            string tem2 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem2, pos2, this.aux_dim[1].inferior.ToString(), "-");

            Master.getInstancia.addBinaria(n2, tem2, n2, "+");

            string n3 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(n3, this.aux_dim[2].superior.ToString(), "1", "+");

            Master.getInstancia.addBinaria(n3, n2, n3, "*");

            string tem3 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem3, pos3, this.aux_dim[2].inferior.ToString(), "-");

            Master.getInstancia.addBinaria(n3, n3, tem3, "+");

            return n3;
        }

    }
}
