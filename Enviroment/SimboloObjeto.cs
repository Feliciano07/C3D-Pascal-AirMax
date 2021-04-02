using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class SimboloObjeto
    {
        private string id;
        private int size;
        private LinkedList<Atributo> atributos;

        public SimboloObjeto(string id, int size, LinkedList<Atributo> atributos)
        {
            this.id = id;
            this.size = size;
            this.atributos = atributos;
        }

        public int getSize()
        {
            return this.size;
        }

        public LinkedList<Atributo> GetAtributos()
        {
            return this.atributos;
        }

    }
}
