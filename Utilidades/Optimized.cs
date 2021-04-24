using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Utilidades
{
    public class Optimized
    {
        public enum Regla
        {
            REGLA_1,
            REGLA_2,
            REGLA_3,
            REGLA_4,
            REGLA_5,
            REGLA_6,
            REGLA_7,
            REGLA_8,
            REGLA_9,
            REGLA_10,
            REGLA_11,
            REGLA_12,
            REGLA_13,
            REGLA_14,
            REGLA_15,
            REGLA_16
        }

        public int fila;
        public string original;
        public string nueva;
        public Regla regla;

        public Optimized(int fila, string original, string nueva, Regla regla)
        {
            this.fila = fila;
            this.original = original;
            this.regla = regla;
            this.nueva = nueva;
        }



    }
}
