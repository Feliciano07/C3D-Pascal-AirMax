using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Manejador;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Operacion : Nodo
    {
        public string id;
        public string operacion;
        public string left;
        public string right;

        public Operacion(int fila, string id, string left, string operacion, string right) : base(fila)
        {
            this.id = id;
            this.operacion = operacion;
            this.left = left;
            this.right = right;
        }

        public override string getOriginal()
        {
            return this.id + "=" + this.left + this.operacion + this.right + ";";
        }

        public override void Mirilla(Interprete interprete)
        {
            if(string.Compare(this.id, this.left) == 0) 
            {
                switch (this.right)
                {
                    case "0":
                        {
                            switch (this.operacion)
                            {
                                case "+":
                                    Master.getInstancia.addOptimized(this.fila, this.getOriginal(), "", Utilidades.Optimized.Regla.REGLA_6);
                                    break;
                                case "-":
                                    Master.getInstancia.addOptimized(this.fila, this.getOriginal(), "", Utilidades.Optimized.Regla.REGLA_7);
                                    break;
                            }
                        }
                        break;
                    case "1":
                        {
                            switch (this.operacion)
                            {
                                case "*":
                                    Master.getInstancia.addOptimized(this.fila, this.getOriginal(), "", Utilidades.Optimized.Regla.REGLA_8);
                                    break;
                                case "/":
                                    Master.getInstancia.addOptimized(this.fila, this.getOriginal(), "", Utilidades.Optimized.Regla.REGLA_9);
                                    break;
                            }
                        }
                        break;
                }
                
            }
            else
            {
               if(string.Compare(this.left,"0") == 0)
                {
                    if(string.Compare(this.operacion, "/") == 0)
                    {
                        string nueva = this.id + " = " + "0;";
                        Master.getInstancia.addOptimized(this.fila, this.getOriginal(), nueva, Utilidades.Optimized.Regla.REGLA_16);
                    }
                }

                switch (this.right)
                {
                    case "0":
                        {
                            switch (this.operacion)
                            {
                                case "+":
                                    this.Regla_10_a_13(Utilidades.Optimized.Regla.REGLA_10);
                                    break;
                                case "-":
                                    this.Regla_10_a_13(Utilidades.Optimized.Regla.REGLA_11);
                                    break;
                                case "*":
                                    string nueva = this.id + " = " + "0;";
                                    Master.getInstancia.addOptimized(this.fila, this.getOriginal(), nueva, Utilidades.Optimized.Regla.REGLA_15);
                                    //TODO: guardar el codigo aca?
                                    break;

                            }
                        }
                        break;
                    case "1":
                        {
                            switch (this.operacion)
                            {
                                case "*":
                                    this.Regla_10_a_13(Utilidades.Optimized.Regla.REGLA_12);
                                    break;
                                case "/":
                                    this.Regla_10_a_13(Utilidades.Optimized.Regla.REGLA_13);
                                    break;
                            }
                        }
                        break;
                    case "2":
                        {
                            switch (this.operacion)
                            {
                                case "*":
                                    string nueva = this.id + " = " + this.left + " + " + this.left + ";";
                                    Master.getInstancia.addOptimized(this.fila, this.getOriginal(), nueva, Utilidades.Optimized.Regla.REGLA_14);
                                    //TODO: guardar codigo aca ?
                                    break;

                            }
                        }
                        break;
                }
            }
        }

        
        public void Regla_10_a_13(Utilidades.Optimized.Regla regla)
        {
            string nueva = this.id + " = " + this.left + ";";
            Master.getInstancia.addOptimized(this.fila, this.getOriginal(), nueva, regla);

            //TODO: guardar el codigo aca?
        }
        
    }
}
