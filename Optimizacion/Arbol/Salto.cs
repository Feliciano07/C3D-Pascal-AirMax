using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Manejador;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Salto : Nodo
    {
        public string label;

        public Salto(int fila, string label) : base(fila)
        {
            this.label = label;
        }

        public override string getOriginal()
        {
            return "goto " + this.label + ";";
        }

        public override void Mirilla(Interprete interprete)
        {
            //Aplicando la regla 2, Codigo muerto
            Master master = Master.getInstancia;
            interprete.IP++; // siguiente instruccion;

            for(int index = 0; index + interprete.IP < interprete.instrucciones.Length; index++)
            {
                Nodo elemento = interprete.instrucciones[index + interprete.IP];

                if(elemento is Etiqueta || elemento is Fin || elemento is Funcion || elemento is Retorno)
                {
                    break;
                }
                else
                {
                    master.addOptimized(elemento.fila, elemento.getOriginal(), "", Utilidades.Optimized.Regla.REGLA_1);
                    elemento.isEnable = false;
                }
            }

        }
    }
}
