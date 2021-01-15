Imports System.ComponentModel
Imports System.ComponentModel.Design

<Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", GetType(IDesigner))> _
<DesignerAttribute(GetType(TransparentControlDesigner))> _
Public Class TransparentControl

    Inherits Control

#Region " Field "

    Private m_transparent As Boolean
    Private m_transparentColor As Color
    Private m_opacity As Double
    Private m_backcolor As Color
    Private m_minSize As Size

#End Region

#Region " Constructor "

    Public Sub New()

        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Opaque, False)
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.UserPaint, True)
        UpdateStyles()
        m_minSize = New Size(100, 75)
        m_transparent = False
        m_transparentColor = Color.OliveDrab
        m_opacity = 1.0R
        m_backcolor = Color.Transparent


    End Sub

#End Region

#Region " Property "

    <System.ComponentModel.Browsable(False)> _
    <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
    <System.ComponentModel.DefaultValue(GetType(Color), "Transparent")> _
    <System.ComponentModel.Description("Set background color.")> _
    <System.ComponentModel.Category("Control Style")> _
    Public Overrides Property BackColor() As System.Drawing.Color
        Get
            Return m_backcolor
        End Get
        Set(ByVal value As System.Drawing.Color)
            m_backcolor = value
            Refresh()
        End Set
    End Property

    <System.ComponentModel.Browsable(True)> _
    <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)> _
    <System.ComponentModel.DefaultValue(1.0R)> _
    <System.ComponentModel.TypeConverter(GetType(OpacityConverter))> _
    <System.ComponentModel.Description("Set the opacity percentage of the control.")> _
    <System.ComponentModel.Category("Control Style")> _
    Public Overridable Property Opacity() As Double
        Get
            Return m_opacity
        End Get
        Set(ByVal value As Double)
            If value = m_opacity Then
                Return
            End If
            m_opacity = value
            UpdateStyles()
            Refresh()
        End Set
    End Property

    <System.ComponentModel.Browsable(True)> _
    <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)> _
    <System.ComponentModel.DefaultValue(GetType(Boolean), "False")> _
    <System.ComponentModel.Description("Enable control trnasparency.")> _
    <System.ComponentModel.Category("Control Style")> _
    Public Overridable Property Transparent() As Boolean
        Get
            Return m_transparent
        End Get
        Set(ByVal value As Boolean)
            If value = m_transparent Then
                Return
            End If
            m_transparent = value
            Refresh()
        End Set
    End Property

    <System.ComponentModel.Browsable(True)> _
    <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)> _
    <System.ComponentModel.DefaultValue(GetType(Color), "OliveDrab")> _
    <System.ComponentModel.Description("Set the fill color of the control.")> _
    <System.ComponentModel.Category("Control Style")> _
    Public Overridable Property TransparentColor() As Color
        Get
            Return m_transparentColor
        End Get
        Set(ByVal value As Color)
            m_transparentColor = value
            Refresh()
        End Set
    End Property

    Protected Overloads Overrides ReadOnly Property DefaultSize() As Size
        Get
            Return New Size(200, 100)
        End Get
    End Property

    Public Overloads Overrides Property MinimumSize() As System.Drawing.Size
        Get
            Return m_minSize
        End Get
        Set(ByVal value As System.Drawing.Size)
            If (value <> (m_minSize)) Then
                m_minSize = value
                Refresh()
            End If
        End Set
    End Property

#End Region

#Region " Event "

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        DrawTransparentBackground(e.Graphics, Me)

    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

#End Region

#Region " Method "

    Public Overridable Sub DrawTransparentBackground(ByVal g As Graphics, ByVal control As TransparentControl)

        If Transparent Then

            Using sb As New SolidBrush(control.BackColor)
                g.FillRectangle(sb, control.ClientRectangle)
                sb.Dispose()

                Using sbt As New SolidBrush(Color.FromArgb(control.Opacity * 255, control.TransparentColor))
                    g.FillRectangle(sbt, control.ClientRectangle)
                    sbt.Dispose()
                End Using
            End Using

        Else

            Using sb As New SolidBrush(control.TransparentColor)
                g.FillRectangle(sb, control.ClientRectangle)
                sb.Dispose()
            End Using
        End If

    End Sub

#End Region

End Class

<System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.Demand, Name:="FullTrust")> _
Public Class TransparentControlDesigner

    Inherits System.Windows.Forms.Design.ParentControlDesigner

#Region " Field "

    Private lists As DesignerActionListCollection

#End Region

#Region " Property "

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If lists Is Nothing Then
                lists = New DesignerActionListCollection()
                lists.Add(New TransparentControlActionList(Me.Component))

            End If

            Return lists
        End Get
    End Property

    Protected Overrides ReadOnly Property DefaultControlLocation() As System.Drawing.Point
        Get
            Return New Point(20, 20)
        End Get
    End Property

    Protected Overrides ReadOnly Property EnableDragRect() As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides ReadOnly Property AllowGenericDragBox() As Boolean
        Get
            Return True
        End Get
    End Property

#End Region

End Class

Public Class TransparentControlActionList

    Inherits DesignerActionList

#Region " Field "
    Private designerActionUISvc As System.ComponentModel.Design.DesignerActionUIService = Nothing
    Private tc As TransparentControl
#End Region

#Region " Constructor "
    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)
        tc = component
        designerActionUISvc = CType(GetService(GetType(DesignerActionUIService)), DesignerActionUIService)
    End Sub
#End Region

#Region " Property "

    <System.ComponentModel.DefaultValue(1.0R)> _
    <System.ComponentModel.TypeConverter(GetType(OpacityConverter))> _
    Public Overridable Property Opacity() As Double
        Get
            Return tc.Opacity
        End Get
        Set(ByVal value As Double)
            GetPropertyByName("Opacity").SetValue(tc, value)
        End Set
    End Property

    <System.ComponentModel.DefaultValue(GetType(Boolean), "False")> _
    Public Overridable Property Transparent() As Boolean
        Get
            Return tc.Transparent
        End Get
        Set(ByVal value As Boolean)
            GetPropertyByName("Transparent").SetValue(tc, value)
        End Set
    End Property

    <System.ComponentModel.DefaultValue(GetType(Color), "OliveDrab")> _
    Public Overridable Property TransparentColor() As Color
        Get
            Return tc.TransparentColor
        End Get
        Set(ByVal value As Color)
            GetPropertyByName("TransparentColor").SetValue(tc, value)
        End Set
    End Property

#End Region

#Region " Function "
    Private Function GetPropertyByName(ByVal propName As String) As PropertyDescriptor

        Dim prop As PropertyDescriptor = TypeDescriptor.GetProperties(tc)(propName)
        If prop Is Nothing Then
            Throw New ArgumentException("Matching property not valid!", propName)
        Else
            Return prop
        End If
    End Function

    Public Overrides Function GetSortedActionItems() As System.ComponentModel.Design.DesignerActionItemCollection

        Dim items As New DesignerActionItemCollection()

        items.Add(New DesignerActionPropertyItem("Opacity", "Opacity", "Set the opacity percentage of the control"))
        items.Add(New DesignerActionPropertyItem("TransparentColor", "TransparentColor", "Set the fill color of the control"))
        items.Add(New DesignerActionPropertyItem("Transparent", "Transparent ", "Enable trnasparency of the control"))

        Return items

    End Function
#End Region

End Class
