using Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_administrador_ConsultarInventario : System.Web.UI.Page
{
    protected string aux;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BT_Inabilitar_Click(object sender, EventArgs e)
    {
        InabilitarDDLS();
        BT_Inabilitar.Visible = false;
    }

    protected void BT_Filtrar_Click(object sender, EventArgs e)
    {
        HabilitarDDLS();
    }

    protected void ODS_ConsultarCategoria_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        GV_inventario.DataSourceID = "ODS_Inventario";

        if (TB_Buscar.Text != "")
        {

            InabilitarDDLS();
        }
        else
        if (DDL_Categoria2.SelectedIndex != 0 || DDL_Marca2.SelectedIndex != 0)
        {

            HabilitarDDLS();
        }
        else
        if (DDL_Categoria2.SelectedIndex == 0 || DDL_Marca2.SelectedIndex == 0)
        {
            InabilitarDDLS();
        }
    }

    protected void BT_Buscar_Click(object sender, EventArgs e)
    {
        if (TB_Buscar.Text != "")
        {
            GV_inventario.DataSourceID = "ODS_Buscar";
            InabilitarDDLS();
        }
        else
       if (DDL_Categoria2.SelectedIndex != 0 && DDL_Marca2.SelectedIndex != 0)
        {
            GV_inventario.DataSourceID = "ODS_BuscarMarcaCategoria";
            HabilitarDDLS();
        }
        else
       if (DDL_Marca2.SelectedIndex != 0)
        {

            GV_inventario.DataSourceID = "ODS_BuscarMarca";
            HabilitarDDLS();
        }
        else
       if (DDL_Categoria2.SelectedIndex != 0)
        {
            GV_inventario.DataSourceID = "ODS_BuscarCategoria";
            HabilitarDDLS();

        }
        else
       if (DDL_Categoria2.SelectedIndex == 0 || DDL_Marca2.SelectedIndex == 0)
        {
            InabilitarDDLS();
        }
    }

    protected void GV_inventario_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //indice de la fila
        GridViewRow row = GV_inventario.Rows[e.RowIndex];
        ClientScriptManager cm = this.ClientScript;

        //traer valor textbox
        var tb_referencia = row.FindControl("tb_referencia") as TextBox;

        FileUpload fu_imagen = (FileUpload)row.FindControl("fu_imagen");



        int id = Convert.ToInt32(GV_inventario.DataKeys[e.RowIndex].Values[0].ToString());
        //EncapInventario inventario = new EncapInventario();//



        string urlExistente = ((Image)row.FindControl("I_editar")).ImageUrl;
        string nombreArchivo = System.IO.Path.GetFileName(fu_imagen.PostedFile.FileName);
        string Ruta = "~\\Inventario\\" + nombreArchivo;

        if (nombreArchivo == "")
        {
            e.NewValues["Imagen"] = urlExistente;
        }
        else
        if ((nombreArchivo != ""))
        {

            string extension = System.IO.Path.GetExtension(fu_imagen.PostedFile.FileName);

            string saveLocationAdmin = HttpContext.Current.Server.MapPath("~\\Inventario\\") + nombreArchivo;

            if (!(extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png") || extension.Equals(".gif")))
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert ('tipo de archivo no valido ' );</script>");
                e.Cancel = true;
            }
            //verificar existencia de un arhivo con el mismo nombre
            if (System.IO.File.Exists(saveLocationAdmin))
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert ('Imagen Existente' );</script>");
                e.Cancel = true;
            }
            try
            {
                if (urlExistente != "")
                {
                    File.Delete(Server.MapPath(urlExistente));
                }
                fu_imagen.PostedFile.SaveAs(saveLocationAdmin);
                e.NewValues["Imagen"] = Ruta;
            }
            catch (Exception exc)

            {

                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert ('Error' );</script>");
                return;
            }

            ValidarControles();

        }

        var db = new Mapeo();
        var consulta = (from x in db.inventario where (x.Referencia == tb_referencia.Text) select x.Referencia).Count();


        //referencia Existente
        if (consulta == 1)
        {
            //si el valorexistente es el mismo
            if (Label4.Text == tb_referencia.Text)
            {
                //agrego valor que habia antes 
                e.NewValues["Referencia"] = Label4.Text;


            }
            else
            {

                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert ('Referencia Existente' );</script>");
                e.Cancel = true;
            }
        }

    }

    protected void GV_inventario_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewRow row = GV_inventario.Rows[e.NewEditIndex];
        int id = Convert.ToInt32(GV_inventario.DataKeys[e.NewEditIndex].Values[0].ToString());

        var lb_referencia = row.FindControl("lb_referencia") as Label;

        //asigno el valor que tenia antes de editarlo

        aux = lb_referencia.Text;
        Label4.Text = aux;
        Label4.Visible = true;

    }

    protected void GV_inventario_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    private void HabilitarDDLS()
    {
        TB_Buscar.Text = "";
        BT_Filtrar.Visible = false;
        BT_Inabilitar.Visible = true;
        TB_Buscar.Enabled = false;
        DDL_Categoria2.Enabled = true;
        DDL_Marca2.Enabled = true;
    }
    private void InabilitarDDLS()
    {
        DDL_Categoria2.SelectedIndex = 0;
        DDL_Marca2.SelectedIndex = 0;
        BT_Filtrar.Visible = true;
        BT_Inabilitar.Visible = false;
        TB_Buscar.Enabled = true;
        DDL_Categoria2.Enabled = false;
        DDL_Marca2.Enabled = false;
    }

    public void ValidarControles()
    {

        if (TB_Buscar.Text != "")
        {

            InabilitarDDLS();
        }
        else
        if (DDL_Categoria2.SelectedIndex != 0 || DDL_Marca2.SelectedIndex != 0)
        {

            HabilitarDDLS();
        }
        else
        if (DDL_Categoria2.SelectedIndex == 0 || DDL_Marca2.SelectedIndex == 0)
        {
            InabilitarDDLS();
        }

    }

    protected void GV_inventario_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}