using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_CRM_Comercial.BE
{
  public  class LicenciaCspBE
    {

        #region Historia   
        //Autor: Cristian Marin.
        //Fecha: 16/04/2019
        //Notas: Clase Entidad Licencia CSP
        #endregion


        #region "Variables"
        private string _Cliente;

        public string Cliente
        {
            get { return _Cliente; }
            set { _Cliente = value; }
        }

        private string _facturarA;

        public string FacturarA
        {
            get { return _facturarA; }
            set { _facturarA = value; }
        }

        private string _Produto;

        public string Producto
        {
            get { return _Produto; }
            set { _Produto = value; }
        }

        private int _cantidad;
                
        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        private string _guid_cliente;

        public string Guid_cliente
        {
            get { return _guid_cliente; }
            set { _guid_cliente = value; }
        }

        private string _guid_Producto;

        public string Guid_Producto
        {
            get { return _guid_Producto; }
            set { _guid_Producto = value; }
        }

        private string _pasi;

        public string Pais
        {
            get { return _pasi; }
            set { _pasi = value; }
        }


        #endregion

    }
}
