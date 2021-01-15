<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim Bloom1 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom2 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom3 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom4 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom5 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom6 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom7 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom8 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom9 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom10 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom11 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom12 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom13 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom14 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom15 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom16 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom17 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom18 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom19 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom20 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom21 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom22 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom23 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom24 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom25 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim Bloom26 As Shop_Service.Bloom = New Shop_Service.Bloom()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Search = New System.Windows.Forms.TextBox()
        Me.dgvData = New System.Windows.Forms.DataGridView()
        Me.pid = New System.Windows.Forms.Label()
        Me.orderID = New System.Windows.Forms.Label()
        Me.Fullname = New System.Windows.Forms.Label()
        Me.ButtonBlue1 = New Shop_Service.ButtonBlue()
        Me.ButtonDark2 = New Shop_Service.ButtonDark()
        Me.ButtonDark1 = New Shop_Service.ButtonDark()
        Me.ButtonRed1 = New Shop_Service.ButtonRed()
        Me.Pico = New System.Windows.Forms.Panel()
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 15.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(58, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(246, 24)
        Me.Label1.TabIndex = 65
        Me.Label1.Text = "แผงควบคุมรายการอาหาร"
        '
        'Search
        '
        Me.Search.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Search.BackColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.Search.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Search.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Search.ForeColor = System.Drawing.Color.White
        Me.Search.Location = New System.Drawing.Point(465, 26)
        Me.Search.Multiline = True
        Me.Search.Name = "Search"
        Me.Search.Size = New System.Drawing.Size(235, 26)
        Me.Search.TabIndex = 63
        Me.Search.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'dgvData
        '
        Me.dgvData.AllowDrop = True
        Me.dgvData.AllowUserToOrderColumns = True
        Me.dgvData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvData.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer))
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvData.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(238, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvData.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvData.EnableHeadersVisualStyles = False
        Me.dgvData.GridColor = System.Drawing.Color.Gray
        Me.dgvData.Location = New System.Drawing.Point(3, 65)
        Me.dgvData.Name = "dgvData"
        Me.dgvData.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.Info
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvData.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvData.Size = New System.Drawing.Size(947, 460)
        Me.dgvData.TabIndex = 60
        '
        'pid
        '
        Me.pid.AutoSize = True
        Me.pid.Location = New System.Drawing.Point(462, 9)
        Me.pid.Name = "pid"
        Me.pid.Size = New System.Drawing.Size(21, 13)
        Me.pid.TabIndex = 67
        Me.pid.Text = "pid"
        Me.pid.Visible = False
        '
        'orderID
        '
        Me.orderID.AutoSize = True
        Me.orderID.Location = New System.Drawing.Point(489, 9)
        Me.orderID.Name = "orderID"
        Me.orderID.Size = New System.Drawing.Size(41, 13)
        Me.orderID.TabIndex = 68
        Me.orderID.Text = "orderid"
        Me.orderID.Visible = False
        '
        'Fullname
        '
        Me.Fullname.AccessibleDescription = ""
        Me.Fullname.AutoSize = True
        Me.Fullname.Location = New System.Drawing.Point(536, 9)
        Me.Fullname.Name = "Fullname"
        Me.Fullname.Size = New System.Drawing.Size(47, 13)
        Me.Fullname.TabIndex = 69
        Me.Fullname.Text = "fullname"
        Me.Fullname.Visible = False
        '
        'ButtonBlue1
        '
        Me.ButtonBlue1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ButtonBlue1.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.ButtonBlue1.Image = Nothing
        Me.ButtonBlue1.Location = New System.Drawing.Point(331, 26)
        Me.ButtonBlue1.Name = "ButtonBlue1"
        Me.ButtonBlue1.NoRounding = False
        Me.ButtonBlue1.Size = New System.Drawing.Size(75, 23)
        Me.ButtonBlue1.TabIndex = 70
        Me.ButtonBlue1.Text = "Test"
        '
        'ButtonDark2
        '
        Me.ButtonDark2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonDark2.BackColor = System.Drawing.Color.Transparent
        Bloom1.Name = "DownGradient1"
        Bloom1.Value = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer))
        Bloom2.Name = "DownGradient2"
        Bloom2.Value = System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(65, Byte), Integer))
        Bloom3.Name = "NoneGradient1"
        Bloom3.Value = System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(65, Byte), Integer))
        Bloom4.Name = "NoneGradient2"
        Bloom4.Value = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer))
        Bloom5.Name = "Shine1"
        Bloom5.Value = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Bloom6.Name = "Shine2A"
        Bloom6.Value = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Bloom7.Name = "Shine2B"
        Bloom7.Value = System.Drawing.Color.Transparent
        Bloom8.Name = "Shine3"
        Bloom8.Value = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Bloom9.Name = "TextShade"
        Bloom9.Value = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Bloom10.Name = "Text"
        Bloom10.Value = System.Drawing.Color.White
        Bloom11.Name = "Glow"
        Bloom11.Value = System.Drawing.Color.FromArgb(CType(CType(10, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Bloom12.Name = "Border"
        Bloom12.Value = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer))
        Bloom13.Name = "Corners"
        Bloom13.Value = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer))
        Me.ButtonDark2.Colors = New Shop_Service.Bloom() {Bloom1, Bloom2, Bloom3, Bloom4, Bloom5, Bloom6, Bloom7, Bloom8, Bloom9, Bloom10, Bloom11, Bloom12, Bloom13}
        Me.ButtonDark2.Customization = "MjIy/0FBQf9BQUH/MjIy/////x7///8e////AP///xQAAAAy/////////woZGRn/GRkZ/w=="
        Me.ButtonDark2.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.ButtonDark2.Image = Nothing
        Me.ButtonDark2.Location = New System.Drawing.Point(867, 26)
        Me.ButtonDark2.Name = "ButtonDark2"
        Me.ButtonDark2.NoRounding = False
        Me.ButtonDark2.Size = New System.Drawing.Size(75, 26)
        Me.ButtonDark2.TabIndex = 66
        Me.ButtonDark2.Text = "รีเฟรช"
        Me.ButtonDark2.Transparent = True
        '
        'ButtonDark1
        '
        Me.ButtonDark1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonDark1.BackColor = System.Drawing.Color.Transparent
        Bloom14.Name = "DownGradient1"
        Bloom14.Value = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer))
        Bloom15.Name = "DownGradient2"
        Bloom15.Value = System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(65, Byte), Integer))
        Bloom16.Name = "NoneGradient1"
        Bloom16.Value = System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(65, Byte), Integer))
        Bloom17.Name = "NoneGradient2"
        Bloom17.Value = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer))
        Bloom18.Name = "Shine1"
        Bloom18.Value = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Bloom19.Name = "Shine2A"
        Bloom19.Value = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Bloom20.Name = "Shine2B"
        Bloom20.Value = System.Drawing.Color.Transparent
        Bloom21.Name = "Shine3"
        Bloom21.Value = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Bloom22.Name = "TextShade"
        Bloom22.Value = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Bloom23.Name = "Text"
        Bloom23.Value = System.Drawing.Color.White
        Bloom24.Name = "Glow"
        Bloom24.Value = System.Drawing.Color.FromArgb(CType(CType(10, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Bloom25.Name = "Border"
        Bloom25.Value = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer))
        Bloom26.Name = "Corners"
        Bloom26.Value = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer))
        Me.ButtonDark1.Colors = New Shop_Service.Bloom() {Bloom14, Bloom15, Bloom16, Bloom17, Bloom18, Bloom19, Bloom20, Bloom21, Bloom22, Bloom23, Bloom24, Bloom25, Bloom26}
        Me.ButtonDark1.Customization = "MjIy/0FBQf9BQUH/MjIy/////x7///8e////AP///xQAAAAy/////////woZGRn/GRkZ/w=="
        Me.ButtonDark1.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.ButtonDark1.Image = Nothing
        Me.ButtonDark1.Location = New System.Drawing.Point(786, 26)
        Me.ButtonDark1.Name = "ButtonDark1"
        Me.ButtonDark1.NoRounding = False
        Me.ButtonDark1.Size = New System.Drawing.Size(75, 26)
        Me.ButtonDark1.TabIndex = 64
        Me.ButtonDark1.Text = "ลบ"
        Me.ButtonDark1.Transparent = True
        '
        'ButtonRed1
        '
        Me.ButtonRed1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRed1.BackColor = System.Drawing.Color.Transparent
        Me.ButtonRed1.Colors = New Shop_Service.Bloom(-1) {}
        Me.ButtonRed1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ButtonRed1.Customization = ""
        Me.ButtonRed1.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.ButtonRed1.Image = Nothing
        Me.ButtonRed1.Location = New System.Drawing.Point(708, 26)
        Me.ButtonRed1.Name = "ButtonRed1"
        Me.ButtonRed1.NoRounding = False
        Me.ButtonRed1.Size = New System.Drawing.Size(72, 26)
        Me.ButtonRed1.TabIndex = 62
        Me.ButtonRed1.Tag = "Red"
        Me.ButtonRed1.Text = "ค้นหา"
        Me.ButtonRed1.Transparent = True
        Me.ButtonRed1.twNoRounding = False
        '
        'Pico
        '
        Me.Pico.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer))
        Me.Pico.BackgroundImage = Global.Shop_Service.My.Resources.Resources.Profile1
        Me.Pico.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Pico.Location = New System.Drawing.Point(12, 12)
        Me.Pico.Name = "Pico"
        Me.Pico.Size = New System.Drawing.Size(40, 40)
        Me.Pico.TabIndex = 61
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(954, 528)
        Me.Controls.Add(Me.ButtonBlue1)
        Me.Controls.Add(Me.Fullname)
        Me.Controls.Add(Me.orderID)
        Me.Controls.Add(Me.pid)
        Me.Controls.Add(Me.ButtonDark2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonDark1)
        Me.Controls.Add(Me.Search)
        Me.Controls.Add(Me.ButtonRed1)
        Me.Controls.Add(Me.Pico)
        Me.Controls.Add(Me.dgvData)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Service"
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonDark2 As Shop_Service.ButtonDark
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonDark1 As Shop_Service.ButtonDark
    Friend WithEvents Search As System.Windows.Forms.TextBox
    Friend WithEvents ButtonRed1 As Shop_Service.ButtonRed
    Friend WithEvents Pico As System.Windows.Forms.Panel
    Friend WithEvents dgvData As System.Windows.Forms.DataGridView
    Friend WithEvents pid As System.Windows.Forms.Label
    Friend WithEvents orderID As System.Windows.Forms.Label
    Friend WithEvents Fullname As System.Windows.Forms.Label
    Friend WithEvents ButtonBlue1 As Shop_Service.ButtonBlue

End Class
