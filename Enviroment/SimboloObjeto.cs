using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class SimboloObjeto
    {
        public string id;
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

        public Atributo_Index getAtributo(string id)
        {
            int contador = 0;
            foreach(Atributo atr in this.atributos)
            {
                if(String.Compare(atr.getId(), id.ToLower()) == 0)
                {
                    return new Atributo_Index(contador, atr);
                }
                contador++;
            }
            return null;
        }

    }
}
