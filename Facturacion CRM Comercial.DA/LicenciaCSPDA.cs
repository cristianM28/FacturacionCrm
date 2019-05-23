using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion_CRM_Comercial.BE;

namespace Facturacion_CRM_Comercial.DA
{
   public class LicenciaCSPDA
    {


        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 11/04/2019
        //Notas: metodos para la clase licencia csp
        #endregion

        #region variables
        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
        FuncionesDA fun = new FuncionesDA();
        LicenciaCspBE licencia = new LicenciaCspBE();
        EntidadesCRM.zth_licenciascsp li = new EntidadesCRM.zth_licenciascsp();

        #endregion





        #region metodos



        /// <summary>
        /// se capturan los datos desde el  crm y se almacenan en la clase licencias csp
        /// </summary>
      public LicenciaCspBE CapturaLicenciaCsp()
        {
           DataTable dt = fun.ObtenerDatosLicenciaCSP();

            
            try
            {

                foreach (DataRow dr in dt.Rows)
                {
                    licencia.Cliente = dr["zth_cliente"].ToString();
                    licencia.FacturarA = dr["zth_facturara"].ToString();
                    licencia.Producto = dr["zth_producto"].ToString();
                    //licencia.Cantidad = int.Parse(dr["zth_cantidad"].ToString());
                     
                }
          
                return licencia;

            }
            catch (Exception ex)
            {

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al captura los datos de LicenciaCspDA : " + ex.Message.ToString());
                return null;
            }
        }



        /// <summary>
        /// Metodo que suma la cantidad de productos
        /// </summary>
        /// <returns>Retorna la suma de la cantidad</returns>
    /*    public int CantidadProductos(string id ,string idpro)
        {
             
            int totalProducto = 0; 
            try
            {
                licencia.Cantidad = fun.ObtenerCantidad(id,idpro);
                if (licencia.Cantidad!=0)
                {
                    totalProducto = totalProducto + licencia.Cantidad;
                }

                return totalProducto;
            }
            catch (Exception ex )
            {

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error obtener la cantidad : " + ex.Message.ToString());
                return 0;
            }
        }
      */


        #endregion 

     

    }
}
