﻿<%@ Page Title="" Language="C#" MasterPageFile="~/View/usuario/usuario.master" AutoEventWireup="true" CodeFile="~/Controller/usuario/configurarUsu.aspx.cs" Inherits="View_usuario_configurarUsu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="text-center text-primary"><strong>Mi cuenta</strong></h1>
    <div align="center">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/ima/configurar.png" align="center" />
    </div>
        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                            <br /> <br />     
                <div class="form-group">
                    Nombre:
                    <asp:Label ID="LB_nombre" runat="server" class="form-control" ></asp:Label>
                    Apellido:
                    <asp:Label ID="LB_apellido" runat="server" Text="Label" class="form-control"></asp:Label>
                    Correo:
                    <asp:Label ID="LB_correo" runat="server" Text="Label" class="form-control"></asp:Label>
                    <asp:TextBox ID="TB_editCorreo" runat="server" class="form-control" placeholder="Ingrese el nuevo correo"  TextMode="Email" ValidationGroup="grupocorreo"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TB_editCorreo" ErrorMessage="*" ValidationGroup="grupocorreo"></asp:RequiredFieldValidator>
                    <asp:Button ID="BTN_editarCorreo" runat="server" Text="Cambiar" class="btn btn-primary" ValidationGroup="grupocorreo"  />
                    <asp:Button ID="BTN_cancelar" runat="server" Text="Cancelar" class="btn btn-primary"  />
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Inventario/engranaje.png" />
                    <br />
                    Contraseña:
                    <asp:Label ID="LB_contraseña" runat="server" Text="Label" class="form-control"></asp:Label>
                    <asp:TextBox ID="TB_editarPass" runat="server" class="form-control" ValidationGroup="grupopass" placeholder="Ingrese la nueva contraseña"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="TB_editarPass" ValidationGroup="grupopass"></asp:RequiredFieldValidator>
                    <asp:Button ID="BTN_editarPass" runat="server" Text="Cambiar" class="btn btn-primary" ValidationGroup="grupopass" O/>
                    <asp:Button ID="BTN_cancelar2" runat="server" Text="Cancelar" Class="btn btn-primary"   />
                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Inventario/engranaje.png"   />
                    <br />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="la contraseña debe tener entre 8 - 10 caracteres tanto letra, numeros y caracteres especiales" ControlToValidate="TB_editarPass" ValidationExpression="(?=^.{8,10}$)(?=.*\d)(?=.*[a-zA-Z])(?=.*[!@#$%^&*()_+}{':;'?/>.<,])(?!.*\s).*$" ValidationGroup="grupopass"></asp:RegularExpressionValidator>
                </div>    
            </div>
        </div>      
</asp:Content>

