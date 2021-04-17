using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Utilidades
{
    public class Llamada
    {
        public int inicio;
        public LinkedList<string> storageTemp;
        public string simulando;

        public Llamada(int inicio, LinkedList<string> storage)
        {
            this.inicio = inicio;
            this.storageTemp = storage;
        }
    }
}
