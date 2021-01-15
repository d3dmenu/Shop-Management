Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Net
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.CompilerServices
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Public Class DashBoard

#Region "Declarations"
    Dim imagebitmap As Bitmap
    Dim graphicsvariable As Graphics
    Dim WithEvents v As New PictureBox
    Dim panelz As Panel
#End Region

    Dim x, y As Integer
    Dim Screen As Point
    Dim newpoint As New Point
    Dim state As Boolean = False

    Private Sub InitGrid()
        '// ตั้งค่าจำนวนหลัก (Columns)
        With dgvData
            .Columns.Add("PK", "PK")
            .Columns.Add("Description", "รายการที่สั่ง")
            .Columns.Add("Unit Price", "ราคา")
            '//
            .Columns(0).Visible = False
            With .Columns(2)
                .ValueType = GetType(Decimal)
                .DefaultCellStyle.Format = "N2"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With
        '//
        With dgvData
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .Font = New Font("Tahoma", 9)
            .RowTemplate.Height = 30
            .RowTemplate.MinimumHeight = 20
            ' Autosize Column
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .AutoResizeColumns()
            '// Even-Odd Color
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35)
            '.ForeColor = Color.Black
            ' Adjust Header Styles
            With .ColumnHeadersDefaultCellStyle
                .BackColor = Color.FromArgb(20, 20, 20)
                .ForeColor = Color.Silver
                .Font = New Font("Tahoma", 10, FontStyle.Bold)
                .WrapMode = DataGridViewTriState.False
            End With
        End With
    End Sub

    Private Sub AddRow(ByVal PK As Integer, ByVal Description As String, ByVal UnitPrice As Double)
        '// มองไปข้างหน้า ... กรณีที่ใช้ฐานข้อมูล เราจะต้องนำค่า PK มาใช้อ้างอิง ซึ่งข้อมูลแถวนี้จะมาจากการทำ Query
        Dim row As String() = New String() {PK, Description, Format(CDbl(UnitPrice), "#,##0.00 บาท")}
        dgvData.Rows.Add(row)
        '// โจทย์ให้นำไปคิดต่อคือ ...
        '// ต้องเช็คก่อนว่าค่า PK มันมีอยู่ในแถวแล้วหรือไม่ หากมีให้ "เพิ่มจำนวนขึ้น 1" ... จะทำอย่างไร?

        '// แสดงผลราคาสินค้าล่าสุด
        cprice.Text = UnitPrice + " บาท"
        '// หาจำนวนเงินรวมใหม่
        Dim Amount As Double
        For i = 0 To dgvData.RowCount - 1
            Amount = Amount + CDbl(dgvData.Rows(i).Cells(2).Value)
        Next
        txtprice.Text = Format(Amount, "#,##0.00")
        txtAmount.Text = Format(Amount, "#,##0.00") + " บาท"
        '// โฟกัสไปที่ DataGridView แล้วย้ายไปแถวล่าสุด
        dgvData.Focus()
        SendKeys.Send("^{END}")
    End Sub

    Sub SendOrder()
        If dgvData.RowCount < 1 Then
            MsgBox("กรุณาทำการสั่งอาหารอย่างน้อย 1 รายการ!", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
            Exit Sub
        End If

        Dim sendcommand As New MySqlCommand("INSERT INTO bill (orderID, productID, title, price, fullname, phone, email, address, service, status) VALUES ( '" & cBill.Text & "', '" & cPID.Text & "', '" & listfood.Text & "', '" & txtprice.Text & "', '" & txtfullname.Text & "', '" & txtphone.Text & "', '" & txtemail.Text & "', '" & txtaddress.Text & "', '" & lblstate.Text & "', 'รอการยืนยัน')", Conn)
        Try
            sendcommand.ExecuteNonQuery()
            MsgBox(" สั่งซื้อรายการอาหารเรียบร้อยแล้วค่ะ ", MsgBoxStyle.Information, "ระบบแจ้งเตือน")
            PID.Text = RandomString()
            cBill.Text = RandomNumber()
            cPID.Text = PID.Text
            dgvData.Rows.Clear()
            txtAmount.Text = "0.00 บาท"
            cAmount.Text = "0.00 บาท"
            cprice.Text = "0.00 บาท"
            counter.Text = "0"
            lblinfo.Text = "จำนวนรายการอาหารในตะกร้าทั้งหมด (0) รายการ"
            TabControl1.SelectedTab = TabPage2
        Catch ex As Exception
            MsgBox(" ไม่สามารถเชื่อมต่อระบบฐานข้อมูลได้ ", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
            MsgBox(ex.Message & vbNewLine & "ไม่สามารถเชื่อมต่อระบบฐานข้อมูลได้ กรุณาลองใหม่ภายหลัง", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
            End
        End Try
    End Sub
#Region "function for getting screenshot"
    Public Function getscreenshot()
        imagebitmap = New Bitmap(Me.Size.Width, Me.Size.Height) 'creates a new blank bitmap having size of the form'
        graphicsvariable = Graphics.FromImage(imagebitmap)      'creates a      picture template that enables the graphics variable to draw on'
        graphicsvariable.CopyFromScreen(Me.PointToScreen(Me.ClientRectangle.Location), New Point(0, 0), Me.ClientRectangle.Size)    'copies graphics that is on the screen to the imagebitmap'
        Return imagebitmap
    End Function
#End Region

#Region "method for blurring"
    Sub BlurBitmap(ByRef image As Bitmap, Optional ByVal BlurForce As Integer = 1) '2
        'We get a graphics object from the image'
        Dim g As Graphics = Graphics.FromImage(image)
        'declare an ImageAttributes to use it when drawing'
        Dim att As New ImageAttributes
        'declare a ColorMatrix'
        Dim m As New ColorMatrix
        ' set Matrix33 to 0.5, which represents the opacity. so the drawing will be semi-trasparent.'
        m.Matrix33 = 0.5
        'Setting this ColorMatrix to the ImageAttributes.'
        att.SetColorMatrix(m)
        'drawing the image on it self, but not in the same coordinates, in a way that every pixel will be drawn on the pixels arround it.'
        For x = -1 To BlurForce
            For y = -1 To BlurForce
                'Drawing image on it self using out ImageAttributes to draw it semi-transparent.'
                g.DrawImage(image, New Rectangle(x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, att)
            Next
        Next
        'disposing ImageAttributes and Graphics. the effect is then applied. '
        att.Dispose() 'dispose att'
        g.Dispose() 'dispose g'
    End Sub
#End Region
#Region "function event close blur method"
    Sub CloseBlur()
        panelz.Controls.Remove(panelz)
        Me.Controls.Remove(panelz)
    End Sub
#End Region
#Region "event that handles the removal of the blurred image when clicked"
    Private Sub v_Click(sender As Object, e As EventArgs) Handles v.Click
        panelz.Controls.Remove(sender) 'removes the picturebox from the pa
        Me.Controls.Remove(panelz)    'removes the panel from the form'
    End Sub
#End Region

    Private Sub DarkForm()
        Screen = Me.Location
        Blur.Size = New Size(Me.Width, Me.Height)
        Blur.BackColor = Color.Black
        Blur.Show()
        Blur.Location = New Point(Screen.X, Screen.Y)
        Blur.BringToFront()
    End Sub
    Private Sub BlurForm()
        panelz = New Panel
        panelz.Size = New Size(Me.Width, Me.Height)
        Me.Controls.Add(panelz)

        panelz.Controls.Add(v)  'add picturebox to panelz, pls also note that i made the panel cover the whole form'
        Dim b As Bitmap = getscreenshot() 'get the screen shot'
        BlurBitmap(b) 'blur the screen shot'
        v.Image = b 'set the picturebox image as b (the blurred image)'
        v.Dock = DockStyle.Fill
        v.BringToFront()
        v.Size = Me.Size
        panelz.BringToFront()
        DarkForm()
        state = True
    End Sub

    Private Sub Config()
        PBtn2.BackgroundImage = My.Resources.Signupoff
        PStart.BackgroundImage = My.Resources.StartOff
        PBtn1.BackgroundImage = My.Resources.Off
        lblheader.Text = "ยืนยันรายการอาหาร - " + PID.Text
        Label3.ForeColor = Color.Gray
        Label4.ForeColor = Color.Gray
        Label5.ForeColor = Color.Gray
    End Sub

    Private Sub TabPage1_MouseDown(sender As Object, e As MouseEventArgs) Handles TabPage1.MouseDown
        x = Control.MousePosition.X - Me.Location.X
        y = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub TabPage1_MouseMove(sender As Object, e As MouseEventArgs) Handles TabPage1.MouseMove
        Config()
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= x
            newpoint.Y -= y
            Me.Location = newpoint
            Application.DoEvents()
        End If
    End Sub

    Private Sub PBtn1_MouseClick(sender As Object, e As MouseEventArgs) Handles PBtn1.MouseClick
        PBtn1.BackgroundImage = My.Resources.Off
        BlurForm()
        Dim frmlogin As New Login
        If state = True Then
            frmlogin.ShowDialog()
            state = False
        End If
    End Sub

    Private Sub PBtn1_MouseMove(sender As Object, e As MouseEventArgs) Handles PBtn1.MouseMove
        PBtn1.BackgroundImage = My.Resources._On
        Label4.ForeColor = Color.DarkGray
    End Sub

    Private Sub PBtn2_MouseClick(sender As Object, e As MouseEventArgs) Handles PBtn2.MouseClick
        PBtn2.BackgroundImage = My.Resources.Signupoff
        BlurForm()
        Dim frmRegister As New Register
        If state = True Then
            frmRegister.ShowDialog()
            state = False
        End If
    End Sub

    Private Sub PBtn2_MouseMove(sender As Object, e As MouseEventArgs) Handles PBtn2.MouseMove
        PBtn2.BackgroundImage = My.Resources.Signupon
        Label5.ForeColor = Color.DarkGray
    End Sub

    Private Sub PStart_MouseClick(sender As Object, e As MouseEventArgs) Handles PStart.MouseClick
        TabControl1.SelectedTab = TabPage2
    End Sub

    Private Sub PStart_MouseMove(sender As Object, e As MouseEventArgs) Handles PStart.MouseMove
        PStart.BackgroundImage = My.Resources.StartOn
        Label3.ForeColor = Color.DarkGray
    End Sub

    Private Sub Panel5_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel5.MouseDown
        x = Control.MousePosition.X - Me.Location.X
        y = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub Panel5_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel5.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= x
            newpoint.Y -= y
            Me.Location = newpoint
            Application.DoEvents()
        End If
    End Sub
    Private Sub Label9_MouseDown(sender As Object, e As MouseEventArgs) Handles Label9.MouseDown
        x = Control.MousePosition.X - Me.Location.X
        y = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub Label9_MouseMove(sender As Object, e As MouseEventArgs) Handles Label9.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= x
            newpoint.Y -= y
            Me.Location = newpoint
            Application.DoEvents()
        End If
    End Sub

    Private Sub TabPage2_MouseDown(sender As Object, e As MouseEventArgs) Handles TabPage2.MouseDown
        x = Control.MousePosition.X - Me.Location.X
        y = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub TabPage2_MouseMove(sender As Object, e As MouseEventArgs) Handles TabPage2.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= x
            newpoint.Y -= y
            Me.Location = newpoint
            Application.DoEvents()
        End If
    End Sub

    Sub MenuList()
        ListControl1.Add("แกงส้มไข่ปลาเรียวเซียว", "Giant Catfish egg in Tamarind flavor soup", "", "250.00", ImageList1.Images(0), 3)
        ListControl1.Add("ปลากระพงนึ่งมะนาว", "Snapper steamed with lemon", "", "200.00", ImageList1.Images(1), 2)
        ListControl1.Add("ปูทะเลผัดผงกะหรี่", "Stir Fried crab curry", "", "550.00", ImageList1.Images(2), 3)
        ListControl1.Add("กุ้งผัดผงกะหรี่", "Stir Fried shrimp curry", "", "350.00", ImageList1.Images(3), 4)
        ListControl1.Add("กุ้งผัดสะตอ", "Stir Fried shrimp with sataw and roasted chili paste", "", "150.00", ImageList1.Images(4), 4)
        ListControl1.Add("กุ้งแม่น้ำเผา", "Grilled Giant River Prawn", "", "600.00", ImageList1.Images(5), 3)
        ListControl1.Add("หมึกกระดองย่าง", "Grilled Amature squid", "", "150.00", ImageList1.Images(6), 4)
        ListControl1.Add("กุ้งอบวุ้นเส้น", "Shrimps with Glass Noodles", "", "175.00", ImageList1.Images(7), 5)
        ListControl1.Add("หอยแครงลวก", "Steamed blanched clams with dipping sauce", "", "150.00", ImageList1.Images(8), 4)
        ListControl1.Add("ข้าวสวย", "Steamed rice", "", "20.00", ImageList1.Images(9), 3)
        ListControl1.Add("น้ำเบอรี่สมูตตี้", "Berry Smoothie", "", "20.00", ImageList1.Images(10), 3)
        ListControl1.Add("ชีสเค้ก", "Cheesecake", "", "80.00", ImageList1.Images(11), 2)
    End Sub
    Private Sub ConfigPreview()
        ListControl1.BorderStyle = BorderStyle.None
        ListControl1.BackColor = Color.FromArgb(26, 26, 26)
        ListControl1.AutoScroll = False
        Call InitGrid()
    End Sub
    Private Sub DashBoard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OpenConnection()
        ConfigPreview()
        MenuList()
        PID.Text = RandomString()
        cBill.Text = RandomNumber()
        cPID.Text = PID.Text
    End Sub
    Function RandomString()
        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 1 To 5
            Dim idx As Integer = r.Next(0, 35)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString()
    End Function
    Function RandomNumber()
        Dim s As String = "0123456789"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 1 To 15
            Dim idx As Integer = r.Next(0, 10)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString()
    End Function

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        PID.Text = RandomString()
        lblheader.Text = "ยืนยันรายการอาหาร - " + PID.Text
        cPID.Text = PID.Text
        cBill.Text = RandomNumber()
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        TabControl1.SelectedTab = TabPage1
    End Sub

    Private Sub btnBack_MouseLeave(sender As Object, e As EventArgs) Handles btnBack.MouseLeave
        btnBack.BackgroundImage = My.Resources.backoff
    End Sub

    Private Sub btnBack_MouseMove(sender As Object, e As MouseEventArgs) Handles btnBack.MouseMove
        btnBack.BackgroundImage = My.Resources.backon
    End Sub

    Private Sub ListControl1_ItemClick(sender As Object, Index As Integer) Handles ListControl1.ItemClick
        ListControl1.Check(Index)
    End Sub

    Private Sub cTimer_Tick(sender As Object, e As EventArgs) Handles cTimer.Tick
        lbltimer.Text = TimeOfDay
    End Sub

    Private Sub BtnManage_Click(sender As Object, e As EventArgs) Handles BtnManage.Click
        TabControl1.SelectedTab = TabPage3
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TabControl1.SelectedTab = TabPage2
    End Sub

    Private Sub TabPage3_MouseDown(sender As Object, e As MouseEventArgs) Handles TabPage3.MouseDown
        x = Control.MousePosition.X - Me.Location.X
        y = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub TabPage3_MouseMove(sender As Object, e As MouseEventArgs) Handles TabPage3.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= x
            newpoint.Y -= y
            Me.Location = newpoint
            Application.DoEvents()
        End If
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If dgvData.RowCount <= 0 Then Exit Sub
        dgvData.Focus()
        '// หาจำนวนเงินรวมใหม่
        Dim Amount As Double
        For i = 0 To dgvData.RowCount - 1
            Amount = Amount + CDbl(dgvData.Rows(i).Cells(2).Value)
        Next
        '// รวมจำนวนเงินทั้งหมด ลบออกจากแถวที่เลือก ด้วยราคาที่อยู่ในหลักที่ 3 (Index = 2)
        txtAmount.Text = Format(Amount - CDbl(dgvData.Item(2, dgvData.CurrentRow.Index).Value), "#,##0.00 บาท")
        cAmount.Text = txtAmount.Text
        '// แล้วค่อยลบแถวออกไป
        dgvData.Rows.Remove(dgvData.CurrentRow)
        cCounter.Text = Format(dgvData.RowCount, "#,##0.00 รายการ")
        lblinfo.Text = Format(dgvData.RowCount, "จำนวนรายการอาหารในตะกร้าทั้งหมด (" + "#0" + ") รายการ")
        dgvData.Refresh()
    End Sub

    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        dgvData.Rows.Clear()
        txtAmount.Text = "0.00 บาท"
        cAmount.Text = "0.00 บาท"
        cprice.Text = "0.00 บาท"
        lblinfo.Text = "จำนวนรายการอาหารในตะกร้าทั้งหมด (0) รายการ"
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        listfood.Text = String.Empty
        Dim c As Integer = 1
        For i As Integer = 0 To dgvData.RowCount - 1
            listfood.Text = listfood.Text + (i + c).ToString + "." + dgvData.Rows(i).Cells(1).Value.ToString() & Environment.NewLine
        Next
        If dgvData.RowCount < 1 Then
            MsgBox("กรุณาทำการสั่งอาหารอย่างน้อย 1 รายการ!", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
            Exit Sub
        End If
        BlurForm()
        Dim frmOrder As New Order
        If state = True Then
            frmOrder.ShowDialog()
            state = False
        End If
    End Sub
    Private Sub step1_MouseLeave(sender As Object, e As EventArgs) Handles step1.MouseLeave
        step1.BackgroundImage = My.Resources.icomenu_off
    End Sub

    Private Sub step1_MouseMove(sender As Object, e As MouseEventArgs) Handles step1.MouseMove
        step1.BackgroundImage = My.Resources.icomenu_on
    End Sub

    Private Sub step2_MouseLeave(sender As Object, e As EventArgs) Handles step2.MouseLeave
        step2.BackgroundImage = My.Resources.ConfigOff
    End Sub

    Private Sub step2_MouseMove(sender As Object, e As MouseEventArgs) Handles step2.MouseMove
        step2.BackgroundImage = My.Resources.ConfigOn
    End Sub

    Private Sub step3_MouseLeave(sender As Object, e As EventArgs) Handles step3.MouseLeave
        step3.BackgroundImage = My.Resources.winoff
    End Sub

    Private Sub step3_MouseMove(sender As Object, e As MouseEventArgs) Handles step3.MouseMove
        step3.BackgroundImage = My.Resources.winon
    End Sub
    Private Sub btnexit_MouseLeave(sender As Object, e As EventArgs) Handles btnexit.MouseLeave
        btnexit.BackgroundImage = My.Resources.logoutoff
    End Sub

    Private Sub btnexit_MouseMove(sender As Object, e As MouseEventArgs) Handles btnexit.MouseMove
        btnexit.BackgroundImage = My.Resources.logouton
    End Sub

    Private Sub btnexit_Click(sender As Object, e As EventArgs) Handles btnexit.Click
        End
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        End
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.WindowState = FormWindowState.Minimized
        Label1.Focus() ' โฟสกันที่ ลาเบล1 เพื่อไม่ให้ปุ่มมีขอบ FlatButton
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        End
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        End
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Me.WindowState = FormWindowState.Minimized
        Label1.Focus() ' โฟสกันที่ ลาเบล1 เพื่อไม่ให้ปุ่มมีขอบ FlatButton
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Me.WindowState = FormWindowState.Minimized
        Label1.Focus() ' โฟสกันที่ ลาเบล1 เพื่อไม่ให้ปุ่มมีขอบ FlatButton
    End Sub

    Private Sub choice1_CheckedChanged(sender As Object) Handles choice1.CheckedChanged
        If choice1.Checked = True Then
            lblstate.Text = "จัดส่งปลายทาง"
        End If
    End Sub

    Private Sub choice2_CheckedChanged(sender As Object) Handles choice2.CheckedChanged
        If choice2.Checked = True Then
            lblstate.Text = "ไปรับด้วยตัวเอง"
        End If
    End Sub

    Private Sub step1_Click(sender As Object, e As EventArgs) Handles step1.Click
        TabControl1.SelectedTab = TabPage2
    End Sub

    Private Sub step2_Click(sender As Object, e As EventArgs) Handles step2.Click
        TabControl1.SelectedTab = TabPage3
    End Sub

    Private Sub CheckBill_Click(sender As Object, e As EventArgs)
        BlurForm()
        Dim frmhistory As New history
        If state = True Then
            frmhistory.ShowDialog()
            state = False
        End If
    End Sub
End Class
