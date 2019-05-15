using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_CRM_Comercial.BE
{
   public class ProductoDeLaFacturaBE
    {

        #region Historia   
        //Autor: Cristian Marin.
        //Fecha: 16/04/2019
        //Notas: Clase Entidad Producto de la factura
        #endregion


        #region Variables

        private int _precioUnidad;

        public int PrecioUnidad
        {
            get { return _precioUnidad; }
            set { _precioUnidad = value; }
        }

        private int _cantidad;

        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        private int _producto;

        public int Producto
        {
            get { return _producto; }
            set { _producto = value; }
        }


        #endregion



    }
}
