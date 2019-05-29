using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
 
using Facturacion_CRM_Comercial.BE;
using Microsoft.Xrm.Sdk;


using Newtonsoft.Json;
using System.Dynamic;

using System.Web;
using System.Web.Script.Serialization;
using ZthSeguridad;

namespace Facturacion_CRM_Comercial.DA
{
    public class FacturaDA
    {
        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 11/04/2019
        //Notas: metodos para la clase factura
        #endregion


        #region Variable
        PrecioLicenciaClienteDA p = new PrecioLicenciaClienteDA();
        LicenciaCSPDA lic = new LicenciaCSPDA();
        IOrganizationService servicio = ConexionCRMDA.ObtenerConexion();
        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
        string ListaPrecios = ConfigurationManager.AppSettings["GuidListaPrecios"];
        string IDUnidad = ConfigurationManager.AppSettings["Unidad"]; 
        decimal spa = decimal.Parse(ConfigurationManager.AppSettings["Zenith Consulting SPA"]);
        int limitada=int.Parse(ConfigurationManager.AppSettings["Zenith Consulting Limitada"]);
        int sac=int.Parse(ConfigurationManager.AppSettings["Zenith Consulting S.A.C"]) ;
        int sap=int.Parse(ConfigurationManager.AppSettings["Zenith Consulting SPA"]);
        int tipo =int.Parse (ConfigurationManager.AppSettings["Tipo de Cambio"]);
        string idpro =  Metodos.Desencriptar(ConfigurationManager.AppSettings["Productos"]);
        #endregion

        #region Metodos


        public void CrearFactura(LicenciaCspBE li, PrecioLicenciaClienteBE pre)
        {
          
            FuncionesDA fun = new FuncionesDA(); 
            EntidadesCRM.Invoice Factura = new EntidadesCRM.Invoice(); 
            string creado; 
            Guid guid;
            string tipopago = "Semestral";
            try
            {
              

                if (pre.TipoPago == "Mensual")
                {
                    DataTable valida;
                    DateTime fechaActual = DateTime.Today; ///fecha cactual  
                    // DateTime fechaActual = new DateTime(2019,06,24) ;    
                    DateTime mes = new DateTime(fechaActual.Year,fechaActual.Month, 1);/// instanciado desde el dia 1 del mes; 
                    DateTime mes2 = new DateTime(fechaActual.Year,fechaActual.Month, 31);//ultimo dia del mes


                    //Valida si existe la factura
                     valida = fun.FechaCreacion(mes, mes2, pre.GuidCliente, tipo);
                    if (valida.Rows.Count > 0)
                    {
                        //exite
                    }
                    else
                    {
                        //no existe
                    }

                }else
                {
                    /// el tipo de pago no es mensual 
                }



                if (pre.TipoPago == "Semestral")
                {
                    DataTable valida = new DataTable();
                    //   DateTime fechaActual = DateTime.Today; ///fecha cactual  
                    


                   ///     string f = "2019/6/31";

                    DateTime fechaActual = new DateTime(2019,6,30);

                        if (fechaActual > new DateTime(fechaActual.Year, 7, 1))
                        {
                            DateTime mess = new DateTime(fechaActual.Year, 7, 1);/// instanciado desde el dia 1 del mes; 
                            DateTime mes2 = new DateTime(fechaActual.Year, 12, 31);//ultimo dia del mes


                            //Valida si existe la factura
                            valida = fun.FechaCreacion(mess, mes2, pre.GuidCliente, tipo);
                        }
                        DateTime mes = new DateTime(fechaActual.Year, 6, 31);
                        if (fechaActual <= mes)
                        {
                            DateTime mes1 = new DateTime(fechaActual.Year, 1, 1);/// instanciado desde el dia 1 del mes; 
                            DateTime mes2 = new DateTime(fechaActual.Year, 6, 31);//ultimo dia del mes


                            //Valida si existe la factura
                            valida = fun.FechaCreacion(mes1, mes2, pre.GuidCliente, tipo);
                        }



                  




                    if (valida.Rows.Count > 0)
                    {
                        //exite
                    }
                    else
                    {
                        //no existe
                        /// se crea la factura
                    }

                }


                    #region  Insercion
                    /*

                    DateTime fechaActual = DateTime.Today; ///fecha cactual  
                    // DateTime fechaActual = new DateTime(2019,06,24) ;    
                    DateTime mes = new DateTime(fechaActual.Year, fechaActual.Month, 1);/// instanciado desde el dia 1 del mes; 
                    DateTime mes2 =new DateTime(fechaActual.Year,fechaActual.Month, 31);//ultimo dia del mes


                    //Valida si existe la factura
                    DataTable valida = fun.FechaCreacion(mes, mes2 ,pre.GuidCliente , tipo ); 

                    if (valida.Rows.Count == 0)
                    {//No existe la factura, se crea


                        pre.Pais = fun.ObtenerPais(pre.GuidCliente.ToString());
                        if (pre.Pais == "Chile")
                        {
                            Factura.zth_zenithempresa = new OptionSetValue(sap);
                        }
                        else
                        {
                            if (pre.Pais == "Perú")
                            {
                                Factura.zth_zenithempresa = new OptionSetValue(sac);
                            }
                        }

                        // NUMERO DE LA FACTURA
                        //Factura.zth_nrofactrurafiscal;
                        //Factura.zth_fechaemision;


                        //Factura.Name = "PRUEBA"+li.Cliente;
                        Factura.CustomerId = new EntityReference(EntidadesCRM.Account.EntityLogicalName, new Guid(pre.GuidCliente)); //li.FacturarA
                        Factura.PriceLevelId = new EntityReference(EntidadesCRM.Invoice.EntityLogicalName, new Guid(ListaPrecios)); 
                        Factura.Description = "FACTURA DE PRUEBA  (nuevo campo TipoDeOrigen)" + li.Cliente + pre.Producto ;
                        //Factura.zth_enviarfacturaa = li.FacturarA;  
                        Factura.zth_TipodeOrigen = new OptionSetValue(tipo);

                        guid = servicio.Create(Factura);

                        //obtine los productos de los clientes por el guid del cliente
                        DataTable productos = fun.ProductosCliente(pre.GuidCliente,pre.GuidProducto);

                        if (productos.Rows.Count > 0)
                        {
                            EntidadesCRM.InvoiceDetail pro = new EntidadesCRM.InvoiceDetail();
                            foreach (DataRow dr in productos.Rows)
                            {
                                // NO EXISTE
                                pro.InvoiceId = new EntityReference(EntidadesCRM.Invoice.EntityLogicalName, guid);
                                pro.IsProductOverridden = false;
                                //if (pro.IsProductOverridden==false)
                                //{
                                //producto existen
                                //}else
                                //{
                                // producto fuera de catalogo
                                //}
                                pro.ProductId = new EntityReference(EntidadesCRM.Invoice.EntityLogicalName, new Guid(dr["zth_producto"].ToString()));

                                if (idpro == pro.ProductId.Id.ToString())
                                {

                                    ZthMetodosVarios.Metodos.GuardarLog(ruta, "No se crea el detalle de la factura si el producto no contiene unidad principal");
                                    continue;
                                }
                                else
                                {
                                    string id = fun.IdProductos(pro.InvoiceId.Id.ToString(), pro.ProductId.Id.ToString());
                                    if (id == null)
                                    {
                                        //se crea
                                        string unidad = fun.ObtenerUnidad(IDUnidad);
                                        pro.UoMId = new EntityReference(EntidadesCRM.UoM.EntityLogicalName, new Guid(unidad));
                                        pro.IsPriceOverridden = true;
                                        //se obtiene el ultimo valor registrado
                                        Money valor = fun.RetornaUltimoValor(pre.GuidCliente, dr["zth_producto"].ToString());
                                        pro.PricePerUnit = valor;

                                        DataTable can = fun.ObtenerCantidad(pre.GuidCliente, dr["zth_producto"].ToString());

                                        int c = 0;
                                        foreach (DataRow ro in can.Rows)
                                        {
                                            c += Convert.ToInt32(ro["zth_cantidad"]);

                                        }
                                        pro.Quantity = c;
                                        if (pro.Quantity == 0)
                                        {
                                            //No se inserta el producto 
                                            continue;
                                        }

                                        pro.zth_tipocambio = Dolar();
                                        //Se crea el producto en el detalle de la factura 
                                        creado = servicio.Create(pro).ToString();

                                    }
                                    else
                                    {

                                        continue;

                                    }

                                } 
                            } 
                        }
                    }
                    else
                    {

                        ZthMetodosVarios.Metodos.GuardarLog(ruta, "La factura ya existe ");
                    } 
                    */
                    #endregion
                }
            catch (Exception ex)
            { 
               
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
              //  return null;
            }
        }





        /// <summary>
        /// Metodo que consume una API Indicadores financieros para obtener el valor del dolar actual
        /// </summary>
        /// <returns>Retorna el valor del dolar </returns>
        public int Dolar()
        {
            try
            {
                string url = " https://api.desarrolladores.datos.gob.cl/indicadores-financieros/v1/dolar/hoy.json/?auth_key=2943d93a3b9001139056e0f106ddda5cc4a5bdbc";
                string jason = new WebClient().DownloadString(url);

                JavaScriptSerializer js = new JavaScriptSerializer();
                var d = js.Deserialize<dynamic>(jason);
                var dolares = d["dolar"];
                var valores = dolares["valor"];
                int Dolar = (int)valores;
                int ValorDolar = Dolar + 5;
                return ValorDolar;
            }
            catch (Exception ex )
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                throw;
            }
        }


        /// <summary>
        /// Metodo que consume una API Indicadores financieros para obtener el valor del uf actual
        /// </summary>
        /// <returns>Retorna el valor del uf </returns>
        public void UF()
        {
            try
            {
                string url = " https://api.desarrolladores.datos.gob.cl/indicadores-financieros/v1/uf/hoy.json/?auth_key=2943d93a3b9001139056e0f106ddda5cc4a5bdbc ";
                string jason = new WebClient().DownloadString(url);
                JavaScriptSerializer js = new JavaScriptSerializer();
                var d = js.Deserialize<dynamic>(jason);
                var dolares = d["uf"];
                var valores = dolares["valor"];
                int Dolar = (int)valores;
                //int ValorDolar = Dolar + 5;
            }
            catch (Exception ex)
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                throw;
            }

          
         
             
        }


        #endregion
    }
    }