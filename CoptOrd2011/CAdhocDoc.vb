Imports System.Text
Imports Microsoft.VisualBasic.Strings
Imports System.Xml
Public Structure s_planField
    Dim fieldName As String
    Dim fieldType As TipoCampo
    Dim len As Integer
    Dim dec As Integer
    Dim key As String
End Structure
Public Structure s_tableValue
    Dim mValue As Object
    Dim mType As TipoCampo
End Structure
Public Class CAdhocDoc
    Private hOP As COpeFile
    Private _CodAzi As String = ""
    Public hFieldVal As New Hashtable
    Public tplDoc_mast As New Hashtable
    Public tplDoc_dett_r As New Hashtable
    Public tplDoc_dett_d As New Hashtable
    Public hTable As New Hashtable 'Return values and type of field table after the read
    Public hPlantable As New Hashtable 'key: tablename.fieldname, value:s_planField 
    Public hForeignTable As New Hashtable
    Public hTableList As New Hashtable 'value : table name, key : description table 
    Public aTableList As New ArrayList
    Private mTipoDb As String = ""
    ''' <summary>
    ''' Load AdHoc Plan Table into hPlantable hashtable
    ''' hPlantable -> key: tablename.fieldname, value:s_planField 
    ''' </summary>
    ''' <param name="pPathFileNamePlan"></param>
    ''' <remarks></remarks>

    Public Sub New(ByVal pPathFileNamePlan As String, ByVal op As COpeFile, ByVal adHocAzi As String, ByVal pTipodb As String)

        Dim XmlNodo As Xml.XmlNodeList
        Dim _s_data As New s_planField
        Dim _tableName As String = ""
        Dim _tableComment As String = ""
        '
        Try
            hOP = op
            _CodAzi = adHocAzi
            mTipoDb = pTipodb
            If Not System.IO.Directory.Exists("xml") Then
                System.IO.Directory.CreateDirectory("xml")
            End If
            If Not System.IO.Directory.Exists("query") Then
                System.IO.Directory.CreateDirectory("query")
            End If
            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument

            'carico il file
            Dim XmlLeggi As New XmlTextReader(pPathFileNamePlan)

            XmlLeggi.WhitespaceHandling = WhitespaceHandling.None

            Xmlfile.Load(XmlLeggi)

            'cerco tutti i nodi da leggere
            XmlNodo = Xmlfile.GetElementsByTagName("ItemTable")
            'Fill doc_mast structure
            For Each nodo As XmlNode In XmlNodo
                'Find TableFields node
                _tableName = nodo.ChildNodes(0).InnerText.Trim
                _tableComment = nodo.ChildNodes(2).InnerText.Trim
                If Not hTableList.ContainsKey(_tableName) Then
                    If _tableComment.Trim <> "" Then
                        hTableList.Add(_tableName, _tableComment)
                        aTableList.Add(_tableName)
                    End If
                End If
                For Each element As XmlNode In nodo.ChildNodes
                    Dim chiave As String = element.Name.Trim
                    If chiave = "TableForeignkeys" Then
                        Dim _iIndex As Integer = 0
                        For Each _childNode As XmlNode In element
                            Dim a As XmlNode = _childNode 'TableForeignkey
                            Try
                                hForeignTable.Add(_tableName & "-" & _iIndex.ToString.Trim, a.ChildNodes(0).InnerText.Trim)
                                _iIndex = _iIndex + 1
                            Catch ex As Exception
                            End Try
                        Next
                    End If
                    If chiave = "TableFields" Then
                        For Each _childNode As XmlNode In element
                            Dim a As XmlNode = _childNode 'Tablefield
                            _s_data.fieldName = a.ChildNodes(0).InnerText
                            _s_data.len = a.ChildNodes(2).InnerText
                            _s_data.dec = a.ChildNodes(3).InnerText
                            _s_data.key = a.ChildNodes(4).InnerText
                            Select Case a.ChildNodes(1).InnerText 'Field type
                                Case "N"
                                    If _s_data.dec = 0 Then
                                        _s_data.fieldType = TipoCampo.TLong
                                    End If
                                    If _s_data.dec > 0 Then
                                        _s_data.fieldType = TipoCampo.TCur
                                    End If
                                Case "C"
                                    _s_data.fieldType = TipoCampo.TChar
                                Case "M" 'Text
                                    _s_data.fieldType = TipoCampo.TChar
                                Case "D"
                                    _s_data.fieldType = TipoCampo.TData
                            End Select
                            Try
                                hPlantable.Add(_tableName & "." & _s_data.fieldName, _s_data)
                            Catch ex As Exception
                            End Try
                        Next
                    End If
                Next
            Next
            aTableList.Sort()
            XmlLeggi.Close()
            XmlLeggi = Nothing
            loadTemplate("T_DOC_MAST.xml", tplDoc_mast)
            loadTemplate("T_DOC_DETT_R.xml", tplDoc_dett_r)
            loadTemplate("T_DOC_DETT_D.xml", tplDoc_dett_d)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub loadTemplate(ByVal tplName As String, ByVal hashName As Hashtable)

        Dim XmlNodo As Xml.XmlNodeList
        Try

            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument

            'carico il file
            Dim XmlLeggi As New XmlTextReader(_CodAzi.Trim & "\" & tplName)

            XmlLeggi.WhitespaceHandling = WhitespaceHandling.Significant

            Xmlfile.Load(XmlLeggi)

            'cerco tutti i nodi da leggere
            XmlNodo = Xmlfile.GetElementsByTagName("row")
            'Fill doc_mast structure
            For Each nodo As XmlNode In XmlNodo
                For Each element As XmlNode In nodo.ChildNodes
                    Dim name As String = element.Name
                    'If name = "MVCODCLA" Then
                    'Dim a As Integer = 0
                    'End If
                    Dim value As String = element.InnerText
                    hashName.Add(name, value)
                Next
            Next
            XmlLeggi.Close()
            XmlLeggi = Nothing
        Catch ex As Exception
            ' MsgBox(ex.Message)
        End Try

    End Sub
    ''' <summary>
    ''' Read adhoc table and return values
    ''' pKey : key name separated by comma
    ''' pValue : values separated by comma
    ''' pSystable : indicates if pTablename is a system table or a normal table 
    ''' </summary>
    ''' <param name="pTablename"></param>
    ''' <param name="pkey">key name separated by comma</param>
    ''' <param name="pValue">value separated by comma</param>
    ''' <returns>hashtable with key : fieldname and value : s_field structure</returns>
    ''' <remarks></remarks>
    Public Function readAdhocTable(ByVal pTablename As String, ByVal pkey As String, ByVal pValue As String, ByVal pSystable As Boolean) As Hashtable
        Dim _hash As New Hashtable
        Dim _fldVal As New s_tableValue
        Dim _keys = pkey.Split(",")
        Dim _kValues = pValue.Split(",")
        Dim _Criteria As String = " where 1=1 "
        Dim _Select As String = ""

        Try
            For ii As Integer = 0 To _keys.length - 1
                _Criteria = _Criteria & "and " & _keys(ii) & "=" & hOP.ValAdapter(_kValues(ii), CType(hPlantable(_keys(ii)), s_planField).fieldType)
            Next
            _Criteria = _Criteria.TrimEnd(" and")
            For Each element As String In hPlantable.Keys
                If element.Split(".")(0) = pTablename Then
                    _Select = _Select & element & ","
                End If
            Next
            _Select = _Select.TrimEnd(",")
            Dim _query As String = "select " & _Select & " from " & IIf(pSystable, "", _CodAzi) & pTablename & " " & pTablename & _Criteria
            Dim ds As DataSet = hOP.esegui_query(_query, Globale.cn_dbext, mTipoDb)
            If ds.Tables(0).Rows.Count > 1 Then
                MsgBox("Trovati " & ds.Tables(0).Rows.Count.ToString.Trim & " record nella tabella " & pTablename & " chiave " & pValue)
            End If
            For Each _Row As DataRow In ds.Tables(0).Rows
                _hash = New Hashtable
                For Each _Field As String In _Select.Split(",")
                    Dim _Field_1 As String = _Field.Split(".")(1)
                    _fldVal.mValue = _Row(_Field_1)
                    _fldVal.mType = CType(hPlantable(_Field), s_planField).fieldType
                    _hash.Add(_Field_1, _fldVal)
                Next
            Next
            Return _hash
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "readAdhocTable")
        End Try
    End Function
    Public Function getSerial() As String
        '
        Try
            Dim _Autonum As Long
            Dim ds As DataSet
            Dim _key As String = "prog\SEDOC\'" & CAdhocDocVar.g_AdhocAzi.Trim & "'"
            Dim strsql As String = "select autonum from cpwarn where tablecode = " & hOP.ValAdapter(_key, TipoCampo.TChar)
            ds = hOP.esegui_query(strsql, Globale.cn_dbext, mTipoDb)
            If ds.Tables(0).Rows.Count > 0 Then
                _Autonum = ds.Tables(0).Rows(0).Item("autonum") + 1
                strsql = "update cpwarn set autonum = " & hOP.ValAdapter(_Autonum, TipoCampo.TLong) & " where tablecode = " & hOP.ValAdapter(_key, TipoCampo.TChar)
            Else
                _Autonum = 1
                strsql = "insert into cpwarn (tablecode, warncode, autonum) values ( " & _
                          hOP.ValAdapter(_key, TipoCampo.TChar) & ", " & _
                          hOP.ValAdapter(CAdhocDocVar.g_AdhocAzi.Trim, TipoCampo.TChar) & ", " & _
                hOP.ValAdapter(_Autonum, TipoCampo.TLong) & ")"
            End If
            ds = hOP.esegui_query(strsql, Globale.cn_dbext, mTipoDb)
            Return _Autonum.ToString.PadLeft(10, "0")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "getSerial")
        End Try
    End Function
    ''' <summary>
    ''' Get the progressive document number
    ''' pType : DV,FV...
    ''' </summary>
    ''' <param name="pCodese"></param>
    ''' <param name="pType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getProg(ByVal pCodese As String, ByVal pType As String, pAlfaDoc As String) As Integer
        '
        Try
            Dim _Prog As Integer
            Dim ds As DataSet
            Dim _AlfaDoc As String = pAlfaDoc.PadRight(10, " ")
            Dim _key As String = "prog\PRDOC\'" & CAdhocDocVar.g_AdhocAzi.Trim & "'\'" & pCodese & "'\'" & pType & "'\'" & _AlfaDoc & "'"
            Dim strsql As String = "select autonum from cpwarn where tablecode = " & hOP.ValAdapter(_key, TipoCampo.TChar)
            ds = hOP.esegui_query(strsql, Globale.cn_dbext, mTipoDb)
            If ds.Tables(0).Rows.Count > 0 Then
                _Prog = ds.Tables(0).Rows(0).Item("autonum") + 1
                strsql = "update cpwarn set autonum = " & hOP.ValAdapter(_Prog, TipoCampo.TLong) & " where tablecode = " & hOP.ValAdapter(_key, TipoCampo.TChar)
            Else
                _Prog = 1
                strsql = "insert into cpwarn (tablecode, warncode, autonum) values ( " & _
                          hOP.ValAdapter(_key, TipoCampo.TChar) & ", " & _
                          hOP.ValAdapter(CAdhocDocVar.g_AdhocAzi.Trim, TipoCampo.TChar) & ", " & _
                hOP.ValAdapter(_Prog, TipoCampo.TLong) & ")"
            End If
            ds = hOP.esegui_query(strsql, Globale.cn_dbext, mTipoDb)
            Return _Prog
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "getProg")
        End Try
    End Function
    ''' <summary>
    ''' Get the progressive customer code
    ''' pType : DV,FV...
    ''' </summary>
    ''' <param name="pCodese"></param>
    ''' <param name="pType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getProgCli(ByVal pKey As String) As Integer
        '
        Try
            Dim _Prog As Integer
            Dim _key As String = ""
            Dim ds As DataSet
            If pKey.Trim = "" Then
                _key = "prog\PRNUCL\'" & CAdhocDocVar.g_AdhocAzi.Trim & "'"
            Else
                _key = "prog\NUMCL\'" & CAdhocDocVar.g_AdhocAzi.Trim & "'\'" & pKey & "'"
            End If
            Dim strsql As String = "select autonum from cpwarn where tablecode = " & hOP.ValAdapter(_key, TipoCampo.TChar)
            ds = hOP.esegui_query(strsql, Globale.cn_dbext, mTipoDb)
            If ds.Tables(0).Rows.Count > 0 Then
                _Prog = ds.Tables(0).Rows(0).Item("autonum") + 1
                strsql = "update cpwarn set autonum = " & hOP.ValAdapter(_Prog, TipoCampo.TLong) & " where tablecode = " & hOP.ValAdapter(_key, TipoCampo.TChar)
            Else
                _Prog = 1
                strsql = "insert into cpwarn (tablecode, warncode, autonum) values ( " & _
                          hOP.ValAdapter(_key, TipoCampo.TChar) & ", " & _
                          hOP.ValAdapter(CAdhocDocVar.g_AdhocAzi.Trim, TipoCampo.TChar) & ", " & _
                hOP.ValAdapter(_Prog, TipoCampo.TLong) & ")"
            End If
            ds = hOP.esegui_query(strsql, Globale.cn_dbext, mTipoDb)
            Return _Prog
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "getProg")
        End Try
    End Function

    Private Function writeDocument_old(ByVal pTable As String, ByVal pTemplate As Hashtable) As Boolean

        Dim element As String = ""
        Try
            Dim _hVal As New Hashtable
            Dim _fieldType As New TipoCampo
            For Each element In pTemplate.Keys
                If element = pTable Then
                    Dim a As String = ""
                End If
                If element = "MVTIPRIG" Then
                    Dim a As String = "1"
                End If
                _fieldType = CType(Me.hPlantable(pTable & "." & element), s_planField).fieldType
                If Me.hFieldVal.ContainsKey(element) Then
                    'get value from array of values
                    _hVal.Add(element, hOP.ValAdapter(Me.hFieldVal(element), _fieldType, mTipoDb))
                Else
                    'get value from template
                    _hVal.Add(element, hOP.ValAdapter(pTemplate(element), _fieldType, mTipoDb))
                End If
            Next
            hOP.insert_query(CAdhocDocVar.g_AdhocAzi.Trim & pTable, _hVal, Globale.cn_dbext, mTipoDb)
            Me.hFieldVal.Clear()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message & " campo : " & element, MsgBoxStyle.Exclamation, "writeDocument")
            Return False
        End Try

    End Function
    Public Function writeDocument(ByVal pTable As String, ByVal pTemplate As Hashtable) As Boolean

        Dim element As String = ""
        Try
            Dim _hVal As New Hashtable
            Dim _fieldType As New TipoCampo
            For Each element In pTemplate.Keys
                If element = pTable Then
                    Dim a As String = ""
                End If
                If element = "MVCODDES" Then
                    Dim a As String = "1"
                End If
                _fieldType = CType(Me.hPlantable(pTable & "." & element), s_planField).fieldType
                If element.Trim = "MV__NOTE" Then
                    Dim a As Integer = 1
                End If
                If Me.hFieldVal.ContainsKey(element) Then
                    'get value from array of values
                    _hVal.Add(element, hOP.ValAdapter(Me.hFieldVal(element), _fieldType, mTipoDb))
                Else
                    'get value from template
                    If pTemplate(element).ToString <> "" Then
                        _hVal.Add(element, hOP.ValAdapter(pTemplate(element), _fieldType, mTipoDb))
                    End If
                End If
            Next
            hOP.insert_query(CAdhocDocVar.g_AdhocAzi.Trim & pTable, _hVal, Globale.cn_dbext, mTipoDb)
            Me.hFieldVal.Clear()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message & " campo : " & element, MsgBoxStyle.Exclamation, "writeDocument")
            Return False
        End Try

    End Function
    ''' <summary>
    ''' reads xml file and loads the datas into tablename
    ''' Fileds name are located into xmlfile  
    ''' If bUpdate is true, if the record already exists on the target table it's overwritten
    ''' instead if bUpdate is false and the target table record exists it's discarded
    ''' </summary>
    ''' <param name="xmlFilename"></param>
    ''' <param name="tableName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function loadTableFromXml(ByVal tableName As String, ByVal bUpdate As Boolean) As ArrayList

        Try
            Dim _array As New ArrayList

            ' This stack stores the directories to process.
            Dim queue As New Queue(Of Object)
            Dim qLiv As New Stack(Of Object)
            Dim htable As New Hashtable

            queue.Enqueue(tableName)
            qLiv.Push(tableName)
            For Each element As String In hForeignTable.Keys
                If element.Contains(tableName & "-") Then
                    queue.Enqueue(hForeignTable(element))
                    qLiv.Push(hForeignTable(element))
                End If
            Next

            ' Continue processing for each stacked table
            Do While (queue.Count > 0)
                ' Get top table string
                Dim _table As String = queue.Dequeue
                If _table = tableName Then
                    Continue Do
                End If
                For Each element As String In hForeignTable.Keys
                    If element.Contains(_table & "-") Then
                        queue.Enqueue(hForeignTable(element))
                        qLiv.Push(hForeignTable(element))
                    End If
                Next
                '
            Loop
            Do While (qLiv.Count > 0)
                Dim element As String = qLiv.Pop
                If Not htable.ContainsKey(element) Then
                    _array.Add(element)
                    htable.Add(element, element)
                    Dim _filename As String = "xml\\" & element & ".xml"
                    Me.loadTableFromXml_1(_filename, element, bUpdate)
                End If
            Loop
            Return _array
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' reads xml file and loads the datas into tablename
    ''' Fileds name are located into xmlfile  
    ''' If bUpdate is true, if the record already exists on the target table it's overwritten
    ''' instead if bUpdate is false and the target table record exists it's discarded
    ''' </summary>
    ''' <param name="xmlFilename"></param>
    ''' <param name="tableName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function loadTableFromXml_1(ByVal xmlFilename As String, ByVal tableName As String, ByVal bUpdate As Boolean) As Integer


        Try
            Dim _fieldType As New TipoCampo
            Dim _key As String = ""
            Dim _hVal As New Hashtable
            Dim _selectField As String = ""
            Dim _hPrimaryKey As New Hashtable

            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument

            Dim XmlNodo As Xml.XmlNodeList

            'carico il file
            Dim XmlLeggi As New XmlTextReader(xmlFilename)

            XmlLeggi.WhitespaceHandling = WhitespaceHandling.None

            Xmlfile.Load(XmlLeggi)

            'cerco tutti i nodi da leggere
            XmlNodo = Xmlfile.GetElementsByTagName("row")
            'Fill doc_mast structure
            For Each nodo As XmlNode In XmlNodo
                _hVal.Clear()
                _hPrimaryKey.Clear()
                'Find TableFields node
                For Each element As XmlNode In nodo.ChildNodes
                    _fieldType = CType(Me.hPlantable(tableName & "." & element.Name), s_planField).fieldType
                    _key = CType(Me.hPlantable(tableName & "." & element.Name), s_planField).key
                    If _key = "1" Then
                        _selectField = element.Name
                        _hPrimaryKey.Add(tableName & "." & element.Name, hOP.ValAdapter(htmlDecode(element.InnerText), _fieldType, mTipoDb))
                    End If
                    If tableName = "ART_ICOL" Then
                        Dim a As String = 1
                    End If
                    If element.InnerText.Trim <> "NULL" Then
                        _hVal.Add(element.Name, hOP.ValAdapter(htmlDecode(element.InnerText), _fieldType, mTipoDb))
                    End If
                Next
                'Build the select query to check record existence of 
                Dim _select_query As String = "SELECT " & _selectField & " FROM " & CAdhocDocVar.g_AdhocAzi.Trim & tableName & " AS " & tableName
                Dim _select_criteria As String = " WHERE "
                For Each element As String In _hPrimaryKey.Keys
                    _fieldType = CType(Me.hPlantable(tableName & "." & element), s_planField).fieldType
                    _select_criteria = _select_criteria & element & " = " & _hPrimaryKey(element) & " and "
                Next
                _select_criteria = _select_criteria.Substring(0, _select_criteria.Length - 5)
                _select_query = _select_query & _select_criteria
                Dim ds As DataSet = hOP.esegui_query(_select_query)
                If ds.Tables(0).Rows.Count > 0 Then
                    hOP.update_query(CAdhocDocVar.g_AdhocAzi.Trim & tableName, _hVal, Globale.cn, mTipoDb, _hPrimaryKey)
                Else
                    hOP.insert_query(CAdhocDocVar.g_AdhocAzi.Trim & tableName, _hVal, Globale.cn, mTipoDb)
                End If
                '
            Next
            XmlLeggi.Close()
            XmlLeggi = Nothing
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "loadTableFromXml_1, tabella" & tableName)
        End Try

    End Function
    ''' <summary>
    ''' Export tablename to xml file and all linked table
    ''' Return arraylist of exported table
    ''' </summary>
    ''' <param name="tablename"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function exportTabletoxml(ByVal tablename As String) As ArrayList
        Try

            ' This stack stores the directories to process.
            Dim _array As New ArrayList
            Dim queue As New Queue(Of Object)
            Dim htable As New Hashtable

            queue.Enqueue(tablename)
            htable.Add(tablename, tablename)
            _array.Add(tablename)
            For Each element As String In hForeignTable.Keys
                If element.Contains(tablename & "-") Then
                    queue.Enqueue(hForeignTable(element))
                End If
            Next

            ' Continue processing for each stacked table
            Do While (queue.Count > 0)
                ' Get top table string
                Dim _table As String = queue.Dequeue
                If _table = tablename Then
                    Continue Do
                End If
                If Not htable.ContainsKey(_table) Then
                    _array.Add(_table)
                    htable.Add(_table, _table)
                End If
                For Each element As String In hForeignTable.Keys
                    If element.Contains(_table & "-") Then
                        queue.Enqueue(hForeignTable(element))
                    End If
                Next
                '
            Loop
            For Each element As String In htable.Keys
                exportTableToXml_1(element)
            Next
            Return _array
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Export to Xml")
            Return Nothing
        End Try

    End Function
    Private Function exportTableToXml_1(ByVal tableName As String) As Integer

        Dim hFields As New Hashtable
        Dim sw As System.IO.FileStream
        Dim stringa As String = ""
        Dim _tablename As String = ""
        Try

            For Each element As String In Me.hPlantable.Keys
                _tablename = element.Split(".")(0)
                If _tablename = tableName Then
                    hFields.Add(element, CType(Me.hPlantable(element), s_planField).fieldName)
                End If
            Next
            Dim _queryName As String = tableName & ".vqr"
            'before to build the select query, read if query for table exists
            Dim strsql As String = Me.loadQuery(_queryName)
            If strsql <> "" Then
                Dim a As String = "1"
            End If
            If strsql = "" Then
                'build the select query
                strsql = "select "
                For Each element As String In hFields.Keys
                    strsql = strsql & hFields(element) & ","
                Next
                strsql = strsql.TrimEnd(",") & " FROM " & CAdhocDocVar.g_AdhocAzi.Trim & tableName
            End If
            '
            Dim fileexp As String = "xml\\" & tableName & ".xml"
            '
            sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
            Dim filewriter As New System.IO.StreamWriter(sw)
            '
            stringa = "<?xml version=""1.0"" standalone=""yes""?>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = "<dataroot>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            '
            Dim ds As DataSet = hOP.esegui_query(strsql)
            For Each _row As DataRow In ds.Tables(0).Rows
                '
                stringa = "<row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                Dim _stringa As String = ""
                For Each _col As String In hFields.Keys
                    '
                    Dim __col As String = hFields(_col)
                    stringa = "<" & hFields(_col) & ">" & htmlEncode(_row(__col)) & "</" & hFields(_col) & ">" & Chr(13) & Chr(10)
                    filewriter.Write(stringa)
                    filewriter.Flush()
                    '
                Next
                '
                stringa = "</row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
            Next
            stringa = "</dataroot>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            filewriter.Close()
            sw.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "ExportTableToXml_1")
        End Try
    End Function
    Private Function loadQuery(ByVal pNamequery As String) As String
        Try
            Dim sr As System.IO.FileStream
            Dim Tablelist As New ArrayList
            Dim stringaQuery As String = ""
            Dim queryPath As String = "query\"
            If System.IO.File.Exists(queryPath & pNamequery) Then
                sr = New System.IO.FileStream(queryPath & pNamequery, IO.FileMode.Open, IO.FileAccess.Read)
                Dim filereader As New System.IO.StreamReader(sr)
                While filereader.Peek > -1
                    stringaQuery = stringaQuery & " " & filereader.ReadLine
                End While
                Return stringaQuery
            Else
                Return ""
            End If
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "loadQuery")
            Return ""
        End Try

    End Function
    Public Function getVal(ByVal pObj As Object) As Object
        Dim _obj As Object

        If CType(pObj, s_tableValue).mValue Is Nothing Then
            Select Case CType(pObj, s_tableValue).mType
                Case TipoCampo.TChar
                    _obj = ""
                Case TipoCampo.TBool
                    _obj = False
                Case TipoCampo.TCur, TipoCampo.TInt, TipoCampo.TLong
                    _obj = 0
                Case TipoCampo.TData
                    _obj = vbNull
            End Select
            Return _obj
        Else
            Return CType(pObj, s_tableValue).mValue
        End If
        If IsDBNull(CType(pObj, s_tableValue).mValue) Then
            Select Case CType(pObj, s_tableValue).mType
                Case TipoCampo.TChar
                    _obj = ""
                Case TipoCampo.TBool
                    _obj = False
                Case TipoCampo.TCur, TipoCampo.TInt, TipoCampo.TLong
                    _obj = 0
                Case TipoCampo.TData
                    _obj = vbNull
            End Select
            Return _obj
        Else
            Return CType(pObj, s_tableValue).mValue
        End If

    End Function
    Public Function htmlEncode(ByVal pStringa As Object) As String

        If pStringa Is DBNull.Value Then
            Return "NULL"
        End If
        If String.IsNullOrEmpty(pStringa) Then
            Return "NULL"
        End If
        Dim Testo As String = pStringa
        Dim lStringa As String = ""
        Try
            Testo = Testo.Replace("&", "&amp;")
            Testo = Testo.Replace("'", "&apos;")
            Testo = Testo.Replace("""", "&quot;")
            Testo = Testo.Replace("<", "&lt;")
            Testo = Testo.Replace(">", "&gt;")
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
        Return Testo
    End Function
    Public Function htmlDecode(ByVal pStringa As String) As String


        Dim Testo As String = pStringa
        Dim lStringa As String = ""
        Try
            lStringa = lStringa.Replace("&quot;", """")
            lStringa = lStringa.Replace("&apos;", "'")
            lStringa = lStringa.Replace("&amp;", "&")
            lStringa = lStringa.Replace("&lt;", "<")
            lStringa = lStringa.Replace("&gt;", ">")
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
        Return lStringa
    End Function
    Private Function codifica(ByVal pStringa As String) As String
        Dim encodedBytes As Byte()
        Dim value As Encoding
        Dim riga As String
        Dim decodedString As String = ""
        Try
            Dim utf8 As New UTF8Encoding()
            riga = pStringa

            riga = htmlEncode(riga)
            encodedBytes = utf8.GetBytes(riga)
            decodedString = utf8.GetString(encodedBytes)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Codifica")
        End Try
        Return decodedString
    End Function
    Public Function getTablename(ByVal tablename As String) As String
        Try
            Return CAdhocDocVar.g_AdhocAzi.Trim.ToLower & tablename.ToLower
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "getTablename")
        End Try
    End Function

End Class
