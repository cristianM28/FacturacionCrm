using System;
using System.Collections.Generic;
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

        public string ObtenerPais( string id)
        {
            try
            {
                return fun.ObtenerPais(id);
            }
            catch (Exception ex )
            {

                throw ex;
            }
        }


      /*  public int ObtenerCantidad(string id ,string idpro)
        {
            try
            {
                return fun.ObtenerCantidad(id,idpro);
            }
            catch (Exception ex)
            {

                throw ex ;
            }
        }
        */
        #endregion



    }
}
