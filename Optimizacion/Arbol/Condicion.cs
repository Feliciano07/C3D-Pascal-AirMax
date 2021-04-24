using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Manejador;
using System.Text.RegularExpressions;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Condicion : Nodo
    {
        private string left;
        private string right;
        private string operacion;
        private string etiqueta;

        public Condicion(int fila, string left, string right, string operacion, string etiqueta) : base(fila)
        {
            this.left = left;
            this.right = right;
            this.operacion = operacion;
            this.etiqueta = etiqueta;
        }

        public override string getOriginal()
        {
            return "if (" + this.left + this.operacion + this.right + ") goto " + this.etiqueta;  
        }

        public override void Mirilla(Interprete interprete)
        {
            if(string.Compare(this.operacion,"==") == 0)
            {
                if(string.Compare(this.left, this.right) == 0)
                {
                    if (isNumber())
                    {
                        /*
                         * Regla 3
                         * if (1 == 1) goto verdadera;
                         * goto falsa;
                         * Salida goto verdadera;
                         */
                        interprete.IP++; // salto la siguiente instruccion deberia ser un goto label_falsa
                        Nodo element = interprete.instrucciones[interprete.IP];
                        if (element is Salto)
                        {
                            this.Regla3();
                        }
                    }
                    else
                    {
                        /* Regla 2
                         *if (T10 == 3) goto verdadera;
                         * goto falsa; 
                         */
                        interprete.IP++; // Verificamos la instruccion siguiente, deberia ser un goto label_falsa
                        Nodo element = interprete.instrucciones[interprete.IP];
                        if (element is Salto salto_aux)
                        {
                            this.Regla2(salto_aux.label);
                            interprete.IP++; //etiqueta verdadera la ignoro y paso a la siguiente

                        }
                    }
                    
                }
                else
                {
                    if (isNumber())
                    {
                        /* Regla 4
                         * if (4 == 1) goto verdadera;
                         * goto falsa;
                         * salida: goto false;
                         */
                        interprete.IP++; // verificamos la instruccion siguiente, deberia ser un goto label_falsa
                        Nodo element = interprete.instrucciones[interprete.IP];
                        if(element is Salto aux_salto)
                        {
                            this.Regla4(aux_salto.label);
                        }
                    }
                }
            }
        }

        public void Regla2(string falsa)
        {
            string nueva = "if (" + this.left + "!=" + this.right + ") goto " + falsa +";s";

            Master.getInstancia.addOptimized(this.fila, this.getOriginal(), nueva, Utilidades.Optimized.Regla.REGLA_2);

            //TODO: deberia guarda el codigo?
             
        }

        public void Regla3()
        {
            string nueva = "goto " + this.etiqueta + ";";
            Master.getInstancia.addOptimized(this.fila, this.getOriginal(), nueva, Utilidades.Optimized.Regla.REGLA_3);
            //TODO: deberia guardar el codigo?
        }

        public void Regla4(string salto)
        {
            string nueva = "goto " + salto + ";";
            Master.getInstancia.addOptimized(this.fila, this.getOriginal(), nueva, Utilidades.Optimized.Regla.REGLA_4);
            //TODO: deberia guardar el codigo?
        }
    
        public bool isNumber()
        {
            if(Regex.IsMatch(this.left, @"^[0-9]+$") || Regex.IsMatch(this.left, @"^[0-9].[0-9]+$"))
            {
                if(Regex.IsMatch(this.right, @"^[0-9]+$") || Regex.IsMatch(this.right, @"^[0-9].[0-9]+$"))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
