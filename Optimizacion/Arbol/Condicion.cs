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
        public string salida;

        public Condicion(int fila, string left, string right, string operacion, string etiqueta) : base(fila)
        {
            this.left = left;
            this.right = right;
            this.operacion = operacion;
            this.etiqueta = etiqueta;
            this.salida = "if (" + this.left + this.operacion + this.right + ") goto " + this.etiqueta + ";";
        }

        public override string getOriginal()
        {
            return this.salida;
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
                            interprete.instrucciones[interprete.IP].isEnable = false;
                            this.Regla3();
                            
                        }
                    }
                    else
                    {
                        /* Regla 2
                         *if (T10 == T10) goto verdadera;
                         * goto falsa; 
                         */
                        interprete.IP++; // Verificamos la instruccion siguiente, deberia ser un goto label_falsa
                        Nodo element = interprete.instrucciones[interprete.IP];
                        if (element is Salto salto_aux)
                        {
                            
                            //this.Regla2(salto_aux.label);

                            interprete.IP++;
                            Nodo element1 = interprete.instrucciones[interprete.IP]; //etiqueta verdadera

                            if(element1 is Etiqueta label_aux)
                            {
                                if(string.Compare(salto_aux.label, label_aux.label) == 0)
                                {
                                    interprete.label_or = this.etiqueta;
                                }
                                else
                                {
                                    if(string.Compare(this.etiqueta, interprete.label_or) == 0)
                                    {
                                        interprete.label_or = "";
                                    }
                                    else
                                    {
                                        this.Regla2(salto_aux.label);
                                        element.isEnable = false;// ignoro goto label_falsa
                                        element1.isEnable = false;// ignor la etiqueta verdadera
                                    }
                                   
                                }
                            }

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
                            interprete.instrucciones[interprete.IP].isEnable = false;
                            this.Regla4(aux_salto.label);
                            
                        }
                    }
                    else
                    {
                        /* Regla 2
                         *if (T10 == -7) goto verdadera;
                         * goto falsa; 
                         */
                        interprete.IP++; // Verificamos la instruccion siguiente, deberia ser un goto label_falsa
                        Nodo element = interprete.instrucciones[interprete.IP];
                        if (element is Salto salto_aux)
                        {
                            //this.Regla2(salto_aux.label);

                            interprete.IP++;
                            Nodo element1 = interprete.instrucciones[interprete.IP]; //etiqueta verdadera

                            if (element1 is Etiqueta label_aux)
                            {
                                if (string.Compare(salto_aux.label, label_aux.label) == 0)
                                {
                                    //caracteristica de un or
                                    interprete.label_or = this.etiqueta;
                                }
                                else
                                {
                                    if(string.Compare(this.etiqueta, interprete.label_or) == 0)
                                    {
                                        interprete.label_or = "";
                                    }
                                    else
                                    {
                                        this.Regla2(salto_aux.label);
                                        element.isEnable = false;// ignoro goto label_falsa
                                        element1.isEnable = false;// ignor la etiqueta verdadera
                                    }
                                    
                                }
                            }


                        }
                    }
                }
            }
            interprete.IP++;
        }

        public void Regla2(string falsa)
        {
            string nueva = "if (" + this.left + "!=" + this.right + ") goto " + falsa +";";

            Master.getInstancia.addOptimized(this.fila, this.getOriginal(), nueva, Utilidades.Optimized.Regla.REGLA_2);

            this.salida = nueva;

        }

        public void Regla3()
        {
            string nueva = "goto " + this.etiqueta + ";";
            Master.getInstancia.addOptimized(this.fila, this.getOriginal(), nueva, Utilidades.Optimized.Regla.REGLA_3);
            this.salida = nueva;
        }

        public void Regla4(string salto)
        {
            string nueva = "goto " + salto + ";";
            Master.getInstancia.addOptimized(this.fila, this.getOriginal(), nueva, Utilidades.Optimized.Regla.REGLA_4);
            this.salida = nueva;
        }
    
        public bool isNumber()
        {
            if(Regex.IsMatch(this.left, @"^(-)?[0-9]+$") || Regex.IsMatch(this.left, @"^(-)?[0-9]+.[0-9]+$"))
            {
                if(Regex.IsMatch(this.right, @"^(-)?[0-9]+$") || Regex.IsMatch(this.right, @"^(-)?[0-9]+.[0-9]+$"))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
