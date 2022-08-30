using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagicSQL;
using REApp.Models;

namespace REApp.Forms
{
    public partial class GestionVants : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();

            if (IsPostBack)
            {
                cargarGvVants();
            }
            if (!IsPostBack)
            {
                //Si tiene rol Admin o Operador
                if (idRolInt == 1 || idRolInt == 2)
                {
                    CargarComboSolicitante();
                    cargarGvVants();
                }
<<<<<<< Updated upstream
                //Si tiene rol Solicitante
                if (idRolInt == 3)
=======
                if(idRolInt == 3)
                //Si tiene rol Solicitante
>>>>>>> Stashed changes
                {
                    CargarComboSolicitante();
                    ddlSolicitante.SelectedValue = id.ToCryptoID().ToString();
                    ddlSolicitante.Enabled = false;
                    cargarGvVants();
                }
            }
        }

        protected void cargarGvVants()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_VantConsultar", P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID())); ;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                gvVants.DataSource = dt;
            }
            else
            {
                gvVants.DataSource = null;
            }
            gvVants.DataBind();
            

        }

        //CARGA DE COMBOS
        protected void CargarComboMarcaVant()
        {
            ddlMarcaVant.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboMarcaVant();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlMarcaVant.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdMarcaVant"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void CargarComboClaseVant()
        {
            ddlClaseVant.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboTipoVant();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlClaseVant.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdTipoVant"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void CargarComboSolicitante()
        {
            ddlSolicitante.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboSolicitante();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlSolicitante.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdUsuario"].ToString().ToInt().ToCryptoID()));
                }
            }
        }
        protected void CargarComboModalSolicitante()
        {
            ddlModalSolicitante.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboSolicitante();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlModalSolicitante.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdUsuario"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void CargarComboProvincia()
        {
            ddlProvincia.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboProvincias();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlProvincia.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdProvincia"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void CargarComboLocalidadPartido(int IdProvincia)
        {
            ddlLocalidadPartido.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboLocalidadPartido(IdProvincia);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlLocalidadPartido.Items.Add(new ListItem(dt.Rows[i]["NombreLocalidad"].ToString(), dt.Rows[i]["IdLocalidad"].ToString().ToInt().ToCryptoID()));
                }
            }
        }


        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idProvincia = ddlProvincia.SelectedItem.Value.ToIntID();
            CargarComboLocalidadPartido(idProvincia);
        }

        //BOTONES NUEVO; MODIFICAR Y ELIMINAR

        protected void btnNuevoVant_Click(object sender, EventArgs e)
        {
            hdnIdVant.Value = "";
            LimpiarModal();
            CargarComboMarcaVant();
            CargarComboModalSolicitante();
            CargarComboClaseVant();
            CargarComboProvincia();
            int idProvincia = ddlProvincia.SelectedItem.Value.ToIntID();
            CargarComboLocalidadPartido(idProvincia);
            ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
            ddlModalSolicitante.Enabled = false;
            MostrarABM();

        }

        protected void btnModificarVant_Click(object sender, EventArgs e)
        {

            int id = int.Parse((sender as LinkButton).CommandArgument);
            Models.Vant Vant = new Models.Vant().Select(id);

            int idLocalidad = Vant.IdLocalidadPartido;
            Models.Localidad Localidad = new Models.Localidad().Select(idLocalidad);



            hdnIdVant.Value = id.ToString();
            LimpiarModal();
            CargarComboMarcaVant();
            CargarComboClaseVant();
            CargarComboModalSolicitante();
            CargarComboProvincia();
            CargarComboLocalidadPartido(Localidad.IdProvincia);

            ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
            ddlModalSolicitante.Enabled = false;


            txtFabricante.Text = Vant.Fabricante;
            txtAñoFabricacion.Text = Vant.AñoFabricacion.ToString();
            txtModelo.Text = Vant.Modelo;
            txtLugarFabricacion.Text = Vant.LugarFabricacion;
            txtLugarGuardado.Text = Vant.LugarGuardado;
            txtNumeroSerie.Text = Vant.NumeroSerie;

            
            ddlProvincia.SelectedValue = Localidad.IdProvincia.ToCryptoID().ToString(); 
            ddlLocalidadPartido.SelectedValue = Localidad.IdLocalidad.ToCryptoID().ToString();



            MostrarABM();

            if(Vant.MotivoBaja != null)
            {
                txtFabricante.Enabled = false;
                txtAñoFabricacion.Enabled = false;
                txtModelo.Enabled = false;
                txtLugarFabricacion.Enabled = false;
                txtLugarGuardado.Enabled = false;
                txtNumeroSerie.Enabled = false;
                ddlLocalidadPartido.Enabled = false;
                ddlProvincia.Enabled = false;
                ddlMarcaVant.Enabled = false;
                ddlClaseVant.Enabled = false;

                pnlMotivoBaja.Visible = true;
                txtMotivoBaja.Text = Vant.MotivoBaja;

                btnGuardar.Visible = false;
            }


        }

        protected void btnEliminarVant_Click(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);

            Models.Vant Vant = new Models.Vant().Select(id);



            if (Vant.FHBaja == null)
            {
                int idLocalidad = Vant.IdLocalidadPartido;
                Models.Localidad Localidad = new Models.Localidad().Select(idLocalidad);

                hdnIdVant.Value = id.ToString();
                LimpiarModal();
                CargarComboMarcaVant();
                CargarComboClaseVant();
                CargarComboModalSolicitante();
                CargarComboProvincia();
                CargarComboLocalidadPartido(Localidad.IdProvincia);

                ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
                ddlModalSolicitante.Enabled = false;


                txtFabricante.Text = Vant.Fabricante;
                txtAñoFabricacion.Text = Vant.AñoFabricacion.ToString();
                txtModelo.Text = Vant.Modelo;
                txtLugarFabricacion.Text = Vant.LugarFabricacion;
                txtLugarGuardado.Text = Vant.LugarGuardado;
                txtNumeroSerie.Text = Vant.NumeroSerie;
                ddlProvincia.SelectedValue = Localidad.IdProvincia.ToCryptoID().ToString();
                ddlLocalidadPartido.SelectedValue = Localidad.IdLocalidad.ToCryptoID().ToString();


                txtFabricante.Enabled = false;
                txtAñoFabricacion.Enabled = false;
                txtModelo.Enabled = false;
                txtLugarFabricacion.Enabled = false;
                txtLugarGuardado.Enabled = false;
                txtNumeroSerie.Enabled = false;
                ddlLocalidadPartido.Enabled = false;
                ddlProvincia.Enabled = false;
                ddlMarcaVant.Enabled = false;
                ddlClaseVant.Enabled = false;


                MostrarABM();
                pnlMotivoBaja.Visible = true;
                txtMotivoBaja.Enabled = true;

            }
            else
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Vant ya dado de baja');", true);
            }

        }



        //protected DropDownList GetDdlLocalidadPartido()
        //{
        //    return ddlLocalidadPartido;
        //}


        //BOTON GUARDAR
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if(ValidarCampos())
            {
                Models.Vant Vant = null;
                if (hdnIdVant.Value.Equals(""))
                { // Insert
                    using (Tn tn = new Tn("bd_reapp"))
                    {
                        Vant = new Models.Vant();
                        Vant.IdMarcaVant = ddlMarcaVant.SelectedValue.ToIntID();
                        Vant.IdTipoVant = ddlClaseVant.SelectedValue.ToIntID();
                        Vant.Modelo = txtModelo.Text;
                        Vant.FHAlta = DateTime.Now;
                        Vant.IdUsuario = ddlSolicitante.SelectedValue.ToIntID();
                        Vant.Fabricante = txtFabricante.Text;
                        Vant.AñoFabricacion = new DateTime(txtAñoFabricacion.Text.ToInt(), 1, 1);
                        Vant.LugarFabricacion = txtLugarFabricacion.Text;
                        Vant.LugarGuardado = txtLugarGuardado.Text;
                        Vant.NumeroSerie = txtNumeroSerie.Text;
                        Vant.IdLocalidadPartido = ddlLocalidadPartido.SelectedValue.ToIntID();
                        //Vant.MotivoBaja = "";
                        Vant.Insert();

                    }
                }
                else if (pnlMotivoBaja.Visible == false)
                { // Update

                    Vant = new Models.Vant().Select(hdnIdVant.Value.ToInt());


                    Vant.IdMarcaVant = ddlMarcaVant.SelectedValue.ToIntID();
                    Vant.IdTipoVant = ddlClaseVant.SelectedValue.ToIntID();
                    Vant.Modelo = txtModelo.Text;
                    Vant.Fabricante = txtFabricante.Text;
                    Vant.AñoFabricacion = txtAñoFabricacion.Text.ToDateTime();
                    Vant.LugarFabricacion = txtLugarFabricacion.Text;
                    Vant.LugarGuardado = txtLugarGuardado.Text;
                    Vant.NumeroSerie = txtNumeroSerie.Text;
                    Vant.IdLocalidadPartido = ddlLocalidadPartido.SelectedValue.ToIntID();
                    Vant.Update();

                }
                else
                {
                    Vant = new Models.Vant().Select(hdnIdVant.Value.ToInt());
                    Vant.MotivoBaja = txtMotivoBaja.Text;
                    Vant.Update();

                    using (SP sp = new SP("bd_reapp"))
                    {
                        sp.Execute("usp_DarDeBajaVant",
                            P.Add("IdVant", Vant.IdVant)
                        );
                    }
                }


                MostrarListado();
                cargarGvVants();
            }


        }

        protected void LimpiarModal()
        {
            ddlModalSolicitante.Items.Clear();
            ddlMarcaVant.Items.Clear();
            ddlClaseVant.Items.Clear();
            txtModelo.Text = "";
            txtAñoFabricacion.Text = "";
            txtLugarFabricacion.Text = "";
            txtLugarGuardado.Text = "";
            txtNumeroSerie.Text = "";
            txtFabricante.Text = "";
            ddlLocalidadPartido.Items.Clear();
            ddlProvincia.Items.Clear();
            txtMotivoBaja.Text = "";

            pnlError.Visible = false;
        }


        protected void MostrarListado()
        {
            pnlListado.Visible = true;
            btnNuevo.Visible = true;
            pnlABM.Visible = false;
            btnVolver.Visible = false;
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            btnNuevo.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;

            pnlMotivoBaja.Visible = false;
            txtMotivoBaja.Enabled = false;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            MostrarListado();
        }

        protected void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected bool ValidarCampos()
        {
            if (txtFabricante.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el nombre del Fabricante del VANT.";
                pnlError.Visible = true;
                return false;
            }
            if (ddlMarcaVant.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, seleccione la Marca del VANT.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModelo.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el modelo del VANT.";
                pnlError.Visible = true;
                return false;
            }
            if (txtNumeroSerie.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el Número de Serie del VANT.";
                pnlError.Visible = true;
                return false;

            }
            if (txtAñoFabricacion.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el Año de Fabricación del VANT.";
                pnlError.Visible = true;
                return false;
            }
            if (txtLugarFabricacion.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el Lugar de Fabricación del VANT.";
                pnlError.Visible = true;
                return false;
            }

            if (txtLugarGuardado.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el Lugar de Guardado del VANT.";
                pnlError.Visible = true;
                return false;
            }
            if (ddlClaseVant.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, seleccione la Clase del VANT.";
                pnlError.Visible = true;
                return false;
            }
            if (ddlLocalidadPartido.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, seleccione la Localidad/Partido.";
                pnlError.Visible = true;
                return false;
            }
            if (ddlProvincia.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, seleccione la Provincia.";
                pnlError.Visible = true;
                return false;
            }
            if(pnlMotivoBaja.Visible & txtMotivoBaja.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el Motivo de Baja.";
                pnlError.Visible = true;
                return false;
            }

            pnlError.Visible = false;
            return true;
        }

    }
}