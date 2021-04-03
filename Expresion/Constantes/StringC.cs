using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Constantes
{
    public class StringC : Operacion
    {
        private Objeto tipos;
        private object valor;

        public StringC(int linea, int columna, Objeto tipo, object valor):base(linea, columna)
        {
            this.tipos = tipo;
            this.valor = valor;
        }

        public override Retorno compilar(Entorno entorno)
        {
            string tem = Master.getInstancia.newTemporal();
            // guardamos la direccion del primer caracter
            Master.getInstancia.addUnaria(tem, Master.getInstancia.heap_p);
            char[] cadena = this.valor.ToString().ToCharArray();
            for(int i = 1; i<cadena.Length-1; i++)
            {
                int ascii = (int)cadena[i];
                Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, ascii.ToString());
                Master.getInstancia.nextHeap();
            }
            // nuestro fin de cadenas sera el -1
            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "-1");
            Master.getInstancia.nextHeap();
            return new Retorno(tem, true, tipos);
        }
    }
}
