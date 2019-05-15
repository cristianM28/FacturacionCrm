using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Facturacion_CRM_Comercial.DA;
using Facturacion_CRM_Comercial.BE;
using Microsoft.Xrm.Sdk;
using System.Data;

namespace Facturacion_CRM_Comercial.BL
{
    public class FacturaBL
    {




        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 11/04/2019
        //Notas: llamada de las metodos de FacturaDA
        #endregion

        #region Variables
        FacturaDA fa = new FacturaDA();
        FuncionesDA fun = new FuncionesDA();
        #endregion


        #region Metodos

        /// <summary>
        /// Metodo que crea la factura con su detalle
        /// </summary>
        /// <param name="lic">parametros de la licencia csp</param>
        /// <param name="pre">parametros de precio licencia cliente</param>
        /// <returns> retorna la factura creada</returns>

        public void factura(LicenciaCspBE lic, PrecioLicenciaClienteBE pre)
        {
            try
            {
               fa.CrearFactura(lic, pre);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Metodo que obtiene el ultimo valor registrado
        /// </summary>
        /// <param name="idcli">parametro de guid del cliente </param>
        /// <param name="idpro">parametro guid del producto </param>
        /// <returns>Retorna el ultimo valor</returns>
        public Money UltimoValor( string idcli, string idpro)
        {
            try
            {
                return fun.RetornaUltimoValor(idcli, idpro);

            }
            catch (Exception ex )
            {

                throw ex ;
            }
        }

       
        #endregion






    } 
}
