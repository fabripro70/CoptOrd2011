Public Class CHashGest
    ''' <summary>
    ''' This Class is a utility for hashtable management
    ''' </summary>
    ''' <remarks></remarks>
    Private m_Hash As Hashtable
    Public Sub New(ByVal pHash As Hashtable)
        m_Hash = pHash
    End Sub
    ''' <summary>
    ''' Add record to hashtable, if exists update it, else inserts it 
    ''' </summary>
    ''' <param name="pKey"></param>
    ''' <param name="pRec"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CHGput(ByVal pKey As String, ByVal pRec As Object)

        Try
            If m_Hash.ContainsKey(pKey) Then
                m_Hash(pKey) = pRec
            Else
                m_Hash.Add(pKey, pRec)
            End If
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "CHashGest Put")
        End Try

    End Function
    ''' <summary>
    ''' Returns the record of the sought key
    ''' </summary>
    ''' <param name="pKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CHGGet(ByVal pKey As String) As Object

        Try
            If Not m_Hash(pKey) Is Nothing Then
                Return m_Hash(pKey)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "CHashGest Get")
        End Try

    End Function
End Class
