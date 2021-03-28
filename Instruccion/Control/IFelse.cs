using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Control
{
    public class IFelse : Nodo
    {
        private Nodo condicion;
        private LinkedList<Nodo> instrucciones_if;
        private LinkedList<Nodo> instrucciones_else;

        public IFelse(int linea, int columna, Nodo condicion, LinkedList<Nodo> iif, LinkedList<Nodo> eelse) : base(linea, columna)
        {
            this.condicion = condicion;
            this.instrucciones_if = iif;
            this.instrucciones_else = eelse;
        }

        public override Retorno compilar(Entorno entorno)
        {
            Master.getInstancia.addComentario("if then else");
            Retorno aux = this.condicion.compilar(entorno);


            if(aux.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
            {
                Master.getInstancia.addLabel(aux.trueLabel);
                Correr_if(entorno);

                string temLabel = Master.getInstancia.newLabel();
                Master.getInstancia.addGoto(temLabel);
                Master.getInstancia.addLabel(aux.falseLabel);
                Correr_Else(entorno);
                Master.getInstancia.addLabel(temLabel);
                return null;

            }
            Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
               "La condicion no es booleana en el if then else: " + aux.getTipo().ToString());
            Master.getInstancia.addError(error);
            throw new Exception("La condicion no es booleana en el if then else");
        }

        public void Correr_if(Entorno entorno)
        {
            foreach(Nodo instruccion in this.instrucciones_if)
            {
                if(instruccion != null)
                {
                    try
                    {
                        Retorno salida = instruccion.compilar(entorno);
                    }catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
        public void Correr_Else(Entorno entorno)
        {
            foreach (Nodo instruccion in this.instrucciones_else)
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
    }
}
