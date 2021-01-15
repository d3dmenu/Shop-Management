Public Class Form1
    Dim x, y As Integer
    Dim newpoint As New Point
    Private Sub Config()
        PBtn2.BackgroundImage = My.Resources.Signupoff
        PStart.BackgroundImage = My.Resources.StartOff
        PBtn1.BackgroundImage = My.Resources.Off
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

    Private Sub PBtn1_MouseMove(sender As Object, e As MouseEventArgs) Handles PBtn1.MouseMove
        PBtn1.BackgroundImage = My.Resources._On
        Label4.ForeColor = Color.DarkGray
    End Sub

    Private Sub PBtn2_MouseMove(sender As Object, e As MouseEventArgs) Handles PBtn2.MouseMove
        PBtn2.BackgroundImage = My.Resources.Signupon
        Label5.ForeColor = Color.DarkGray
    End Sub

    Private Sub PStart_MouseMove(sender As Object, e As MouseEventArgs) Handles PStart.MouseMove
        PStart.BackgroundImage = My.Resources.StartOn
        Label3.ForeColor = Color.DarkGray
    End Sub
End Class
