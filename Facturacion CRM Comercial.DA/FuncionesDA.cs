﻿using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Facturacion_CRM_Comercial.BE;
using ZthFetchXml365;

namespace Facturacion_CRM_Comercial.DA
{
    public  class FuncionesDA
    {

        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 11/04/2019
        //Notas: Clase para las Funciones Varias
        #endregion

        #region "Variables"

      
        

         
        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
        IOrganizationService servicio = ConexionCRMDA.ObtenerConexion();
        LicenciaCspBE licencia = new LicenciaCspBE();

        #endregion


        #region "Metodos" 

        //capturar datos de la entidad Licencia csp ,precio licencia ,factura ,producto de la factura 

        /// <summary>
        /// Se obtiene los datos  la licencia csp  
        /// </summary>
        /// <param name=" "></param>
        /// <returns>Retorna la entidad la licencia csp </returns>
        public DataTable ObtenerDatosLicenciaCSP()
        {        
            try
            {
                
                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("zth_licenciascsp", ref servicio);
                //se capturan los datos de la entidad licencia csp
                fetch.AgregarCampoRetorno("zth_licenciascsp", "zth_pais", ZthFetchXml365.zthFetch.TipoRetorno.PicklistName);
                fetch.AgregarCampoRetorno("zth_licenciascsp", "zth_cliente", ZthFetchXml365.zthFetch.TipoRetorno.LookupName);
                fetch.AgregarCampoRetorno("zth_licenciascsp", "zth_cliente", ZthFetchXml365.zthFetch.TipoRetorno.LookupValue,"guidCliente");
             //   fetch.AgregarCampoRetorno("zth_licenciascsp", "zth_facturara", ZthFetchXml365.zthFetch.TipoRetorno.LookupName);
                fetch.AgregarCampoRetorno("zth_licenciascsp", "zth_cantidad", ZthFetchXml365.zthFetch.TipoRetorno.CrmNumber);
                fetch.AgregarCampoRetorno("zth_licenciascsp", "zth_producto", ZthFetchXml365.zthFetch.TipoRetorno.LookupName);
                fetch.AgregarCampoRetorno("zth_licenciascsp", "zth_producto", ZthFetchXml365.zthFetch.TipoRetorno.LookupValue,"guidProducto");
              
               
                //se agrupa los clientes y los productos
               
                fetch.AgregarOrdenResultadoEntidad("zth_licenciascsp", "zth_cliente",
               ZthFetchXml365.zthFetch.TipoOrdenCampoResultadoEntidad.Descendiente);

                fetch.AgregarOrdenResultadoEntidad("zth_licenciascsp", "zth_producto",
               ZthFetchXml365.zthFetch.TipoOrdenCampoResultadoEntidad.Descendiente);
                

                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);
 

                if (Dato.Rows.Count > 0)    
                {
                    return Dato;
                }
                else
                {
                    return Dato = null;
                }
            }
            catch (Exception ex)
            {
               
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al obtener los datos de licecia csp: " + ex.Message.ToString());
                return null;
            }

            

        }
        /// <summary>
        ///  se obtiene los datos de la entidad Precio Licencia Cliente 
        /// </summary>
        /// <returns>Retorna la entidad Precio Licencia Cliente</returns>
        public DataTable ObtenerDatosPrecioLicenciaCliente()
        {
            try
            {

                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("zth_preciolicenciacliente", ref servicio);
                //se capturan los datos de la entidad licencia csp
                fetch.AgregarCampoRetorno("zth_preciolicenciacliente", "zth_cliente", ZthFetchXml365.zthFetch.TipoRetorno.LookupName);
                fetch.AgregarCampoRetorno("zth_preciolicenciacliente", "zth_cliente", ZthFetchXml365.zthFetch.TipoRetorno.LookupValue,"idCliente");
                fetch.AgregarCampoRetorno("zth_preciolicenciacliente", "zth_fecha", ZthFetchXml365.zthFetch.TipoRetorno.CrmDateTime);
                fetch.AgregarCampoRetorno("zth_preciolicenciacliente", "zth_valor", ZthFetchXml365.zthFetch.TipoRetorno.CrmMoney);
                fetch.AgregarCampoRetorno("zth_preciolicenciacliente", "zth_producto", ZthFetchXml365.zthFetch.TipoRetorno.LookupName);
                fetch.AgregarCampoRetorno("zth_preciolicenciacliente", "zth_producto", ZthFetchXml365.zthFetch.TipoRetorno.LookupValue,"idProducto");


                fetch.AgregarFiltroPlano("zth_preciolicenciacliente", ZthFetchXml365.zthFetch.TipoFiltro.and, "createdon",
                     ZthFetchXml365.zthFetch.TipoComparacionFiltro.FechaMayorIgualQue, "04/01/2019");

                 

                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);

                if (Dato.Rows.Count > 0)
                {

                    //return Dato=Dato.Rows[0]["zth_fecha"].ToString();
                    return Dato;
                }
                else
                {
                    return Dato = null;

                }
            }
            catch (Exception ex)
            {

                 
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al obtener los datos de precio licencia cliente: " + ex.Message.ToString());
                return null;
            }



        }
         
        /// <summary>
        /// Se obtiena el valor de Precio Licencia Clinete  
        /// </summary>
        /// <param name=""></param>
        /// <returns>Rtornar el valor de Precio Licencia Cliente </returns> 
        public Money  RetornaUltimoValor( string idcli, string idpro)
        { 
            try
            {
                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("zth_preciolicenciacliente", ref servicio);
                //se capturan los datos de la entidad licencia csp                
                fetch.AgregarCampoRetorno("zth_preciolicenciacliente", "zth_fecha", ZthFetchXml365.zthFetch.TipoRetorno.CrmDateTime);
                fetch.AgregarCampoRetorno("zth_preciolicenciacliente", "zth_valor", ZthFetchXml365.zthFetch.TipoRetorno.CrmMoney);
                
                //se ordena la columna fecha de manera descendente 
                fetch.AgregarOrdenResultadoEntidad("zth_preciolicenciacliente", "zth_fecha",
                   ZthFetchXml365.zthFetch.TipoOrdenCampoResultadoEntidad.Descendiente);

                fetch.AgregarFiltroPlano("zth_preciolicenciacliente", ZthFetchXml365.zthFetch.TipoFiltro.and, "zth_cliente",
                ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, idcli);

                fetch.AgregarFiltroPlano("zth_preciolicenciacliente", ZthFetchXml365.zthFetch.TipoFiltro.and, "zth_producto",
                 ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, idpro);

                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);
                Money valor=null; 
                if (Dato.Rows.Count > 0)
                { 
                      return  valor =(Money)Dato.Rows[0]["zth_valor"]; 
                }
                else
                {
                    return valor = null; 
                }
            }
            catch (Exception ex)
            {

           
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al retornar el valor : " + ex.Message.ToString());
                return null;
            }
             
        }
         

        /// <summary>
        /// Metodo que obtiene el id de la unidad
        /// </summary>
        /// <param name="guid">se pasa el id de la unidad para comprar</param>
        /// <returns>se retorna el id de la unidad</returns>
        public string ObtenerUnidad(string guid)
        {
            try
            {
                string gid="";
                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("uomschedule", ref servicio);
                fetch.AgregarEntidadLinkJoin("uomschedule", "uom", (zthFetch.TipoRelacionEntidadLink)1, "uomscheduleid", "uomscheduleid");
            
                //se capturan los datos de la entidad licencia csp
                fetch.AgregarCampoRetorno("uom", "uomid", ZthFetchXml365.zthFetch.TipoRetorno.Key);

                 
                fetch.AgregarFiltroPlano("uomschedule", ZthFetchXml365.zthFetch.TipoFiltro.and, "uomscheduleid",
                     ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, guid);
                 

                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);

                if (Dato.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in Dato.Rows)
                    {

                        gid = dataRow["uomid"].ToString();


                    }
         
                    return gid;
                }
                else
                {
                    return gid = null;

                }
            }
            catch (Exception ex)
            {

                 
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al obtener la unidad: " + ex.Message.ToString());
                return null;
            }



        }

         
        /// <summary>
        /// Metododo que obtiene la canitdad de los productos  por el id del cliente
        /// </summary>
        /// <param name="id"> di del cliente</param>
        /// <returns>retorna la cantidad del producto del cliente</returns>
       public int ObtenerCantidad(string id,string idpro)
        {
            try
            {
                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("zth_licenciascsp", ref servicio); 

                fetch.AgregarCampoRetorno("zth_licenciascsp", "zth_cantidad", ZthFetchXml365.zthFetch.TipoRetorno.String);
                 
                fetch.AgregarFiltroPlano("zth_licenciascsp", ZthFetchXml365.zthFetch.TipoFiltro.and, "zth_cliente",
                    ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual,id);
                fetch.AgregarFiltroPlano("zth_licenciascsp", ZthFetchXml365.zthFetch.TipoFiltro.and, "zth_producto",
                   ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, idpro);

                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);

                int cantidad;
                if (Dato.Rows.Count > 0)
                {
                    return cantidad=int.Parse( Dato.Rows[0]["zth_cantidad"].ToString());
                }
                else
                {
                    return cantidad = 0;
                }
            }
            catch (Exception ex)
            { 
                
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                return 0;
            }
        }


        /// <summary>
        /// Metodo que obtiene el pais del cliente p
        /// </summary>
        /// <param name="idcli"> id del cliente</param>
        /// <returns></returns>
        public string ObtenerPais(string idcli )
        {
            try
            {

                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("account", ref servicio); 
                fetch.AgregarCampoRetorno("account", "zth_pais", ZthFetchXml365.zthFetch.TipoRetorno.PicklistName);
               
                fetch.AgregarFiltroPlano("account", ZthFetchXml365.zthFetch.TipoFiltro.and, "accountid",
                ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, idcli);
 

                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);

                string empresa;
                if (Dato.Rows.Count > 0)
                {
                    return empresa= Dato.Rows[0]["zth_pais"].ToString();
                }
                else
                {
                    return empresa = null;
                }




            }

            catch (Exception ex)
            {

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al obtener los datos de licecia csp: " + ex.Message.ToString());
                return null;
            }
        }



        /// <summary>
        /// Metodo que obtiene las factura en orden descendente
        /// </summary>
        /// <returns>retorna las factura por el id</returns>
        public DataTable ObtenerDatosFactura (string cli,string pro)
        { 
            try
            {

                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("invoice", ref servicio);
                //fetch.AgregarEntidadLinkJoin("invoice", "invoicedetail", (zthFetch.TipoRelacionEntidadLink)1, "invoiceid", "invoicedetailid");

                fetch.AgregarCampoRetorno("invoice", "invoiceid", ZthFetchXml365.zthFetch.TipoRetorno.Key);

                fetch.AgregarCampoRetorno("invoice", "customerid", ZthFetchXml365.zthFetch.TipoRetorno.LookupName);
                fetch.AgregarCampoRetorno("invoicedetail", "invoicedetail", ZthFetchXml365.zthFetch.TipoRetorno.LookupName);
                //fetch.AgregarOrdenResultadoEntidad("invoice", "invoiceid",
                // ZthFetchXml365.zthFetch.TipoOrdenCampoResultadoEntidad.Descendiente);

                //fetch.AgregarFiltroPlano("invoice", ZthFetchXml365.zthFetch.TipoFiltro.and, "customerid",
                //   ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, cli);

                //fetch.AgregarFiltroPlano("invoicedetail", ZthFetchXml365.zthFetch.TipoFiltro.and, "invoicedetail",
                //  ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, pro);


                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);


                if (Dato.Rows.Count > 0)
                {
                    return Dato;
                }
                else
                {
                    return Dato = null;
                }
            }
            catch (Exception ex)
            {

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al obtener los datos de licecia csp: " + ex.Message.ToString());
                return null;
            }
        }



        /// <summary>
        /// Metodo que valida si existe la factura por id de la factura 
        /// </summary>
        /// <param name="idf">id de la factur a</param>
        /// <returns>retorna null si no existe la factura y si existe retorna el id de la factura</returns>
        public string ValidaFactura(DataTable idf)
            {

            try
            {
                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("invoice", ref servicio); 
                fetch.AgregarCampoRetorno("invoice", "customerid", ZthFetchXml365.zthFetch.TipoRetorno.LookupName);

                fetch.AgregarFiltroPlano("invoice", ZthFetchXml365.zthFetch.TipoFiltro.and, "invoiceid",
               ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual,  idf.ToString());





                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);

                string id;
                if (Dato.Rows.Count > 0)
                {
                    return id = Dato.Rows[0]["customerid"].ToString();
                }
                else
                {
                    return id = null;
                } 
            }
            catch (Exception ex)
            {

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al obtener los datos de licecia csp: " + ex.Message.ToString());
                return null;
            }

        }


        /// <summary>
        /// Captura losproductos de los clienetes 
        /// </summary>
        /// <param name="guid">parametro guid del cliente registrado</param>
        /// <returns>retorna los productos de los clientes</returns>
        public DataTable ProductosCliente(string guid)
        {
             
            try
            {

                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("zth_preciolicenciacliente", ref servicio);

              
                fetch.AgregarCampoRetorno("zth_preciolicenciacliente", "zth_producto", ZthFetchXml365.zthFetch.TipoRetorno.LookupValue);
 
                fetch.AgregarFiltroPlano("zth_preciolicenciacliente", ZthFetchXml365.zthFetch.TipoFiltro.and, "zth_cliente",
                ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, guid.ToString());




                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);
               
                if (Dato.Rows.Count > 0)
                {
                    return  Dato  ;
                }
                else
                {
                    return Dato = null;
                }
            }
            catch (Exception ex)
            {

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al obtener los datos de licecia csp: " + ex.Message.ToString());
                return null;
            }
        }


        

        public string IdProductos(string idcli ,string idpro )
        {
            try
            {
                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("invoicedetail", ref servicio);
                
 
                fetch.AgregarCampoRetorno("invoicedetail", "productid", ZthFetchXml365.zthFetch.TipoRetorno.LookupName);
                //se ordena la columna fecha de manera descendente 
                //fetch.AgregarOrdenResultadoEntidad("zth_preciolicenciacliente", "zth_fecha",
                //   ZthFetchXml365.zthFetch.TipoOrdenCampoResultadoEntidad.Descendiente);

                fetch.AgregarFiltroPlano("invoicedetail", ZthFetchXml365.zthFetch.TipoFiltro.and, "invoiceid",
                ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, idcli  );
                fetch.AgregarFiltroPlano("invoicedetail", ZthFetchXml365.zthFetch.TipoFiltro.and, "productid",
               ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, idpro);


                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);
                string id;
                if (Dato.Rows.Count > 0)
                {
                    return id=  Dato.Rows[0]["productid"].ToString();
                }
                else
                {
                    return id = null;
                }
            }
            catch (Exception ex)
            {


                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al retornar el valor : " + ex.Message.ToString());
                return null;
            }

        }

        #endregion





    }
}