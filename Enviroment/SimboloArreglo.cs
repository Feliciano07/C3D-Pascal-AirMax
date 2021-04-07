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

        public SimboloArreglo(string id, LinkedList<Dimension> dimensions, Objeto objeto)
        {
            this.id = id;
            this.dimensiones = dimensions;
            this.cantidad = dimensiones.Count + 1;
        }

        public void setObjeto(Objeto objeto)
        {
            this.objeto = objeto;
        }

        public string get_cantidad_dimensiones()
        {
            return this.cantidad.ToString();
        }

        public void Espacios_Utilizar()
        {
            if(this.cantidad == 1)
            {

            }else if(this.cantidad == 2)
            {

            }else if(this.cantidad == 3)
            {

            }
        }

        public string Cantidad_Una_Dimension()
        {
            Dimension[] aux_dim = new Dimension[this.dimensiones.Count];
            this.dimensiones.CopyTo(aux_dim, 0);

            string tem1 = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(tem1, aux_dim[0].inferior.ToString(), aux_dim[0].superior.ToString(), "-");

            return tem1;

        }

        public string Cantidad_Dos_Dimensiones()
        {
            //TODO: definir la cantidad
        }

        public string Cantidad_Tres_dimensiones()
        {
            //TODO: definir la cantidad
        }
       

    }
}
