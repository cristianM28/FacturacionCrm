using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_CRM_Comercial.BE
{
  public   class FacturasBE
    {

        #region Historia   
        //Autor: Cristian Marin.
        //Fecha: 16/04/2019
        //Notas: Clase Entidad Factura
        #endregion

        #region Variables

        private string _nombreFactura;

        public string NombreFactura
        {
            get { return _nombreFactura; }
            set { _nombreFactura = value; }
        }

        private string _divisa;

        public string Divisa
        {
            get { return _divisa; }
            set { _divisa = value; }
        }


        private string _listaPrecios;

        public string ListaPrecios
        {
            get { return _listaPrecios; }
            set { _listaPrecios = value; }
        }

        private int _producto;

        public int Producto
        {
            get { return _producto; }
            set { _producto = value; }
        }

        private string _cliente;

        public string Cliente
        {
            get { return _cliente; }
            set { _cliente = value; }
        }

        private string _contacto;

        public string Contacto
        {
            get { return _contacto; }
            set { _contacto = value; }
        }

        private int _zenithEmpresa;

        public int ZenithEmpresa
        {
            get { return _zenithEmpresa; }
            set { _zenithEmpresa = value; }
        }


        #endregion

    }
}
