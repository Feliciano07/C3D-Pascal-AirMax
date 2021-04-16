using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using C3D_Pascal_AirMax.Utilidades;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Instruccion.Funciones;

namespace C3D_Pascal_AirMax.Analisis
{
    public static class Declaracion_Proc
    {

        /*
         * Metodo que captura la declaracion de un procedimiento
         */


        public static void Crear_Procedimiento(ParseTreeNode entrada)
        {
            string id = entrada.ChildNodes[1].Token.Text;
            int linea = entrada.ChildNodes[1].Span.Location.Line;
            int columna = entrada.ChildNodes[1].Span.Location.Column;

            
            LinkedList<Parametro> parametros = Obtener_parametros(entrada.ChildNodes[3]);
            Objeto objeto = new Objeto(Objeto.TipoObjeto.VOID);

            if (entrada.ChildNodes.Count == 8)
            {
                LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();
                Declaracion_Variables(entrada.ChildNodes[6], instrucciones);
                Instrucciones_Funcion(entrada.ChildNodes[7], instrucciones);

                Procedimiento proc = new Procedimiento(linea, columna, id, parametros, instrucciones, objeto);
                Manejador.Master.getInstancia.compilar_fun(proc);
                

            }else if(entrada.ChildNodes.Count == 7)
            {
                LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();
                Instrucciones_Funcion(entrada.ChildNodes[6], instrucciones);
                Procedimiento proc = new Procedimiento(linea, columna, id, parametros, instrucciones,objeto);
                Manejador.Master.getInstancia.compilar_fun(proc);
            }

        }


        public static LinkedList<Parametro> Obtener_parametros(ParseTreeNode entrada)
        {
            LinkedList<Parametro> parametros = new LinkedList<Parametro>();
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                string term = node.Term.Name;
                switch (term)
                {
                    case "parametros_valor":
                        Parametros_Valor(node, parametros);
                        break;
                    case "parametros_referencia":
                        Parametros_Referencia(node, parametros);
                        break;
                }
            }
            return parametros;

        }

        public static void Parametros_Valor(ParseTreeNode entrada, LinkedList<Parametro> parametros)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;

            Objeto tipo = new Objeto(Variable.getTipo(entrada.ChildNodes[2]), Variable.Nombre_Tipo(entrada.ChildNodes[2]));

            getId(entrada.ChildNodes[0], tipo, Parametro.Tipo_Parametro.VALOR, parametros);
        }

        public static void Parametros_Referencia(ParseTreeNode entrada, LinkedList<Parametro> parametros)
        {
            ParseTreeNode referencia = entrada.ChildNodes[1];

            int linea = referencia.Span.Location.Line;
            int columna = referencia.Span.Location.Column;

            Objeto tipo = new Objeto(Variable.getTipo(referencia.ChildNodes[2]), Variable.Nombre_Tipo(referencia.ChildNodes[2]));

            getId(referencia.ChildNodes[0], tipo, Parametro.Tipo_Parametro.REFERENCIA, parametros);
        }

        public static void getId(ParseTreeNode entrada, Objeto objeto, Parametro.Tipo_Parametro tipo_Parametro, LinkedList<Parametro> parametros)
        {
            for(int i = 0; i < entrada.ChildNodes.Count; i++)
            {
                int linea = entrada.ChildNodes[i].Span.Location.Line;
                int columna = entrada.ChildNodes[i].Span.Location.Column;
                string nombre = entrada.ChildNodes[i].Token.Text;

                parametros.AddLast(new Parametro(nombre, objeto, tipo_Parametro));
            }
        }


        public static void Declaracion_Variables(ParseTreeNode entrada, LinkedList<Nodo> instrucciones)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                Declaraciones(node.ChildNodes[0], instrucciones);
            }
        }

        public static void Declaraciones(ParseTreeNode entrada, LinkedList<Nodo> instrucciones)
        {
            string toke = entrada.Term.Name;
            switch (toke)
            {
                case "variable":
                    lista_var(entrada.ChildNodes[1], instrucciones);
                    break;
                case "constante":
                    lista_constante(entrada.ChildNodes[1], instrucciones);
                    break;
            }
        }

        public static void lista_var(ParseTreeNode entrada, LinkedList<Nodo> instrucciones)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                Nodo salida = Variable.Evaluar_Variable(node);
                salida.pre_compilar = true;
                instrucciones.AddLast(salida);
            }
        }

        public static void lista_constante(ParseTreeNode entrada, LinkedList<Nodo> instrucciones)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                Nodo salida = Variable.Evaluar_Constante(node);
                salida.pre_compilar = true;
                instrucciones.AddLast(salida);
            }
        }


        public static void Instrucciones_Funcion(ParseTreeNode entrada, LinkedList<Nodo> lista)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes[1].ChildNodes)
            {
                Instrucciones(node.ChildNodes[0], lista);
            }
        }

        public static void Instrucciones(ParseTreeNode actual, LinkedList<Nodo> lista)
        {
            String toke = actual.Term.Name;

            switch (toke)
            {
                case "write":
                    lista.AddLast(Main.Inst_Write(actual, false));
                    break;
                case "writeln":
                    lista.AddLast(Main.Inst_Write(actual, true));
                    break;
                case "ifthen":
                    lista.AddLast(Main.Instruccion_IfThen(actual));
                    break;
                case "ifelse":
                    lista.AddLast(Main.Instruccion_Ifelse(actual));
                    break;
                case "caseof":
                    lista.AddLast(Main.Instruccion_case_of(actual.ChildNodes[0]));
                    break;
                case "whiledo":
                    lista.AddLast(Main.Instruccion_While(actual));
                    break;
                case "repeat":
                    lista.AddLast(Main.Repeat(actual));
                    break;
                case "no_for":
                    lista.AddLast(Main.For(actual));
                    break;
                case "asignacion":
                    lista.AddLast(Asignaciones.Tipo_asignacion(actual));
                    break;
                case "continue":
                    lista.AddLast(Sentencias.Sentencia_Continue(actual));
                    break;
                case "break":
                    lista.AddLast(Sentencias.Sentencia_break(actual));
                    break;
                case "exit":
                    lista.AddLast(Sentencias.Sentencia_Exit(actual));
                    break;
                case "sentencia_case":
                    lista.AddLast(Main.Instruccion_case_of(actual));
                    break;
                case "sentencia_while":
                    lista.AddLast(Main.Instruccion_While_If(actual));
                    break;
                case "sentencia_repeat":
                    lista.AddLast(Main.Repeat(actual));
                    break;
                case "llamada_funciones":
                    lista.AddLast(Main.Llamada_Funcion(actual));
                    break;
            }
        }


        public static void Crear_funcion(ParseTreeNode entrada)
        {
            int linea = entrada.ChildNodes[1].Span.Location.Line;
            int columna = entrada.ChildNodes[1].Span.Location.Column;

            string nombre_funcion = entrada.ChildNodes[1].Token.Text;
            LinkedList<Parametro> parametros = Obtener_parametros(entrada.ChildNodes[3]);

            Objeto tipo_objeto = new Objeto(Variable.getTipo(entrada.ChildNodes[6]), Variable.Nombre_Tipo(entrada.ChildNodes[6]));

            if(entrada.ChildNodes.Count == 10)
            {
                LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();

                Declaracion_Variables(entrada.ChildNodes[8], instrucciones);
                Instrucciones_Funcion(entrada.ChildNodes[9], instrucciones);

                Funcion funcion = new Funcion(linea, columna, nombre_funcion, parametros, instrucciones, tipo_objeto);

                Manejador.Master.getInstancia.compilar_fun(funcion);

            }
            else if(entrada.ChildNodes.Count == 9)
            {
                LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();
                Instrucciones_Funcion(entrada.ChildNodes[8], instrucciones);

                Funcion funcion = new Funcion(linea, columna, nombre_funcion, parametros, instrucciones, tipo_objeto);
                Manejador.Master.getInstancia.compilar_fun(funcion);
            }

        }

    }
}
