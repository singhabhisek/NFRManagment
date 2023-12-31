﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.4/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

    <link rel="stylesheet" href="Resources/css/Ultimate.css" />
    <%-- <link rel="stylesheet" href="Bootstrap.css" />--%>


    <style>
        .float-container {
            border: 3px solid #fff;
            margin-left: 200px;
            margin-top: 30px;
            width: 700px;
        }

        .float-child {
            width: 50%;
            height: 300px;
            float: left;
            padding: 20px;
            margin-top: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>

    <div>
        <table style="width: 100%; background-color: #0099FF; margin-left:100px">
            <tr>
                <td style="width: 20%">
                    <asp:Image ID="Image1" runat="server" AlternateText="Home" ImageUrl="~/Images/test_logo.png" Height="54px" Width="83px" /></td>
                <td style="text-align: center; width: 70%"><b>Login for additional access</b></td>
                <td style="width: 10%">Version 1.0</td>
            </tr>
        </table>
        <div class="float-container">

            <div class="float-child">

                <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/draw2.webp" alt="aa" style="display: block; margin-left: auto; margin-right: auto; margin-top: 50px; margin-bottom: auto; align-items: center; justify-content: center; width: 400px;" />
            </div>

            <div class="float-child">
                <table style="table-layout: auto; width: 400px; margin-left: auto; margin-right: auto; margin-top: 100px; margin-bottom: auto; top: 50px;">

                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr style="width: 100%;">
                        <td style="text-align: right; vertical-align: middle"><span class="txt">UserName: <span class="redstar">*</span> &nbsp;&nbsp;</span></td>
                        <td style="vertical-align: middle">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                <asp:TextBox ID="txtUserName" runat="server" placeholder=" UserName" Font-Size="14px"
                                    CssClass="form-control"></asp:TextBox><br />
                            </div>
                            <asp:RequiredFieldValidator ID="rfvUserName" runat="server"
                                ErrorMessage="Please enter  User Name" ControlToValidate="txtUserName"
                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><br /></td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align: middle"><span class="txt">Password: <span class="redstar">*</span> &nbsp;&nbsp;</span></td>
                        <td style="vertical-align: middle">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                <asp:TextBox ID="txtPwd" CssClass="form-control form-control-sm" Font-Size="14px" runat="server" TextMode="Password" placeholder=" Password"></asp:TextBox>
                            </div>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txtPwd" Display="Dynamic"
                                ErrorMessage="Please enter Password" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>

                <div style="text-align: right;display: inline;margin-left: 150px;margin-top:0px">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Login" OnClick="btnSubmit_Click" />
                   <asp:Button ID="btnclose" runat="server" Text="Back" CssClass="btn btn-secondary" CausesValidation="false" />
                </div>

                <div style="text-align: left; vertical-align: middle; display: inline;margin-left:100px;margin-top:0px; width:400px">
                <asp:Label ID="Label1" Width="100%" Text="" runat="server"></asp:Label>

            </div>
            </div>
            

            <%--  </asp:Panel>--%>
        </div>
    </div>
</asp:Content>

