using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Control
{
    public class CaseOf : Nodo
    {
        private Nodo expresion;
        private LinkedList<Case> casos;
        private LinkedList<Nodo> instrucciones_else;


        public CaseOf(int linea, int columna, Nodo exp, LinkedList<Case> casos) : base(linea, columna)
        {
            this.expresion = exp;
            this.casos = casos;
        }

        public CaseOf(int linea, int columna,Nodo exp, LinkedList<Case> casos, LinkedList<Nodo> instruc) : base(linea, columna)
        {
            this.expresion = exp;
            this.casos = casos;
            this.instrucciones_else = instruc;
        }

        public override Retorno compilar(Entorno entorno)
        {
            string label_salida = Master.getInstancia.newLabel();
            Master.getInstancia.addComentario("case of");
            foreach(Case caso in this.casos)
            {
                if(caso != null)
                {
                    try
                    {
                        Retorno salida = caso.compilar_caso(entorno, this.expresion, label_salida);
                    }catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            Master.getInstancia.addLabel(label_salida);
            return null;
        }
    }
}
