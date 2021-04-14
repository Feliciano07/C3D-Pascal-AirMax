using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Transferencia
{
    public class Exit : Nodo
    {
        private Nodo exp;

        public Exit(int linea, int columna, Nodo exp):base(linea, columna)
        {
            this.exp = exp;
        }

        public override Retorno compilar(Entorno entorno)
        {
            if (exp == null)
            {
                //exit de un procedimiento
                if(entorno.label_return == null)
                {
                    throw new Exception("Exit fuera del ambito");
                }
                Master.getInstancia.addGoto(entorno.label_return);

                return null;
            }

            return null;
        }
    }
}
