Imports MySql.Data.MySqlClient
Imports System.Net
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.CompilerServices

Public Class Form1
    Dim x, y As Integer
    Dim newpoint As New Point

    Private Sub FrmPanel_MouseDown(sender As Object, e As MouseEventArgs)
        x = Control.MousePosition.X - Me.Location.X
        y = Control.MousePosition.Y - Me.Location.Y
    End Sub
    Sub Notify()
        Try
            Cursor.Current = Cursors.WaitCursor
            System.Net.ServicePointManager.Expect100Continue = False
            Dim Request = DirectCast(WebRequest.Create("https://notify-api.line.me/api/notify"), HttpWebRequest)
            '// Message to Line
            Dim LineMessage = String.Format("message={0}",
                                            "------------------------" & vbCrLf &
                                            "  MEMO SERVICE FOOD    " & vbCrLf &
                                            "------------------------" & vbCrLf &
                                            " PID - " + pid.Text & vbCrLf &
                                            " ใบเสร็จ - " + orderID.Text & vbCrLf &
                                            " ผู้ใช้งาน - " + Fullname.Text & vbCrLf &
                                            " เวลา : " & FormatDateTime(Now(), DateFormat.GeneralDate) & vbCrLf &
                                            "")

            Dim MyData = Encoding.UTF8.GetBytes(LineMessage)
            '//
            Request.Method = "POST"
            '// Initialize
            With Request
                .ContentType = "application/x-www-form-urlencoded"
                .ContentLength = MyData.Length
                '// Change your Token and don't cut "Bearer".
                .Headers.Add("Authorization", "Bearer Idi8v57JWEUaayv14lWYnSxFRKKrJcZ8r7YQFYNagdG")
                .AllowWriteStreamBuffering = True
                .KeepAlive = False
                .Credentials = CredentialCache.DefaultCredentials
            End With
            '//
            Using Stream = Request.GetRequestStream()
                Stream.Write(MyData, 0, MyData.Length)
            End Using
            Dim response = DirectCast(Request.GetResponse(), HttpWebResponse)
            Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub
    Private Sub FrmPanel_MouseMove(sender As Object, e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= x
            newpoint.Y -= y
            Me.Location = newpoint
            Application.DoEvents()
        End If
    End Sub
    Private Sub SyncInfo()
        If Search.Text = "" Then
            Dim adpt As New MySqlDataAdapter
            'Dim sqlqry = "SELECT id, orderID, productID, title, price, quantity, fullname, phone, email, address, service, times FROM bill ORDERBY id DESC;"
            Dim sqlqry = "SELECT DISTINCT id, orderID, productID, title, price, quantity, fullname, phone, email, address, service, times FROM bill ORDER BY id;"
            Cmd.Connection = Conn
            Cmd.CommandText = sqlqry
            adpt.SelectCommand = Cmd
            Dim dt As New DataTable
            adpt.Fill(dt)
            Me.dgvData.DataSource = dt
        Else
            Dim adpt As New MySqlDataAdapter
            Dim sqlqry = "SELECT DISTINCT id, orderID, productID, title, price, quantity, fullname, phone, email, address, service, times FROM bill WHERE productID='" & Me.Search.Text & "' ORDER BY id;"
            'Dim sqlqry = "SELECT * FROM bill WHERE productID='" & Me.Search.Text & "'"
            Cmd.Connection = Conn
            Cmd.CommandText = sqlqry
            adpt.SelectCommand = Cmd
            Dim dt As New DataTable
            adpt.Fill(dt)
            Me.dgvData.DataSource = dt
        End If
    End Sub
    Private Sub Delete()
        Dim adpt As New MySqlDataAdapter
        Dim sqlqry = "DELETE FROM bill WHERE productID='" & Me.Search.Text & "'"
        Cmd.Connection = Conn
        Cmd.CommandText = sqlqry
        adpt.SelectCommand = Cmd
        Dim dt As New DataTable
        adpt.Fill(dt)
        Me.dgvData.DataSource = dt
        Conn.Close()
        Search.Text = ""
        SyncInfo()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OpenConnection()
        SyncInfo()
    End Sub

    Private Sub ButtonRed1_Click(sender As Object, e As EventArgs)
        SyncInfo()
    End Sub
    Private Sub ButtonRed1_Click_1(sender As Object, e As EventArgs) Handles ButtonRed1.Click
        SyncInfo()
    End Sub

    Private Sub ButtonDark2_Click_1(sender As Object, e As EventArgs) Handles ButtonDark2.Click
        Search.Text = ""
        SyncInfo()
    End Sub

    Private Sub ButtonDark1_Click(sender As Object, e As EventArgs) Handles ButtonDark1.Click
        Delete()
    End Sub

    Private Sub dgvData_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvData.CellClick
        Dim index As Integer
        index = e.RowIndex
        Dim selectedRow As DataGridViewRow
        selectedRow = dgvData.Rows(index)
        Search.Text = selectedRow.Cells(2).Value.ToString()
        pid.Text = selectedRow.Cells(2).Value.ToString()
        orderID.Text = selectedRow.Cells(1).Value.ToString()
        Fullname.Text = selectedRow.Cells(6).Value.ToString()
    End Sub

    Private Sub ButtonBlue1_Click(sender As Object, e As EventArgs) Handles ButtonBlue1.Click
        Notify()
    End Sub
End Class
