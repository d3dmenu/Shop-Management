Imports System.Security.Cryptography
Imports System.IO
Imports MySql.Data.MySqlClient

Module Con_Add
    Public Conn As New MySqlConnection
    Public ConTemp As New MySqlConnection
    Public Cmd As New MySqlCommand
    Public MsgSubject As String = String.Empty

    Sub ConTempOpen()
        ConTemp.ConnectionString = "server=" & My.Settings.Host & "; user id=" & My.Settings.HUser & "; password=" & My.Settings.HPass & "; database= " & My.Settings.HName & "; charset=" & My.Settings.Charset
        Try
            ConTemp.Open()
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & "ไม่สามารถเชื่อมต่อระบบฐานข้อมูลได้ กรุณาลองใหม่ภายหลัง", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
            Application.Exit()
        End Try
    End Sub

    Sub OpenConnection()
        Conn.ConnectionString = "server=" & My.Settings.Host & "; user id=" & My.Settings.HUser & "; password=" & My.Settings.HPass & "; database= " & My.Settings.HName & "; charset=" & My.Settings.Charset
        Try
            Conn.Open()
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & "ไม่สามารถเชื่อมต่อระบบฐานข้อมูลได้ กรุณาลองใหม่ภายหลัง", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
            Application.Exit()
        End Try
    End Sub
End Module
