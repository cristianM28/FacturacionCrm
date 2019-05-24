using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion_CRM_Comercial.DA;

namespace Facturacion_CRM_Comercial.BL
{
   public class FuncionesBL
    {


        #region Variables

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
        /// Se captura los datos desde crm
        /// </summary>
        /// <returns>Se retorna la clase con los datos </returns>
        public DataTable CapturaDatosCrm()
        {
            try
            {
                return fun.ObtenerDatosPrecioLicenciaCliente();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

         //public int ObtenerCantidad(string id ,string idpro)
         // {
         //     try
         //     {
         //         return fun.ObtenerCantidad(id,idpro);
         //     }
         //     catch (Exception ex)
         //     {

         //         throw ex ;
         //     }
         // }
          
        #endregion



    }
}
