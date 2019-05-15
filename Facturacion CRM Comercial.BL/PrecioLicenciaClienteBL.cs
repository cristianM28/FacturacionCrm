using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion_CRM_Comercial.DA;
using Facturacion_CRM_Comercial.BE;
using System.Data;
using Microsoft.Xrm.Sdk;

namespace Facturacion_CRM_Comercial.BL
{
  public  class PrecioLicenciaClienteBL
    {


        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 11/04/2019
        //Notas: llamada de las metodos de PrecioLicenciaClienteDA
        #endregion

        #region Variables

        PrecioLicenciaClienteDA pre = new PrecioLicenciaClienteDA();

        FuncionesDA fun = new FuncionesDA();
        #endregion

        #region Metodos

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
            catch (Exception ex )
            {

                throw ex ;
            }
        }


        /// <summary>
        /// Se obtiene el ultimo valor registrado 
        /// </summary>
        /// <returns>Retorna el ultimo valor </returns>
        public Money UltimoValor( string idcli, string idpro)
        {
            try
            {
                return pre.UltimoValor(idcli,  idpro);
            }
            catch (Exception ex)
            {

                throw ex ;
            }
        }
        #endregion
    }
}
