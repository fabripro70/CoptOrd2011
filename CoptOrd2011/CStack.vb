Public Class CStack
    Inherits Stack
    Public Overrides Function Pop() As Object
        If MyBase.Count = 0 Then
            Return " "
        Else
            Return MyBase.Pop()
        End If
    End Function
End Class
