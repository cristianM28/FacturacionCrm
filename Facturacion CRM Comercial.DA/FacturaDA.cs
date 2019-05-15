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
        IOrganizationService servicio = ConexionCRMDA.ObtenerConexion();
        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
        // Guid Divisa = ConfigurationManager.AppSettings["GuidDivisa"];
        string ListaPrecios = ConfigurationManager.AppSettings["GuidListaPrecios"];
        string IDUnidad = ConfigurationManager.AppSettings["Unidad"];
        PrecioLicenciaClienteDA p = new PrecioLicenciaClienteDA();
        LicenciaCSPDA lic = new LicenciaCSPDA();
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
                //string cliente = "Oil Malal S.A.";
                //string cliente = "ac94d5c7-853e-e911-a843-000d3a4fcfb2";
           //     string producto = "Office 365 Business Essentials";
           //string producto= "ec4bb360-b03e-e911-a83b-000d3a1b730f";
                //validación por si existe la factura ,por el numero de la factura  
                DataTable valida = fun.ObtenerDatosFactura(pre.GuidCliente,pre.GuidProducto) ;
                string existe =fun.ValidaFactura(valida);

                //si no existe la factura 
                if (existe==null)
                {
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
                   DataTable productos= fun.ProductosCliente(pre.GuidCliente);
              //  string id = fun.IdProductos(pre.GuidCliente);

                    if (productos.Rows.Count > 0)
                    {
                        EntidadesCRM.InvoiceDetail pro = new EntidadesCRM.InvoiceDetail();
                        foreach (DataRow dr in productos.Rows)
                        {
                           // Guid gid = new Guid(id);
                            
                                // NO EXISTE
                                pro.InvoiceId = new EntityReference(EntidadesCRM.Invoice.EntityLogicalName, guid); 
 
                                pro.ProductId = new EntityReference(EntidadesCRM.Invoice.EntityLogicalName, new Guid(dr["zth_producto"].ToString()));
                                string id = fun.IdProductos(pro.InvoiceId.Id.ToString(),pro.ProductId.Id.ToString());
                            if (id==null)
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

                                creado = servicio.Create(pro).ToString();
                            }
                            else
                            {
                                break;
                            }

                              
                            
                            






                        }
                       
                            //SE INSERTAN LOS PRODUCTOS EN LA ENTIDAD PRODUCTOS DE LA FACTURa
                           // EntidadesCRM.InvoiceDetail pro = new EntidadesCRM.InvoiceDetail();
                            //pro.InvoiceId = new EntityReference(EntidadesCRM.Invoice.EntityLogicalName, guid); 
                            //pro.ProductId = new EntityReference(EntidadesCRM.Product.EntityLogicalName, new Guid(pre.GuidProducto));
                            //string unidad = fun.ObtenerUnidad(IDUnidad);
                            //pro.UoMId = new EntityReference(EntidadesCRM.UoM.EntityLogicalName, new Guid(unidad));
                            ////pro.Quantity = fun.ObtenerCantidad(pre.GuidCliente);
                            //pro.Quantity = 1;
                            //pro.IsPriceOverridden = true;
                            ////se obtiene el ultimo valor registrado
                            //Money valor = fun.RetornaUltimoValor(pre.GuidCliente,pre.GuidProducto);
                            //pro.PricePerUnit = valor;
                            //pro.zth_tipocambio = 678;

                            //se crea el detalle de la factura
                            //creado = servicio.Create(pro).ToString();
                
                       
                        
                    } 
                }
                else
                {
                    /// LA FACTURA YA EXISTE!!!
                } 
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
