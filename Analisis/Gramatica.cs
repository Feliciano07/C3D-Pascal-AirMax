using System;
using System.Collections.Generic;
using System.Text;
using Irony.Ast;
using Irony.Parsing;

namespace C3D_Pascal_AirMax.Analisis
{
    public class Gramatica: Grammar
    {
        public Gramatica() : base(caseSensitive: false)
        {
            #region ER

            //TODO: seccion de comentarios
            CommentTerminal comentarioSimple = new CommentTerminal("CMS", "//", "\r", "\n", "\r\n", "\u2085", "\u2028", "\u2029");
            CommentTerminal comentarioMulti = new CommentTerminal("CMM", "(*", "*)");
            CommentTerminal comentariMulti2 = new CommentTerminal("CMM2", "{", "}");
            base.NonGrammarTerminals.Add(comentarioSimple);
            base.NonGrammarTerminals.Add(comentarioMulti);
            base.NonGrammarTerminals.Add(comentariMulti2);
            // seccion de numero 
            var Entero = new RegexBasedTerminal("entero", "[0-9]+");
            var Decimal = new RegexBasedTerminal("decimal", "[0-9]+[.][0-9]+");
            // Seccion de cadena
            var Cadena = new RegexBasedTerminal("cadena", "\'[^\'\n]*[\']");
            //Seccion de id
            IdentifierTerminal Id = new IdentifierTerminal("Id");
            #endregion

            #region TERMINALES

            // SECCION DE OPERADORES aritmeticos
            var Tsuma = ToTerm("+");
            var Tresta = ToTerm("-");
            var Tpor = ToTerm("*");
            var Tdiv = ToTerm("/");
            var Tmod = ToTerm("mod");
            var Tfalse = ToTerm("false");
            var Ttrue = ToTerm("true");
            //Seccion de operadores relacionales
            var Tmayorq = ToTerm(">");
            var Tmenorq = ToTerm("<");
            var Tmayori = ToTerm(">=");
            var Tmenori = ToTerm("<=");
            var Tigual = ToTerm("="); // para comparar
            var Tdiferencia = ToTerm("<>"); // son diferentes

            //seccion de operaciones logicas
            var Tand = ToTerm("and");
            var Tor = ToTerm("or");
            var Tnot = ToTerm("not");

            var TparA = ToTerm("(");
            var TparC = ToTerm(")");
            var TDIV = ToTerm("div");

            //declaracion de variables y constantes, asi mismo inicar con un valor
            var Tvar = ToTerm("var");
            var TCon = ToTerm("const");
            var Tpuntocoma = ToTerm(";");
            var Tcoma = ToTerm(",");
            var Tdospunto = ToTerm(":");
            var Tasignar = ToTerm(":=");
            var Tpunto = ToTerm(".");

            //TIPO DE DATOS VALIDOS
            var Tstring = ToTerm("string");
            var Tinteger = ToTerm("integer");
            var Treal = ToTerm("real");
            var Tboolean = ToTerm("boolean");

            

            var Ttype = ToTerm("type");
            var Tarray = ToTerm("array");
            var Tof = ToTerm("of");
            var Tdimension = ToTerm("..");
            var TcorA = ToTerm("[");
            var TcorC = ToTerm("]");

            var Tobject = ToTerm("object");
            var Tend = ToTerm("end");
            var Tbegin = ToTerm("begin");

            var Tif = ToTerm("if");
            var Tthen = ToTerm("then");
            var Twriteln = ToTerm("writeln");
            var Telse = ToTerm("else");

            var Tcase = ToTerm("case");
            var Twhile = ToTerm("while");
            var Tdo = ToTerm("do");

            var Trepeat = ToTerm("repeat");
            var Tuntil = ToTerm("until");

            var Tfor = ToTerm("for");
            var Tto = ToTerm("to");
            var Tdown = ToTerm("downto");

            var Tbreak = ToTerm("break");
            var Tcontinue = ToTerm("continue");

            var Twrite = ToTerm("write");

            var Tfuncion = ToTerm("function");

            var Texit = ToTerm("exit");

            var Tprocedure = ToTerm("procedure");

            var Tprogram = ToTerm("program");

            var Tgraficar = ToTerm("graficar_ts");

            #endregion


            #region NO_TERMINALES
            NonTerminal init = new NonTerminal("init");
            NonTerminal instrucciones = new NonTerminal("instrucciones");
            NonTerminal instruccion = new NonTerminal("instruccion");
            NonTerminal exp = new NonTerminal("exp");
            NonTerminal variable = new NonTerminal("variable");
            NonTerminal tipo_dato = new NonTerminal("tipo_dato");
            NonTerminal lista_id = new NonTerminal("lista_id");
            NonTerminal lista_dec = new NonTerminal("lista_dec");
            NonTerminal lista_variable = new NonTerminal("lista_variable");

            NonTerminal constante = new NonTerminal("constante");
            NonTerminal lista_constante = new NonTerminal("lista_constante");
            NonTerminal id_constante = new NonTerminal("id_constante");

            NonTerminal arrays = new NonTerminal("arrays");
            NonTerminal lista_arrays = new NonTerminal("lista_arrays");
            NonTerminal id_arrays = new NonTerminal("id_arrays");

            NonTerminal objectos = new NonTerminal("objectos");


            NonTerminal lista_main = new NonTerminal("lista_main");
            NonTerminal main = new NonTerminal("main");
            NonTerminal opciones_main = new NonTerminal("opciones_main");
            NonTerminal sentencias_main = new NonTerminal("sentencias_main");

            NonTerminal inicio_programa = new NonTerminal("inicio_programa");



            NonTerminal asignacion = new NonTerminal("asignacion");

            NonTerminal writeln = new NonTerminal("writeln");
            NonTerminal write = new NonTerminal("write");

            NonTerminal ifthen = new NonTerminal("ifthen");
            NonTerminal opcion_if = new NonTerminal("opcion_if");
            NonTerminal opcion_else = new NonTerminal("opcion_else");

            NonTerminal ifelse = new NonTerminal("ifelse");
            NonTerminal main_stm = new NonTerminal("main_stm");

            NonTerminal caseof = new NonTerminal("caseof");
            NonTerminal sentencia_case = new NonTerminal("sentencia_case");
            NonTerminal lista_exp = new NonTerminal("lista_exp");
            NonTerminal lista_casos = new NonTerminal("lista_casos");
            NonTerminal caso = new NonTerminal("caso");

            NonTerminal sentencia_while = new NonTerminal("sentencia_while");
            NonTerminal whiledo = new NonTerminal("whiledo");

            NonTerminal repeat = new NonTerminal("repeat");
            NonTerminal sentencia_repeat = new NonTerminal("sentencia_repeat");

            NonTerminal non_for = new NonTerminal("no_for");
            NonTerminal sentencia_for = new NonTerminal("sentencia_for");

            NonTerminal lista_dimension = new NonTerminal("lista_dimension");
            NonTerminal dimension = new NonTerminal("dimension");

            NonTerminal acceso_array = new NonTerminal("acceso_array");

            NonTerminal dec_funcion = new NonTerminal("dec_funcion");

            NonTerminal parametros = new NonTerminal("parametros");
            NonTerminal parametros_valor = new NonTerminal("parametros_valor");
            NonTerminal parametros_referencia = new NonTerminal("parametros_referencia");

            NonTerminal var_funciones = new NonTerminal("var_funciones");
            NonTerminal variable_fun = new NonTerminal("variable_fun");



            NonTerminal dec_procedimiento = new NonTerminal("dec_procedimiento");

            NonTerminal llamada_funciones = new NonTerminal("llamada_funciones");

            NonTerminal acceso_objeto = new NonTerminal("acceso_objeto");
            NonTerminal ingreso = new NonTerminal("ingreso");


            NonTerminal encabezado = new NonTerminal("encabezado");

            NonTerminal instrucciones_objeto = new NonTerminal("instrucciones_objeto");
            NonTerminal instruccion_objeto = new NonTerminal("instruccion_objeto");

            NonTerminal exit = new NonTerminal("exit");

            NonTerminal if_then_else = new NonTerminal("if_then_else");

            NonTerminal lista_if_else = new NonTerminal("lista_if_else");

            NonTerminal mas_if = new NonTerminal("mas_if");

            NonTerminal tabla_sym = new NonTerminal("tabla_sym");


            #endregion


            #region GRAMATICA

            init.Rule = encabezado;

            encabezado.Rule = Tprogram + Id + Tpuntocoma + instrucciones + inicio_programa
                             | Tprogram + Id + Tpuntocoma + inicio_programa;


            instrucciones.Rule = MakePlusRule(instrucciones, instruccion);

            // esto es lo que estaria arriba antes del main, exepto exp
            instruccion.Rule = 
                              variable
                              | constante
                              | arrays
                              | objectos
                              | dec_funcion
                              | dec_procedimiento;

            inicio_programa.Rule = Tbegin + opciones_main + Tend + Tpunto;


            

            opciones_main.Rule = MakePlusRule(opciones_main, sentencias_main)
                                 | MakePlusRule(opciones_main, main); //*

            sentencias_main.Rule = Tbegin + lista_main + Tend + Tpuntocoma;


            exp.Rule = Tnot + exp //*
                       | Tresta + exp //*
                       | exp + Tsuma + exp //*
                       | exp + Tresta + exp //*
                       | exp + Tpor + exp //*
                       | exp + Tdiv + exp //*
                       | exp + TDIV + exp //*
                       | exp + Tmod + exp //*
                       | exp + Tmayorq + exp //*
                       | exp + Tmenorq + exp //*
                       | exp + Tmayori + exp //*
                       | exp + Tmenori + exp //*
                       | exp + Tigual + exp //*
                       | exp + Tdiferencia + exp //*
                       | exp + Tand + exp //*
                       | exp + Tor + exp //*
                       | Entero //*
                       | Decimal //*
                       | Cadena //*
                       | Ttrue //*
                       | Tfalse //*
                       | Id //
                       | acceso_array //
                       | llamada_funciones
                       | acceso_objeto //
                       | TparA + exp + TparC; //*

            // LLamada de una funcion

            llamada_funciones.Rule = Id + TparA + lista_exp + TparC
                                    | Id + TparA + TparC;



            tipo_dato.Rule = Tstring
                             | Tinteger
                             | Treal
                             | Tboolean
                             | Id; // como se declaran variables de objectos

            //************************ DECLARACION DE VARIABLES
            variable.Rule = Tvar + lista_variable; //*

            lista_variable.Rule = MakePlusRule(lista_variable, lista_dec); //*


            lista_dec.Rule = lista_id + Tdospunto + tipo_dato + Tpuntocoma //*
                             | Id + PreferShiftHere() + Tdospunto + tipo_dato + Tigual + exp + Tpuntocoma //*
                             | Id + PreferShiftHere() + Tdospunto + tipo_dato + Tpuntocoma //*

                             // lo de abajo ya no
                             | lista_id + Tdospunto + Tarray + TcorA + lista_dimension + TcorC + Tof + tipo_dato + Tpuntocoma
                             | Id + PreferShiftHere() + Tdospunto + Tarray + TcorA + lista_dimension + TcorC + Tof + tipo_dato + Tpuntocoma;

            lista_id.Rule = MakePlusRule(lista_id, Tcoma, Id);

            //*************************** DECLARACION DE CONSTANTES

            constante.Rule = TCon + lista_constante; //*

            lista_constante.Rule = MakePlusRule(lista_constante, id_constante); //*

            id_constante.Rule = Id + Tdospunto + tipo_dato + Tigual + exp + Tpuntocoma //*
                               | Id + Tigual + exp + Tpuntocoma; //*


            //***************************** DECLARACION DE UN ARRAY

            arrays.Rule = Ttype + lista_arrays;//*

            lista_arrays.Rule = MakePlusRule(lista_arrays, id_arrays);//*

            id_arrays.Rule = Id + Tigual + Tarray + TcorA + lista_dimension + TcorC + Tof + tipo_dato + Tpuntocoma;//*

            lista_dimension.Rule = MakeListRule(lista_dimension, Tcoma, dimension);//*

            dimension.Rule = exp + Tdimension + exp;//*

            //******************************* DECLARACION DE OBJECTOS

            objectos.Rule = Ttype + Id + Tigual + Tobject + instrucciones_objeto + Tend + Tpuntocoma //*
                           | Ttype + Id + Tigual + Tobject + Tend + Tpuntocoma;//*


            instrucciones_objeto.Rule = MakePlusRule(instrucciones_objeto, instruccion_objeto); //*

            instruccion_objeto.Rule = variable//*
                              | constante//*
                                         //esto ya no
                              | arrays
                              | objectos;




            // ******************* flujo interno del programa, sentencias y asignaciones
            lista_main.Rule = MakePlusRule(lista_main, main);


            //posible que venga dentro mas begin end

            main.Rule = asignacion + Tpuntocoma
                         | writeln + Tpuntocoma //*
                         | write + Tpuntocoma //*
                         | exit + Tpuntocoma
                         | tabla_sym + Tpuntocoma
                         | ifthen //*
                         | ifelse //*
                         | caseof ///*
                         | whiledo //*
                         | repeat //*
                         | non_for //*
                         | Tcontinue + Tpuntocoma//*
                         | Tbreak + Tpuntocoma //*
                         | llamada_funciones + Tpuntocoma
                         ;





            asignacion.Rule = Id + Tasignar + exp //*
                            | acceso_array + Tasignar + exp //*
                            | acceso_objeto + Tasignar + exp; //*



            acceso_array.Rule = Id + TcorA + lista_exp + TcorC; //*


            // acceso objeto
            acceso_objeto.Rule = Id + Tpunto + ingreso //*
                                | Id + TcorA + lista_exp + TcorC + Tpunto + ingreso; //*

            ingreso.Rule = MakeListRule(ingreso, Tpunto, Id) //*
                           | MakeListRule(ingreso, Tpunto, acceso_array); //*





            //funciones nativas

            writeln.Rule = Twriteln + TparA + lista_exp + TparC //*
                            | Twriteln + TparA + TparC
                            | Twriteln;  //*


            write.Rule = Twrite + TparA + lista_exp + TparC //*
                        | Twrite + TparA + TparC
                        | Twrite; //*


            exit.Rule = Texit + TparA + exp + TparC
                        | Texit + TparA + TparC
                        | Texit;

            tabla_sym.Rule = Tgraficar + TparA + TparC;


            //**************************** sentencias if then, puede venir con punto y coma?

            ifthen.Rule = Tif + exp + Tthen + main //*
                          | Tif + exp + Tthen + Tbegin + lista_main + Tend + Tpuntocoma //*
                          | Tif + exp + Tthen + Tbegin + Tend + Tpuntocoma; //*



            //**************************** SENTENCIA IF THEN ELSE


            ifelse.Rule = Tif + exp + Tthen + opcion_if + Telse + main //*
                         | Tif + exp + Tthen + opcion_if + Telse + Tbegin + lista_main + Tend + Tpuntocoma;


            //lista_if_else.Rule = MakePlusRule(lista_if_else, if_then_else);

            //           if_then_else.Rule = Telse + Tif + exp + Tthen + opcion_if;


            

            opcion_if.Rule = asignacion //*
                             | writeln //*
                             | write //*
                             | exit
                             | tabla_sym
                             | opcion_else //*
                             | sentencia_case //*
                             | sentencia_while //*
                             | sentencia_repeat //*
                             | sentencia_for
                             | Tbreak
                             | Tcontinue
                             | llamada_funciones
                             | Tbegin + lista_main + Tend;

            main_stm.Rule = asignacion + Tpuntocoma
                             | writeln + Tpuntocoma
                             | opcion_else
                             | caseof
                             | whiledo
                             | Tbegin + lista_main + Tend + Tpuntocoma;


            opcion_else.Rule = Tif + exp + Tthen + opcion_if + Telse + opcion_if; //*






            //*************************** SENTENCIA CASE, existe conflicto reduce pero sirve 

            caseof.Rule = sentencia_case + Tpuntocoma; //*



            sentencia_case.Rule = Tcase + exp + Tof + lista_casos + Tend//*
                                  | Tcase + exp + Tof + lista_casos + Telse + main + Tend//*
                                  | Tcase + exp + Tof + lista_casos + Telse + Tbegin + lista_main + Tend + Tpuntocoma + Tend; //*

            lista_exp.Rule = MakeListRule(lista_exp, Tcoma, exp); //*

            lista_casos.Rule = MakePlusRule(lista_casos, caso); //*

            caso.Rule = lista_exp + Tdospunto + main //*
                        | lista_exp + Tdospunto + Tbegin + lista_main + Tend + Tpuntocoma; //*


            //**************************** SENTENCIA WHILE DO

            whiledo.Rule = Twhile + exp + Tdo + main //*
                          | Twhile + exp + Tdo + Tbegin + lista_main + Tend + Tpuntocoma; //*


            sentencia_while.Rule = Twhile + exp + Tdo + opcion_if; //*

            //********************************  SENTENCIA REPEAT
            /*
             * dentro del bloque funcion igual al main
             */

            repeat.Rule = Trepeat + opciones_main + Tuntil + exp + Tpuntocoma; //*

            sentencia_repeat.Rule = Trepeat + opciones_main + Tuntil + exp; //*


            //*************************** SENTENCIA FOR-DO, el for solo aceptar variables normales

            non_for.Rule = Tfor + asignacion + Tto + exp + Tdo + main
                          | Tfor + asignacion + Tto + exp + Tdo + Tbegin + lista_main + Tend + Tpuntocoma
                          | Tfor + asignacion + Tdown + exp + Tdo + main
                          | Tfor + asignacion + Tdown + exp + Tdo + Tbegin + lista_main + Tend + Tpuntocoma;


            sentencia_for.Rule = Tfor + asignacion + Tto + exp + Tdo + opcion_if
                                | Tfor + asignacion + Tdown + exp + Tdo + opcion_if;


            // declaracion de una funcion

            dec_funcion.Rule = Tfuncion + Id + TparA + parametros + TparC + Tdospunto + tipo_dato + Tpuntocoma
                                 + var_funciones + sentencias_main

                              | Tfuncion + Id + TparA + parametros + TparC + Tdospunto + tipo_dato + Tpuntocoma
                                 + sentencias_main;

            parametros.Rule = MakeListRule(parametros, Tpuntocoma, parametros_valor)
                            | MakeListRule(parametros, Tpuntocoma, parametros_referencia)
                            | this.Empty;



            parametros_valor.Rule = lista_id + Tdospunto + tipo_dato;

            parametros_referencia.Rule = Tvar + parametros_valor;

            var_funciones.Rule = MakePlusRule(var_funciones, variable_fun);

            variable_fun.Rule = variable
                              | constante
                              // esto de abajo no
                              | arrays
                              | objectos
                               ;

            // declarar prrocedimientos

            dec_procedimiento.Rule = Tprocedure + Id + TparA + parametros + TparC + Tpuntocoma
                                 + var_funciones + sentencias_main

                              | Tprocedure + Id + TparA + parametros + TparC + Tpuntocoma
                                 + sentencias_main;



            #endregion


            #region PREFERENCIA
            this.Root = init;
            //RegisterOperators(1, Associativity.Left, Tor);
            //RegisterOperators(2, Associativity.Left, Tand);
            //RegisterOperators(1, Associativity.Left, Tigual, Tdiferencia);
            RegisterOperators(1, Associativity.Left, Tmayorq, Tmenorq, Tmayori, Tmenori, Tigual, Tdiferencia);
            RegisterOperators(2, Associativity.Left, Tsuma, Tresta, Tor);
            RegisterOperators(3, Associativity.Left, Tpor, Tdiv, Tmod, TDIV, Tand); // aca se agrega el modulo
            RegisterOperators(4, Associativity.Right, Tnot);
            RegisterOperators(5, Associativity.Left, TparA, TparC);


            this.MarkReservedWords("var", "const", "if", "else", "begin", "end",
                "case", "false", "true", "string", "integer", "real", "boolean", "type", "of", "array", "object",
                "then", "writeln", "while", "do", "repeat", "until", "write", "for", "to", "down", "function", "break", "continue",
                "exit", "procedure", "mod", "graficar_ts");


            //this.MarkTransient(lista_id);
            #endregion
        }
    }
}
