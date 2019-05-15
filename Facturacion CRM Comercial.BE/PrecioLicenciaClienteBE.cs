using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_CRM_Comercial.BE
{
   public class PrecioLicenciaClienteBE
    {

        #region Historia   
        //Autor: Cristian Marin.
        //Fecha: 16/04/2019
        //Notas: Clase Entidad Precio licencia Cliente
        #endregion



        #region Variables

        private string _cliente;

        public string Cliente
        {
            get { return _cliente; }
            set { _cliente = value; }
        }

        private DateTime _fecha;

        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }



        private string _producto;

        public string Producto
        {
            get { return _producto; }
            set { _producto = value; }
        }


        private Money _valor;

        public Money Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }


        private string _guidCliente;

        public string GuidCliente
        {
            get { return _guidCliente; }
            set { _guidCliente = value; }
        }
        private string _guidProducto;

        public string GuidProducto
        {
            get { return _guidProducto; }
            set { _guidProducto = value; }
        }

        private string _unidadPrede;

        public string UnidadPrede
        {
            get { return _unidadPrede; }
            set { _unidadPrede = value; }
        }

        private string _pais;

        public string Pais
        {
            get { return _pais; }
            set { _pais = value; }
        }
        private int _cantidad;

        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }



        #endregion

    }
}
