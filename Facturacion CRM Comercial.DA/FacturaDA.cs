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
            
            try
            {

                #region Facturacion Mensual

                if (pre.TipoPago == "Mensual")
                { 
                    DataTable valida=new DataTable();
                     
         
                 DateTime fechaActual = DateTime.Today; ///fecha cactual  
          ///      DateTime fechaActual = new DateTime(2019,06,28);
                    DateTime mes = new DateTime(fechaActual.Year,fechaActual.Month, 1);/// instanciado desde el dia 1 del mes; 
                    DateTime mes2 = new DateTime(fechaActual.Year,fechaActual.Month, 30);//ultimo dia del mes 
                    //Valida si existe la factura
                    valida = fun.FechaCreacion(mes, mes2, pre.GuidCliente );

                    if (valida.Rows.Count > 0)
                    {
                        ZthMetodosVarios.Metodos.GuardarLog(ruta, "La factura ya existe dentro del mes ");
                    }
                    else
                    {
                        

                        //DateTime ultimo_dia_mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)); 
                        //DateTime ultimo_dia = new DateTime();

                        //// si  ultimo dia habil es domingo lo devuelve a viernes
                        //if (ultimo_dia_mes.DayOfWeek == DayOfWeek.Sunday)
                        //{
                        //    ultimo_dia = ultimo_dia_mes.AddDays(-2);
                        //}
                        //else
                        //{

                        //    // si  ultimo dia habil es sabado lo devuelve a viernes
                        //    if (ultimo_dia_mes.DayOfWeek == DayOfWeek.Saturday)
                        //    {
                        //        ultimo_dia = ultimo_dia_mes.AddDays(-1);
                        //    }
                        //}

                        //if ( fechaActual ==ultimo_dia )
                        //{
                            ///cuando la fechaActual se ha igual al ultimodia_dia se va a crear la factura
                            Factura(li, pre);
                        //}
                        //else
                        //{
                        //    ZthMetodosVarios.Metodos.GuardarLog(ruta, "Para crear la factura tiene que ser el ultimo dia habil del mes");
                        //} 
                    }

                }


                #endregion

              



                #region FACTURACION SEMESTRAL

                if (pre.TipoPago == "Semestral")
                {
                    DataTable valida = new DataTable();

                    //DateTime fecha_actual = DateTime.Now;
                    //     DateTime fecha_actual = new DateTime(2019,12,05);
                    DateTime fecha_actual = new DateTime(2019,11,17);
                    //fecha de creacion licencia csp
                    DateTime fecha = fun.ObtenerFechaCreacion(pre.GuidCliente, pre.GuidProducto);                
                    DateTime mes = new DateTime(fecha.Year, fecha.Month, fecha.Day);/// instanciado desde el dia 1 del mes; 
                    DateTime mes2 = new DateTime(fecha.Year, fecha.Month, fecha.Day).AddMonths(6);//ultimo dia del mes


                    //Valida si existe la factura 
                    valida = fun.FechaCreacion(mes, mes2, pre.GuidCliente );
                     
                    if (valida.Rows.Count > 0)
                    {
                        ZthMetodosVarios.Metodos.GuardarLog(ruta, "La factura ya existe dentro del semestre ");
                        DateTime semestre_sgt=new DateTime();
                       
                        // fecha de creacion de la factura(17/11/2019)
                           DateTime fecha_factura = fun.FechaCreacionFactura(mes, mes2, pre.GuidCliente );
                        //17/05/2020
                            semestre_sgt = new DateTime(fecha_factura.Year,fecha_factura.Month, fecha_factura.Day).AddMonths(6);
                         
                        if ( semestre_sgt.Day == fecha_actual.Day  &&  semestre_sgt.Month == fecha_actual.Month)
                            {
                                Factura(li,pre);
                            }
                        
                    }
                    else
                    { 
                        if (mes2.Day == fecha_actual.Day && mes2.Month==fecha_actual.Month )
                        {
                            Factura(li, pre);
                        }
                    } 
            }

                #endregion

                #region FACTURACION Trimestral

                if (pre.TipoPago == "Trimestral")
                {
                    DataTable valida = new DataTable();
                    DateTime fecha_actual = DateTime.Now;
                    //    DateTime fecha_actual = new DateTime(2019,11,17);
                    //fecha de creacion licencia csp
                    DateTime fecha = fun.ObtenerFechaCreacion(pre.GuidCliente, pre.GuidProducto);

                    DateTime mes = new DateTime(fecha.Year, fecha.Month, fecha.Day);/// instanciado desde el dia 1 del mes; 
                    DateTime mes2 = new DateTime(fecha.Year, fecha.Month, fecha.Day).AddMonths(3);//ultimo dia del mes
                    //Valida si existe la factura 
                    valida = fun.FechaCreacion(mes, mes2, pre.GuidCliente);

                    if (valida.Rows.Count > 0)
                    {
                        ZthMetodosVarios.Metodos.GuardarLog(ruta, "La factura ya existe dentro del semestre ");


                        // fecha de creacion de la factura(17/11/2019)
                        DateTime fecha_factura = fun.FechaCreacionFactura(mes, mes2, pre.GuidCliente);
                        //17/05/2020
                        DateTime trimestre_sgt = new DateTime(fecha_factura.Year, fecha_factura.Month, fecha_factura.Day).AddMonths(3);

                        if (trimestre_sgt.Day == fecha_actual.Day && trimestre_sgt.Month == fecha_actual.Month)
                        {
                            Factura(li, pre);
                        }

                    }
                    else
                    {
                        if (mes2.Day == fecha_actual.Day && mes2.Month == fecha_actual.Month)
                        {
                            Factura(li, pre);
                        }
                    }





                }
                #endregion

                #region Facturacion Anual

                if (pre.TipoPago == "Anual")
                {
                    DataTable valida =new DataTable();
            //     DateTime fecha_actual = DateTime.Now;
                    DateTime fecha_actual = new DateTime(2020,05,3);
                    ///fecha de creacion licencia csp       
                    DateTime fecha = fun.ObtenerFechaCreacion(pre.GuidCliente, pre.GuidProducto);
                    //Valida si existe la factura 
                    DateTime mes = new DateTime(fecha.Year, fecha.Month, fecha.Day);/// instanciado desde el dia 1 del mes; 
                    DateTime mes2 = new DateTime(fecha.Year, fecha.Month, fecha.Day).AddYears(1);//ultimo dia del mes
                    valida = fun.FechaCreacion(mes, mes2, pre.GuidCliente );

                    if (valida.Rows.Count > 0)
                    {
                        ZthMetodosVarios.Metodos.GuardarLog(ruta, "La factura ya existe dentro del mes ");
                   

                        // fecha de creacion de la factura(17/11/2019)
                        DateTime fecha_factura = fun.FechaCreacionFactura(mes, mes2, pre.GuidCliente );
                        //17/05/2020
                        DateTime anio_sgt = new DateTime(fecha_factura.Year, fecha_factura.Month, fecha_factura.Day).AddYears(1);

                        if (anio_sgt.Day == fecha_actual.Day && anio_sgt.Month == fecha_actual.Month && anio_sgt.Year !=fecha.Year)
                        {
                            Factura(li, pre);
                        }


                    } 
                }

           
                

                #endregion 
            }
            catch (Exception ex)
            { 
               
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
              //  return null;
            }
        }

        /// <summary>
        /// Metodo que crea la factura 
        /// </summary>
        /// <param name="lic">Entidad licencia csp </param>
        /// <param name="prec">Entidad precio licencia cliente</param>
        public void Factura(LicenciaCspBE lic, PrecioLicenciaClienteBE prec)
        {
            FuncionesDA fun = new FuncionesDA();
            EntidadesCRM.Invoice Factura = new EntidadesCRM.Invoice();
            string creado;
            Guid guid;
            try
            {
                #region  Insercion
      

                //DateTime fechaActual = DateTime.Today; ///fecha cactual  
                //// DateTime fechaActual = new DateTime(2019,06,24) ;    
                //DateTime mes = new DateTime(fechaActual.Year, fechaActual.Month, 1);/// instanciado desde el dia 1 del mes; 
                //DateTime mes2 =new DateTime(fechaActual.Year,fechaActual.Month, 30);//ultimo dia del mes


                ////Valida si existe la factura
                //DataTable valida = fun.FechaCreacion(mes, mes2 ,prec.GuidCliente ,tipo  ); 

                //if (valida.Rows.Count == 0)
                //{//No existe la factura, se crea


                    prec.Pais = fun.ObtenerPais(prec.GuidCliente.ToString());
                    if (prec.Pais == "Chile")
                    {
                        Factura.zth_zenithempresa = new OptionSetValue(sap);
                    }
                    else
                    {
                        if (prec.Pais == "Perú")
                        {
                            Factura.zth_zenithempresa = new OptionSetValue(sac);
                        }
                    }

                // NUMERO DE LA FACTURA
                //Factura.zth_nrofactrurafiscal;
                //Factura.zth_fechaemision;

           //     Factura.CreatedOn.GetValueOrDefault(new DateTime(2019, 6, 4)); 
                
            
                //Factura.Name = "PRUEBA"+li.Cliente;
                    Factura.CustomerId = new EntityReference(EntidadesCRM.Account.EntityLogicalName, new Guid(prec.GuidCliente)); //li.FacturarA
                    Factura.PriceLevelId = new EntityReference(EntidadesCRM.Invoice.EntityLogicalName, new Guid(ListaPrecios)); 
                    Factura.Description = "FACTURA DE PRUEBA  (nuevo campo TipoDeOrigen)" + lic.Cliente + prec.Producto ;
                    //Factura.zth_enviarfacturaa = li.FacturarA;  
                    Factura.zth_TipodeOrigen = new OptionSetValue(tipo);

                    guid = servicio.Create(Factura);

                    //obtine los productos de los clientes por el guid del cliente
                    DataTable productos = fun.ProductosCliente(prec.GuidCliente,prec.GuidProducto);

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
                                    Money valor = fun.RetornaUltimoValor(prec.GuidCliente, dr["zth_producto"].ToString());
                                    pro.PricePerUnit = valor;

                                    DataTable can = fun.ObtenerCantidad(prec.GuidCliente, dr["zth_producto"].ToString());

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
          //      }
                else
                {

                    ZthMetodosVarios.Metodos.GuardarLog(ruta, "La factura ya existe ");
                }

                #endregion
            }
            catch (Exception ex)
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                throw;
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