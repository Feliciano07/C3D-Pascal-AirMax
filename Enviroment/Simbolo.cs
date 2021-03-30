﻿using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class Simbolo
    {

        public enum Rol:int{
            VARIABLE= 1,
            CONSTANTE =2,
        }
        public enum Pointer:int
        {
            STACK = 1,
            HEAP = 2
        }

        private Objeto.TipoObjeto tipo;
        private string id;
        private int posicion;
        private Rol rol;
        private Pointer pointer;
        private string entorno;
        private bool isGlobal;

        // para guardar variables locales
        public Simbolo(string id, Objeto.TipoObjeto tipo, Rol rol, Pointer pointer, int posicion, string entorno, bool global)
        {
            this.id = id;
            this.tipo = tipo;
            this.rol = rol;
            this.pointer = pointer;
            this.posicion = posicion;
            this.entorno = entorno;
            this.isGlobal = global;
        }


        public string getId()
        {
            return this.id;
        }


        public Objeto.TipoObjeto getTipo()
        {
            return this.tipo;
        }

        public Simbolo.Rol getRol()
        {
            return this.rol;
        }

        public string getPosicion()
        {
            return this.posicion.ToString();
        }
        public void setPosicion(int posicion)
        {
            this.posicion = posicion;
        }

        public bool getGlobal()
        {
            return this.isGlobal;
        }

        public string getEntorno()
        {
            return this.entorno;
        }
    }
}
