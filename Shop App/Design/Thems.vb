Imports System, System.Collections
Imports System.Drawing, System.Drawing.Drawing2D
Imports System.ComponentModel, System.Windows.Forms
Imports System.IO, System.Collections.Generic


'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 08/02/2011
'Changed: 09/23/2011
'Version: 1.5.2
'------------------
Module Drawing

    Public Function RoundRect(ByVal rect As Rectangle, ByVal slope As Integer) As GraphicsPath
        Dim gp As GraphicsPath = New GraphicsPath()
        Dim arcWidth As Integer = slope * 2
        gp.AddArc(New Rectangle(rect.X, rect.Y, arcWidth, arcWidth), -180, 90)
        gp.AddArc(New Rectangle(rect.Width - arcWidth + rect.X, rect.Y, arcWidth, arcWidth), -90, 90)
        gp.AddArc(New Rectangle(rect.Width - arcWidth + rect.X, rect.Height - arcWidth + rect.Y, arcWidth, arcWidth), 0, 90)
        gp.AddArc(New Rectangle(rect.X, rect.Height - arcWidth + rect.Y, arcWidth, arcWidth), 90, 90)
        gp.CloseAllFigures()
        Return gp
    End Function

End Module
MustInherit Class ThemeContainer152
    Inherits ContainerControl

    Protected G As Graphics

    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)
        _ImageSize = Size.Empty

        MeasureBitmap = New Bitmap(1, 1)
        MeasureGraphics = Graphics.FromImage(MeasureBitmap)

        Font = New Font("Verdana", 8S)

        InvalidateCustimization()
    End Sub

    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
        If Not _LockWidth = 0 Then width = _LockWidth
        If Not _LockHeight = 0 Then height = _LockHeight
        MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

    Private Header As Rectangle
    Protected NotOverridable Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If _Movable AndAlso Not _ControlMode Then Header = New Rectangle(7, 7, Width - 14, _MoveHeight - 7)
        Invalidate()
        MyBase.OnSizeChanged(e)
    End Sub

    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return
        G = e.Graphics
        PaintHook()
    End Sub

    Protected NotOverridable Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        InvalidateCustimization()
        ColorHook()

        If Not _LockWidth = 0 Then Width = _LockWidth
        If Not _LockHeight = 0 Then Height = _LockHeight
        If Not _ControlMode Then MyBase.Dock = DockStyle.Fill

        MyBase.OnHandleCreated(e)
    End Sub

    Protected NotOverridable Overrides Sub OnParentChanged(ByVal e As EventArgs)
        MyBase.OnParentChanged(e)

        If Parent Is Nothing Then Return
        _IsParentForm = TypeOf Parent Is Form

        If Not _ControlMode Then
            InitializeMessages()

            If _IsParentForm Then
                ParentForm.FormBorderStyle = _BorderStyle
                ParentForm.TransparencyKey = _TransparencyKey
            End If

            Parent.BackColor = BackColor
        End If

        OnCreation()
    End Sub

    Protected Overridable Sub OnCreation()
    End Sub

#Region " Sizing and Movement "

    Protected State As MouseState
    Private Sub SetState(ByVal current As MouseState)
        State = current
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If Not (_IsParentForm AndAlso ParentForm.WindowState = FormWindowState.Maximized) Then
            If _Sizable AndAlso Not _ControlMode Then InvalidateMouse()
        End If

        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(ByVal e As EventArgs)
        If Enabled Then SetState(MouseState.None) Else SetState(MouseState.Block)
        MyBase.OnEnabledChanged(e)
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        SetState(MouseState.Over)
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        SetState(MouseState.Over)
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        SetState(MouseState.None)

        If GetChildAtPoint(PointToClient(MousePosition)) IsNot Nothing Then
            If _Sizable AndAlso Not _ControlMode Then
                Cursor = Cursors.Default
                Previous = 0
            End If
        End If

        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then SetState(MouseState.Down)

        If Not (_IsParentForm AndAlso ParentForm.WindowState = FormWindowState.Maximized OrElse _ControlMode) Then
            If _Movable AndAlso Header.Contains(e.Location) Then
                Capture = False
                WM_LMBUTTONDOWN = True
                DefWndProc(Messages(0))
            ElseIf _Sizable AndAlso Not Previous = 0 Then
                Capture = False
                WM_LMBUTTONDOWN = True
                DefWndProc(Messages(Previous))
            End If
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Private WM_LMBUTTONDOWN As Boolean
    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)

        If WM_LMBUTTONDOWN AndAlso m.Msg = 513 Then
            WM_LMBUTTONDOWN = False

            SetState(MouseState.Over)
            If Not _SmartBounds Then Return

            If IsParentMdi Then
                CorrectBounds(New Rectangle(Point.Empty, Parent.Parent.Size))
            Else
                CorrectBounds(Screen.FromControl(Parent).WorkingArea)
            End If
        End If
    End Sub

    Private GetIndexPoint As Point
    Private B1, B2, B3, B4 As Boolean
    Private Function GetIndex() As Integer
        GetIndexPoint = PointToClient(MousePosition)
        B1 = GetIndexPoint.X < 7
        B2 = GetIndexPoint.X > Width - 7
        B3 = GetIndexPoint.Y < 7
        B4 = GetIndexPoint.Y > Height - 7

        If B1 AndAlso B3 Then Return 4
        If B1 AndAlso B4 Then Return 7
        If B2 AndAlso B3 Then Return 5
        If B2 AndAlso B4 Then Return 8
        If B1 Then Return 1
        If B2 Then Return 2
        If B3 Then Return 3
        If B4 Then Return 6
        Return 0
    End Function

    Private Current, Previous As Integer
    Private Sub InvalidateMouse()
        Current = GetIndex()
        If Current = Previous Then Return

        Previous = Current
        Select Case Previous
            Case 0
                Cursor = Cursors.Default
            Case 1, 2
                Cursor = Cursors.SizeWE
            Case 3, 6
                Cursor = Cursors.SizeNS
            Case 4, 8
                Cursor = Cursors.SizeNWSE
            Case 5, 7
                Cursor = Cursors.SizeNESW
        End Select
    End Sub

    Private Messages(8) As Message
    Private Sub InitializeMessages()
        Messages(0) = Message.Create(Parent.Handle, 161, New IntPtr(2), IntPtr.Zero)
        For I As Integer = 1 To 8
            Messages(I) = Message.Create(Parent.Handle, 161, New IntPtr(I + 9), IntPtr.Zero)
        Next
    End Sub

    Private Sub CorrectBounds(ByVal bounds As Rectangle)
        If Parent.Width > bounds.Width Then Parent.Width = bounds.Width
        If Parent.Height > bounds.Height Then Parent.Height = bounds.Height

        Dim X As Integer = Parent.Location.X
        Dim Y As Integer = Parent.Location.Y

        If X < bounds.X Then X = bounds.X
        If Y < bounds.Y Then Y = bounds.Y

        Dim Width As Integer = bounds.X + bounds.Width
        Dim Height As Integer = bounds.Y + bounds.Height

        If X + Parent.Width > Width Then X = Width - Parent.Width
        If Y + Parent.Height > Height Then Y = Height - Parent.Height

        Parent.Location = New Point(X, Y)
    End Sub

#End Region


#Region " Property Overrides "

    Overrides Property Dock As DockStyle
        Get
            Return MyBase.Dock
        End Get
        Set(ByVal value As DockStyle)
            If Not _ControlMode Then Return
            MyBase.Dock = value
        End Set
    End Property

    <Category("Misc")> _
    Overrides Property BackColor() As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            If value = BackColor Then Return
            MyBase.BackColor = value

            If Parent IsNot Nothing Then
                If Not _ControlMode Then Parent.BackColor = value
                ColorHook()
            End If
        End Set
    End Property

    Overrides Property MinimumSize As Size
        Get
            Return MyBase.MinimumSize
        End Get
        Set(ByVal value As Size)
            MyBase.MinimumSize = value
            If Parent IsNot Nothing Then Parent.MinimumSize = value
        End Set
    End Property

    Overrides Property MaximumSize As Size
        Get
            Return MyBase.MaximumSize
        End Get
        Set(ByVal value As Size)
            MyBase.MaximumSize = value
            If Parent IsNot Nothing Then Parent.MaximumSize = value
        End Set
    End Property

    Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property

    Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            Invalidate()
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property ForeColor() As Color
        Get
            Return Color.Empty
        End Get
        Set(ByVal value As Color)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
        Set(ByVal value As Image)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImageLayout() As ImageLayout
        Get
            Return ImageLayout.None
        End Get
        Set(ByVal value As ImageLayout)
        End Set
    End Property

#End Region

#Region " Properties "

    Private _SmartBounds As Boolean = True
    Property SmartBounds() As Boolean
        Get
            Return _SmartBounds
        End Get
        Set(ByVal value As Boolean)
            _SmartBounds = value
        End Set
    End Property

    Private _Movable As Boolean = True
    Property Movable() As Boolean
        Get
            Return _Movable
        End Get
        Set(ByVal value As Boolean)
            _Movable = value
        End Set
    End Property

    Private _Sizable As Boolean = True
    Property Sizable() As Boolean
        Get
            Return _Sizable
        End Get
        Set(ByVal value As Boolean)
            _Sizable = value
        End Set
    End Property

    Private _TransparencyKey As Color
    Property TransparencyKey() As Color
        Get
            If _IsParentForm AndAlso Not _ControlMode Then Return ParentForm.TransparencyKey Else Return _TransparencyKey
        End Get
        Set(ByVal value As Color)
            If value = _TransparencyKey Then Return
            _TransparencyKey = value

            If _IsParentForm AndAlso Not _ControlMode Then
                ParentForm.TransparencyKey = value
                ColorHook()
            End If
        End Set
    End Property

    Private _BorderStyle As FormBorderStyle
    Property BorderStyle() As FormBorderStyle
        Get
            If _IsParentForm AndAlso Not _ControlMode Then Return ParentForm.FormBorderStyle Else Return _BorderStyle
        End Get
        Set(ByVal value As FormBorderStyle)
            _BorderStyle = value

            If _IsParentForm AndAlso Not _ControlMode Then
                ParentForm.FormBorderStyle = value

                If Not value = FormBorderStyle.None Then
                    Movable = False
                    Sizable = False
                End If
            End If
        End Set
    End Property

    Private _NoRounding As Boolean
    Property NoRounding() As Boolean
        Get
            Return _NoRounding
        End Get
        Set(ByVal v As Boolean)
            _NoRounding = v
            Invalidate()
        End Set
    End Property

    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            If value Is Nothing Then _ImageSize = Size.Empty Else _ImageSize = value.Size

            _Image = value
            Invalidate()
        End Set
    End Property

    Private _ImageSize As Size
    Protected ReadOnly Property ImageSize() As Size
        Get
            Return _ImageSize
        End Get
    End Property

    Private _IsParentForm As Boolean
    Protected ReadOnly Property IsParentForm As Boolean
        Get
            Return _IsParentForm
        End Get
    End Property

    Protected ReadOnly Property IsParentMdi As Boolean
        Get
            If Parent Is Nothing Then Return False
            Return Parent.Parent IsNot Nothing
        End Get
    End Property

    Private _LockWidth As Integer
    Protected Property LockWidth() As Integer
        Get
            Return _LockWidth
        End Get
        Set(ByVal value As Integer)
            _LockWidth = value
            If Not LockWidth = 0 AndAlso IsHandleCreated Then Width = LockWidth
        End Set
    End Property

    Private _LockHeight As Integer
    Protected Property LockHeight() As Integer
        Get
            Return _LockHeight
        End Get
        Set(ByVal value As Integer)
            _LockHeight = value
            If Not LockHeight = 0 AndAlso IsHandleCreated Then Height = LockHeight
        End Set
    End Property

    Private _MoveHeight As Integer = 24
    Protected Property MoveHeight() As Integer
        Get
            Return _MoveHeight
        End Get
        Set(ByVal v As Integer)
            If v < 8 Then Return
            Header = New Rectangle(7, 7, Width - 14, v - 7)
            _MoveHeight = v
            Invalidate()
        End Set
    End Property

    Private _ControlMode As Boolean
    Protected Property ControlMode() As Boolean
        Get
            Return _ControlMode
        End Get
        Set(ByVal v As Boolean)
            _ControlMode = v
        End Set
    End Property

    Private Items As New Dictionary(Of String, Color)
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Property Colors() As Bloom()
        Get
            Dim T As New List(Of Bloom)
            Dim E As Dictionary(Of String, Color).Enumerator = Items.GetEnumerator

            While E.MoveNext
                T.Add(New Bloom(E.Current.Key, E.Current.Value))
            End While

            Return T.ToArray
        End Get
        Set(ByVal value As Bloom())
            For Each B As Bloom In value
                If Items.ContainsKey(B.Name) Then Items(B.Name) = B.Value
            Next

            InvalidateCustimization()
            ColorHook()
            Invalidate()
        End Set
    End Property

    Private _Customization As String
    Property Customization() As String
        Get
            Return _Customization
        End Get
        Set(ByVal value As String)
            If value = _Customization Then Return

            Dim Data As Byte()
            Dim Items As Bloom() = Colors

            Try
                Data = Convert.FromBase64String(value)
                For I As Integer = 0 To Items.Length - 1
                    Items(I).Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4))
                Next
            Catch
                Return
            End Try

            _Customization = value

            Colors = Items
            ColorHook()
            Invalidate()
        End Set
    End Property

#End Region

#Region " Property Helpers "

    Protected Function GetColor(ByVal name As String) As Color
        Return Items(name)
    End Function

    Protected Sub SetColor(ByVal name As String, ByVal value As Color)
        If Items.ContainsKey(name) Then Items(name) = value Else Items.Add(name, value)
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(a, r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal value As Color)
        SetColor(name, Color.FromArgb(a, value))
    End Sub

    Private Sub InvalidateCustimization()
        Dim M As New MemoryStream(Items.Count * 4)

        For Each B As Bloom In Colors
            M.Write(BitConverter.GetBytes(B.Value.ToArgb), 0, 4)
        Next

        M.Close()
        _Customization = Convert.ToBase64String(M.ToArray)
    End Sub

#End Region


#Region " User Hooks "

    Protected MustOverride Sub ColorHook()
    Protected MustOverride Sub PaintHook()

#End Region


#Region " Center Overloads "

    Private CenterReturn As Point

    Protected Function Center(ByVal r1 As Rectangle, ByVal s1 As Size) As Point
        CenterReturn = New Point((r1.Width \ 2 - s1.Width \ 2) + r1.X, (r1.Height \ 2 - s1.Height \ 2) + r1.Y)
        Return CenterReturn
    End Function
    Protected Function Center(ByVal r1 As Rectangle, ByVal r2 As Rectangle) As Point
        Return Center(r1, r2.Size)
    End Function

    Protected Function Center(ByVal w1 As Integer, ByVal h1 As Integer, ByVal w2 As Integer, ByVal h2 As Integer) As Point
        CenterReturn = New Point(w1 \ 2 - w2 \ 2, h1 \ 2 - h2 \ 2)
        Return CenterReturn
    End Function

    Protected Function Center(ByVal s1 As Size, ByVal s2 As Size) As Point
        Return Center(s1.Width, s1.Height, s2.Width, s2.Height)
    End Function

    Protected Function Center(ByVal r1 As Rectangle) As Point
        Return Center(ClientRectangle.Width, ClientRectangle.Height, r1.Width, r1.Height)
    End Function
    Protected Function Center(ByVal s1 As Size) As Point
        Return Center(Width, Height, s1.Width, s1.Height)
    End Function
    Protected Function Center(ByVal w1 As Integer, ByVal h1 As Integer) As Point
        Return Center(Width, Height, w1, h1)
    End Function

#End Region

#Region " Measure Overloads "

    Private MeasureBitmap As Bitmap
    Private MeasureGraphics As Graphics

    Protected Function Measure(ByVal text As String) As Size
        Return MeasureGraphics.MeasureString(text, Font, Width).ToSize
    End Function
    Protected Function Measure() As Size
        Return MeasureGraphics.MeasureString(Text, Font).ToSize
    End Function

#End Region

#Region " DrawCorners Overloads "

    Private DrawCornersBrush As SolidBrush

    Protected Sub DrawCorners(ByVal c1 As Color)
        DrawCorners(c1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        If _NoRounding Then Return
        DrawCornersBrush = New SolidBrush(c1)
        G.FillRectangle(DrawCornersBrush, x, y, 1, 1)
        G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1)
        G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1)
        G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1)
    End Sub

#End Region

#Region " DrawBorders Overloads "

    'TODO: Remove triple overload?

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal offset As Integer)
        DrawBorders(p1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle, ByVal offset As Integer)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset)
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        G.DrawRectangle(p1, x, y, width - 1, height - 1)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen)
        DrawBorders(p1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height)
    End Sub

#End Region

#Region " DrawText Overloads "

    'TODO: Remove triple overloads?

    Private DrawTextPoint As Point
    Private DrawTextSize As Size

    Protected Sub DrawText(ByVal b1 As Brush, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawText(b1, Text, a, x, y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal p1 As Point)
        DrawText(b1, Text, p1.X, p1.Y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal x As Integer, ByVal y As Integer)
        DrawText(b1, Text, x, y)
    End Sub

    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If text.Length = 0 Then Return
        DrawTextSize = Measure(text)

        If _ControlMode Then
            DrawTextPoint = Center(DrawTextSize)
        Else
            DrawTextPoint = New Point(Width \ 2 - DrawTextSize.Width \ 2, MoveHeight \ 2 - DrawTextSize.Height \ 2)
        End If

        Select Case a
            Case HorizontalAlignment.Left
                DrawText(b1, text, x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Center
                DrawText(b1, text, DrawTextPoint.X + x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Right
                DrawText(b1, text, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y)
        End Select
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal p1 As Point)
        DrawText(b1, text, p1.X, p1.Y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal x As Integer, ByVal y As Integer)
        If text.Length = 0 Then Return
        G.DrawString(text, Font, b1, x, y)
    End Sub

#End Region

#Region " DrawImage Overloads "

    'TODO: Remove triple overloads?

    Private DrawImagePoint As Point

    Protected Sub DrawImage(ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, a, x, y)
    End Sub
    Protected Sub DrawImage(ByVal p1 As Point)
        DrawImage(_Image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, x, y)
    End Sub

    Protected Sub DrawImage(ByVal image As Image, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return

        If _ControlMode Then
            DrawImagePoint = Center(image.Size)
        Else
            DrawImagePoint = New Point(Width \ 2 - image.Width \ 2, MoveHeight \ 2 - image.Height \ 2)
        End If

        Select Case a
            Case HorizontalAlignment.Left
                DrawImage(image, x, DrawImagePoint.Y + y)
            Case HorizontalAlignment.Center
                DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y)
            Case HorizontalAlignment.Right
                DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y)
        End Select
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal p1 As Point)
        DrawImage(image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        G.DrawImage(image, x, y, image.Width, image.Height)
    End Sub

#End Region

#Region " DrawGradient Overloads "

    'TODO: Remove triple overload?

    Private DrawGradientBrush As LinearGradientBrush
    Private DrawGradientRectangle As Rectangle

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradient(blend, x, y, width, height, 90S)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradient(c1, c2, x, y, width, height, 90S)
    End Sub

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(blend, DrawGradientRectangle, angle)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(c1, c2, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, Color.Empty, Color.Empty, angle)
        DrawGradientBrush.InterpolationColors = blend
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, c1, c2, angle)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub

#End Region

End Class

MustInherit Class ThemeControl152
    Inherits Control

    Protected G As Graphics, B As Bitmap

    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)

        _ImageSize = Size.Empty

        MeasureBitmap = New Bitmap(1, 1)
        MeasureGraphics = Graphics.FromImage(MeasureBitmap)

        Font = New Font("Verdana", 8S)

        InvalidateCustimization()
    End Sub

    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
        If Not _LockWidth = 0 Then width = _LockWidth
        If Not _LockHeight = 0 Then height = _LockHeight
        MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

    Protected NotOverridable Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If _Transparent AndAlso Not (Width = 0 OrElse Height = 0) Then
            B = New Bitmap(Width, Height)
            G = Graphics.FromImage(B)
        End If

        Invalidate()
        MyBase.OnSizeChanged(e)
    End Sub

    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return

        If _Transparent Then
            PaintHook()
            e.Graphics.DrawImage(B, 0, 0)
        Else
            G = e.Graphics
            PaintHook()
        End If
    End Sub

    Protected NotOverridable Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        InvalidateCustimization()
        ColorHook()

        If Not _LockWidth = 0 Then Width = _LockWidth
        If Not _LockHeight = 0 Then Height = _LockHeight

        Transparent = _Transparent
        If _BackColorU AndAlso _Transparent Then BackColor = Color.Transparent

        MyBase.OnHandleCreated(e)
    End Sub

    Protected NotOverridable Overrides Sub OnParentChanged(ByVal e As EventArgs)
        If Parent IsNot Nothing Then OnCreation()
        MyBase.OnParentChanged(e)
    End Sub

    Protected Overridable Sub OnCreation()
    End Sub

#Region " State Handling "

    Private InPosition As Boolean
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        InPosition = True
        SetState(MouseState.Over)
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        If InPosition Then SetState(MouseState.Over)
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then SetState(MouseState.Down)
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        InPosition = False
        SetState(MouseState.None)
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(ByVal e As EventArgs)
        If Enabled Then SetState(MouseState.None) Else SetState(MouseState.Block)
        MyBase.OnEnabledChanged(e)
    End Sub

    Protected State As MouseState
    Private Sub SetState(ByVal current As MouseState)
        State = current
        Invalidate()
    End Sub

#End Region


#Region " Property Overrides "

    Private _BackColorU As Boolean
    <Category("Misc")> _
    Overrides Property BackColor() As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            If Not IsHandleCreated AndAlso value = Color.Transparent Then
                _BackColorU = True
                Return
            End If

            MyBase.BackColor = value
            If Parent IsNot Nothing Then ColorHook()
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property ForeColor() As Color
        Get
            Return Color.Empty
        End Get
        Set(ByVal value As Color)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
        Set(ByVal value As Image)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImageLayout() As ImageLayout
        Get
            Return ImageLayout.None
        End Get
        Set(ByVal value As ImageLayout)
        End Set
    End Property

    Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property

    Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Properties "

    Private _NoRounding As Boolean
    Property NoRounding() As Boolean
        Get
            Return _NoRounding
        End Get
        Set(ByVal v As Boolean)
            _NoRounding = v
            Invalidate()
        End Set
    End Property

    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            If value Is Nothing Then
                _ImageSize = Size.Empty
            Else
                _ImageSize = value.Size
            End If

            _Image = value
            Invalidate()
        End Set
    End Property

    Private _ImageSize As Size
    Protected ReadOnly Property ImageSize() As Size
        Get
            Return _ImageSize
        End Get
    End Property

    Private _LockWidth As Integer
    Protected Property LockWidth() As Integer
        Get
            Return _LockWidth
        End Get
        Set(ByVal value As Integer)
            _LockWidth = value
            If Not LockWidth = 0 AndAlso IsHandleCreated Then Width = LockWidth
        End Set
    End Property

    Private _LockHeight As Integer
    Protected Property LockHeight() As Integer
        Get
            Return _LockHeight
        End Get
        Set(ByVal value As Integer)
            _LockHeight = value
            If Not LockHeight = 0 AndAlso IsHandleCreated Then Height = LockHeight
        End Set
    End Property

    Private _Transparent As Boolean
    Property Transparent() As Boolean
        Get
            Return _Transparent
        End Get
        Set(ByVal value As Boolean)
            _Transparent = value
            If Not IsHandleCreated Then Return

            If Not value AndAlso Not BackColor.A = 255 Then
                Throw New Exception("Unable to change value to false while a transparent BackColor is in use.")
            End If

            SetStyle(ControlStyles.Opaque, Not value)
            SetStyle(ControlStyles.SupportsTransparentBackColor, value)

            If value Then InvalidateBitmap() Else B = Nothing
            Invalidate()
        End Set
    End Property

    Private Items As New Dictionary(Of String, Color)
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Property Colors() As Bloom()
        Get
            Dim T As New List(Of Bloom)
            Dim E As Dictionary(Of String, Color).Enumerator = Items.GetEnumerator

            While E.MoveNext
                T.Add(New Bloom(E.Current.Key, E.Current.Value))
            End While

            Return T.ToArray
        End Get
        Set(ByVal value As Bloom())
            For Each B As Bloom In value
                If Items.ContainsKey(B.Name) Then Items(B.Name) = B.Value
            Next

            InvalidateCustimization()
            ColorHook()
            Invalidate()
        End Set
    End Property

    Private _Customization As String
    Property Customization() As String
        Get
            Return _Customization
        End Get
        Set(ByVal value As String)
            If value = _Customization Then Return

            Dim Data As Byte()
            Dim Items As Bloom() = Colors

            Try
                Data = Convert.FromBase64String(value)
                For I As Integer = 0 To Items.Length - 1
                    Items(I).Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4))
                Next
            Catch
                Return
            End Try

            _Customization = value

            Colors = Items
            ColorHook()
            Invalidate()
        End Set
    End Property

#End Region

#Region " Property Helpers "

    Private Sub InvalidateBitmap()
        If Width = 0 OrElse Height = 0 Then Return
        B = New Bitmap(Width, Height)
        G = Graphics.FromImage(B)
    End Sub

    Protected Function GetColor(ByVal name As String) As Color
        Return Items(name)
    End Function

    Protected Sub SetColor(ByVal name As String, ByVal value As Color)
        If Items.ContainsKey(name) Then Items(name) = value Else Items.Add(name, value)
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(a, r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal value As Color)
        SetColor(name, Color.FromArgb(a, value))
    End Sub

    Private Sub InvalidateCustimization()
        Dim M As New MemoryStream(Items.Count * 4)

        For Each B As Bloom In Colors
            M.Write(BitConverter.GetBytes(B.Value.ToArgb), 0, 4)
        Next

        M.Close()
        _Customization = Convert.ToBase64String(M.ToArray)
    End Sub

#End Region


#Region " User Hooks "

    Protected MustOverride Sub ColorHook()
    Protected MustOverride Sub PaintHook()

#End Region


#Region " Center Overloads "

    Private CenterReturn As Point

    Protected Function Center(ByVal r1 As Rectangle, ByVal s1 As Size) As Point
        CenterReturn = New Point((r1.Width \ 2 - s1.Width \ 2) + r1.X, (r1.Height \ 2 - s1.Height \ 2) + r1.Y)
        Return CenterReturn
    End Function
    Protected Function Center(ByVal r1 As Rectangle, ByVal r2 As Rectangle) As Point
        Return Center(r1, r2.Size)
    End Function

    Protected Function Center(ByVal w1 As Integer, ByVal h1 As Integer, ByVal w2 As Integer, ByVal h2 As Integer) As Point
        CenterReturn = New Point(w1 \ 2 - w2 \ 2, h1 \ 2 - h2 \ 2)
        Return CenterReturn
    End Function

    Protected Function Center(ByVal s1 As Size, ByVal s2 As Size) As Point
        Return Center(s1.Width, s1.Height, s2.Width, s2.Height)
    End Function

    Protected Function Center(ByVal r1 As Rectangle) As Point
        Return Center(ClientRectangle.Width, ClientRectangle.Height, r1.Width, r1.Height)
    End Function
    Protected Function Center(ByVal s1 As Size) As Point
        Return Center(Width, Height, s1.Width, s1.Height)
    End Function
    Protected Function Center(ByVal w1 As Integer, ByVal h1 As Integer) As Point
        Return Center(Width, Height, w1, h1)
    End Function

#End Region

#Region " Measure Overloads "

    Private MeasureBitmap As Bitmap
    Private MeasureGraphics As Graphics

    Protected Function Measure(ByVal text As String) As Size
        Return MeasureGraphics.MeasureString(text, Font, Width).ToSize
    End Function
    Protected Function Measure() As Size
        Return MeasureGraphics.MeasureString(Text, Font, Width).ToSize
    End Function

#End Region

#Region " DrawCorners Overloads "

    Private DrawCornersBrush As SolidBrush

    Protected Sub DrawCorners(ByVal c1 As Color)
        DrawCorners(c1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        If _NoRounding Then Return

        If _Transparent Then
            B.SetPixel(x, y, c1)
            B.SetPixel(x + (width - 1), y, c1)
            B.SetPixel(x, y + (height - 1), c1)
            B.SetPixel(x + (width - 1), y + (height - 1), c1)
        Else
            DrawCornersBrush = New SolidBrush(c1)
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1)
        End If
    End Sub

#End Region

#Region " DrawBorders Overloads "

    'TODO: Remove triple overload?

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal offset As Integer)
        DrawBorders(p1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle, ByVal offset As Integer)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset)
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        G.DrawRectangle(p1, x, y, width - 1, height - 1)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen)
        DrawBorders(p1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height)
    End Sub

#End Region

#Region " DrawText Overloads "

    'TODO: Remove triple overloads?

    Private DrawTextPoint As Point
    Private DrawTextSize As Size

    Protected Sub DrawText(ByVal b1 As Brush, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawText(b1, Text, a, x, y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal p1 As Point)
        DrawText(b1, Text, p1.X, p1.Y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal x As Integer, ByVal y As Integer)
        DrawText(b1, Text, x, y)
    End Sub

    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If text.Length = 0 Then Return
        DrawTextSize = Measure(text)
        DrawTextPoint = Center(DrawTextSize)

        Select Case a
            Case HorizontalAlignment.Left
                DrawText(b1, text, x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Center
                DrawText(b1, text, DrawTextPoint.X + x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Right
                DrawText(b1, text, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y)
        End Select
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal p1 As Point)
        DrawText(b1, text, p1.X, p1.Y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal x As Integer, ByVal y As Integer)
        If text.Length = 0 Then Return
        G.DrawString(text, Font, b1, x, y)
    End Sub

#End Region

#Region " DrawImage Overloads "

    'TODO: Remove triple overloads?

    Private DrawImagePoint As Point

    Protected Sub DrawImage(ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, a, x, y)
    End Sub
    Protected Sub DrawImage(ByVal p1 As Point)
        DrawImage(_Image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, x, y)
    End Sub

    Protected Sub DrawImage(ByVal image As Image, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        DrawImagePoint = Center(image.Size)

        Select Case a
            Case HorizontalAlignment.Left
                DrawImage(image, x, DrawImagePoint.Y + y)
            Case HorizontalAlignment.Center
                DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y)
            Case HorizontalAlignment.Right
                DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y)
        End Select
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal p1 As Point)
        DrawImage(image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        G.DrawImage(image, x, y, image.Width, image.Height)
    End Sub

#End Region

#Region " DrawGradient Overloads "

    'TODO: Remove triple overload?

    Private DrawGradientBrush As LinearGradientBrush
    Private DrawGradientRectangle As Rectangle

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradient(blend, x, y, width, height, 90S)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradient(c1, c2, x, y, width, height, 90S)
    End Sub

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(blend, DrawGradientRectangle, angle)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(c1, c2, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, Color.Empty, Color.Empty, angle)
        DrawGradientBrush.InterpolationColors = blend
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, c1, c2, angle)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub

#End Region

End Class

Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum

Class Bloom

    Private _Name As String
    Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _Value As Color
    Property Value() As Color
        Get
            Return _Value
        End Get
        Set(ByVal value As Color)
            _Value = value
        End Set
    End Property

    Sub New()
    End Sub

    Sub New(ByVal name As String, ByVal value As Color)
        _Name = name
        _Value = value
    End Sub
End Class

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 9/23/2011
'Changed: 9/23/2011
'Version: 1.0.0
'Theme Base: 1.5.2
'------------------
MustInherit Class ThemeControl
    Inherits Control

#Region " Initialization "

    Protected G As Graphics
    Protected B As Bitmap
    Public Sub New()
        SetStyle(CType(139270, ControlStyles), True)
        B = New Bitmap(1, 1)
        G = Graphics.FromImage(B)
    End Sub

    Public Sub AllowTransparent()
        SetStyle(ControlStyles.Opaque, False)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property
#End Region

#Region " Mouse Handling "

    Protected Enum State As Byte
        MouseNone = 0
        MouseOver = 1
        MouseDown = 2
    End Enum

    Protected MouseState As State
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        ChangeMouseState(State.MouseNone)
        MyBase.OnMouseLeave(e)
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        ChangeMouseState(State.MouseOver)
        MyBase.OnMouseEnter(e)
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        ChangeMouseState(State.MouseOver)
        MyBase.OnMouseUp(e)
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            ChangeMouseState(State.MouseDown)
        End If
        MyBase.OnMouseDown(e)
    End Sub

    Private Sub ChangeMouseState(e As State)
        MouseState = e
        Invalidate()
    End Sub

#End Region

#Region " Convienence "

    Public MustOverride Sub PaintHook()
    Protected NotOverridable Overrides Sub OnPaint(e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then
            Return
        End If
        PaintHook()
        e.Graphics.DrawImage(B, 0, 0)
    End Sub

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        If Not (Width = 0) AndAlso Not (Height = 0) Then
            B = New Bitmap(Width, Height)
            G = Graphics.FromImage(B)
            Invalidate()
        End If
        MyBase.OnSizeChanged(e)
    End Sub

    Private _NoRounding As Boolean
    Public Property NoRounding() As Boolean
        Get
            Return _NoRounding
        End Get
        Set(value As Boolean)
            _NoRounding = value
            Invalidate()
        End Set
    End Property

    Private _Image As Image
    Public Property Image() As Image
        Get
            Return _Image
        End Get
        Set(value As Image)
            _Image = value
            Invalidate()
        End Set
    End Property
    Public ReadOnly Property ImageWidth() As Integer
        Get
            If _Image Is Nothing Then
                Return 0
            End If
            Return _Image.Width
        End Get
    End Property
    Public ReadOnly Property ImageTop() As Integer
        Get
            If _Image Is Nothing Then
                Return 0
            End If
            Return Height \ 2 - _Image.Height \ 2
        End Get
    End Property

    Private _Size As Size
    Private _Rectangle As Rectangle
    Private _Gradient As LinearGradientBrush

    Private _Brush As SolidBrush
    Protected Sub DrawCorners(c As Color, rect As Rectangle)
        If _NoRounding Then
            Return
        End If

        B.SetPixel(rect.X, rect.Y, c)
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c)
        B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c)
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c)
    End Sub

    Protected Sub DrawBorders(p1 As Pen, p2 As Pen, rect As Rectangle)
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3)
    End Sub

    Protected Sub DrawText(a As HorizontalAlignment, c As Color, x As Integer)
        DrawText(a, c, x, 0)
    End Sub
    Protected Sub DrawText(a As HorizontalAlignment, c As Color, x As Integer, y As Integer)
        If String.IsNullOrEmpty(Text) Then
            Return
        End If
        _Size = G.MeasureString(Text, Font).ToSize()
        _Brush = New SolidBrush(c)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(Text, Font, _Brush, x, Height \ 2 - _Size.Height \ 2 + y)
                Exit Select
            Case HorizontalAlignment.Right
                G.DrawString(Text, Font, _Brush, Width - _Size.Width - x, Height \ 2 - _Size.Height \ 2 + y)
                Exit Select
            Case HorizontalAlignment.Center
                G.DrawString(Text, Font, _Brush, Width \ 2 - _Size.Width \ 2 + x, Height \ 2 - _Size.Height \ 2 + y)
                Exit Select
        End Select
    End Sub

    Protected Sub DrawIcon(a As HorizontalAlignment, x As Integer)
        DrawIcon(a, x, 0)
    End Sub
    Protected Sub DrawIcon(a As HorizontalAlignment, x As Integer, y As Integer)
        If _Image Is Nothing Then
            Return
        End If
        Select Case a
            Case HorizontalAlignment.Left
                G.DrawImage(_Image, x, Height \ 2 - _Image.Height \ 2 + y)
                Exit Select
            Case HorizontalAlignment.Right
                G.DrawImage(_Image, Width - _Image.Width - x, Height \ 2 - _Image.Height \ 2 + y)
                Exit Select
            Case HorizontalAlignment.Center
                G.DrawImage(_Image, Width \ 2 - _Image.Width \ 2, Height \ 2 - _Image.Height \ 2)
                Exit Select
        End Select
    End Sub

    Protected Sub DrawGradient(c1 As Color, c2 As Color, x As Integer, y As Integer, width As Integer, height As Integer, _
        angle As Single)
        _Rectangle = New Rectangle(x, y, width, height)
        _Gradient = New LinearGradientBrush(_Rectangle, c1, c2, angle)
        G.FillRectangle(_Gradient, _Rectangle)
    End Sub
#End Region

End Class
NotInheritable Class Draw
    Private Sub New()
    End Sub
    Public Shared Function RoundRect(Rectangle As Rectangle, Curve As Integer) As GraphicsPath
        Dim P As New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function
End Class
Class StudioTheme
    Inherits ThemeContainer152

    Private Path As GraphicsPath

    Sub New()
        MoveHeight = 30
        BackColor = Color.FromArgb(20, 40, 70)
        TransparencyKey = Color.Fuchsia

        SetColor("Sides", 50, 70, 100)
        SetColor("Gradient1", 65, 85, 115)
        SetColor("Gradient2", 50, 70, 100)
        SetColor("Hatch1", 20, 40, 70)
        SetColor("Hatch2", 40, 60, 90)
        SetColor("Shade1", 15, Color.Black)
        SetColor("Shade2", Color.Transparent)
        SetColor("Border1", 12, 32, 62)
        SetColor("Border2", 20, Color.Black)
        SetColor("Border3", 30, Color.White)
        SetColor("Border4", Color.Black)
        SetColor("Text", Color.White)

        Path = New GraphicsPath
    End Sub

    Private C1, C2, C3, C4, C5 As Color
    Private P1, P2, P3, P4, P5 As Pen
    Private B1 As HatchBrush
    Private B2 As SolidBrush

    Protected Overrides Sub ColorHook()
        P1 = New Pen(TransparencyKey, 3)
        P2 = New Pen(GetColor("Border1"))
        P3 = New Pen(GetColor("Border2"), 2)
        P4 = New Pen(GetColor("Border3"))
        P5 = New Pen(GetColor("Border4"))

        C1 = GetColor("Sides")
        C2 = GetColor("Gradient1")
        C3 = GetColor("Gradient2")
        C4 = GetColor("Shade1")
        C5 = GetColor("Shade2")

        B1 = New HatchBrush(HatchStyle.LightDownwardDiagonal, GetColor("Hatch1"), GetColor("Hatch2"))
        B2 = New SolidBrush(GetColor("Text"))

        BackColor = GetColor("Hatch2")
    End Sub

    Private RT1 As Rectangle

    Protected Overrides Sub PaintHook()
        G.DrawRectangle(P1, ClientRectangle)
        G.SetClip(Path)

        G.Clear(C1)
        DrawGradient(C2, C3, 0, 0, Width, 30)

        RT1 = New Rectangle(12, 30, Width - 24, Height - 12 - 30)
        G.FillRectangle(B1, RT1)

        DrawGradient(C4, C5, 12, 30, Width - 24, 30)

        DrawBorders(P2, RT1)
        DrawBorders(P3, 14, 32, Width - 26, Height - 12 - 32)

        DrawText(B2, HorizontalAlignment.Left, 12, 0)

        DrawBorders(P4, 1)

        G.ResetClip()
        G.DrawPath(P5, Path)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        Path.Reset()
        Path.AddLines(New Point() { _
              New Point(2, 0), _
              New Point(Width - 3, 0), _
              New Point(Width - 1, 2), _
              New Point(Width - 1, Height - 3), _
              New Point(Width - 3, Height - 1), _
              New Point(2, Height - 1), _
              New Point(0, Height - 3), _
              New Point(0, 2), _
              New Point(2, 0)})

        MyBase.OnResize(e)
    End Sub

End Class

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 9/23/2011
'Changed: 9/23/2011
'Version: 1.0.0
'Theme Base: 1.5.2
'------------------

Class StudioButton1
    Inherits ThemeControl152

    Sub New()
        Transparent = True
        BackColor = Color.Transparent

        SetColor("DownGradient1", 28, 38, 47) '45, 65, 95
        SetColor("DownGradient2", 49, 58, 67) '65, 85, 115
        SetColor("NoneGradient1", 49, 58, 67) '65, 85, 115
        SetColor("NoneGradient2", 28, 38, 47) '45, 65, 95
        SetColor("Shine1", 30, Color.White)
        SetColor("Shine2A", 30, Color.White)
        SetColor("Shine2B", Color.Transparent)
        SetColor("Shine3", 20, Color.White)
        SetColor("TextShade", 50, Color.Black)
        SetColor("Text", Color.White)
        SetColor("Glow", 10, Color.White)
        SetColor("Border", 3, 13, 22) ' 20, 40, 70
        SetColor("Corners", 3, 13, 22) ' 20, 40, 70
    End Sub

    Private C1, C2, C3, C4, C5, C6, C7 As Color
    Private P1, P2, P3 As Pen
    Private B1, B2, B3 As SolidBrush

    Protected Overrides Sub ColorHook()
        C1 = GetColor("DownGradient1")
        C2 = GetColor("DownGradient2")
        C3 = GetColor("NoneGradient1")
        C4 = GetColor("NoneGradient2")
        C5 = GetColor("Shine2A")
        C6 = GetColor("Shine2B")
        C7 = GetColor("Corners")

        P1 = New Pen(GetColor("Shine1"))
        P2 = New Pen(GetColor("Shine3"))
        P3 = New Pen(GetColor("Border"))

        B1 = New SolidBrush(GetColor("TextShade"))
        B2 = New SolidBrush(GetColor("Text"))
        B3 = New SolidBrush(GetColor("Glow"))
    End Sub

    Protected Overrides Sub PaintHook()

        If State = MouseState.Down Then
            DrawGradient(C1, C2, ClientRectangle, 90.0F)
        Else
            DrawGradient(C3, C4, ClientRectangle, 90.0F)
        End If

        G.DrawLine(P1, 1, 1, Width, 1)
        DrawGradient(C5, C6, 1, 1, 1, Height)
        DrawGradient(C5, C6, Width - 2, 1, 1, Height)
        G.DrawLine(P2, 1, Height - 2, Width, Height - 2)

        If State = MouseState.Down Then
            DrawText(B1, HorizontalAlignment.Center, 2, 2)
            DrawText(B2, HorizontalAlignment.Center, 1, 1)
        Else
            DrawText(B1, HorizontalAlignment.Center, 1, 1)
            DrawText(B2, HorizontalAlignment.Center, 0, 0)
        End If

        If State = MouseState.Over Then
            G.FillRectangle(B3, ClientRectangle)
        End If

        DrawBorders(P3)
        DrawCorners(C7, 1, 1, Width - 2, Height - 2)

        DrawCorners(BackColor)
    End Sub
End Class

Class StudioButton2
    Inherits ThemeControl152

    Sub New()
        Transparent = True
        BackColor = Color.Transparent

        SetColor("DownGradient1", 45, 65, 95) '45, 65, 95
        SetColor("DownGradient2", 65, 85, 115) '65, 85, 115
        SetColor("NoneGradient1", 65, 85, 115) '65, 85, 115
        SetColor("NoneGradient2", 45, 65, 95) '45, 65, 95
        SetColor("Shine1", 30, Color.White)
        SetColor("Shine2A", 30, Color.White)
        SetColor("Shine2B", Color.Transparent)
        SetColor("Shine3", 20, Color.White)
        SetColor("TextShade", 50, Color.Black)
        SetColor("Text", Color.White)
        SetColor("Glow", 10, Color.White)
        SetColor("Border", 20, 40, 70) ' 20, 40, 70
        SetColor("Corners", 20, 40, 70) ' 20, 40, 70
    End Sub

    Private C1, C2, C3, C4, C5, C6, C7 As Color
    Private P1, P2, P3 As Pen
    Private B1, B2, B3 As SolidBrush

    Protected Overrides Sub ColorHook()
        C1 = GetColor("DownGradient1")
        C2 = GetColor("DownGradient2")
        C3 = GetColor("NoneGradient1")
        C4 = GetColor("NoneGradient2")
        C5 = GetColor("Shine2A")
        C6 = GetColor("Shine2B")
        C7 = GetColor("Corners")

        P1 = New Pen(GetColor("Shine1"))
        P2 = New Pen(GetColor("Shine3"))
        P3 = New Pen(GetColor("Border"))

        B1 = New SolidBrush(GetColor("TextShade"))
        B2 = New SolidBrush(GetColor("Text"))
        B3 = New SolidBrush(GetColor("Glow"))
    End Sub

    Protected Overrides Sub PaintHook()

        If State = MouseState.Down Then
            DrawGradient(C1, C2, ClientRectangle, 90.0F)
        Else
            DrawGradient(C3, C4, ClientRectangle, 90.0F)
        End If

        G.DrawLine(P1, 1, 1, Width, 1)
        DrawGradient(C5, C6, 1, 1, 1, Height)
        DrawGradient(C5, C6, Width - 2, 1, 1, Height)
        G.DrawLine(P2, 1, Height - 2, Width, Height - 2)

        If State = MouseState.Down Then
            DrawText(B1, HorizontalAlignment.Center, 2, 2)
            DrawText(B2, HorizontalAlignment.Center, 1, 1)
        Else
            DrawText(B1, HorizontalAlignment.Center, 1, 1)
            DrawText(B2, HorizontalAlignment.Center, 0, 0)
        End If

        If State = MouseState.Over Then
            G.FillRectangle(B3, ClientRectangle)
        End If

        DrawBorders(P3)
        DrawCorners(C7, 1, 1, Width - 2, Height - 2)

        DrawCorners(BackColor)
    End Sub
End Class
Class ButtonGray
    Inherits ThemeControl
    Public Overrides Sub PaintHook()
        Me.Font = New Font("Tahoma", 8)
        G.Clear(Me.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        Select Case MouseState
            Case State.MouseNone
                Dim p1 As New Pen(Color.FromArgb(57, 57, 57), 1)
                Dim x1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(90, 90, 90), Color.FromArgb(57, 57, 57), LinearGradientMode.Vertical)
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4))
                G.DrawPath(p1, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
                G.DrawLine(New Pen(Color.FromArgb(90, 90, 90)), 2, 1, Width - 3, 1)
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0)
                Exit Select
            Case State.MouseDown
                Dim p2 As New Pen(Color.FromArgb(57, 57, 57), 1)
                Dim x2 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(70, 70, 70), Color.FromArgb(36, 36, 36), LinearGradientMode.Vertical)
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4))
                G.DrawPath(p2, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
                G.DrawLine(New Pen(Color.FromArgb(90, 90, 90)), 2, 1, Width - 3, 1)
                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1)
                Exit Select
            Case State.MouseOver
                Dim p3 As New Pen(Color.FromArgb(57, 57, 57), 1)
                Dim x3 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(130, 130, 130), Color.FromArgb(31, 35, 14), LinearGradientMode.Vertical)
                G.FillPath(x3, Draw.RoundRect(ClientRectangle, 4))
                G.DrawPath(p3, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
                G.DrawLine(New Pen(Color.FromArgb(90, 90, 90)), 2, 1, Width - 3, 1)
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1)
                Exit Select
        End Select
        Me.Cursor = Cursors.Hand
    End Sub
End Class
Class ButtonRed
    Inherits ThemeControl152
    Public Property twNoRounding As Boolean
    Sub New()
        Cursor = Cursors.Hand
        Size = New Size(105, 31)
        Tag = "Red"
        Transparent = True
        BackColor = Color.Transparent
        Font = New Font("Verdana", 8.0!, FontStyle.Regular)
        twNoRounding = False
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(BackColor)
        Select Case Tag
            Case "Red"
                Select Case State
                    Case MouseState.Down
                        If twNoRounding = True Then
                            DrawGradient(Color.FromArgb(222, 11, 21), Color.FromArgb(210, 2, 5), New Rectangle(0, 0, Width, Height), 90.0F)
                            G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(25, 25, 25))), New Rectangle(0, 0, Width - 1, Height - 1))
                            DrawText(New SolidBrush(Color.White), HorizontalAlignment.Center, 0, 0)
                        Else
                            Dim BackGrad As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(222, 11, 21), Color.FromArgb(210, 2, 5), 90S)
                            G.FillPath(BackGrad, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 4))
                            G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(25, 25, 25))), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 4))
                            'Really easy to use. Instead of DrawRectangle, use DrawPath. Then instead of a rectangle, use my Draw function.
                            'The curve should be somewhere along the lines of 3-7
                        End If
                        DrawText(New SolidBrush(Color.White), HorizontalAlignment.Center, 0, 0)
                    Case Else
                        If twNoRounding = True Then
                            DrawGradient(Color.FromArgb(245, 51, 68), Color.FromArgb(229, 14, 31), New Rectangle(0, 0, Width, Height), 90.0F)
                            G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(25, 25, 25))), New Rectangle(0, 0, Width - 1, Height - 1))
                            DrawText(New SolidBrush(Color.White), HorizontalAlignment.Center, 0, 0)
                        Else
                            Dim BackGrad As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(245, 51, 68), Color.FromArgb(229, 14, 31), 90S)
                            G.FillPath(BackGrad, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 4))
                            G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(25, 25, 25))), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 4))
                            DrawText(New SolidBrush(Color.White), HorizontalAlignment.Center, 0, 0)
                        End If
                End Select
            Case "white"
                Select Case State
                    Case MouseState.None
                        DrawGradient(Color.FromArgb(242, 242, 242), Color.FromArgb(225, 225, 225), New Rectangle(0, 0, Width, Height), 90.0F)
                        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(158, 158, 158))), New Rectangle(0, 0, Width - 1, Height - 1))
                        DrawText(New SolidBrush(Color.FromArgb(124, 124, 124)), HorizontalAlignment.Center, 0, 0)
                    Case MouseState.Down
                        DrawGradient(Color.FromArgb(213, 213, 213), Color.FromArgb(188, 188, 188), New Rectangle(0, 0, Width, Height), 90.0F)
                        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(121, 121, 121))), New Rectangle(0, 0, Width - 1, Height - 1))
                        DrawText(New SolidBrush(Color.FromArgb(124, 124, 124)), HorizontalAlignment.Center, 0, 0)
                    Case MouseState.Over
                        DrawGradient(Color.FromArgb(242, 242, 242), Color.FromArgb(223, 223, 223), New Rectangle(0, 0, Width, Height), 90.0F)
                        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(158, 158, 158))), New Rectangle(0, 0, Width - 1, Height - 1))
                        DrawText(New SolidBrush(Color.FromArgb(124, 124, 124)), HorizontalAlignment.Center, 0, 0)
                End Select
            Case Else
                Select Case State
                    Case MouseState.Down
                        DrawGradient(Color.FromArgb(80, 56, 129), Color.FromArgb(58, 37, 103), New Rectangle(0, 0, Width, Height), 90.0F)
                        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(25, 25, 25))), New Rectangle(0, 0, Width - 1, Height - 1))
                        DrawText(New SolidBrush(Color.White), HorizontalAlignment.Center, 0, 0)
                    Case Else
                        DrawGradient(Color.FromArgb(124, 96, 176), Color.FromArgb(87, 59, 139), New Rectangle(0, 0, Width, Height), 90.0F)
                        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(25, 25, 25))), New Rectangle(0, 0, Width - 1, Height - 1))
                        DrawText(New SolidBrush(Color.White), HorizontalAlignment.Center, 0, 0)
                End Select
        End Select
    End Sub
End Class
Class ButtonDark
    Inherits ThemeControl152

    Sub New()
        Transparent = True
        BackColor = Color.Transparent

        SetColor("DownGradient1", 50, 50, 50)
        SetColor("DownGradient2", 65, 65, 65)
        SetColor("NoneGradient1", 65, 65, 65)
        SetColor("NoneGradient2", 50, 50, 50)
        SetColor("Shine1", 30, Color.White)
        SetColor("Shine2A", 30, Color.White)
        SetColor("Shine2B", Color.Transparent)
        SetColor("Shine3", 20, Color.White)
        SetColor("TextShade", 50, Color.Black)
        SetColor("Text", Color.White)
        SetColor("Glow", 10, Color.White)
        SetColor("Border", 25, 25, 25)
        SetColor("Corners", 25, 25, 25)
    End Sub

    Private C1, C2, C3, C4, C5, C6, C7 As Color
    Private P1, P2, P3 As Pen
    Private B1, B2, B3 As SolidBrush

    Protected Overrides Sub ColorHook()
        C1 = GetColor("DownGradient1")
        C2 = GetColor("DownGradient2")
        C3 = GetColor("NoneGradient1")
        C4 = GetColor("NoneGradient2")
        C5 = GetColor("Shine2A")
        C6 = GetColor("Shine2B")
        C7 = GetColor("Corners")

        P1 = New Pen(GetColor("Shine1"))
        P2 = New Pen(GetColor("Shine3"))
        P3 = New Pen(GetColor("Border"))

        B1 = New SolidBrush(GetColor("TextShade"))
        B2 = New SolidBrush(GetColor("Text"))
        B3 = New SolidBrush(GetColor("Glow"))
    End Sub

    Protected Overrides Sub PaintHook()

        If State = MouseState.Down Then
            DrawGradient(C1, C2, ClientRectangle, 90.0F)
        Else
            DrawGradient(C3, C4, ClientRectangle, 90.0F)
        End If

        G.DrawLine(P1, 1, 1, Width, 1)
        DrawGradient(C5, C6, 1, 1, 1, Height)
        DrawGradient(C5, C6, Width - 2, 1, 1, Height)
        G.DrawLine(P2, 1, Height - 2, Width, Height - 2)

        If State = MouseState.Down Then
            DrawText(B1, HorizontalAlignment.Center, 2, 2)
            DrawText(B2, HorizontalAlignment.Center, 1, 1)
        Else
            DrawText(B1, HorizontalAlignment.Center, 1, 1)
            DrawText(B2, HorizontalAlignment.Center, 0, 0)
        End If

        If State = MouseState.Over Then
            G.FillRectangle(B3, ClientRectangle)
        End If

        DrawBorders(P3)
        DrawCorners(C7, 1, 1, Width - 2, Height - 2)

        DrawCorners(BackColor)
    End Sub
End Class
Class ButtonGreen
    Inherits ThemeControl
    Public Overrides Sub PaintHook()
        Me.Font = New Font("Tahoma", 8)
        G.Clear(Me.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        Select Case MouseState
            Case State.MouseNone
                Dim p1 As New Pen(Color.FromArgb(120, 159, 22), 1)
                Dim x1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(157, 209, 57), Color.FromArgb(130, 181, 18), LinearGradientMode.Vertical)
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4))
                G.DrawPath(p1, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
                G.DrawLine(New Pen(Color.FromArgb(190, 232, 109)), 2, 1, Width - 3, 1)
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0)
                Exit Select
            Case State.MouseDown
                Dim p2 As New Pen(Color.FromArgb(120, 159, 22), 1)
                Dim x2 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 171, 25), Color.FromArgb(142, 192, 40), LinearGradientMode.Vertical)
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4))
                G.DrawPath(p2, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
                G.DrawLine(New Pen(Color.FromArgb(142, 172, 30)), 2, 1, Width - 3, 1)
                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1)
                Exit Select
            Case State.MouseOver
                Dim p3 As New Pen(Color.FromArgb(120, 159, 22), 1)
                Dim x3 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(165, 220, 59), Color.FromArgb(137, 191, 18), LinearGradientMode.Vertical)
                G.FillPath(x3, Draw.RoundRect(ClientRectangle, 4))
                G.DrawPath(p3, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
                G.DrawLine(New Pen(Color.FromArgb(190, 232, 109)), 2, 1, Width - 3, 1)
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1)
                Exit Select
        End Select
        Me.Cursor = Cursors.Hand
    End Sub
End Class
Class ButtonBlue
    Inherits ThemeControl
    Public Overrides Sub PaintHook()
        Me.Font = New Font("Tahoma", 9)
        G.Clear(Me.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        Select Case MouseState
            Case State.MouseNone
                Dim p As New Pen(Color.FromArgb(34, 112, 171), 1)
                Dim x As New LinearGradientBrush(ClientRectangle, Color.FromArgb(51, 159, 231), Color.FromArgb(33, 128, 206), LinearGradientMode.Vertical)
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4))
                G.DrawPath(p, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
                G.DrawLine(New Pen(Color.FromArgb(131, 197, 241)), 2, 1, Width - 3, 1)
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0)
                Exit Select
            Case State.MouseDown
                Dim p1 As New Pen(Color.FromArgb(34, 112, 171), 1)
                Dim x1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(37, 124, 196), Color.FromArgb(53, 153, 219), LinearGradientMode.Vertical)
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4))
                G.DrawPath(p1, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))

                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1)
                Exit Select
            Case State.MouseOver
                Dim p2 As New Pen(Color.FromArgb(34, 112, 171), 1)
                Dim x2 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(54, 167, 243), Color.FromArgb(35, 165, 217), LinearGradientMode.Vertical)
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4))
                G.DrawPath(p2, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))
                G.DrawLine(New Pen(Color.FromArgb(131, 197, 241)), 2, 1, Width - 3, 1)
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1)
                Exit Select
        End Select
        Me.Cursor = Cursors.Hand
    End Sub
End Class
Class StudioProgressBar
    Inherits Control

    Private _Maximum As Integer = 100
    Public Property Maximum As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal v As Integer)
            If v < 1 Then v = 1
            If v < _Value Then _Value = v
            _Maximum = v
            Invalidate()
        End Set
    End Property

    Private _Value As Integer
    Public Property Value As Integer
        Get
            Return _Value
        End Get
        Set(ByVal v As Integer)
            If v > _Maximum Then v = Maximum
            _Value = v
            Invalidate()
        End Set
    End Property

    Private _percent As Integer
    Public ReadOnly Property Percent As Integer
        Get
            Return _percent
        End Get
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw, True)
        Size = New Size(200, 40)
        Font = New Font("Arial", 11)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim G As Graphics = e.Graphics

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 3
        _percent = (_Value / _Maximum) * 100

        Dim midY As Integer = ((Height - 1) / 2)
        Dim mainRect As New Rectangle(12, midY - 4, Width - 25, 7)
        Dim mainPath As GraphicsPath = RoundRect(mainRect, slope)
        Dim barBrush As New LinearGradientBrush(mainRect, Color.FromArgb(6, 22, 38), Color.FromArgb(26, 42, 58), 90.0F)
        G.FillPath(barBrush, mainPath)

        Dim barRect As New Rectangle(12, midY - 4, CInt(((Width / _Maximum) * _Value) - ((_percent - 1) / 4)), 7)
        If barRect.Width > 0 Then
            Dim barHorizontal As New LinearGradientBrush(barRect, Color.FromArgb(5, 80, 140), Color.FromArgb(45, 180, 200), 0.0F)
            G.FillPath(barHorizontal, RoundRect(barRect, slope))

            Dim vertCB As New ColorBlend(5)
            vertCB.Colors(0) = Color.Transparent
            vertCB.Colors(1) = Color.Transparent
            vertCB.Colors(2) = Color.FromArgb(0, 150, 220)
            vertCB.Colors(3) = Color.Transparent
            vertCB.Colors(4) = Color.Transparent
            vertCB.Positions = New Single() {0.0, 0.4, 0.5, 0.6, 1.0}
            Dim barVertical As New LinearGradientBrush(barRect, Color.Black, Color.Black, 90.0F)
            barVertical.InterpolationColors = vertCB
            G.FillPath(barVertical, RoundRect(barRect, slope))
        End If

        If _Value > 0 Then
            Dim bubbleRect As New Rectangle(barRect.Width - 3, 0, midY * 2 - 3, midY * 2)
            Dim bubblePath As GraphicsPath = RoundRect(bubbleRect, midY)
            Dim bubbleBrush As New PathGradientBrush(bubblePath)
            bubbleBrush.CenterColor = Color.FromArgb(230, 245, 255)
            bubbleBrush.SurroundColors = {Color.Transparent}
            G.FillPath(bubbleBrush, bubblePath)
        End If

    End Sub
End Class
<DefaultEvent("CheckedChanged")>
Public Class StudioRadioButton
    Inherits Control

#Region "Declarations"
    Private _Checked As Boolean
    Private State As MouseState = MouseState.None
    Private _HoverColour As Color = Color.FromArgb(50, 49, 51)
    Private _CheckedColour As Color = Color.FromArgb(173, 173, 174)
    Private _BorderColour As Color = Color.FromArgb(50, 50, 50)
    Private _BackColour As Color = Color.FromArgb(26, 26, 26)
    Private _TextColour As Color = Color.FromArgb(255, 255, 255)
#End Region

#Region "Colour & Other Properties"

    <Category("Colours")>
    Public Property HighlightColour As Color
        Get
            Return _HoverColour
        End Get
        Set(value As Color)
            _HoverColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BackColour
        End Get
        Set(value As Color)
            _BackColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property CheckedColour As Color
        Get
            Return _CheckedColour
        End Get
        Set(value As Color)
            _CheckedColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property FontColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property

    Event CheckedChanged(ByVal sender As Object)
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
            InvalidateControls()
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnClick(e As EventArgs)
        If Not _Checked Then Checked = True
        MyBase.OnClick(e)
    End Sub
    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return
        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is StudioRadioButton Then
                DirectCast(C, StudioRadioButton).Checked = False
                Invalidate()
            End If
        Next
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        InvalidateControls()
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 22
    End Sub
#End Region

#Region "Mouse States"

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        Size = New Size(100, 22)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim G = e.Graphics
        Dim Base As New Rectangle(1, 1, Height - 2, Height - 2)
        Dim Circle As New Rectangle(6, 6, Height - 12, Height - 12)
        With G
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(_BackColour)
            .FillEllipse(New SolidBrush(_BackColour), Base)
            .DrawEllipse(New Pen(_BorderColour, 2), Base)
            If Checked Then
                Select Case State
                    Case MouseState.Over
                        .FillEllipse(New SolidBrush(_HoverColour), New Rectangle(2, 2, Height - 4, Height - 4))
                End Select
                .FillEllipse(New SolidBrush(_CheckedColour), Circle)
            Else
                Select Case State
                    Case MouseState.Over
                        .FillEllipse(New SolidBrush(_HoverColour), New Rectangle(2, 2, Height - 4, Height - 4))
                End Select
            End If
            .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(24, 3, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
            .InterpolationMode = CType(7, InterpolationMode)
        End With
    End Sub
#End Region
End Class