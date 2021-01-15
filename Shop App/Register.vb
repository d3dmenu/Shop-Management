Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Public Class Register
    Dim x, y As Integer
    Dim newpoint As New Point
    Public _Shadow As Dropshadow
    Sub SystemRegister()
        If txtusername.Text = String.Empty Or txtpassword.Text = String.Empty Or txtaddress.Text = String.Empty Or txtphone.Text = String.Empty Or txtemail.Text = String.Empty Or txtfullname.Text = String.Empty Then
            MsgBox("กรุณาใส่ข้อมุลให้ครบถ้วน!", MsgBoxStyle.Exclamation, "ระบบแจ้งเตือน")
            Exit Sub
        End If
        Dim readcommand As New MySqlCommand("SELECT * FROM members", Conn)
        Dim datareader As MySqlDataReader
        Try
            datareader = readcommand.ExecuteReader()
            While datareader.Read()
                If datareader(1).ToString = txtusername.Text Then
                    MsgBox("บัญชีผู้ใช้: " + txtusername.Text + " มีผู้ใช้งานแล้ว!", MsgBoxStyle.Exclamation, "ระบบแจ้งเตือน")
                    datareader.Close()
                    Exit Sub
                End If
                If datareader(5).ToString = txtphone.Text Then
                    MsgBox("เบอร์โทร: " + txtphone.Text + " มีผู้ใช้งานแล้ว!", MsgBoxStyle.Exclamation, "ระบบแจ้งเตือน")
                    datareader.Close()
                    Exit Sub
                End If
            End While
            datareader.Close()
        Catch
            MsgBox("[Error] - Win32 connected database", MsgBoxStyle.Exclamation, "Win32")
        End Try

        Dim insertcommand As New MySqlCommand("INSERT INTO members (username, password, fullname, email, phone, address) VALUES('" & txtusername.Text & "', '" & MD5Encrypter(txtpassword.Text) & "', '" & txtfullname.Text & "',  '" & txtemail.Text & "',  '" & txtphone.Text & "',  '" & txtaddress.Text & "')", Conn)
        Try
            insertcommand.ExecuteNonQuery()
            MsgBox("ลงทะเบียนใช้งานสำเร็จแล้ว !", MsgBoxStyle.Information, "ระบบแจ้งเตือน")
            Login.txtuser.Text = txtusername.Text
            Login.txtpass.Focus()
            DashBoard.CloseBlur()
            Blur.Close()
            DashBoard.Activate()
            Me.Close()

            txtusername.Clear()
            txtpassword.Clear()
            txtfullname.Clear()
            txtemail.Clear()
            txtaddress.Clear()
            txtphone.Clear()
            Login.Show()
            Me.Close()
        Catch
            MsgBox(" ไม่สามารถเชื่อมต่อระบบฐานข้อมูลได้ ", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
            End
        End Try
    End Sub
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DashBoard.CloseBlur()
        Blur.Close()
        DashBoard.Activate()
        Me.Close()
    End Sub

    Private Sub Panel1_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel1.MouseDown
        x = Control.MousePosition.X - Me.Location.X
        y = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub Panel1_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel1.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= x
            newpoint.Y -= y
            Me.Location = newpoint
            Application.DoEvents()
        End If
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        SystemRegister()
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
    Private Sub Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        shadow()
    End Sub
End Class