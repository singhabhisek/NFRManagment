<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default4.aspx.cs" Inherits="Default4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" href="Ultimate.css" />
</head>

<body>
    <form id="form1" runat="server">
        <!--Header-->
        <div class="banner-style">
            <img src="Images/Amazon_logo.svg.png" class="logo-style" />
            <div style="padding-left: 400px; justify-content: center;">
                <h3>TRUIST MANAGEMENT SYSTEM</h3>
            </div>
        </div>

        <!-- The sidebar -->
        <div class="sidebar">
            <div class="sidebar">
                <a href="#home"><i class="fa fa-fw fa-home"></i>Home</a>
                <a href="#dashboard"><i class="fa fa-fw fa-dashboard"></i>Dashboard</a>
                <a href="#uploadnfr"><i class="fa fa-fw fa-upload"></i>Upload NFR</a>
                <a href="#comparenfr"><i class="fa fa-fw fa-compare"></i>Compare NFR</a>
                <a href="#contact"><i class="fa fa-fw fa-contact"></i>Contact</a>
            </div>
        </div>

        <!--Body-->

        <div style="margin-left: 600px; top: 200px;" class="form-floating mb-3">
            <input type="email" style="width: 200px; height: 15px;" class="form-control" id="floatingInput" placeholder="name@example.com" />
            <label for="floatingInput">Email address</label>
        </div>
        <div style="margin-left: 600px; top: 200px;" class="form-floating">
            <input type="password" class="form-control" style="width: 200px; height: 15px;" id="floatingPassword" placeholder="Password" />
            <label for="floatingPassword">Password</label>
        </div>

        <div style="margin-left: 600px; top: 200px;" class="form-floating">
            <select class="form-select" id="floatingSelect" style="width: 225px;" aria-label="Floating label select example" />
            <option selected>Open this select menu</option>
            <option value="1">One</option>
            <option value="2">Two</option>
            <option value="3">Three</option>
            </select>
        <label for="floatingSelect">Works with selects</label>
        </div>

    </form>
</body>
</html>
