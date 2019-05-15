using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion_CRM_Comercial.BE;
using Microsoft.Xrm.Sdk;

namespace Facturacion_CRM_Comercial.DA
{
  public  class PrecioLicenciaClienteDA
    {
        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 11/04/2019
        //Notas: Metodos para la clase precio licencia cliente
        #endregion


        #region Variables
        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
        PrecioLicenciaClienteBE plc = new PrecioLicenciaClienteBE();
        FuncionesDA fun = new FuncionesDA();

        #endregion


        #region Metodos


        /// <summary>
        /// metodo que captura los datos desde el crm y los almacena en la clase precio licencia cliente 
        /// </summary>
        /// <returns></returns>
        public  PrecioLicenciaClienteBE CapturarDatoPLC()
        {
          DataTable dt= fun.ObtenerDatosPrecioLicenciaCliente();

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                     
                    plc.Cliente = dr["zth_cliente"].ToString();
                    plc.Producto = dr["zth_producto"].ToString();
                    plc.Fecha =DateTime.Parse( dr["zth_fecha"].ToString());
                    plc.Valor = (Money) dr["zth_valor"];

                }
                ///string ultimo =UltimoValorRegistrado().ToString();
                return plc;
            }
            catch (Exception ex )
            {

               
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al capturar los datos de precios licencia cliente: " + ex.Message.ToString());
                return null;
            }
        }
         


        /// <summary>
        /// Metodo que rescata el ultino valor regitrado
        /// </summary>
        /// <returns>ultimo valor del registro </returns>
        public Money UltimoValor(string idcli, string idpro )
        {

           Money valor=fun.RetornaUltimoValor(idcli, idpro);
            try
            {
                if (valor!=null)
                {
                    return valor;
                }
                else
                {
                    return null;
                }



            }
            catch (Exception ex)
            {
             
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al capturar el ultimo valor : " + ex.Message.ToString());
                return null;
            }
        }


        



        
    
        #endregion


    }
}
