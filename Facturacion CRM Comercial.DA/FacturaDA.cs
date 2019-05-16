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
        #endregion

        #region Metodos


        public void CrearFactura(LicenciaCspBE li, PrecioLicenciaClienteBE pre)
        {

            FuncionesDA fun = new FuncionesDA();
            EntidadesCRM.Invoice Factura = new EntidadesCRM.Invoice();
            Guid guid;
            string creado;


            try
            {
                DateTime fechaActual = DateTime.Today; ///fecha cactual  
                DateTime mes = new DateTime(fechaActual.Year, fechaActual.Month, 1);/// instanciado desde el dia 1 del mes; 
                DateTime mes2 =new DateTime(fechaActual.Year,fechaActual.Month, 30);//ultimo dia del mes
                
                ///Facturas creadas dentro del mes 
               DataTable fa = fun.Facturas(mes,mes2,pre.GuidCliente);
                if (fa.Rows.Count>0) {
                    foreach (DataRow dr in fa.Rows)
                    {
                        //string idFactura = fun.IdFactura(dr["invoiceid"].ToString());
                        //DateTime valida = fun.FechaCreaciónFactura(pre.GuidCliente, idFactura);
                        string c = dr["customerid"].ToString();
                        if (c==pre.Cliente)
                        {
                            ///facturas creadas dentro del mes 
                        }
                        else
                        {
                            //facturas creadas fuera del mes
                        }
                    }

                }


                            //Valida si existe la factura  por la feche de creacion             
                //if (valida > mes && valida < mes2)
                //{
                //    /// LA FACTURA YA EXISTE!!!
                //    ZthMetodosVarios.Metodos.GuardarLog(ruta, "La factura ya existe ");
                //}
                //else
                //{

                    //SE CREA LA FACTURA
                    //pre.Pais = fun.ObtenerPais(pre.GuidCliente.ToString());
                    //if (pre.Pais == "Chile")
                    //{ 
                    //    Factura.zth_zenithempresa = new OptionSetValue(sap);
                    //}
                    //else
                    //{
                    //    if (pre.Pais == "Perú")
                    //    {
                    //        Factura.zth_zenithempresa = new OptionSetValue(sac);
                    //    }
                    //}

                    // NUMERO DE LA FACTURA
                    //Factura.zth_nrofactrurafiscal;
                    //Factura.zth_fechaemision;


                    //Factura.Name = "PRUEBA"+li.Cliente;
                    Factura.CustomerId = new EntityReference(EntidadesCRM.Account.EntityLogicalName, new Guid(pre.GuidCliente));
                    Factura.PriceLevelId = new EntityReference(EntidadesCRM.Invoice.EntityLogicalName, new Guid(ListaPrecios));
                    Factura.Description = "FACTURA DE PRUEBA :" + li.Cliente + pre.Producto;
                    guid = servicio.Create(Factura);

                    //obtine los productos de los clientes por el guid del cliente
                    DataTable productos = fun.ProductosCliente(pre.GuidCliente);


                    if (productos.Rows.Count > 0)
                    {
                        EntidadesCRM.InvoiceDetail pro = new EntidadesCRM.InvoiceDetail();
                        foreach (DataRow dr in productos.Rows)
                        {
                            // NO EXISTE
                            pro.InvoiceId = new EntityReference(EntidadesCRM.Invoice.EntityLogicalName, guid);
                            pro.ProductId = new EntityReference(EntidadesCRM.Invoice.EntityLogicalName, new Guid(dr["zth_producto"].ToString()));
                            string id = fun.IdProductos(pro.InvoiceId.Id.ToString(), pro.ProductId.Id.ToString());

                            if (id == null)
                            {
                                //se crea
                                string unidad = fun.ObtenerUnidad(IDUnidad);
                                pro.UoMId = new EntityReference(EntidadesCRM.UoM.EntityLogicalName, new Guid(unidad));
                                pro.Quantity = fun.ObtenerCantidad(pre.GuidCliente, dr["zth_producto"].ToString());
                                pro.IsPriceOverridden = true;
                                //se obtiene el ultimo valor registrado
                                Money valor = fun.RetornaUltimoValor(pre.GuidCliente, dr["zth_producto"].ToString());
                                pro.PricePerUnit = valor;
                                pro.zth_tipocambio = 678;
                                //Se crea el detalle de la factura 
                                creado = servicio.Create(pro).ToString();
                            }
                            else
                            {
                                break;
                            }

                        }

                    }
                // }
 
                }

                
             
             
            catch (Exception ex)
            { 
               
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
              //  return null;
            }
        }


        #endregion
    }
}
