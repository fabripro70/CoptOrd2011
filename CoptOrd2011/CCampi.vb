Public Class CListField
    Private m_ColumnId As Integer
    Private m_fieldName As String
    Private m_dataType As Integer
    Private m_Precision As Integer
    Private m_scale As Integer
    Private m_value As String
    Public Sub New()
        m_ColumnId = 0
        m_fieldName = ""
        m_dataType = 0
        m_Precision = 0
        m_scale = 0
        m_value = 0
    End Sub
    Public Property Scale() As Integer
        Get
            Try
                Return m_scale
            Catch ex As SystemException
                MsgBox(ex.Message)
            End Try
        End Get
        Set(ByVal value As Integer)
            m_scale = value
        End Set
    End Property
    Public Property Precision() As Integer
        Get
            Try
                Return m_Precision
            Catch ex As SystemException
                MsgBox(ex.Message)
            End Try
        End Get
        Set(ByVal value As Integer)
            m_Precision = value
        End Set
    End Property

    Public Property ColumnId() As Integer
        Get
            Try
                Return m_ColumnId
            Catch ex As SystemException
                MsgBox(ex.Message)
            End Try
        End Get
        Set(ByVal value As Integer)
            m_ColumnId = value
        End Set
    End Property
    Public Property FieldName() As String
        Get
            Try
                Return m_fieldName
            Catch ex As SystemException
                MsgBox(ex.Message)
            End Try
        End Get
        Set(ByVal value As String)
            m_fieldName = value
        End Set
    End Property
    Public Property DataType() As TipoCampo
        Get
            Try
                Return m_dataType
            Catch ex As SystemException
                MsgBox(ex.Message)
            End Try
        End Get
        Set(ByVal value As TipoCampo)
            m_dataType = value
        End Set
    End Property
    Public Property Value() As String
        Get
            Try
                Return m_value
            Catch ex As SystemException
                MsgBox(ex.Message)
            End Try
        End Get
        Set(ByVal value As String)
            m_value = value
        End Set
    End Property
End Class
Public Class CCampi

    Public tab_campi()
    Public tab_key()
    Public tab_numrec As Integer
    Public tab_campi_n() As CListField

    Function dimesiona_campi_n(ByVal m_of As COpeFile, ByVal Tabella As String) As CListField()
        Dim ds As New DataSet
        Dim mLF() As CListField
        Dim wTabella As String = m_of.getTablename(Tabella.ToLower)
        'Dim stringa_sql = "select * from " & Trim(Tabella) & " where 1=0"
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                Dim stringa_sql As String = "select dbo.syscolumns.[name] as fieldname, dbo.systypes.[name] as fieldtype, dbo.syscolumns.[prec], dbo.syscolumns.[scale], dbo.syscolumns.colorder " & _
                         "from dbo.syscolumns inner join dbo.systypes on dbo.syscolumns.[xtype] = dbo.systypes.[xtype] " & _
                         "where id = object_id(N'[dbo].[" & wTabella & "]') AND dbo.syscolumns.[name] <> 'NUMREC' ORDER BY dbo.syscolumns.colorder"
                Dim da1 As New SqlClient.SqlDataAdapter(stringa_sql, CType(Globale.cn, SqlClient.SqlConnection))
                Try
                    da1.Fill(ds)
                    Dim tIndex As Integer = 0
                    ReDim mLF(ds.Tables(0).Rows.Count - 1)
                    ReDim tab_campi(ds.Tables(0).Rows.Count)
                    For Each element As DataRow In ds.Tables(0).Rows
                        tIndex = element("colorder") - 2
                        mLF(tIndex) = New CListField
                        mLF(tIndex).ColumnId = element("colorder") - 2
                        mLF(tIndex).FieldName = element("fieldname")
                        mLF(tIndex).DataType = CType(Globale.gMappaCampi.Item(wTabella & "." & element("fieldname")), StructCampi).STipo
                        mLF(tIndex).Precision = element("prec")
                        mLF(tIndex).Scale = CTran(element("scale"), 0)
                        mLF(tIndex).Value = ""
                    Next
                Catch ex As SqlClient.SqlException
                    MsgBox(ex.Message())
                Catch ex As SystemException
                    MsgBox(ex.Message())
                End Try
            Case "ORACLE"
                Dim stringa_sql As String = "SELECT COLUMN_NAME AS FIELDNAME, DATA_TYPE AS FIELDTYPE, DATA_PRECISION AS PREC," & _
                         " DATA_SCALE AS SCALE, COLUMN_ID, DATA_LENGTH AS LENGTH FROM USER_TAB_COLUMNS WHERE TABLE_NAME = '" & wTabella & "'" & _
                         " AND COLUMN_NAME <> 'NUMREC' ORDER BY COLUMN_ID"
                Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(stringa_sql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
                Try
                    da1.Fill(ds)
                    Dim tIndex As Integer = 0
                    ReDim mLF(ds.Tables(0).Rows.Count - 1)
                    ReDim tab_campi(ds.Tables(0).Rows.Count)
                    For Each element As DataRow In ds.Tables(0).Rows
                        tIndex = element("COLUMN_ID") - 2 'Because index must begin from 0; consider that numrec = 0, field1=1, ....
                        mLF(tIndex) = New CListField
                        mLF(tIndex).ColumnId = element("COLUMN_ID") - 2
                        mLF(tIndex).FieldName = element("FIELDNAME")
                        mLF(tIndex).DataType = CType(Globale.gMappaCampi.Item(wTabella & "." & element("fieldname")), StructCampi).STipo
                        If CTran(element("PREC"), 0) = 0 Then
                            mLF(tIndex).Precision = element("LENGTH")
                        Else
                            mLF(tIndex).Precision = element("PREC")
                        End If
                        mLF(tIndex).Scale = CTran(element("SCALE"), 0)
                        mLF(tIndex).Value = ""
                    Next
                Catch ex As Oracle.DataAccess.Client.OracleException
                    MsgBox(ex.Message())
                Catch ex As SystemException
                    MsgBox(ex.Message())
                End Try
            Case "MYSQL"
                Dim stringa_sql As String = "SELECT COLUMN_NAME AS FIELDNAME, DATA_TYPE AS FIELDTYPE, CHARACTER_MAXIMUM_LENGTH AS CHARLENGTH, " & _
                         "NUMERIC_PRECISION AS PREC, NUMERIC_SCALE AS SCALE, ORDINAL_POSITION FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" & wTabella & "' " & _
                         " AND TABLE_SCHEMA = '" & Globale.DataBase & "' AND COLUMN_NAME <> 'NUMREC' ORDER BY ORDINAL_POSITION"
                Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(stringa_sql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
                Try
                    da1.Fill(ds)
                    Dim tIndex As Integer = 0
                    ReDim mLF(ds.Tables(0).Rows.Count - 1)
                    ReDim tab_campi(ds.Tables(0).Rows.Count)
                    For Each element As DataRow In ds.Tables(0).Rows
                        tIndex = element("ORDINAL_POSITION") - 2 'Because index must begin from 0; consider that numrec = 0, field1=1, ....
                        mLF(tIndex) = New CListField
                        mLF(tIndex).ColumnId = element("ORDINAL_POSITION") - 2
                        mLF(tIndex).FieldName = element("FIELDNAME")
                        mLF(tIndex).DataType = CType(Globale.gMappaCampi.Item(wTabella & "." & element("fieldname")), StructCampi).STipo
                        If CTran(element("PREC"), 0) = 0 Then
                            mLF(tIndex).Precision = CTran(element("CHARLENGTH"), 0)
                        Else
                            mLF(tIndex).Precision = CTran(element("PREC"), 0)
                        End If
                        mLF(tIndex).Scale = CTran(element("SCALE"), 0)
                        mLF(tIndex).Value = ""
                    Next
                Catch ex As Oracle.DataAccess.Client.OracleException
                    MsgBox(ex.Message())
                Catch ex As SystemException
                    MsgBox(ex.Message())
                End Try
        End Select
        ds = Nothing
        Return mLF
    End Function
    Function dimesiona_campi(ByVal m_of As COpeFile, ByVal Tabella As String) As Boolean
        Dim ds As New DataSet
        Dim wTabella As String = Tabella.ToLower

        'Dim stringa_sql = "select * from " & Trim(Tabella) & " where 1=0"
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                Dim stringa_sql As String = "select dbo.syscolumns.[name] " & _
                         "from dbo.syscolumns " & _
                         "where id = object_id(N'[dbo].[" & wTabella & "]') ORDER BY dbo.syscolumns.colorder"
                Dim da1 As New SqlClient.SqlDataAdapter(stringa_sql, CType(Globale.cn, SqlClient.SqlConnection))
                Try
                    da1.Fill(ds)
                    ReDim tab_campi(ds.Tables(0).Rows.Count)
                Catch ex As SqlClient.SqlException
                    MsgBox(ex.Message())
                End Try
            Case "ORACLE"
                Dim stringa_sql As String = "SELECT COLUMN_NAME FROM USER_TAB_COLUMNS WHERE TABLE_NAME = '" & wTabella & "'" & _
                         " ORDER BY COLUMN_ID'"
                Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(stringa_sql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
                Try
                    da1.Fill(ds)
                    ReDim tab_campi(ds.Tables(0).Columns.Count)
                Catch ex As Oracle.DataAccess.Client.OracleException
                    MsgBox(ex.Message())
                End Try
            Case "MYSQL"
                Dim stringa_sql As String = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" & wTabella & "'" & _
                         " ORDER BY ORDINAL_POSITION'"
                Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(stringa_sql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
                Try
                    da1.Fill(ds)
                    ReDim tab_campi(ds.Tables(0).Columns.Count)
                Catch ex As MySql.Data.MySqlClient.MySqlException
                    MsgBox(ex.Message())
                End Try
        End Select
        ds = Nothing
        Return True
    End Function

End Class
