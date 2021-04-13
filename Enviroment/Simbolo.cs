using C3D_Pascal_AirMax.TipoDatos;
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

        private Objeto objeto;
        private string id;
        public int posicion;
        private Rol rol;
        public Pointer pointer;
        private string entorno;
        private bool isGlobal;
        public bool isReferencia;

        // para guardar variables locales
        public Simbolo(string id, Objeto tipo, Rol rol, Pointer pointer, int posicion, string entorno, bool global)
        {
            this.id = id;
            this.objeto = tipo;
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
            return this.objeto.getTipo();
        }

        public Objeto getObjeto()
        {
            return this.objeto;
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
