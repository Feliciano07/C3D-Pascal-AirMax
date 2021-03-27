using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Funciones
{
    public class Write : Nodo
    {
        private LinkedList<Nodo> expresiones;
        private bool salto;

        public Write(int linea, int columna, LinkedList<Nodo> exp, bool salto) : base(linea, columna)
        {
            this.expresiones = exp;
            this.salto = salto;
        }

        public override Retorno compilar(Entorno entorno)
        {
            // Recorrer las espresiones
            foreach(Nodo instruccion in this.expresiones)
            {
                try
                {
                    Retorno salida = instruccion.compilar(entorno);

                    switch (salida.getTipo())
                    {
                        case Objeto.TipoObjeto.INTEGER:
                            Master.getInstancia.addPrint("d", salida.getValor(), "int");
                            break;
                        case Objeto.TipoObjeto.REAL:
                            Master.getInstancia.addPrint("f", salida.getValor(),"float");
                            break;
                        case Objeto.TipoObjeto.BOOLEAN:
                            string label_salida = Master.getInstancia.newLabel();
                            Master.getInstancia.addLabel(salida.trueLabel);
                            Master.getInstancia.addPrintTrue();
                            Master.getInstancia.addGoto(label_salida);
                            Master.getInstancia.addLabel(salida.falseLabel);
                            Master.getInstancia.addPrintFalse();
                            Master.getInstancia.addLabel(label_salida);
                            break;
                        case Objeto.TipoObjeto.STRING:
                            LLamada_Nativa(salida.getValor(), entorno.getSize());
                            break;
                        default:
                            Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                                "No se puede imprimir un tipo de dato: " + salida.getTipo());
                            Master.getInstancia.addError(error);
                            break;
                    }

                }catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            if (salto)
            {
                Master.getInstancia.addPrint("c", "10", "char");
            }
            return null;
        }

        public void LLamada_Nativa(string valor, int size)
        {
            string tem = Master.getInstancia.newTemporal();
            Master.getInstancia.addBinaria(tem, Master.getInstancia.stack_p, size.ToString(), "+");
            string tem1 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem1, tem, "1", "+");
            Master.getInstancia.addSetStack(tem1, valor);
            Master.getInstancia.plusStack(size.ToString());
            Master.getInstancia.callFuncion("native_imprimir_str");
            Master.getInstancia.substracStack(size.ToString());
        }

    }
}
