using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facturacion_CRM_Comercial.BL;
using Facturacion_CRM_Comercial.BE;
using Microsoft.Xrm.Sdk;

namespace Facturacion_CRM_Comercial.UI
{
    public partial class Form1 : Form
    {

        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 11/04/2019
        //Notas: Formulario para la creacion de las facturas
        #endregion

        #region Variables
        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
        LicenciaCspBL li = new LicenciaCspBL(); 
        PrecioLicenciaClienteBL pre = new PrecioLicenciaClienteBL(); 
        PrecioLicenciaClienteBE precios = new PrecioLicenciaClienteBE();
        LicenciaCspBE licencia = new LicenciaCspBE(); 
        FacturaBL fa = new FacturaBL();
        FacturasBE factura = new FacturasBE();
        FuncionesBL fun = new FuncionesBL();
        #endregion


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            CargarDatosFactura();
            lblMensaje1.Text = "PROCESO  TERMINADO"; 
        }


        #region Metodos

        /// <summary>
        /// Metodo donde se cargan los datos en la entidad factura
        /// </summary>
        public void CargarDatosFactura()
        {
        
           
            DataTable ldt = li.CapturDatosLicencia();
            DataTable pdt = pre.CapturaDatosCrm();
            string creado;
            try
            {


                //int contador = 0;
                //foreach (DataRow ldr in ldt.Rows)
                //{
                //    //if (contador ==1)
                //    //{
                //    //    break;

                //    //}
                //    licencia.Cliente = ldr["zth_cliente"].ToString();
                //    licencia.Cantidad = int.Parse(ldr["zth_cantidad"].ToString());
                //    licencia.Producto = ldr["zth_producto"].ToString();
                //    licencia.Guid_cliente = ldr["guidCliente"].ToString();
                //    licencia.Guid_Producto = ldr["guidProducto"].ToString();
                //    licencia.Cantidad = int.Parse(ldr["zth_cantidad"].ToString());
                //    licencia.Pais = ldr["zth_pais"].ToString();
                           //licencia.Cantidad = li.ObtenerCantidad(licencia.Guid_cliente);


                    foreach (DataRow pdr in pdt.Rows)
                    {
                        precios.Cliente = pdr["zth_cliente"].ToString();
                        precios.Producto = pdr["zth_producto"].ToString();
                        precios.Fecha = DateTime.Parse(pdr["zth_fecha"].ToString()); 
                        precios.GuidCliente = pdr["idCliente"].ToString();
                        precios.GuidProducto = pdr["idProducto"].ToString();
                        precios.Pais = "";

                    //  string cliente = "Zenith Consulting SPA"; 
                    //string producto = "Dynamics 365 for Customer Service Enterprise";

                    //string cliente = "73989d7c-2626-e911-a835-000d3a1b730f";
                    //string producto = "d5ff654c-b13e-e911-a83b-000d3a1b730f";


                    //string cliente = "Oil Malal S.A.";
                    //string producto = "Office 365 Business Essentials";

                    string cliente = "ac94d5c7-853e-e911-a843-000d3a4fcfb2";
                    string producto = "ec4bb360-b03e-e911-a83b-000d3a1b730f";

                    if (precios.GuidCliente == cliente && precios.GuidProducto== producto)
                        { 

                            /// VALIDAD SI EXISTE LA FACTURA 
                 
                             fa.factura(licencia, precios);
                             break;
                        }
                        else
                        {
                        creado = null;
                        }
                 
                    }

               
             // }

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Creación exitasa" );
            }
            catch (Exception ex )
                    {
           
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                
            }



#endregion




        }

       

        private void lblMensaje1_Click(object sender, EventArgs e)
        {

        }
    }

        









     
}
        