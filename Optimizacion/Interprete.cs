using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.Optimizacion.Arbol;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion
{
    public class Interprete
    {
        public LinkedList<Nodo> original;
        public Nodo[] instrucciones;
        public string header;
        public int IP;// saber en que instruccion vamos navegando

        public string label_or;

        public Interprete(string header, LinkedList<Nodo> instrucciones)
        {
            this.original = instrucciones;
            this.header = header;
            this.instrucciones = new Nodo[this.original.Count];
            this.original.CopyTo(this.instrucciones, 0); // copia la lista a un arreglo
            this.IP = 0;
            this.label_or = "";
        }


        public bool Mirrilla()
        {
            //Empieza la optimizacion del codigo

            Master.getInstancia.addOptimizedCode(this.header);

            try
            {
                while(this.IP < this.instrucciones.Length)
                {
                    if (!this.instrucciones[this.IP].isEnable)
                    {
                        this.IP++;
                    }
                    else
                    {
                        this.instrucciones[this.IP].Mirilla(this);
                    }
                }

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }


        public void GenerarCodigo()
        {
            for(int i = 0; i< this.instrucciones.Length; i++)
            {
                if (this.instrucciones[i].isEnable)
                {
                    Master.getInstancia.addOptimizedCode(this.instrucciones[i].getOriginal());
                }
            }
        }

    }
}
