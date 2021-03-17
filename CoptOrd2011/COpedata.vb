Class COpedata
    Public Function ConvertData(ByVal data As String) As String
        Dim giorno As String
        Dim mese As String
        Dim anno As String
        If Trim$(data) = "" Then
            Exit Function
        End If
        If IsDate(data) Then
            ConvertData = data
            Exit Function
        End If
        Select Case Len(Trim$(data))
            Case 6
                giorno = data.Substring(0, 2)
                mese = data.Substring(2, 2)
                anno = data.Substring(4, 2)
            Case 8
                giorno = data.Substring(0, 2)
                mese = data.Substring(2, 2)
                anno = data.Substring(4, 4)
            Case Else
                MsgBox("Formato data Errato.", vbOKOnly, "ATTENZIONE!")
                ConvertData = "ERR"
                Exit Function
        End Select
        If Not IsDate(giorno & "/" & mese & "/" & anno) Then
            MsgBox("Data Errata.", vbOKOnly, "ATTENZIONE!")
            ConvertData = "ERR"
            Exit Function
        End If
        ConvertData = giorno & "/" & mese & "/" & anno
    End Function

End Class
