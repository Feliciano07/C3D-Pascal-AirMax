using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Expresion.Relacionales;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Control
{
    public class Case : Nodo
    {
        private LinkedList<Nodo> condiciones;
        private LinkedList<Nodo> instrucciones;

        public Case(int linea, int columna, LinkedList<Nodo> condi, LinkedList<Nodo> instru):base(linea, columna)
        {
            this.condiciones = condi;
            this.instrucciones = instru;
        }

        public override Retorno compilar(Entorno entorno)
        {
            throw new NotImplementedException();
        }

        public Retorno compilar_caso(Entorno entorno, Nodo left, string label_salida)
        {
            foreach(Nodo right in this.condiciones)
            {

                Igual igual = new Igual(base.getLinea(), base.getColumna(), left, right);

                Retorno salida = igual.compilar(entorno);

                if(salida.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
                {
                    Master.getInstancia.addLabel(salida.trueLabel);
                    Correr_Caso(entorno);
                    Master.getInstancia.addGoto(label_salida);
                    Master.getInstancia.addLabel(salida.falseLabel);
                }

            }
            return null;
        }

        public void Correr_Caso(Entorno entorno)
        {
            foreach(Nodo instruccion in this.instrucciones)
            {
                if (instruccion != null)
                {
                    try
                    {
                        Retorno salida = instruccion.compilar(entorno);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        public void Evaluar_Tipo(Retorno left, Retorno right)
        {
            if(left.getTipo() != right.getTipo())
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "La constante y el caso evaluado no son del mismo tipo");
                Master.getInstancia.addError(error);
                throw new Exception("la constante y el caso evaluado no son del mismo tipo");
            }
        }
    }
}
