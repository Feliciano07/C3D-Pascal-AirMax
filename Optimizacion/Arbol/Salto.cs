﻿using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }

        public override void Mirilla(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }
}
