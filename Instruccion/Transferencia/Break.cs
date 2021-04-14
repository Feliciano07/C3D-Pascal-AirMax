using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Transferencia
{
    public class Break : Nodo
    {
        public Break(int linea, int columna):base(linea, columna)
        {

        }
        public override Retorno compilar(Entorno entorno)
        {
            string label = entorno.getBreak();
            if(label == null)
            {
                throw new Exception("Break en un ambito incorrecto");
            }
            Master.getInstancia.addGoto(label);
            return null;
        }
    }
}
