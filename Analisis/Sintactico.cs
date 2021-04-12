using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.Nativas;
using Irony.Parsing;

namespace C3D_Pascal_AirMax.Analisis
{
    public class Sintactico
    {

        public enum Pasada
        {
            TYPES,
            FUNCIONES,
            VARIABLES
        }
        
        public bool Analizar(string texto)
        {
            Gramatica gramatica= new Gramatica();
            LanguageData languageData = new LanguageData(gramatica);
            Parser parser = new Parser(languageData);
            ParseTree parseTree = parser.Parse(texto);
            ParseTreeNode raiz = parseTree.Root;

            if(raiz == null)
            {
                return false;
            }

           // genera el ast, para ver como se esta construyendo
            GenerarAST(raiz);

            // carga las funciones nativas utilizadas para operaciones basica
            CargarNativas();

            encabezado(raiz.ChildNodes[0]);

            // ejecutar
            Master.getInstancia.ejecutar_nativas();
            Master.getInstancia.ejecutar();

            Master.getInstancia.ReccorerErrores();

            return true;

        }

        public void encabezado(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count == 5)
            {
                // declaracion de variables, objetos, arrays, funciones
                declaraciones(actual.ChildNodes[3], Pasada.TYPES);
                declaraciones(actual.ChildNodes[3], Pasada.FUNCIONES);
                declaraciones(actual.ChildNodes[3], Pasada.VARIABLES);

                Instrucciones(actual.ChildNodes[4].ChildNodes[1]);

            }
            else if (actual.ChildNodes.Count == 4)
            {
                Instrucciones(actual.ChildNodes[3].ChildNodes[1]);
            }
        }

        public void declaraciones(ParseTreeNode actual, Pasada pasada)
        {

            foreach (ParseTreeNode node in actual.ChildNodes)
            {

                declaracion(node.ChildNodes[0], pasada);

            }
        }

        // esto deberia de guardarse antes
        public void declaracion(ParseTreeNode actual, Pasada pasada)
        {
            String toke = actual.Term.Name;

            if(pasada == Pasada.TYPES)
            {
                switch (toke)
                {
                    case "objectos":
                        TypeObjeto.Definicion_Objeto(actual);
                        break;
                    case "arrays":
                        TypeArray.Declaracion_Arreglo(actual.ChildNodes[1]);
                        break;
                }
            }else if(pasada == Pasada.FUNCIONES)
            {
                switch (toke)
                {
                    case "dec_procedimiento":
                        Declaracion_Proc.Crear_Procedimiento(actual);
                        break;
                    case "dec_funcion":
                        break;
                }
            }
            else
            {
                switch (toke)
                {

                    case "variable":
                        Variable.Lista_variables(actual.ChildNodes[1]);
                        break;
                    case "constante":
                        Variable.Lista_Constante(actual.ChildNodes[1]);
                        break;
                }
            }


        }

        public void Instrucciones(ParseTreeNode entrada)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                String tipo = node.Term.Name;
                switch (tipo)
                {
                    case "main":
                        Master.getInstancia.addInstruccion(Instruccion(node.ChildNodes[0]));
                        break;
                    case "sentencias_main":
                        foreach (ParseTreeNode node2 in node.ChildNodes[1].ChildNodes)
                        {
                            Master.getInstancia.addInstruccion(Instruccion(node2.ChildNodes[0]));
                        }
                        break;

                }
            }
        }

        public Nodo Instruccion(ParseTreeNode actual)
        {
            String toke = actual.Term.Name;
            switch (toke)
            {
                case "write":
                    return Main.Inst_Write(actual,false);
                case "writeln":
                    return Main.Inst_Write(actual,true);
                case "ifthen":
                    return Main.Instruccion_IfThen(actual);
                case "ifelse":
                    return Main.Instruccion_Ifelse(actual);
                case "caseof":
                    return Main.Instruccion_case_of(actual.ChildNodes[0]);
                case "whiledo":
                    return Main.Instruccion_While(actual);
                case "repeat":
                    return Main.Repeat(actual);
                case "no_for":
                    return Main.For(actual);
                case "asignacion":
                    return Asignaciones.Tipo_asignacion(actual);
                case "llamada_funciones":
                    return Main.Llamada_Funcion(actual);
                default:
                    break;
            }

            return null;
        }


        public void CargarNativas()
        {
            // esto meterlo en una estructura aparte

            Master.getInstancia.nativas.AddLast(new Nativa_Concatenar(0,0));
            Master.getInstancia.nativas.AddLast(new Nativa_Igual(0, 0));
            Master.getInstancia.nativas.AddLast(new Nativa_MayorIgual(0, 0));
            Master.getInstancia.nativas.AddLast(new Nativa_Mayor());
            Master.getInstancia.nativas.AddLast(new Nativa_NoIgual());
            Master.getInstancia.nativas.AddLast(new Nativa_Menor());
            Master.getInstancia.nativas.AddLast(new Nativa_MenorIgual());
            Master.getInstancia.nativas.AddLast(new Nativa_Imprimir());
        }

        public void GenerarAST(ParseTreeNode raiz)
        {
            Utilidades.Dot.GenerarAST(raiz);
        }
    }
}
