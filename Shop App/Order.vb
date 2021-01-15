Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.IO
Public Class Order
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
    Sub SendOrder()

        If txtfullname.Text = String.Empty Or txtaddress.Text = String.Empty Or txtphone.Text = String.Empty Then
            MsgBox("กรุณากรอกรายละเอียดให้ครบ!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        Dim ms As New MemoryStream
        ' เมื่อทำโปรเจคตัวนี้เสร้จอย่าลืมลบ try ออก เพราะการสั่งสินค้าต้องโอนสลิมเงินยืนยันทุกครั้ง
        Try
            ImgTransaction.Image.Save(ms, ImgTransaction.Image.RawFormat)
        Catch ex As Exception

        End Try

        Dim orderby As New List(Of String)()
        Dim priceby As New List(Of String)()
        Dim countname As String
        Dim orderprice As String
        For i As Integer = 0 To DashBoard.dgvData.RowCount - 1
            countname = DashBoard.dgvData.Rows(i).Cells(1).Value.ToString()
            orderprice = CSng(DashBoard.dgvData.Rows(i).Cells(2).Value).ToString()
            If Not orderby.Contains(countname) Then
                orderby.Add(countname)
                priceby.Add(orderprice)
            End If
        Next

        For c As Integer = 0 To orderby.Count - 1
            Dim name As String = orderby.Item(c)
            Dim price As String = priceby.Item(c)
            Dim count As Integer = 0
            For i As Integer = 0 To DashBoard.dgvData.RowCount - 1
                If DashBoard.dgvData.Rows(i).Cells(1).Value.ToString() = name Then
                    count += 1
                End If
            Next

            'Dim sendcommand As New MySqlCommand("INSERT INTO bill (orderID, productID, title, price, quantity, fullname, phone, email, address) VALUES ( '" & DashBoard.cBill.Text & "', '" & DashBoard.cPID.Text & "', '" & name.ToString & "', '" & price.ToString & "', '" & count.ToString & "', '" & txtfullname.Text & "', '" & txtphone.Text & "', '" & DashBoard.txtemail.Text & "', '" & txtaddress.Text & "')", Conn)
            Dim sendcommand As New MySqlCommand("INSERT INTO bill (orderID, productID, title, price, quantity, fullname, phone, email, address, service, times, transaction) VALUES ( '" & DashBoard.cBill.Text & "', '" & DashBoard.cPID.Text & "', '" & name.ToString & "', '" & price.ToString & "', '" & count.ToString & "', '" & txtfullname.Text & "', '" & txtphone.Text & "', '" & DashBoard.txtemail.Text & "', '" & txtaddress.Text & "', '" & service.Text & "', @time, @img)", Conn)
            sendcommand.Parameters.Add("@img", MySqlDbType.Blob).Value = ms.ToArray()
            sendcommand.Parameters.Add("@time", MySqlDbType.Text).Value = hour.Text + ":" + min.Text
            Try
                sendcommand.ExecuteNonQuery()
                ms.SetLength(0)
            Catch ex As Exception
                MsgBox(" ไม่สามารถเชื่อมต่อระบบฐานข้อมูลได้ ", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
                MsgBox(ex.Message & vbNewLine & "ไม่สามารถเชื่อมต่อระบบฐานข้อมูลได้ กรุณาลองใหม่ภายหลัง", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
                End
            End Try
        Next
        MsgBox(" สั่งซื้อรายการอาหารเรียบร้อยแล้วค่ะ " + DashBoard.cBill.Text, MsgBoxStyle.Information, "ระบบแจ้งเตือน")
        clearconfig()
    End Sub
    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        DashBoard.CloseBlur()
        Blur.Close()
        DashBoard.Activate()
        Me.Close()
    End Sub
    Private Sub autoconfig()
        txtaddress.Text = DashBoard.txtaddress.Text
        txtphone.Text = DashBoard.txtphone.Text
        txtfullname.Text = DashBoard.txtfullname.Text
        shadow()
    End Sub
    Private Sub clearconfig()
        txtfullname.Text = String.Empty
        txtaddress.Text = String.Empty
        txtphone.Text = String.Empty
        DashBoard.PID.Text = DashBoard.RandomString()
        DashBoard.cBill.Text = DashBoard.RandomNumber()
        DashBoard.lblheader.Text = "ยืนยันรายการอาหาร - " + DashBoard.PID.Text
        DashBoard.cPID.Text = DashBoard.PID.Text
        DashBoard.dgvData.Rows.Clear()
        hour.Text = "00"
        min.Text = "00"
        DashBoard.txtAmount.Text = "0.00 บาท"
        DashBoard.cAmount.Text = "0.00 บาท"
        DashBoard.cprice.Text = "0.00 บาท"
        DashBoard.counter.Text = "0"
        DashBoard.lblinfo.Text = "จำนวนรายการอาหารในตะกร้าทั้งหมด (0) รายการ"
        DashBoard.TabControl1.SelectedTab = DashBoard.TabPage2
        DashBoard.CloseBlur()
        Blur.Close()
        DashBoard.Activate()
        Me.Close()
    End Sub
    Private Sub Order_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pid.Text = DashBoard.cPID.Text
        bill.Text = DashBoard.cBill.Text
        If DashBoard.choice1.Checked = True Then
            service.Text = "จัดส่งปลายทาง"
        ElseIf DashBoard.choice2.Checked = True Then
            service.Text = "ไปรับด้วยตัวเอง"
        End If
        autoconfig()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DashBoard.CloseBlur()
        Blur.Close()
        DashBoard.Activate()
        Me.Close()
    End Sub

    Private Sub btninfo_Click(sender As Object, e As EventArgs) Handles btninfo.Click
        SendOrder()
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

    Private Sub browse_Click(sender As Object, e As EventArgs) Handles browse.Click
        Dim opf As New OpenFileDialog
        opf.Filter = "Choose Image(*.JPG;*.PNG;*.GIF|*.jpg;*.png;*.gif)"
        If opf.ShowDialog = Windows.Forms.DialogResult.OK Then
            ImgTransaction.Image = Image.FromFile(opf.FileName)
        End If
    End Sub

    Private Sub ButtonDark1_Click(sender As Object, e As EventArgs)

    End Sub
End Class