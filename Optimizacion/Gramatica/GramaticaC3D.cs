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
            IdentifierTerminal Id = new IdentifierTerminal("Id");

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
            NonTerminal temporales = new NonTerminal("temporales");


            NonTerminal funcion = new NonTerminal("funcion");
            NonTerminal cuerpo = new NonTerminal("cuerpo");

            NonTerminal arit = new NonTerminal("arit");
            NonTerminal cond = new NonTerminal("cond");

            NonTerminal asignar = new NonTerminal("asignar");
            NonTerminal acceso_array = new NonTerminal("acceso_array");

            NonTerminal no_condicional = new NonTerminal("no_condicional");
            NonTerminal condicional = new NonTerminal("condicional");

            NonTerminal punto = new NonTerminal("punto");
            NonTerminal retorno = new NonTerminal("retorno");
            NonTerminal llamada = new NonTerminal("llamada");

            NonTerminal printf = new NonTerminal("printf");

            NonTerminal instrucciones = new NonTerminal("instrucciones");
            NonTerminal lista_funciones = new NonTerminal("lista_funciones");


            #endregion

            #region Gramatica

            init.Rule = encabezado;

            encabezado.Rule = libreria + estructura + accesos + temporales + lista_funciones;


            libreria.Rule = Tinclude + Tlibreria;

            estructura.Rule = arreglo + arreglo;

            arreglo.Rule = Tfloat + Id + TcorA + Entero + TcorC + TpuntoC;

            accesos.Rule = puntero + puntero;

            puntero.Rule = Tint + Id + TpuntoC;


            temporales.Rule = temp_float + temp_int;

            temp_int.Rule = Tint + lista_temp + TpuntoC;
            temp_float.Rule = Tfloat + lista_temp + TpuntoC;


            lista_temp.Rule = MakePlusRule(lista_temp, Tcoma, Temporal);

            lista_funciones.Rule = MakePlusRule(lista_funciones, funcion);


            //Manejo de funciones


            funcion.Rule = Tvoid + Id + TparA + TparC + TllaveA + instrucciones + TllaveC;

            instrucciones.Rule = MakePlusRule(instrucciones, cuerpo);

            cuerpo.Rule = asignar
                          | no_condicional
                          | condicional
                          | punto
                          | retorno
                          | llamada
                          | printf;


            //45 * T33;
            arit.Rule = Tresta + arit
                      | arit + Tsuma + arit
                      | arit + Tresta + arit
                      | arit + Tpor + arit
                      | arit + Tdiv + arit
                      | arit + Tmod + arit
                      | Entero
                      | Decimal
                      | Temporal
                      | acceso_array
                      | Id;


            cond.Rule = Tresta + cond
                      | cond + Tmenorq + cond
                      | cond + Tmenori + cond
                      | cond + Tmayorq + cond
                      | cond + Tmayori + cond
                      | cond + Tigual + cond
                      | cond + Tdiferente + cond
                      | Entero
                      | Decimal
                      | Temporal
                      | Id;

            acceso_array.Rule = Id + TcorA + (Temporal | Entero | Id) + TcorC;

            asignar.Rule = Temporal + Tasig + arit + TpuntoC
                           | Id + Tasig + arit + TpuntoC
                           | acceso_array + Tasig + arit + TpuntoC;


            // goto L2;
            no_condicional.Rule = Tsalto + Etiqueta + TpuntoC;

            // if (4==5) goto L1;
            condicional.Rule = Tif + TparA + cond + TparC + Tsalto + Etiqueta + TpuntoC;


            // L2:
            punto.Rule = Etiqueta + TdosP;

            //return ;
            retorno.Rule = Treturn + TpuntoC;


            //getcurso()
            llamada.Rule = Id + TparA + TparC + TpuntoC;


            printf.Rule = Tprint + TparA + (Td | Tf | Tc) + Tcoma + TparA + (Tint | Tfloat | Tchar) + TparC + arit + TparC + TpuntoC;

            #endregion

            #region Preferencias
            this.Root = init;

            RegisterOperators(1, Associativity.Left, Tmayorq, Tmenorq, Tmayori, Tmenori, Tigual, Tdiferente);
            RegisterOperators(2, Associativity.Left, Tsuma, Tresta, Tpor, Tdiv, Tmod);

            this.MarkReservedWords("void", "int", "float", "char", "return");

            #endregion

        }
    }
}
