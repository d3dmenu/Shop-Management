Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Public Class history
    Dim x, y As Integer
    Dim newpoint As New Point
    Public _Shadow As Dropshadow
    Private Sub LoadData()
        Dim stringCmd As String = "SELECT orderID FROM bill WHERE fullname = '" & My.Settings.fullname & "'"
        Dim myCmd As MySqlCommand
        myCmd = New MySqlCommand(stringCmd, Conn)
        Dim myReader As MySqlDataReader
        myReader = myCmd.ExecuteReader()

        'Reset your List box here.
        S132655984478558.Items.Clear()

        While (myReader.Read())
            'Add the items from db one by one into the list box.
            S132655984478558.Items.Add(myReader.GetString(0))
        End While

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
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        DashBoard.CloseBlur()
        Blur.Close()
        DashBoard.Activate()
        Me.Close()
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

    Private Sub history_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    Private Sub S132655984478558_DrawItem(sender As Object, e As DrawItemEventArgs) Handles S132655984478558.DrawItem
        On Error Resume Next
        e.DrawBackground()

        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.DimGray, e.Bounds)
        End If
        Using b As New SolidBrush(e.ForeColor)
            e.Graphics.DrawString(S132655984478558.GetItemText(S132655984478558.Items(e.Index)), e.Font, b, e.Bounds)
        End Using
        e.DrawFocusRectangle()
    End Sub
End Class