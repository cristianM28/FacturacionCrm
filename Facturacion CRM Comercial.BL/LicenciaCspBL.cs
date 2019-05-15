using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion_CRM_Comercial.DA;
using Facturacion_CRM_Comercial.BE;
using System.Data;

namespace Facturacion_CRM_Comercial.BL
{
   public class LicenciaCspBL
    {

        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 11/04/2019
        //Notas: llamada de las metodos de LicenciaCspDA
        #endregion

        #region Variables

        LicenciaCSPDA licencia = new LicenciaCSPDA();
        FuncionesDA fun = new FuncionesDA();
        #endregion

        #region Metodos
        /// <summary>
        /// Obtiene los datos desde el crm 
        /// </summary>
        /// <returns>Retorna la clase con los datos</returns>
        public DataTable CapturDatosLicencia()
        {
            try
            {
                return fun.ObtenerDatosLicenciaCSP();


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la cantidad de los productos 
        /// </summary>
        /// <returns>Retorna la cantidad de las productos</returns>
        public int ObtenerCantidad(string id ,string idpro)
        {
            try
            {
                return licencia.CantidadProductos(id,idpro);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        #endregion




    }
}
