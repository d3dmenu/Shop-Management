Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Public Class Login
    Dim x, y As Integer
    Dim newpoint As New Point
    Public _Shadow As Dropshadow
    Sub shadow()
        If Not DesignMode Then
            _Shadow = New Dropshadow(Me) With {
                .ShadowH = 0,
                .ShadowV = 0,
                .ShadowBlur = 9,
                .ShadowSpread = 2,
                .Opacity = 120,
                .ShadowColor = Color.FromArgb(CInt(.Opacity), 0, 0, 0),
                .ShadowRadius = 0
            }
            _Shadow.RefreshShadow()
        End If
        _Shadow.ShadowSpread = CInt(2)
        _Shadow.ShadowBlur = CInt(10)
        _Shadow.ShadowColor = Color.FromArgb(CInt(130), 0, 0, 0)
        _Shadow.ShadowRadius = 7
        _Shadow.RefreshShadow()
    End Sub
    Sub SystemLogin()
        Dim usrfound As Boolean = False
        Dim readcommand As New MySqlCommand("SELECT * FROM members WHERE username='" & txtuser.Text & "'", Conn)
        Dim datareader As MySqlDataReader

        Try
            datareader = readcommand.ExecuteReader
            While datareader.Read()
                If datareader(1).ToString = txtuser.Text And datareader(2).ToString = MD5Encrypter(txtpass.Text) Then
                    My.Settings.Username = datareader(1).ToString
                    My.Settings.fullname = datareader(3).ToString
                    My.Settings.email = datareader(4).ToString
                    My.Settings.phone = datareader(5).ToString
                    My.Settings.Address = datareader(6).ToString
                    My.Settings.Save()
                    DashBoard.txtfullname.Text = My.Settings.fullname
                    DashBoard.txtphone.Text = My.Settings.phone
                    DashBoard.txtemail.Text = My.Settings.email
                    DashBoard.txtaddress.Text = My.Settings.Address
                    usrfound = True
                    Exit While
                End If
            End While
            datareader.Close()
            If usrfound = True Then
                DashBoard.TabControl1.SelectedTab = DashBoard.TabPage2
                Blur.Close()
                DashBoard.CloseBlur()
                DashBoard.Activate()
                Me.Close()
            Else
                MsgBox("บัญชีผู้ใช้ หรือ พาสเวิร์ดไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
            End If
        Catch
            MsgBox("[Error] - Win32 connected database", MsgBoxStyle.Exclamation, "Win32")
            End
        End Try
        txtpass.Clear()

    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtuser.Text = My.Settings.Username
        txtpass.Select()
        shadow()
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        DashBoard.CloseBlur()
        Blur.Close()
        DashBoard.Activate()
        Me.Close()

    End Sub

    Private Sub Label1_MouseDown(sender As Object, e As MouseEventArgs) Handles Label1.MouseDown
        x = Control.MousePosition.X - Me.Location.X
        y = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub Label1_MouseMove(sender As Object, e As MouseEventArgs) Handles Label1.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= x
            newpoint.Y -= y
            Me.Location = newpoint
            Application.DoEvents()
        End If
    End Sub

    Private Sub Panel2_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel2.MouseDown
        x = Control.MousePosition.X - Me.Location.X
        y = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub Panel2_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel2.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= x
            newpoint.Y -= y
            Me.Location = newpoint
            Application.DoEvents()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
        DashBoard.CloseBlur()
        Blur.Close()
    End Sub

    Private Sub btnlogin_Click(sender As Object, e As EventArgs) Handles btnlogin.Click
        SystemLogin()
    End Sub

    Private Sub txtpass_KeyDown(sender As Object, e As KeyEventArgs) Handles txtpass.KeyDown
        If e.KeyCode = Keys.Enter Then
            SystemLogin()
        End If
    End Sub
End Class