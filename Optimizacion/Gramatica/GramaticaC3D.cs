using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using Irony.Ast;

namespace C3D_Pascal_AirMax.Optimizacion.Gramatica
{
    public class GramaticaC3D:Grammar
    {
        public GramaticaC3D() : base(caseSensitive: false)
        {
            #region Expresiones
            CommentTerminal comentarioMulti = new CommentTerminal("CMM", "/*", "*/");
            base.NonGrammarTerminals.Add(comentarioMulti);
            //Enteros
            var Entero = new RegexBasedTerminal("entero", "[0-9]+");
            var Decimal = new RegexBasedTerminal("decimal", "[0-9]+[.][0-9]+");

            //Id de las funciones
            var Id = new RegexBasedTerminal("Id", "([a-zA-Z_])[a-zA-Z_]+");


            var Temporal = new RegexBasedTerminal("Temporal", "T[0-9]+");
            var Etiqueta = new RegexBasedTerminal("Etiqueta", "L[0-9]+");
            #endregion

            #region TERMINALES
            var Tsuma = ToTerm("+");
            var Tresta = ToTerm("-");
            var Tpor = ToTerm("*");
            var Tdiv = ToTerm("/");
            var Tmod = ToTerm("%");

            var Tsalto = ToTerm("goto");

            var Tmenorq = ToTerm("<");
            var Tmayorq = ToTerm(">");
            var Tmenori = ToTerm("<=");
            var Tmayori = ToTerm(">=");
            var Tigual = ToTerm("==");
            var Tdiferente = ToTerm("!=");

            var Tasig = ToTerm("=");

            var Tif = ToTerm("if");

            var TparA = ToTerm("(");
            var TparC = ToTerm(")");
            var TpuntoC = ToTerm(";");
            var TdosP = ToTerm(":");
            var Tcoma = ToTerm(",");

            var Tvoid = ToTerm("void");
            var TllaveA = ToTerm("{");
            var TllaveC = ToTerm("}");
            var Treturn = ToTerm("return");


            var Tprint = ToTerm("printf");

            var Tint = ToTerm("int");
            var Tfloat = ToTerm("float");
            var Tchar = ToTerm("char");

            var Td = ToTerm("\"%d\"");
            var Tc = ToTerm("\"%c\"");
            var Tf = ToTerm("\"%f\"");

            var TcorA = ToTerm("[");
            var TcorC = ToTerm("]");

            var Tinclude = ToTerm("#include");
            var Tlibreria = ToTerm("<stdio.h>");

            var Tstack = ToTerm("stack");
            var Theap = ToTerm("heap");
            var TstackP = ToTerm("SP");
            var TheapP = ToTerm("HP");

            #endregion


            #region NO_TERMINALES
            NonTerminal init = new NonTerminal("init");
            NonTerminal encabezado = new NonTerminal("encabezado");
            NonTerminal libreria = new NonTerminal("libreria");
            NonTerminal estructura = new NonTerminal("estructura");
            NonTerminal arreglo = new NonTerminal("arreglo");
            NonTerminal accesos = new NonTerminal("accesos");
            NonTerminal puntero = new NonTerminal("puntero");

            NonTerminal lista_temp = new NonTerminal("lista_temp");
            NonTerminal temp_int = new NonTerminal("temp_int");
            NonTerminal temp_float = new NonTerminal("temp_float");


            NonTerminal funcion = new NonTerminal("funcion");
            NonTerminal cuerpo = new NonTerminal("cuerpo");


            NonTerminal asignar = new NonTerminal("asignar");

            NonTerminal no_condicional = new NonTerminal("no_condicional");
            NonTerminal condicional = new NonTerminal("condicional");

            NonTerminal punto = new NonTerminal("punto");
            NonTerminal retorno = new NonTerminal("retorno");
            NonTerminal llamada = new NonTerminal("llamada");

            NonTerminal printf = new NonTerminal("printf");
            NonTerminal fin = new NonTerminal("fin");

            NonTerminal instrucciones = new NonTerminal("instrucciones");

            NonTerminal terminal = new NonTerminal("terminal");
            NonTerminal no_terminal = new NonTerminal("no_terminal");

            NonTerminal aux_terminal = new NonTerminal("aux_terminal");
            NonTerminal operacion = new NonTerminal("operacion");


            #endregion

            #region Gramatica

            init.Rule = encabezado + instrucciones;

            encabezado.Rule = libreria + estructura + accesos + temp_float + temp_int;


            libreria.Rule = Tinclude + Tlibreria;

            estructura.Rule = arreglo + arreglo;

            arreglo.Rule = Tfloat + (Tstack | Theap) + TcorA + Entero + TcorC + TpuntoC;

            accesos.Rule = puntero + puntero;

            puntero.Rule = Tint + (TstackP | TheapP) + TpuntoC;


            temp_int.Rule = Tint + lista_temp + TpuntoC;
            temp_float.Rule = Tfloat + lista_temp + TpuntoC;


            lista_temp.Rule = MakePlusRule(lista_temp, Tcoma, Temporal);


            instrucciones.Rule = MakePlusRule(instrucciones, cuerpo);


            funcion.Rule = Tvoid + Id + TparA + TparC + TllaveA;


            cuerpo.Rule = funcion //-
                          | operacion + TpuntoC
                          | asignar + TpuntoC
                          | punto //-
                          | condicional + TpuntoC
                          | no_condicional + TpuntoC
                          | retorno
                          | fin//-
                          | printf
                          | llamada; // -
            //| printf
            //| fin;


            terminal.Rule = Entero
                           | Decimal
                           | Temporal
                           | TstackP
                           | TheapP; //pointer stack pointer heap

            no_terminal.Rule = Tstack
                              | Theap;


            aux_terminal.Rule = Tresta + terminal
                       | terminal;


            asignar.Rule = terminal + Tasig + aux_terminal
                           |no_terminal + TcorA + aux_terminal + TcorC + Tasig + aux_terminal
                           | terminal + Tasig + no_terminal + TcorA + aux_terminal + TcorC;



            operacion.Rule = terminal + Tasig + aux_terminal + Tsuma + aux_terminal
                            | terminal + Tasig + aux_terminal + Tresta + aux_terminal
                            | terminal + Tasig + aux_terminal + Tpor + aux_terminal
                            | terminal + Tasig + aux_terminal + Tdiv + aux_terminal
                            | terminal + Tasig + aux_terminal + Tmod + aux_terminal;


            condicional.Rule = Tif + TparA + aux_terminal + Tmenorq + aux_terminal + TparC + Tsalto + Etiqueta
                              | Tif + TparA + aux_terminal + Tmenori + aux_terminal + TparC + Tsalto + Etiqueta
                              | Tif + TparA + aux_terminal + Tmayorq + aux_terminal + TparC + Tsalto + Etiqueta
                              | Tif + TparA + aux_terminal + Tmayori + aux_terminal + TparC + Tsalto + Etiqueta
                              | Tif + TparA + aux_terminal + Tigual + aux_terminal + TparC + Tsalto + Etiqueta
                              | Tif + TparA + aux_terminal + Tdiferente + aux_terminal + TparC + Tsalto + Etiqueta;

             


            // goto L2;
            no_condicional.Rule = Tsalto + Etiqueta;




            // L2:
            punto.Rule = Etiqueta + TdosP;

            //return ;
            retorno.Rule = Treturn + TpuntoC;


            //getcurso()
            llamada.Rule = Id + TparA + TparC + TpuntoC;


            printf.Rule = Tprint + TparA + (Td | Tf | Tc) + Tcoma + TparA + (Tint | Tfloat | Tchar) + TparC  + terminal + TparC + TpuntoC;

            fin.Rule = TllaveC;

            #endregion

            #region Preferencias
            this.Root = init;
            this.MarkReservedWords("void", "int", "float", "char", "return", "heap", "stack", "SP", "HP"
                ,"if", "goto", "printf");

            #endregion

        }
    }
}
