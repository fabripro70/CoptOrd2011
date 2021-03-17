Imports System.Globalization
Imports ClSLib
Imports System.Data.SqlClient
Imports Oracle.DataAccess.Client
Imports MySql.Data.MySqlClient
Imports system.reflection
Imports System.io
Public Enum rw As Integer
    Read = 0
    Write = 1
End Enum
Public Enum TipoCampo As Integer
    TChar = 10
    TInt = 3
    TData = 8
    TCur = 5
    TLong = 4
    TBool = 1
End Enum
Public Structure sColonna
    Dim m_Tipo As String
    Dim m_Precision As Integer
    Dim m_Scale As Integer
    Dim m_esist As Boolean
End Structure
Public Structure OrdineCampi
    Public m_elenco As Hashtable
    Public m_tabindex As ArrayList
End Structure
Public Structure StructCampi
    Public SIndex As String
    Public SNome As String
    Public STipo As TipoCampo
    Public Slen As Integer
    Public SValue As String
    Public SCampoForm As String
    Public STabellaJoin As String
    Public SChiaveJoin As String
    Public SCampoDes As String
    Public SOpzione As String 'KEYMASTER = Il valore da registrare si trova nel campo rappresentato da ChiaveMaster
    'nella struttura schede.
End Structure
Public Structure recJoin 'Serve per memorizzare tutti i campi che hanno bisogno di join, in popolacapi, per essere estratti
    Dim Tabella As String
    Dim campoSx As String
    Dim campoDx As String
End Structure

Public Class COpeFile
    Enum TOpe
        PG_ULTPROG = 0
        PG_NXTPROG = 1
    End Enum
    Private Structure l_Schede
        Dim NomeScheda As String
        Dim Scheda As Object
        Dim Tabella As String
        Dim ElencoTabIndex As ArrayList    'Elenco di tutti i tab index
        Dim ElencoCampi As Hashtable       'chiave=tabindex
        Dim StrutturaTabella As ArrayList      'contiene il contenuto del file db1 
        'ogni elemento va messo dentro alla StructCampi
        Dim masterHookkey As String        'Campo chiave di collegamento con la tabella master 
        'usato solo nei from master/detail tabella detail 
        Dim ChiavePrimaria As String       ' Nome chiave primaria
        Dim ValChiavePrimaria As String
        Dim ChiaveRicerca As String           'Nome della chiave primaria della tabella es: txNumrec
        Dim ValChiaveRicerca As String         'Valore della chiave primaria.
        Dim ChiaveMaster As Object            'Esempio : nel dettaglio fattura ci va anche il numrec dell'anagrafica fatt.
        Dim MainQuery As String             ' Query principale eseguita da carica_griglia nelle form con griglia
        Dim OrderBy As String               ' Criterio di ordinamento di MainQuery
        Dim mainContainer As Object         ' Contenitore principale del form, di solito è Tab
    End Structure
    Public m_Struttura As String = "struttura.db1"

    Dim da As New Object
    'Dim da As New SqlClient.SqlDataAdapter
    Dim stringa As String
    'Public tab_campi()
    Function imposta_connessione(ByVal pconnectionString As String)
        Try
            Select Case Trim$(Globale.TipoDb)
                Case "SQLSERVER"
                    cn = New SqlClient.SqlConnection
                    da = New SqlClient.SqlDataAdapter
                    If String.IsNullOrEmpty(pconnectionstring) Then
                        If Globale.User.Trim() <> "" Then
                            cn.ConnectionString = "server=" & Globale.Server & ";" & _
                                                  "user id=" & Globale.User & ";" & _
                                                  "password=" & Globale.Password & ";" & _
                                                  "database=" & Globale.DataBase & ";pooling=false"
                        Else
                            cn.ConnectionString = "server=" & Globale.Server & ";" & _
                                                  "database=" & Globale.DataBase & ";pooling=false" & _
                                                  ";Trusted_Connection=Yes"
                        End If
                    Else
                        cn.connectionstring = pconnectionstring
                    End If
                    cn.Open()
                Case "ORACLE"
                    cn = New OracleConnection
                    da = New OracleDataAdapter
                    If String.IsNullOrEmpty(pconnectionString) Then
                        cn.ConnectionString = "user id=" & Globale.User & ";" & _
                                              "password=" & Globale.Password & ";" & _
                                              "Data source=" & Globale.Server
                        'cn.ConnectionString = "User Id=fabrizio;Password=pasima;Data Source=XE"
                    Else
                        cn.connectionstring = pconnectionString
                    End If
                    cn.open()
                Case "MYSQL"
                    cn = New MySqlConnection
                    da = New MySqlDataAdapter
                    If String.IsNullOrEmpty(pconnectionString) Then
                        cn.ConnectionString = "server=" & Globale.Server & ";" & _
                                              "UID=" & Globale.User & ";" & _
                                              "PWD=" & Globale.Password & ";" & _
                                              "database=" & Globale.DataBase & ";" & _
                                              "Data source=" & Globale.Server
                        cn.open()
                    Else
                        cn.connectionstring = pconnectionString
                    End If
            End Select
            ' Per il formato DBF : DBF Data Source=C:\\AZW32\\DB\\001
            If Trim(Globale.FonteExt_1) <> "" Then
                Select Case Trim(Globale.FonteExt_1.Substring(0, 4))
                    Case "DBF"
                        cn_dbf.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                                                  Globale.FonteExt_1.Substring(4) & ";Extended Properties=DBASE IV"
                        cn_dbf.Open()
                    Case "MDB"
                        cn_dbf.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                                                  Globale.FonteExt_1.Substring(4)
                        cn_dbf.Open()
                    Case Else
                        Dim sCn() As String = Globale.FonteExt_1.Trim.Split(";")
                        If sCn.Length = 0 Then
                            cn_dbf.ConnectionString = Trim(Globale.FonteExt_1)
                            cn_dbf.Open()
                        End If
                        Select Case sCn(0).Trim.Split("=")(1)
                            Case "SQLSERVER"
                                cn_dbext = New SqlClient.SqlConnection
                            Case "ORACLE"
                                cn_dbext = New OracleConnection
                            Case "MYSQL"
                                cn_dbext = New MySqlConnection
                        End Select
                        Try
                            cn_dbext.ConnectionString = sCn(1).Trim & ";" & sCn(2).Trim & ";" & sCn(3).Trim & ";" & sCn(4).Trim
                        Catch ex As System.Exception
                            MsgBox("Errore nella costruzione della stringa" & vbCr & _
                                   "di connessione al db esterno", MsgBoxStyle.Critical, "Imposta connessione")
                        End Try
                        cn_dbext.Open()
                End Select
            End If
        Catch ex As MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As OracleException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As OleDb.OleDbException
            If ex.ErrorCode = -2147467259 Then
                MsgBox("Collegamento alla fonte dati 1 assente!", MsgBoxStyle.Information, "Attenzione!")
                Globale.ConnessoG1 = False
            Else
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            End If
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        End Try
        Return 0
    End Function
    Public Function BeginTrans()
        Try
            Select Case Globale.TipoDb
                Case "SQLSERVER"
                Case "ORACLE"
                Case "MYSQL"
                    esegui_query("START TRANSACTION")
            End Select
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As OracleException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As System.Exception
        End Try
    End Function
    Public Function CommitTrans()
        Try
            Select Case Globale.TipoDb
                Case "SQLSERVER"
                Case "ORACLE"
                Case "MYSQL"
                    esegui_query("COMMIT")
            End Select
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As OracleException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As System.Exception
        End Try
    End Function
    Public Function RollbackTrans()
        Try
            Select Case Globale.TipoDb
                Case "SQLSERVER"
                Case "ORACLE"
                Case "MYSQL"
                    esegui_query("ROLLBACK")
            End Select
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As OracleException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Imposta Connessione!")
            Application.Exit()
        Catch ex As System.Exception
        End Try
    End Function
    Function leggi_ordine(ByVal pannello As Object, ByVal Str As Object) As OrdineCampi
        Try
            Dim Hs As New Hashtable
            Dim ar As New ArrayList
            Dim strc As New StructCampi
            Dim Campo As StructCampi
            Dim str_schede As New Schede
            str_schede = Str
            Dim listFld As List(Of Object) = GetRecursiveFields(pannello)
            For Each ob As Control In listFld
                For Each fld As StructCampi In str_schede.StrutturaTabella
                    If fld.SCampoForm.ToUpper = ob.Name.ToUpper And fld.SCampoForm.Trim <> "" Then
                        Campo = fld
                        Hs.Add(ob.TabIndex.ToString, fld)
                        ar.Add(ob.TabIndex)
                        Exit For
                    End If
                Next
                '            If TypeOf ob Is TextBox Or TypeOf ob Is ComboBox Then
                'Hs.Add(ob.TabIndex.ToString, ob)
                'ar.Add(ob.TabIndex)
                'End If
            Next
            leggi_ordine.m_elenco = Hs
            leggi_ordine.m_tabindex = ar
        Catch ex As SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Leggi Ordine!")
        End Try
    End Function
    Function leggi_ordine_dialog(ByVal pannello As Object, ByVal Str As Object) As OrdineCampi
        Try
            Dim Hs As New Hashtable
            Dim ar As New ArrayList
            Dim strc As New StructCampi
            Dim fld As StructCampi
            Dim oText As New ClSLib.CTextBoxEx
            Dim Campo As StructCampi
            Dim str_schede As New Schede
            str_schede = Str

            For Each ob As Control In pannello.controls
                If TypeOf ob Is ClSLib.CTextBoxEx Then
                    If Not IsNothing(CType(ob, ClSLib.CTextBoxEx).LinkDati.TabellaCollegata) Then
                        fld.SCampoForm = ob.Name
                        fld.STabellaJoin = CType(ob, ClSLib.CTextBoxEx).LinkDati.TabellaCollegata
                        Hs.Add(ob.TabIndex.ToString, fld)
                        ar.Add(ob.TabIndex)
                    End If
                End If
            Next
            leggi_ordine_dialog.m_elenco = Hs
            leggi_ordine_dialog.m_tabindex = ar
        Catch ex As SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Leggi Ordine!")
        End Try
    End Function
    Overloads Function leggi_struttura(ByVal Tabella As String) As ArrayList
        Dim riga As String
        Dim ar As New ArrayList
        Dim MainTag As String = ""
        Dim EndMainTag As String = ""
        If Globale.gSysTable.Contains(Tabella.ToLower) Then
            MainTag = "<SYSTABLE=" & Trim(Tabella).ToUpper & ">"
            EndMainTag = "</TABLE>"
        Else
            MainTag = "<TABLE=" & Trim(Tabella).ToUpper & ">"
            EndMainTag = "</TABLE>"
        End If
        '        Dim EndMainTag As String = "</" & Trim(Tabella) & ">"
        Dim StartRead As Boolean = False
        Dim StartReadField As Boolean = False
        Dim StartReadIndex As Boolean = False
        Dim campi
        Dim sf As New StructCampi
        Try
            Dim file_config As String = percorsoApp & "\" & Me.m_Struttura
            Dim sr As System.IO.FileStream
            sr = New System.IO.FileStream(file_config, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            While filereader.Peek > -1
                riga = filereader.ReadLine.ToUpper
                If riga.Trim() <> "" Then
                    If riga.Contains("'") Then
                        Continue While
                    End If
                    If riga.Contains(MainTag) Then
                        StartRead = True
                        Continue While
                    End If
                    If Not StartRead Then Continue While
                    If riga.Contains(EndMainTag) Then Exit While
                    If riga.Contains("<FIELDS>") Then
                        StartReadField = True
                        Continue While
                    End If
                    If riga.Contains("</FIELDS>") Then
                        StartReadField = False
                        Continue While
                    End If
                    If StartReadField Then
                        campi = riga.Split(",")
                        sf.SIndex = 0
                        sf.SCampoForm = ""
                        sf.SChiaveJoin = ""
                        sf.Slen = 0
                        sf.SNome = ""
                        sf.STabellaJoin = ""
                        sf.STipo = 0
                        sf.SValue = 0
                        sf.SCampoDes = ""
                        sf.SOpzione = ""
                        For ii As Integer = 0 To UBound(campi)
                            Select Case ii
                                Case 0
                                    sf.SIndex = CTran(campi(0), 0)
                                Case 1
                                    sf.SNome = CTran(campi(1), "")
                                    If sf.SNome.Contains("(") Then
                                        Dim stCampo()
                                        stCampo = sf.SNome.Split("(")
                                        If stCampo.Length <> 0 Then
                                            sf.SNome = stCampo(0)
                                            Dim lung As Int16
                                            Dim pos As Int16
                                            pos = stCampo(1).ToString.LastIndexOf(")")
                                            lung = stCampo(1).ToString.Length
                                            'se lung = pos+1, allora significa che la parentesi è l' ultimo carattere
                                            If lung = (pos + 1) Then
                                                sf.SOpzione = stCampo(1).ToString.Remove(pos)
                                            Else
                                                Throw New SystemException("Sintassi errata nella riga : " & vbCr & riga.ToString)
                                            End If
                                        End If

                                    End If
                                Case 2
                                    sf.SCampoForm = CTran(campi(2), "")
                                Case 3
                                    sf.STipo = CTran(campi(3), 0)
                                Case 4
                                    sf.Slen = CTran(campi(4), 0)
                                    '                            sf.Slen = IIf(IsDBNull(campi(4)), 0, campi(4))
                                Case 5
                                    sf.STabellaJoin = CTran(campi(5), "").ToString.ToLower
                                    If sf.STabellaJoin <> "" Then
                                        sf.SCampoDes = sf.SNome = sf.SNome & "_d"
                                    End If
                                Case 6
                                    sf.SChiaveJoin = CTran(campi(6), "")
                            End Select
                        Next ii
                        ar.Add(sf)
                    End If
                End If
            End While
            filereader.Close()
            sr.Close()
            leggi_struttura = ar
        Catch ex As SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Leggi Struttura")
            leggi_struttura = Nothing
        End Try
    End Function
    Overloads Function leggi_struttura(ByVal Tabella As String, ByRef pSchede As Schede) As ArrayList
        Dim riga As String
        Dim ar As New ArrayList
        Dim MainTag As String = ""
        Dim EndMainTag As String = ""
        If Globale.gSysTable.Contains(Tabella.ToLower) Then
            MainTag = "<SYSTABLE=" & Trim(Tabella).ToUpper & ">"
            EndMainTag = "</TABLE>"
        Else
            MainTag = "<TABLE=" & Trim(Tabella).ToUpper & ">"
            EndMainTag = "</TABLE>"
        End If
        '        Dim EndMainTag As String = "</" & Trim(Tabella) & ">"
        Dim StartRead As Boolean = False
        Dim StartReadField As Boolean = False
        Dim StartReadIndex As Boolean = False
        Dim campi
        Dim sf As New StructCampi
        Try
            Dim file_config As String = percorsoApp & "\" & Me.m_Struttura
            Dim sr As System.IO.FileStream
            sr = New System.IO.FileStream(file_config, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            While filereader.Peek > -1
                riga = filereader.ReadLine
                If riga.Trim() <> "" Then
                    If riga.Contains("'") Then
                        Continue While
                    End If
                    If riga.Contains(MainTag) Then
                        StartRead = True
                        Continue While
                    End If
                    If Not StartRead Then Continue While
                    If riga.Contains(EndMainTag) Then Exit While
                    If riga.Contains("<MAINQUERY>") Then
                        Dim startkk As Integer = 13
                        Dim kk As Integer = riga.IndexOf("</MAINQUERY>")
                        Dim lenStr As Integer = kk - startkk
                        Dim wMainquery As String = riga.Substring(startkk, lenStr)
                        pSchede.MainQuery = wMainquery
                    End If
                    If riga.Contains("<FIELDPRIMARYKEY>") Then
                        Dim startkk As Integer = 19
                        Dim kk As Integer = riga.IndexOf("</FIELDPRIMARYKEY>")
                        Dim lenStr As Integer = kk - startkk
                        Dim wFieldprimarykey As String = riga.Substring(startkk, lenStr)
                        pSchede.ChiavePrimaria = wFieldprimarykey
                    End If
                    If riga.Contains("<FIELDTBLKEY>") Then
                        Dim startkk As Integer = 15
                        Dim kk As Integer = riga.IndexOf("</FIELDTBLKEY>")
                        Dim lenStr As Integer = kk - startkk
                        Dim wFieldtblkey As String = riga.Substring(startkk, lenStr)
                        pSchede.ChiaveRicerca = wFieldtblkey
                    End If
                    If riga.Contains("<ORDER>") Then
                        Dim startkk As Integer = 9
                        Dim kk As Integer = riga.IndexOf("</ORDER>")
                        Dim lenStr As Integer = kk - startkk
                        Dim wOrder As String = riga.Substring(startkk, lenStr)
                        pSchede.OrderBy = wOrder
                    End If
                    If riga.Contains("<FIELDS>") Then
                        StartReadField = True
                        Continue While
                    End If
                    If riga.Contains("</FIELDS>") Then
                        StartReadField = False
                        Continue While
                    End If
                    If StartReadField Then
                        campi = riga.Split(",")
                        sf.SIndex = 0
                        sf.SCampoForm = ""
                        sf.SChiaveJoin = ""
                        sf.Slen = 0
                        sf.SNome = ""
                        sf.STabellaJoin = ""
                        sf.STipo = 0
                        sf.SValue = 0
                        sf.SCampoDes = ""
                        sf.SOpzione = ""
                        For ii As Integer = 0 To UBound(campi)
                            Select Case ii
                                Case 0
                                    sf.SIndex = CTran(campi(0), 0)
                                Case 1
                                    sf.SNome = CTran(campi(1), "")
                                    If sf.SNome.Contains("(") Then
                                        Dim stCampo()
                                        stCampo = sf.SNome.Split("(")
                                        If stCampo.Length <> 0 Then
                                            sf.SNome = stCampo(0)
                                            pSchede.masterHookkey = sf.SNome
                                            Dim lung As Int16
                                            Dim pos As Int16
                                            pos = stCampo(1).ToString.LastIndexOf(")")
                                            lung = stCampo(1).ToString.Length
                                            'se lung = pos+1, allora significa che la parentesi è l' ultimo carattere
                                            If lung = (pos + 1) Then
                                                sf.SOpzione = stCampo(1).ToString.Remove(pos)
                                            Else
                                                Throw New SystemException("Sintassi errata nella riga : " & vbCr & riga.ToString)
                                            End If
                                        End If

                                    End If
                                Case 2
                                    sf.SCampoForm = CTran(campi(2), "")
                                Case 3
                                    sf.STipo = CTran(campi(3), 0)
                                Case 4
                                    sf.Slen = CTran(campi(4), 0)
                                    '                            sf.Slen = IIf(IsDBNull(campi(4)), 0, campi(4))
                                Case 5
                                    sf.STabellaJoin = CTran(campi(5), "").ToString.ToLower
                                    'If sf.STabellaJoin <> "" Then
                                    'sf.SCampoDes = sf.SNome = sf.SNome & "_d"
                                    'End If
                                Case 6
                                    sf.SChiaveJoin = CTran(campi(6), "")
                            End Select
                        Next ii
                        ar.Add(sf)
                    End If
                End If
            End While
            filereader.Close()
            sr.Close()
            leggi_struttura = ar
        Catch ex As SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Leggi Struttura")
            leggi_struttura = Nothing
        End Try
    End Function

    Function controlla_database() As ArrayList
        Dim ds As New DataSet
        Dim riga As String
        Dim TableExist As Boolean = False
        Dim FieldExist As Boolean = False
        Dim NomeTabella As String = ""
        Dim StringaCreate As String
        Dim stringaAdd As String
        Dim Lunghezza As String
        Dim TipoC As TipoCampo
        Dim TipoStr As String
        Dim NomeCampo As String
        Dim stringaPrimaryOracle As String = ""
        Dim stringaAutonumOracle As String = ""
        Dim stringaSeqOracle As String = ""
        Dim StringaStandard As String = "COLLATE SQL_Latin1_General_CP1_CI_AS"

        Dim MainTag As String = ""
        Dim EndMainTag As String = ""
        '        Dim EndMainTag As String = "</" & Trim(Tabella) & ">"
        Dim StartRead As Boolean = False
        Dim StartReadField As Boolean = False
        Dim StartReadIndex As Boolean = False
        Dim campi
        Dim campiIdx
        Dim sf As New StructCampi
        Dim wColonna As sColonna
        Try
            If Globale.MultiAzi Then
                If Globale.CodAzi = "" Then 'Controlla solo tabelle di sistema
                    MainTag = "<SYSTABLE"
                    EndMainTag = "</TABLE>"
                Else
                    MainTag = "<TABLE"  'Controlla tutte le altre tabelle
                    EndMainTag = "</TABLE>"
                End If
            Else
                MainTag = "<TABLE"  'Controlla tutte le altre tabelle
                EndMainTag = "</TABLE>"
            End If
            Globale.gMappaCampi.Clear()
            Dim file_config As String = percorsoApp & "\" & Me.m_Struttura
            Dim sr As System.IO.FileStream
            sr = New System.IO.FileStream(file_config, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            While filereader.Peek > -1
                riga = filereader.ReadLine
                If riga.Contains(EndMainTag) Then
                    TableExist = False
                End If
                If riga.Trim() <> "" Then
                    If riga.Contains(MainTag) Then
                        StartRead = True
                        NomeTabella = Me.EstraiParametro(riga).ToString.ToLower
                        If Globale.CodAzi = "" Then
                            Globale.gSysTable.Add(NomeTabella, NomeTabella)
                        Else
                            Globale.gTable.Add(NomeTabella, NomeTabella)
                        End If
                        TableExist = Me.EsisteTab(Globale.CodAzi & NomeTabella)
                        If Not TableExist Then
                            Select Case Globale.TipoDb
                                Case "SQLSERVER"
                                    StringaCreate = "CREATE TABLE [dbo].[" & Globale.CodAzi & NomeTabella & "] (" & _
                                                  "[NUMREC][numeric] (18,0) IDENTITY (1, 1) PRIMARY KEY NOT NULL"
                                Case "ORACLE"
                                    StringaCreate = "CREATE TABLE " & Globale.CodAzi & NomeTabella.ToLower & " (" & _
                                                  "NUMREC NUMBER (18) NOT NULL"
                                    stringaPrimaryOracle = "ALTER TABLE " & Globale.CodAzi & NomeTabella.ToLower & " ADD (CONSTRAINT " & Globale.CodAzi & NomeTabella & "_pk PRIMARY KEY (NUMREC))"
                                    stringaAutonumOracle = "CREATE OR REPLACE TRIGGER " & NomeTabella.ToLower & "_trig " & _
                                                           "BEFORE INSERT ON " & Globale.CodAzi & NomeTabella.ToLower & _
                                                           " FOR EACH ROW " & _
                                                           "WHEN (new.NUMREC IS NULL) " & _
                                                           "BEGIN " & _
                                                           "Select " & Globale.CodAzi & NomeTabella.ToLower & "_seq.NEXTVAL " & _
                                                           "INTO   :new.NUMREC " & _
                                                           "FROM   dual; " & _
                                                           "END;"
                                    stringaSeqOracle = "CREATE SEQUENCE " & Globale.CodAzi & NomeTabella.ToLower & "_seq"
                                Case "MYSQL"
                                    StringaCreate = "CREATE TABLE `" & Globale.CodAzi & NomeTabella.ToLower & "` (" & _
                                                  "`NUMREC` double(18,0) NOT NULL AUTO_INCREMENT"
                            End Select

                        End If
                        Continue While
                    End If
                    If Not StartRead Then Continue While
                    If riga.Contains(EndMainTag) Then
                        StartRead = False
                    End If
                    If riga.Contains(EndMainTag) Then Continue While
                    If riga.Contains("<FIELDS>") Then
                        StartReadField = True
                        Continue While
                    End If
                    If riga.Contains("</FIELDS>") Then
                        StartReadField = False
                        If Not TableExist Then
                            Select Case Globale.TipoDb
                                Case "SQLSERVER"
                                    StringaCreate = StringaCreate & ") ON [PRIMARY]"
                                    opConn.esegui_query(StringaCreate)
                                    StringaCreate = ""
                                Case "ORACLE"
                                    StringaCreate = StringaCreate & ")"
                                    opConn.esegui_query(StringaCreate)
                                    opConn.esegui_query(stringaPrimaryOracle)
                                    opConn.esegui_query(stringaSeqOracle)
                                    opConn.esegui_query(stringaAutonumOracle)
                                    StringaCreate = ""
                                Case "MYSQL"
                                    StringaCreate = StringaCreate & ", PRIMARY KEY (`NUMREC`)) ENGINE=InnoDB DEFAULT CHARSET=latin1;"
                                    opConn.esegui_query(StringaCreate)
                                    StringaCreate = ""
                            End Select
                        End If
                        Continue While
                    End If
                    If riga.Contains("<INDEX>") Then
                        StartReadIndex = True
                        Continue While
                    End If
                    If riga.Contains("</INDEX>") Then
                        StartReadIndex = False
                        Continue While
                    End If
                    If StartReadField Then
                        If riga.Contains("'") Then
                            Continue While
                        End If
                        campi = riga.Split(",")
                        sf.SIndex = 0
                        sf.SCampoForm = ""
                        sf.SChiaveJoin = ""
                        sf.Slen = 0
                        sf.SNome = ""
                        sf.STabellaJoin = ""
                        sf.STipo = 0
                        sf.SValue = 0
                        sf.SOpzione = ""
                        For ii As Integer = 0 To UBound(campi)
                            Select Case ii
                                Case 0
                                    sf.SIndex = CTran(campi(0), 0)
                                Case 1
                                    sf.SNome = CTran(campi(1), "")
                                    If sf.SNome.Contains("(") Then
                                        Dim stCampo()
                                        stCampo = sf.SNome.Split("(")
                                        If stCampo.Length <> 0 Then
                                            sf.SNome = stCampo(0)
                                            Dim lung As Int16
                                            Dim pos As Int16
                                            pos = stCampo(1).ToString.LastIndexOf(")")
                                            lung = stCampo(1).ToString.Length
                                            'se lung = pos+1, allora significa che la parentesi è l' ultimo carattere
                                            If lung = (pos + 1) Then
                                                sf.SOpzione = stCampo(1).ToString.Remove(pos)
                                            Else
                                                Throw New SystemException("Sintassi errata nella riga : " & vbCr & riga.ToString)
                                            End If
                                        End If
                                    End If
                                Case 2
                                    sf.SCampoForm = CTran(campi(2), "")
                                Case 3
                                    sf.STipo = CTran(campi(3), 0)
                                Case 4
                                    sf.Slen = CTran(campi(4), 0)
                                    '                            sf.Slen = IIf(IsDBNull(campi(4)), 0, campi(4))
                                Case 5
                                    sf.STabellaJoin = CTran(campi(5), "")
                                Case 6
                                    sf.SChiaveJoin = CTran(campi(6), "")
                            End Select
                        Next ii
                        Lunghezza = "(" & sf.Slen & ")"
                        Select Case Globale.TipoDb
                            Case "SQLSERVER"
                                NomeCampo = "[" & sf.SNome & "]"
                            Case "ORACLE"
                                NomeCampo = sf.SNome
                            Case "MYSQL"
                                NomeCampo = "`" & sf.SNome & "`"
                        End Select
                        TipoC = sf.STipo
                        Select Case Globale.TipoDb
                            Case "SQLSERVER"
                                Select Case TipoC
                                    Case TipoCampo.TChar
                                        TipoStr = "[char]"
                                        Lunghezza = Lunghezza & " " & StringaStandard
                                    Case TipoCampo.TCur
                                        TipoStr = "[money]"
                                        Lunghezza = ""
                                    Case TipoCampo.TData
                                        TipoStr = "[datetime]"
                                        Lunghezza = ""
                                    Case TipoCampo.TInt
                                        TipoStr = "[int]"
                                        Lunghezza = ""
                                    Case TipoCampo.TLong
                                        TipoStr = "[numeric]"
                                        Lunghezza = "(18,0)"
                                End Select
                            Case "ORACLE"
                                Select Case TipoC
                                    Case TipoCampo.TChar
                                        TipoStr = "CHAR"
                                        Lunghezza = Lunghezza
                                    Case TipoCampo.TCur
                                        TipoStr = "NUMBER"
                                        Lunghezza = "(18,5)"
                                    Case TipoCampo.TData
                                        TipoStr = "DATE"
                                        Lunghezza = ""
                                    Case TipoCampo.TInt
                                        TipoStr = "NUMBER"
                                        Lunghezza = "(5,0)"
                                    Case TipoCampo.TLong
                                        TipoStr = "NUMBER"
                                        Lunghezza = "(18,0)"
                                End Select
                            Case "MYSQL"
                                Select Case TipoC
                                    Case TipoCampo.TChar
                                        TipoStr = "varchar"
                                        Lunghezza = Lunghezza
                                    Case TipoCampo.TCur
                                        TipoStr = "decimal"
                                        Lunghezza = "(18,5)"
                                    Case TipoCampo.TData
                                        TipoStr = "date"
                                        Lunghezza = ""
                                    Case TipoCampo.TInt
                                        TipoStr = "int"
                                        Lunghezza = "(5)"
                                    Case TipoCampo.TLong
                                        TipoStr = "double"
                                        Lunghezza = "(18,0)"
                                End Select
                        End Select
                        If NomeTabella = "CAUMAG" Then
                            Dim a As Boolean = True
                        End If
                        Globale.gMappaCampi.Add(Globale.CodAzi.ToLower & NomeTabella & "." & sf.SNome, sf) ' Aggiunge la struttura del campo nella mappa tabella
                        'If sf.SNome = "CL_BANCA" Then
                        'Dim a As Integer = 1
                        'End If
                        wColonna = Me.EsisteCampo(sf.SNome, Globale.CodAzi & NomeTabella)
                        If Not TableExist Then
                            Select Case Globale.TipoDb
                                Case "SQLSERVER"
                                    StringaCreate = StringaCreate & "," & vbCr & NomeCampo & " " & TipoStr & " " & Lunghezza & " NULL"
                                Case "ORACLE"
                                    StringaCreate = StringaCreate & "," & vbCr & NomeCampo & " " & TipoStr & " " & Lunghezza & " NULL"
                                Case "MYSQL"
                                    StringaCreate = StringaCreate & "," & vbCr & NomeCampo & " " & TipoStr & " " & Lunghezza & " NULL"
                            End Select
                        Else
                            If Not wColonna.m_esist Then ' Se il campo non esiste lo aggiungo
                                Select Case Globale.TipoDb
                                    Case "SQLSERVER"
                                        stringaAdd = "ALTER TABLE " & Globale.CodAzi & NomeTabella & " ADD " & _
                                                     NomeCampo & " " & TipoStr & " " & Lunghezza & " NULL"
                                        opConn.esegui_query(stringaAdd)
                                        stringaAdd = ""
                                    Case "ORACLE"
                                        stringaAdd = "ALTER TABLE " & Globale.CodAzi & NomeTabella & " ADD " & _
                                                     NomeCampo & " " & TipoStr & " " & Lunghezza & " NULL"
                                        opConn.esegui_query(stringaAdd)
                                        stringaAdd = ""
                                    Case "MYSQL"
                                        stringaAdd = "ALTER TABLE " & Globale.CodAzi & NomeTabella & " ADD " & _
                                                     NomeCampo & " " & TipoStr & " " & Lunghezza & " NULL"
                                        opConn.esegui_query(stringaAdd)
                                        stringaAdd = ""
                                End Select
                            Else ' Allora vuol dire che il campo esiste già ma può essere cambiato il tipo o la lunghezza
                                Select Case Globale.TipoDb
                                    Case "SQLSERVER"
                                        If (TipoStr <> wColonna.m_Tipo Or sf.Slen <> wColonna.m_Precision) And sf.Slen <> 0 Then
                                            CancellaIndici(NomeCampo, Globale.CodAzi & NomeTabella) 'Cancella eventuali indici collegati alla colonna
                                            stringaAdd = "ALTER TABLE " & Globale.CodAzi & NomeTabella & " ALTER COLUMN " & _
                                                         NomeCampo & " " & TipoStr & " " & Lunghezza & " NULL"
                                            opConn.esegui_query(stringaAdd)
                                            stringaAdd = ""
                                        End If
                                    Case "ORACLE"
                                        If (TipoStr <> wColonna.m_Tipo Or sf.Slen <> wColonna.m_Precision) And sf.Slen <> 0 Then
                                            CancellaIndici(NomeCampo, Globale.CodAzi & NomeTabella) 'Cancella eventuali indici collegati alla colonna
                                            stringaAdd = "ALTER TABLE " & Globale.CodAzi & NomeTabella & " MODIFY COLUMN " & _
                                                         NomeCampo & " " & TipoStr & " " & Lunghezza & " NULL"
                                            opConn.esegui_query(stringaAdd)
                                            stringaAdd = ""
                                        End If
                                    Case "MYSQL"
                                        'If NomeTabella = "LISART" Then
                                        'Dim a As Integer = 1
                                        'End If
                                        If (TipoStr <> wColonna.m_Tipo Or sf.Slen <> wColonna.m_Precision) And sf.Slen <> 0 Then
                                            CancellaIndici(NomeCampo, Globale.CodAzi & NomeTabella) 'Cancella eventuali indici collegati alla colonna
                                            stringaAdd = "ALTER TABLE " & Globale.CodAzi & NomeTabella & " CHANGE " & NomeCampo & " " & _
                                                         NomeCampo & " " & TipoStr & " " & Lunghezza & " NULL"
                                            opConn.esegui_query(stringaAdd)
                                            stringaAdd = ""
                                        End If
                                End Select
                            End If
                        End If
                    End If
                    If StartReadIndex Then
                        If riga.Contains("'") Then
                            Continue While
                        End If
                        campi = riga.Split("=")
                        If UBound(campi) = 1 Then
                            Dim chiave As String
                            Dim PropIndex As String
                            Dim NomeChiave = Trim(campi(0))
                            campiIdx = Trim(campi(1)).Split(" ")
                            If UBound(campiIdx) = 1 Then
                                chiave = Trim(campiIdx(0))
                                PropIndex = campiIdx(1)
                            Else
                                PropIndex = ""
                                chiave = Trim(campiIdx(0))
                            End If
                            stringaAdd = "CREATE " & PropIndex & " INDEX " & NomeChiave & " ON " & Globale.CodAzi & NomeTabella & " (" & chiave & ")"
                            Try
                                Dim cmd As New Object
                                Select Case Globale.TipoDb
                                    Case "SQLSERVER"
                                        da = New SqlClient.SqlDataAdapter
                                        cmd = New SqlClient.SqlCommand
                                    Case "ORACLE"
                                        da = New Oracle.DataAccess.Client.OracleDataAdapter
                                        cmd = New Oracle.DataAccess.Client.OracleCommand
                                    Case "MYSQL"
                                        da = New MySql.Data.MySqlClient.MySqlDataAdapter
                                        cmd = New MySql.Data.MySqlClient.MySqlCommand
                                End Select
                                cmd.CommandText = stringaAdd
                                cmd.Connection = Globale.cn
                                ' Set command properties
                                ' Set data adapter command
                                da.SelectCommand = cmd
                                da.fill(ds)
                            Catch ex As Exception
                                'MsgBox(ex.Message)
                            Finally
                                stringaAdd = ""
                            End Try
                        End If
                    End If
                End If
            End While
            filereader.Close()
            sr.Close()
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Message, MsgBoxStyle.Information, "Controlla DataBase")
        Catch ex As IO.FileNotFoundException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Controlla DataBase")
        End Try
    End Function
    Private Sub CancellaIndici(ByVal pColonna As String, ByVal pTabella As String)

        Select Case Globale.TipoDb
            Case "SQLSERVER"
                Try
                    Dim hIndex As New Hashtable
                    Dim lCOLUMN_NAME As String = ""
                    Dim lCOLUMN_TEST As String = pColonna.ToString.Trim
                    Dim ds As DataSet = Me.esegui_query("sp_statistics " & pTabella.Trim)
                    For Each dr As DataRow In ds.Tables(0).Rows
                        lCOLUMN_NAME = "[" & CTran(dr("COLUMN_NAME"), "").ToString.Trim & "]"
                        If lCOLUMN_NAME = lCOLUMN_TEST Then
                            If Not hIndex.ContainsKey(dr("INDEX_NAME")) Then
                                hIndex.Add(dr("INDEX_NAME"), dr("INDEX_NAME"))
                            End If
                        End If
                    Next
                    For Each element As String In hIndex.Keys
                        Me.esegui_query("DROP INDEX " & pTabella.Trim & "." & hIndex(element).ToString.Trim)
                    Next
                Catch ex As Exception

                End Try
            Case "ORACLE"
            Case "MYSQL"
                Try
                    Dim hIndex As New Hashtable
                    Dim lCOLUMN_NAME As String = ""
                    Dim lCOLUMN_TEST As String = pColonna.ToString.Trim
                    Dim ds As DataSet = Me.esegui_query("show index from " & pTabella.Trim & " where key_name <> 'PRIMARY'")
                    For Each dr As DataRow In ds.Tables(0).Rows
                        lCOLUMN_NAME = "[" & CTran(dr("COLUMN_NAME"), "").ToString.Trim & "]"
                        If lCOLUMN_NAME = lCOLUMN_TEST Then
                            If Not hIndex.ContainsKey(dr("KEY_NAME")) Then
                                hIndex.Add(dr("KEY_NAME"), dr("KEY_NAME"))
                            End If
                        End If
                    Next
                    For Each element As String In hIndex.Keys
                        Me.esegui_query("DROP INDEX " & pTabella.Trim & "." & hIndex(element).ToString.Trim)
                    Next
                Catch ex As Exception

                End Try
        End Select
    End Sub
    Private Function EstraiParametro(ByVal riga As String)
        Dim var
        Dim NomeTabella As String

        var = riga.Split("=")
        NomeTabella = var(1)
        Dim kk As Integer = NomeTabella.IndexOf(">")
        NomeTabella = NomeTabella.Substring(0, kk)
        Return NomeTabella
    End Function
    Private Function EsisteTab(ByVal tabella As String) As Boolean

        Dim strsql As String = ""
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                strsql = "select * from dbo.sysobjects where id = object_id(N'[dbo].[" & tabella & "]')" & _
                                       " and OBJECTPROPERTY(id, N'IsUserTable') = 1"
            Case "ORACLE"
                strsql = "select * from USER_ALL_TABLES where TABLE_NAME = '" & tabella & "'"
            Case "MYSQL"
                strsql = "SHOW TABLES where Tables_in_" & Globale.DataBase & " = '" & tabella & "'"
        End Select
        Dim dts As Data.DataSet = opConn.esegui_query(strsql)
        If dts.Tables.Item(0).Rows.Count <> 0 Then
            dts = Nothing
            Return True
        Else
            dts = Nothing
            Return False
        End If
        ''

    End Function
    Private Function EsisteCampo(ByVal campo As String, ByVal tabella As String) As sColonna
        Dim strsql As String = ""
        Dim wColonna As sColonna
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                strsql = "select dbo.syscolumns.[name], dbo.systypes.[name] as fieldName, dbo.syscolumns.[prec], dbo.syscolumns.[scale] " & _
                         "from dbo.syscolumns inner join dbo.systypes on dbo.syscolumns.[xtype] = dbo.systypes.[xtype] " & _
                         "where id = object_id(N'[dbo].[" & tabella & "]') and dbo.syscolumns.[name] = '" & campo & "'"
            Case "ORACLE"
                strsql = "SELECT DATA_TYPE AS FIELDTYPE, DATA_LENGTH AS LENGTH, DATA_SCALE AS SCALE, DATA_PRECISION AS PREC FROM USER_TAB_COLUMNS WHERE TABLE_NAME = '" & tabella & "' " & _
                         " AND COLUMN_NAME = '" & campo & "'"
            Case "MYSQL"
                strsql = "SELECT DATA_TYPE AS FIELDTYPE, CHARACTER_MAXIMUM_LENGTH AS CHARLENGTH, NUMERIC_PRECISION AS PREC, NUMERIC_SCALE AS SCALE FROM INFORMATION_SCHEMA.`COLUMNS` WHERE `TABLE_SCHEMA` = '" & Globale.DataBase.Trim & "'" & _
                         " AND `TABLE_NAME` = '" & tabella & "' " & _
                         " AND `COLUMN_NAME` = '" & campo & "'"
        End Select
        Dim dts As Data.DataSet = opConn.esegui_query(strsql)
        If dts.Tables.Item(0).Rows.Count <> 0 Then
            wColonna.m_esist = True
            Select Case Globale.TipoDb
                Case "SQLSERVER"
                    wColonna.m_Tipo = "[" & dts.Tables(0).Rows(0).Item("fieldName") & "]"
                    wColonna.m_Precision = dts.Tables(0).Rows(0).Item("prec")
                    wColonna.m_Scale = CTran(dts.Tables(0).Rows(0).Item("scale"), 0)
                Case "ORACLE"
                    wColonna.m_Tipo = dts.Tables(0).Rows(0).Item("fieldtype")
                    If wColonna.m_Tipo = "CHAR" Then
                        wColonna.m_Precision = dts.Tables(0).Rows(0).Item("length")
                        wColonna.m_Scale = CTran(dts.Tables(0).Rows(0).Item("scale"), 0)
                    Else
                        wColonna.m_Precision = CTran(dts.Tables(0).Rows(0).Item("prec"), 0)
                        wColonna.m_Scale = CTran(dts.Tables(0).Rows(0).Item("scale"), 0)
                    End If
                Case "MYSQL"
                    wColonna.m_Tipo = dts.Tables(0).Rows(0).Item("fieldtype")
                    If wColonna.m_Tipo = "char" Or wColonna.m_Tipo = "varchar" Then
                        wColonna.m_Precision = dts.Tables(0).Rows(0).Item("charlength")
                        wColonna.m_Scale = 0
                    Else
                        wColonna.m_Precision = CTran(dts.Tables(0).Rows(0).Item("prec"), 0)
                        wColonna.m_Scale = CTran(dts.Tables(0).Rows(0).Item("scale"), 0)
                    End If
            End Select
            dts = Nothing
            Return wColonna
        Else
            dts = Nothing
            wColonna.m_esist = False
            wColonna.m_Precision = 0
            wColonna.m_Scale = 0
            wColonna.m_Tipo = ""
            Return wColonna
        End If
    End Function
    Function chiudi_connessione()
        Select Case Trim$(Globale.TipoDb)
            Case "SQLSERVER"
                cn.Close()
        End Select
    End Function
    Function registra_maschera_old(ByVal Struttura As Object) As Boolean
        Dim str As Schede
        str = Struttura
        Dim campo As New Object
        Dim strUpdate As String
        Dim virgola As Boolean = False

        Try
            Dim strsql As String = "SELECT * FROM " & str.Tabella
            'Sono arrivato qui
            Dim StrWhere As String = " WHERE NUMREC = " & CTran(str.ValChiavePrimaria, 0)
            'For i = 0 To UBound(cp.tab_key) - 1
            'If Trim$(StrWhere) = "WHERE" Then
            'StrWhere = StrWhere & cp.tab_key(i)
            'Else
            'StrWhere = StrWhere & " and " & cp.tab_key(i)
            'End If
            'Next i
            strsql = strsql & StrWhere
            Dim cmd As New SqlClient.SqlCommand(strsql, Globale.cn)
            '       Dim numrec As Long = cmd.ExecuteNonQuery()
            Dim myreader As SqlClient.SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
            If myreader.HasRows Then
                strUpdate = "UPDATE " & str.Tabella & " SET "
                For Each fld As StructCampi In str.StrutturaTabella
                    campo = str.Scheda.controls(fld.SCampoForm.ToString).text
                    If virgola Then strUpdate = strUpdate & ", "
                    Select Case fld.STipo
                        Case TipoCampo.TCur
                            strUpdate = strUpdate & fld.SNome & " = " & Convert.ToDecimal(CTran(campo, 0))
                        Case TipoCampo.TData
                            strUpdate = strUpdate & fld.SNome & " = " & String.Format("{0:yyyyMMdd}", CTran(campo, 0))
                        Case TipoCampo.TInt
                            strUpdate = strUpdate & fld.SNome & " = " & CTran(Convert.ToInt32(campo), 0)
                        Case TipoCampo.TLong
                            strUpdate = strUpdate & fld.SNome & " = " & CTran(Convert.ToDouble(campo), 0)
                        Case TipoCampo.TBool
                            strUpdate = strUpdate & fld.SNome & " = " & CTran(Convert.ToBoolean(campo), False)
                        Case Else
                            strUpdate = strUpdate & fld.SNome & " = '" & CTran(Convert.ToString(campo), " ") & "'"
                    End Select
                    virgola = True
                Next
                strUpdate = strUpdate & " WHERE NUMREC = " & CTran(str.ValChiavePrimaria, 0)
            End If
            myreader.Close()
            Me.esegui_query(strUpdate)
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Errors(0).Message)
        Catch ex As System.ArgumentException
            MsgBox(ex.Message)
        Catch ex As System.SystemException
            MsgBox(ex.Message)
        End Try
    End Function
    Function registra_maschera(ByRef Struttura As Schede) As Boolean
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                Return registra_maschera_sqlserver(Struttura)
            Case "ORACLE"
                Return registra_maschera_oracle(Struttura)
            Case "MYSQL"
                Return registra_maschera_mysql(Struttura)
        End Select
    End Function
    Function registra_maschera_sqlserver(ByVal Struttura As Schede) As Boolean
        Dim str As New Schede
        str = Struttura
        Dim campo As New Object
        Dim sch As New Object
        sch = str.Scheda
        sch.select()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim wNewrec As Boolean = True
        str.Tabella = getTablename(str.Tabella)
        Dim wQryinsert As String = "INSERT INTO " & str.Tabella & " "
        Dim wQryinsfield As String = ""
        Dim wQryinsvalue As String = ""
        Dim wQryUpdate As String = "UPDATE " & str.Tabella & " SET "
        Dim wQryLastNumrec As String = "SELECT MAX(NUMREC) AS NUMREC FROM " & str.Tabella
        Dim wQryupdfldval As String = ""
        Dim wExecqry As String = ""
        Dim wRw As rw
        Dim wNumrec As Long = 0
        Try
            Dim strsql As String = "SELECT * FROM " & str.Tabella
            Dim _obj As Object = Me.GetCtrl(str.Scheda, str.ChiavePrimaria)
            Dim StrWhere As String = " WHERE NUMREC = " & CTran(_obj.text, 0)
            'For i = 0 To UBound(cp.tab_key) - 1
            'If Trim$(StrWhere) = "WHERE" Then
            'StrWhere = StrWhere & cp.tab_key(i)
            'Else
            'StrWhere = StrWhere & " and " & cp.tab_key(i)
            'End If
            'Next i
            strsql = strsql & StrWhere
            Dim da1 As New SqlClient.SqlDataAdapter(strsql, CType(Globale.cn, SqlClient.SqlConnection))
            '
            'AddHandler da1.RowUpdated, New SqlRowUpdatedEventHandler(AddressOf OnRowUpdated)
            '
            Dim custCB As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(da1)
            da1.Fill(ds)
            dt = ds.Tables.Item(0)
            If dt.Rows.Count = 0 Then ' Se non ha trovato niente : scrivi se no riscrivi
                wNewrec = True
            Else
                wNewrec = False
            End If
            ' se si vuole testare il tipo di campo usare : dt.Columns(0).DataType.ToString
            For Each fld As StructCampi In str.StrutturaTabella
                If (fld.SCampoForm.ToString.Trim <> "" And fld.SOpzione <> "KEYMASTER") Or _
                    fld.SOpzione = "KEYMASTER" Then
                    If Not GetCtrl(str.Scheda, fld.SCampoForm.ToString) Is Nothing Or fld.SOpzione = "KEYMASTER" Then
                        If fld.SOpzione = "KEYMASTER" Then
                            campo = str.ChiaveMaster.text
                        Else
                            campo = GetCtrl(sch, fld.SCampoForm.ToString).text
                        End If
                        'if fld.stabellajoin is not blank, this is a numrec field
                        If Trim(fld.STabellaJoin) <> "" Then
                            If GetCtrl(sch, fld.SCampoForm.ToString).linkdati.usanumrec.ToString = "SI" Then
                                campo = GetCtrl(sch, fld.SCampoForm.ToString).linkdati.numrec
                            Else
                                campo = CTran(Me.LookTab(fld.STabellaJoin, "NUMREC", fld.SChiaveJoin, CTran(campo, 0)).Pop, 0)
                            End If
                            '
                        End If
                        'Fields list string for insert 
                        wQryinsfield = wQryinsfield & fld.SNome & ","
                        wQryinsvalue = wQryinsvalue & Me.ValAdapter(campo, fld.STipo) & ","
                        wQryupdfldval = wQryupdfldval & fld.SNome & "=" & Me.ValAdapter(campo, fld.STipo) & ","
                    End If
                End If
            Next
            '
            If wNewrec Then
                Dim ds1 As DataSet = Me.esegui_query(wQryLastNumrec)
                If ds1.Tables(0).Rows.Count = 0 Then
                    Throw New Exception("Errore nella numerazione record!")
                End If
                wNumrec = CTran(ds1.Tables(0).Rows(0).Item("NUMREC"), 0) + 1
                Struttura.ValChiavePrimaria = wNumrec
                _obj = Me.GetCtrl(Struttura.Scheda, Struttura.ChiavePrimaria)
                _obj.text = wNumrec
            End If
            '
            wQryinsert += "(NUMREC, " & wQryinsfield.TrimEnd(",") & ") VALUES (" & wNumrec & ", " & wQryinsvalue.TrimEnd(",") & ")"
            wQryUpdate += wQryupdfldval.TrimEnd(",") & " WHERE NUMREC = " & CTran(_obj.text, 0)
            '
            If wNewrec Then
                wExecqry = wQryinsert
            Else
                wExecqry = wQryUpdate
            End If
            Me.esegui_query(wExecqry)
            Return True
        Catch ex As NullReferenceException
            Return False
            Exit Function
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        Catch ex As System.ArgumentException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        End Try
    End Function
    Function registra_maschera_oracle(ByVal Struttura As Schede) As Boolean
        Dim str As New Schede
        str = Struttura
        Dim campo As New Object
        Dim sch As New Object
        sch = str.Scheda
        sch.select()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim wNewrec As Boolean = True
        str.Tabella = getTablename(str.Tabella)
        Dim wQryinsert As String = "INSERT INTO " & str.Tabella & " "
        Dim wQryinsfield As String = ""
        Dim wQryinsvalue As String = ""
        Dim wQryUpdate As String = "UPDATE " & str.Tabella & " SET "
        Dim wQryLastNumrec As String = "SELECT MAX(NUMREC) AS NUMREC FROM " & str.Tabella
        Dim wQryupdfldval As String = ""
        Dim wExecqry As String = ""
        Dim wRw As rw
        Dim wNumrec As Long = 0
        Try
            Dim strsql As String = "SELECT * FROM " & str.Tabella

            Dim _obj As Object = Me.GetCtrl(str.Scheda, str.ChiavePrimaria)
            Dim StrWhere As String = " WHERE NUMREC = " & CTran(_obj.text, 0)
            'For i = 0 To UBound(cp.tab_key) - 1
            'If Trim$(StrWhere) = "WHERE" Then
            'StrWhere = StrWhere & cp.tab_key(i)
            'Else
            'StrWhere = StrWhere & " and " & cp.tab_key(i)
            'End If
            'Next i
            strsql = strsql & StrWhere
            Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(strsql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
            '
            'AddHandler da1.RowUpdated, New SqlRowUpdatedEventHandler(AddressOf OnRowUpdated)
            '
            Dim custCB As Oracle.DataAccess.Client.OracleCommandBuilder = New Oracle.DataAccess.Client.OracleCommandBuilder(da1)
            da1.Fill(ds)
            dt = ds.Tables.Item(0)
            If dt.Rows.Count = 0 Then ' Se non ha trovato niente : scrivi se no riscrivi
                wNewrec = True
            Else
                wNewrec = False
            End If
            ' se si vuole testare il tipo di campo usare : dt.Columns(0).DataType.ToString
            For Each fld As StructCampi In str.StrutturaTabella
                If (fld.SCampoForm.ToString.Trim <> "" And fld.SOpzione <> "KEYMASTER") Or _
                    fld.SOpzione = "KEYMASTER" Then
                    If Not GetCtrl(str.Scheda, fld.SCampoForm.ToString) Is Nothing Or fld.SOpzione = "KEYMASTER" Then
                        If fld.SOpzione = "KEYMASTER" Then
                            campo = str.ChiaveMaster.text
                        Else
                            campo = GetCtrl(sch, fld.SCampoForm.ToString).text
                        End If
                        'if fld.stabellajoin is not blank, this is a numrec field
                        If Trim(fld.STabellaJoin) <> "" Then
                            If GetCtrl(sch, fld.SCampoForm.ToString).linkdati.usanumrec.ToString = "SI" Then
                                campo = GetCtrl(sch, fld.SCampoForm.ToString).linkdati.numrec
                            Else
                                campo = CTran(Me.LookTab(fld.STabellaJoin, "NUMREC", fld.SChiaveJoin, CTran(campo, 0)).Pop, 0)
                            End If
                            '
                        End If
                        'Fields list string for insert 
                        wQryinsfield = wQryinsfield & fld.SNome & ","
                        wQryinsvalue = wQryinsvalue & Me.ValAdapter(campo, fld.STipo) & ","
                        wQryupdfldval = wQryupdfldval & fld.SNome & "=" & Me.ValAdapter(campo, fld.STipo) & ","
                    End If
                End If
            Next

            If wNewrec Then
                Dim ds1 As DataSet = Me.esegui_query(wQryLastNumrec)
                If ds1.Tables(0).Rows.Count = 0 Then
                    Throw New Exception("Errore nella numerazione record!")
                End If
                wNumrec = CTran(ds1.Tables(0).Rows(0).Item("NUMREC"), 0) + 1
                Struttura.ValChiavePrimaria = wNumrec
                _obj = Me.GetCtrl(Struttura.Scheda, Struttura.ChiavePrimaria)
                _obj.text = wNumrec
            End If
            '
            wQryinsert += "(NUMREC, " & wQryinsfield.TrimEnd(",") & ") VALUES (" & wNumrec & ", " & wQryinsvalue.TrimEnd(",") & ")"
            wQryUpdate += wQryupdfldval.TrimEnd(",") & " WHERE NUMREC = " & CTran(_obj.text, 0)
            '
            If wNewrec Then
                wExecqry = wQryinsert
            Else
                wExecqry = wQryUpdate
            End If
            Me.esegui_query(wExecqry)
            Return True
        Catch ex As NullReferenceException
            Return False
            Exit Function
        Catch ex As OracleException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        Catch ex As System.ArgumentException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        End Try
    End Function
    Function registra_maschera_mysql(ByRef Struttura As Schede) As Boolean
        Dim str As New Schede
        str = Struttura
        Dim campo As New Object
        Dim sch As New Object
        sch = str.Scheda
        sch.select()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        str.Tabella = getTablename(str.Tabella)
        Dim wNewrec As Boolean = True
        Dim wQryinsert As String = "INSERT INTO " & str.Tabella & " "
        Dim wQryinsfield As String = ""
        Dim wQryinsvalue As String = ""
        Dim wQryUpdate As String = "UPDATE " & str.Tabella & " SET "
        Dim wQryLastNumrec As String = "SELECT MAX(NUMREC) AS NUMREC FROM " & str.Tabella
        Dim wQryupdfldval As String = ""
        Dim wExecqry As String = ""
        Dim wRw As rw
        Dim StrWhere As String = ""
        Dim wNumrec As Long = 0

        Try
            Dim strsql As String = "SELECT * FROM " & str.Tabella

            'Get the primary key value
            Dim _obj As Object = Me.GetCtrl(str.Scheda, str.ChiavePrimaria)
            StrWhere = " WHERE NUMREC = " & CTran(_obj.text, 0)
            'For i = 0 To UBound(cp.tab_key) - 1
            'If Trim$(StrWhere) = "WHERE" Then
            'StrWhere = StrWhere & cp.tab_key(i)
            'Else
            'StrWhere = StrWhere & " and " & cp.tab_key(i)
            'End If
            'Next i
            strsql = strsql & StrWhere
            Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(strsql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
            '
            'AddHandler da1.RowUpdated, New SqlRowUpdatedEventHandler(AddressOf OnRowUpdated)
            '
            Dim custCB As MySql.Data.MySqlClient.MySqlCommandBuilder = New MySql.Data.MySqlClient.MySqlCommandBuilder(da1)
            da1.Fill(ds)
            dt = ds.Tables.Item(0)
            If dt.Rows.Count = 0 Then ' Se non ha trovato niente : scrivi se no riscrivi
                wNewrec = True
            Else
                wNewrec = False
            End If
            ' se si vuole testare il tipo di campo usare : dt.Columns(0).DataType.ToString
            For Each fld As StructCampi In str.StrutturaTabella
                If (fld.SCampoForm.ToString.Trim <> "" And fld.SOpzione <> "KEYMASTER") Or _
                    fld.SOpzione = "KEYMASTER" Then
                    If Not GetCtrl(str.Scheda, fld.SCampoForm.ToString) Is Nothing Or fld.SOpzione = "KEYMASTER" Then
                        If fld.SOpzione = "KEYMASTER" Then
                            campo = str.ChiaveMaster.text
                        Else
                            campo = GetCtrl(sch, fld.SCampoForm.ToString).text
                        End If
                        'if fld.stabellajoin is not blank, this is a numrec field
                        If Trim(fld.STabellaJoin) <> "" Then
                            If GetCtrl(sch, fld.SCampoForm.ToString).linkdati.usanumrec.ToString = "SI" Then
                                campo = GetCtrl(sch, fld.SCampoForm.ToString).linkdati.numrec
                            Else
                                campo = CTran(Me.LookTab(fld.STabellaJoin, "NUMREC", fld.SChiaveJoin, CTran(campo, 0)).Pop, 0)
                            End If
                            '
                        End If
                        'Fields list string for insert 
                        wQryinsfield = wQryinsfield & fld.SNome & ","
                        wQryinsvalue = wQryinsvalue & Me.ValAdapter(campo, fld.STipo) & ","
                        wQryupdfldval = wQryupdfldval & fld.SNome & "=" & Me.ValAdapter(campo, fld.STipo) & ","
                    End If
                End If
            Next
            If wNewrec Then
                Dim ds1 As DataSet = Me.esegui_query(wQryLastNumrec)
                If ds1.Tables(0).Rows.Count = 0 Then
                    Throw New Exception("Errore nella numerazione record!")
                End If
                wNumrec = CTran(ds1.Tables(0).Rows(0).Item("NUMREC"), 0) + 1
                Struttura.ValChiavePrimaria = wNumrec
                _obj = Me.GetCtrl(Struttura.Scheda, Struttura.ChiavePrimaria)
                _obj.text = wNumrec
            End If
            '
            wQryinsert += "(NUMREC, " & wQryinsfield.TrimEnd(",") & ") VALUES (" & wNumrec & ", " & wQryinsvalue.TrimEnd(",") & ")"
            wQryUpdate += wQryupdfldval.TrimEnd(",") & " WHERE NUMREC = " & CTran(_obj.text, 0)
            '
            If wNewrec Then
                wExecqry = wQryinsert
            Else
                wExecqry = wQryUpdate
            End If
            Me.esegui_query(wExecqry)
            Return True
        Catch ex As NullReferenceException
            Return False
            Exit Function
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        Catch ex As System.ArgumentException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        End Try
    End Function

    Overloads Function ValAdapter(ByVal pParam As Object, ByVal pTypeVal As Integer) As Object
        Dim retValue As Object
        Dim w_data As New Date
        Dim w_Integer As Integer
        Dim w_long As Long

        Try
            Select Case Globale.TipoDb
                Case "SQLSERVER"
                    Select Case pTypeVal
                        Case TipoCampo.TCur
                            Dim stringa As String = Convert.ToDecimal(CTran(pParam, 0))
                            retValue = "'" & stringa.Replace(",", ".") & "'"
                        Case TipoCampo.TData
                            If IsDBNull(pParam) Then
                                retValue = "null"
                                Exit Select
                            End If
                            'retValue = "'" & String.Format("{0:yyyyMMdd}", CTran(pParam, System.DBNull.Value)) & "'"
                            retValue = IIf(String.IsNullOrEmpty(pParam), "Null", "'" & reverseDataSqlserver(pParam) & "'")
                        Case TipoCampo.TInt
                            w_Integer = CTran(pParam, 0)
                            retValue = w_Integer
                        Case TipoCampo.TLong
                            w_long = pParam
                            retValue = CTran(w_long, 0)
                        Case TipoCampo.TBool
                            retValue = Convert.ToBoolean(CTran(pParam, False))
                        Case Else
                            retValue = "'" & trasforma_stringa(Convert.ToString(CTran(pParam, " "))).ToString.Trim & "'"
                    End Select
                Case "ORACLE"
                    Select Case pTypeVal
                        Case TipoCampo.TCur
                            Dim stringa As String = Convert.ToDecimal(CTran(pParam, 0))
                            retValue = "'" & stringa.Replace(",", ".") & "'"
                        Case TipoCampo.TData
                            If IsDBNull(pParam) Then
                                retValue = "null"
                                Exit Select
                            End If
                            retValue = IIf(String.IsNullOrEmpty(pParam), "Null", "TO_DATE('" & CTran(pParam, System.DBNull.Value) & "', 'DD/MM/YYYY')")
                            'retValue = "TO_DATE('" & CTran(pParam, System.DBNull.Value) & "', 'DD/MM/YYYY')"
                        Case TipoCampo.TInt
                            w_Integer = CTran(pParam, 0)
                            retValue = w_Integer
                        Case TipoCampo.TLong
                            w_long = pParam
                            retValue = CTran(w_long, 0)
                        Case TipoCampo.TBool
                            retValue = Convert.ToBoolean(CTran(pParam, False))
                        Case Else
                            retValue = "'" & trasforma_stringa(Convert.ToString(CTran(pParam, " "))).ToString.Trim & "'"
                    End Select
                Case "MYSQL"
                    Select Case pTypeVal
                        Case TipoCampo.TCur
                            Dim stringa As String = Convert.ToDecimal(CTran(pParam, 0))
                            retValue = "'" & stringa.Replace(",", ".") & "'"
                        Case TipoCampo.TData
                            If IsDBNull(pParam) Then
                                retValue = "null"
                                Exit Select
                            End If
                            retValue = IIf(String.IsNullOrEmpty(pParam), "null", "'" & Me.reverseData(pParam) & "'")
                            'retValue = "TO_DATE('" & CTran(pParam, System.DBNull.Value) & "', 'DD/MM/YYYY')"
                        Case TipoCampo.TInt
                            w_Integer = CTran(pParam, 0)
                            retValue = w_Integer
                        Case TipoCampo.TLong
                            w_long = pParam
                            retValue = CTran(w_long, 0)
                        Case TipoCampo.TBool
                            retValue = Convert.ToBoolean(CTran(pParam, False))
                        Case Else
                            retValue = "'" & trasforma_stringa(Convert.ToString(CTran(pParam, " "))).ToString.Trim & "'"
                    End Select
            End Select

            Return retValue
        Catch ex As System.Exception
            MsgBox(ex.Message & " " & pParam.ToString, MsgBoxStyle.Critical, "ValAdapter")
            Return ""
        End Try
    End Function
    Overloads Function ValAdapter(ByVal pParam As String, ByVal pTypeVal As TipoCampo) As Object
        Dim retValue As Object
        Dim w_data As New Date
        Dim w_Integer As Integer
        Dim w_long As Long

        Try
            Select Case Globale.TipoDb
                Case "SQLSERVER"
                    Select Case pTypeVal
                        Case TipoCampo.TCur
                            Dim stringa As String = Convert.ToDecimal(Convert.ToString(CTran(pParam, 0)).Replace(".", ","))
                            retValue = "'" & stringa.Replace(",", ".") & "'"
                        Case TipoCampo.TData
                            If IsDBNull(pParam) Then
                                retValue = "null"
                                Exit Select
                            End If
                            If pParam = "" Then
                                retValue = "null"
                                Exit Select
                            End If
                            'retValue = "'" & String.Format("{0:yyyyMMdd}", CTran(pParam, System.DBNull.Value)) & "'"
                            retValue = IIf(String.IsNullOrEmpty(pParam), "'Null'", "'" & reverseDataSqlserver(pParam) & "'")
                        Case TipoCampo.TInt
                            w_Integer = CTran(pParam, 0)
                            retValue = w_Integer
                        Case TipoCampo.TLong
                            w_long = pParam
                            retValue = CTran(w_long, 0)
                        Case TipoCampo.TBool
                            retValue = Convert.ToBoolean(CTran(pParam, False))
                        Case Else
                            retValue = "'" & trasforma_stringa(Convert.ToString(CTran(pParam, " "))).ToString.Trim & "'"
                    End Select
                Case "ORACLE"
                    Select Case pTypeVal
                        Case TipoCampo.TCur
                            Dim stringa As String = Convert.ToDecimal(CTran(pParam, 0))
                            retValue = "'" & stringa.Replace(",", ".") & "'"
                        Case TipoCampo.TData
                            If IsDBNull(pParam) Then
                                retValue = "null"
                                Exit Select
                            End If
                            If pParam = "" Then
                                retValue = "null"
                                Exit Select
                            End If
                            retValue = IIf(String.IsNullOrEmpty(pParam), "null", "TO_DATE('" & CTran(pParam, System.DBNull.Value) & "', 'DD/MM/YYYY')")
                            'retValue = "TO_DATE('" & CTran(pParam, System.DBNull.Value) & "', 'DD/MM/YYYY')"
                        Case TipoCampo.TInt
                            w_Integer = CTran(pParam, 0)
                            retValue = w_Integer
                        Case TipoCampo.TLong
                            w_long = pParam
                            retValue = CTran(w_long, 0)
                        Case TipoCampo.TBool
                            retValue = Convert.ToBoolean(CTran(pParam, False))
                        Case Else
                            retValue = "'" & trasforma_stringa(Convert.ToString(CTran(pParam, " "))).ToString.Trim & "'"
                    End Select
                Case "MYSQL"
                    Select Case pTypeVal
                        Case TipoCampo.TCur
                            Dim stringa As String = Convert.ToDecimal(CTran(pParam, 0))
                            retValue = "'" & stringa.Replace(",", ".") & "'"
                        Case TipoCampo.TData
                            If IsDBNull(pParam) Then
                                retValue = "null"
                                Exit Select
                            End If
                            If pParam = "" Then
                                retValue = "null"
                                Exit Select
                            End If
                            retValue = IIf(String.IsNullOrEmpty(pParam), "''", "'" & Me.reverseData(pParam) & "'")
                            'retValue = "TO_DATE('" & CTran(pParam, System.DBNull.Value) & "', 'DD/MM/YYYY')"
                        Case TipoCampo.TInt
                            w_Integer = CTran(pParam, 0)
                            retValue = w_Integer
                        Case TipoCampo.TLong
                            w_long = pParam
                            retValue = CTran(w_long, 0)
                        Case TipoCampo.TBool
                            retValue = Convert.ToBoolean(CTran(pParam, False))
                        Case Else
                            retValue = "'" & trasforma_stringa(Convert.ToString(CTran(pParam, " "))).ToString.Trim & "'"
                    End Select
            End Select

            Return retValue
        Catch ex As System.Exception
            MsgBox(ex.Message & " " & pParam.ToString, MsgBoxStyle.Critical, "ValAdapter")
            Return ""
        End Try
    End Function
    Overloads Function ValAdapter(ByVal pParam As String, ByVal pTypeVal As TipoCampo, ByVal pTipoDb As String) As Object
        Dim retValue As Object
        Dim w_data As New Date
        Dim w_Integer As Integer
        Dim w_long As Long

        Try
            Select Case pTipoDb
                Case "SQLSERVER"
                    Select Case pTypeVal
                        Case TipoCampo.TCur
                            Dim stringa As String = Convert.ToDecimal(Convert.ToString(CTran(pParam, 0)).Replace(".", ","))
                            retValue = "'" & stringa.Replace(",", ".") & "'"
                        Case TipoCampo.TData
                            If IsDBNull(pParam) Then
                                retValue = "null"
                                Exit Select
                            End If
                            If pParam = "" Then
                                retValue = "null"
                                Exit Select
                            End If
                            retValue = IIf(String.IsNullOrEmpty(pParam), "Null", "'" & reverseDataSqlserver(pParam) & "'")
                        Case TipoCampo.TInt
                            w_Integer = CTran(pParam, 0)
                            retValue = w_Integer
                        Case TipoCampo.TLong
                            w_long = pParam
                            retValue = CTran(w_long, 0)
                        Case TipoCampo.TBool
                            retValue = Convert.ToBoolean(CTran(pParam, False))
                        Case Else
                            retValue = "'" & trasforma_stringa(Convert.ToString(CTran(pParam, " "))).ToString.Trim & "'"
                    End Select
                Case "ORACLE"
                    Select Case pTypeVal
                        Case TipoCampo.TCur
                            Dim stringa As String = Convert.ToDecimal(CTran(pParam, 0))
                            retValue = "'" & stringa.Replace(",", ".") & "'"
                        Case TipoCampo.TData
                            If IsDBNull(pParam) Then
                                retValue = "null"
                                Exit Select
                            End If
                            If pParam = "" Then
                                retValue = "null"
                                Exit Select
                            End If
                            retValue = IIf(String.IsNullOrEmpty(pParam), "null", "TO_DATE('" & CTran(pParam, System.DBNull.Value) & "', 'DD/MM/YYYY')")
                            'retValue = "TO_DATE('" & CTran(pParam, System.DBNull.Value) & "', 'DD/MM/YYYY')"
                        Case TipoCampo.TInt
                            w_Integer = CTran(pParam, 0)
                            retValue = w_Integer
                        Case TipoCampo.TLong
                            w_long = pParam
                            retValue = CTran(w_long, 0)
                        Case TipoCampo.TBool
                            retValue = Convert.ToBoolean(CTran(pParam, False))
                        Case Else
                            retValue = "'" & trasforma_stringa(Convert.ToString(CTran(pParam, " "))).ToString.Trim & "'"
                    End Select
                Case "MYSQL"
                    Select Case pTypeVal
                        Case TipoCampo.TCur
                            Dim stringa As String = Convert.ToDecimal(CTran(pParam, 0))
                            retValue = "'" & stringa.Replace(",", ".") & "'"
                        Case TipoCampo.TData
                            If IsDBNull(pParam) Then
                                retValue = "null"
                                Exit Select
                            End If
                            If pParam = "" Then
                                retValue = "null"
                                Exit Select
                            End If
                            retValue = IIf(String.IsNullOrEmpty(pParam), "''", "'" & Me.reverseData(pParam) & "'")
                            'retValue = "TO_DATE('" & CTran(pParam, System.DBNull.Value) & "', 'DD/MM/YYYY')"
                        Case TipoCampo.TInt
                            w_Integer = CTran(pParam, 0)
                            retValue = w_Integer
                        Case TipoCampo.TLong
                            w_long = pParam
                            retValue = CTran(w_long, 0)
                        Case TipoCampo.TBool
                            retValue = Convert.ToBoolean(CTran(pParam, False))
                        Case Else
                            retValue = "'" & trasforma_stringa(Convert.ToString(CTran(pParam, " "))).ToString.Trim & "'"
                    End Select
            End Select

            Return retValue
        Catch ex As System.Exception
            MsgBox(ex.Message & " " & pParam.ToString, MsgBoxStyle.Critical, "ValAdapter")
            Return ""
        End Try
    End Function
    Function registra_maschera_v1(ByVal Struttura As Schede) As Boolean
        Dim str As New Schede
        str = Struttura
        Dim campo As New Object
        Dim sch As New Object
        sch = str.Scheda
        sch.select()
        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet

        Try
            Dim strsql As String = "SELECT * FROM " & str.Tabella
            'Sono arrivato qui
            Dim StrWhere As String = " WHERE NUMREC = " & CTran(str.ValChiavePrimaria, 0)
            'For i = 0 To UBound(cp.tab_key) - 1
            'If Trim$(StrWhere) = "WHERE" Then
            'StrWhere = StrWhere & cp.tab_key(i)
            'Else
            'StrWhere = StrWhere & " and " & cp.tab_key(i)
            'End If
            'Next i
            strsql = strsql & StrWhere
            Dim da1 As New SqlClient.SqlDataAdapter(strsql, CType(Globale.cn, SqlClient.SqlConnection))
            '
            'AddHandler da1.RowUpdated, New SqlRowUpdatedEventHandler(AddressOf OnRowUpdated)
            '
            Dim custCB As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(da1)
            da1.Fill(ds)
            dt = ds.Tables.Item(0)
            If dt.Rows.Count = 0 Then ' Se non ha trovato niente : scrivi se no riscrivi
                dr = dt.NewRow
            Else
                dr = ds.Tables(0).Rows(0)
            End If
            ' se si vuole testare il tipo di campo usare : dt.Columns(0).DataType.ToString
            For Each fld As StructCampi In str.StrutturaTabella
                If (fld.SCampoForm.ToString.Trim <> "" And fld.SOpzione <> "KEYMASTER") Or _
                    fld.SOpzione = "KEYMASTER" Then
                    If sch.controls.ContainsKey(fld.SCampoForm.ToString) Or fld.SOpzione = "KEYMASTER" Then
                        If fld.SOpzione = "KEYMASTER" Then
                            campo = str.ChiaveMaster.text
                        Else
                            campo = sch.controls(fld.SCampoForm.ToString).text
                        End If
                        Select Case fld.STipo
                            Case TipoCampo.TCur
                                dr(fld.SNome) = Convert.ToDecimal(CTran(campo, 0))
                            Case TipoCampo.TData
                                dr(fld.SNome) = CTran(campo, System.DBNull.Value)
                                'dr(fld.SNome) = String.Format("{0:yyyyMMdd}", CTran(campo, System.DBNull.Value))
                                'dr(fld.SNome) = IIf(IsDBNull(campo) Or Trim(campo) = "", System.DBNull.Value, String.Format("{0:yyyyMMdd}", CTran(campo, 0)))
                            Case TipoCampo.TInt
                                dr(fld.SNome) = Convert.ToInt32(CTran(campo, 0))
                            Case TipoCampo.TLong
                                If Trim(fld.STabellaJoin) <> "" Then
                                    If sch.controls(fld.SCampoForm.ToString).linkdati.usanumrec.ToString = "SI" Then
                                        dr(fld.SNome) = sch.controls(fld.SCampoForm.ToString).linkdati.numrec
                                    Else
                                        dr(fld.SNome) = CTran(Me.LookTab(fld.STabellaJoin, "NUMREC", fld.SChiaveJoin, CTran(campo, 0)).Pop, 0)
                                    End If
                                Else
                                    dr(fld.SNome) = Convert.ToDouble(CTran(campo, 0))
                                End If
                            Case TipoCampo.TBool
                                dr(fld.SNome) = Convert.ToBoolean(CTran(campo, False))
                            Case Else
                                dr(fld.SNome) = Trim(Convert.ToString(CTran(campo, " ")))
                        End Select
                        '                    Else
                        '                        MsgBox("Il Campo " & fld.SCampoForm.ToString & " non è stato trovato" & vbCr & _
                        '                               "per la tabella : " & Struttura.Tabella, MsgBoxStyle.Critical, "Registra Maschera")
                        '                        Return False
                        '                        Exit Function
                    End If
                End If
            Next
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dr)
                Dim myDataRowArray() As DataRow = ds.Tables(0).Select(Nothing, Nothing, DataViewRowState.Added)
                da1.Update(myDataRowArray)
            Else
                da1.Update(ds)
            End If
            Return True
        Catch ex As NullReferenceException
            Return False
            Exit Function
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        Catch ex As System.ArgumentException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Registra maschera")
            Return False
        End Try
    End Function

    Private Shared Sub OnRowUpdated( _
    ByVal sender As Object, ByVal args As SqlRowUpdatedEventArgs)
        If args.Status = UpdateStatus.ErrorsOccurred Then
            Dim strerr As String = args.Errors.Message & vbCrLf
            Dim ii As Integer
            Dim element As String
            For ii = 0 To args.Row.ItemArray.Length - 1
                element = CTran(args.Row.ItemArray(ii), " ")
                strerr = strerr & element & vbCrLf
            Next
            MsgBox(strerr, MsgBoxStyle.Critical, "Operazione su database")
            args.Status = UpdateStatus.SkipCurrentRow
            args.Row.RowError = args.Errors.Message
            args.Status = UpdateStatus.Continue
        End If
    End Sub
    Function leggi_maschera(ByVal Struttura As Object) As Boolean
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                Return leggi_maschera_sqlserver(Struttura)
            Case "ORACLE"
                Return leggi_maschera_oracle(Struttura)
            Case "MYSQL"
                Return leggi_maschera_mysql(Struttura)
        End Select
    End Function
    Function leggi_maschera_oracle(ByVal Struttura As Object) As Boolean
        Dim str As New Schede
        str = Struttura
        Dim campo As New Object
        Dim campo_d As New Object
        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim w_data As New Date

        Try
            str.Tabella = getTablename(str.Tabella)
            If str.ChiaveRicerca.Trim = "" Then
                Throw New Exception("Chiave di ricerca vuota!")
            End If
            Dim strsql As String = "SELECT * FROM " & str.Tabella
            Dim StrWhere As String = " WHERE " & str.ChiaveRicerca & " = " & CTran(str.ValChiaveRicerca, 0)
            'For i = 0 To UBound(cp.tab_key) - 1
            'If Trim$(StrWhere) = "WHERE" Then
            'StrWhere = StrWhere & cp.tab_key(i)
            'Else
            'StrWhere = StrWhere & " and " & cp.tab_key(i)
            'End If
            'Next i
            strsql = strsql & StrWhere
            Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(strsql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
            Dim custCB As Oracle.DataAccess.Client.OracleCommandBuilder = New Oracle.DataAccess.Client.OracleCommandBuilder(da1)
            da1.Fill(ds)
            dt = ds.Tables.Item(0)
            If dt.Rows.Count = 0 Then
                Return False
                Exit Function
            End If
            dr = ds.Tables(0).Rows(0)
            ' se si vuole testare il tipo di campo usare : dt.Columns(0).DataType.ToString
            'Riempie il campo contenente il numrec
            campo = GetCtrl(str.mainContainer, str.ChiavePrimaria)
            If campo Is Nothing Then
                Throw New Exception(str.ChiavePrimaria & ", non presente nel form!")
            End If
            campo.text = CTran(dr(0), 0)
            For Each fld As StructCampi In str.StrutturaTabella
                If fld.SCampoForm.Trim = "" Then
                    Continue For
                End If
                If GetCtrl(str.Scheda, fld.SCampoForm.ToString) Is Nothing And fld.SOpzione <> "KEYMASTER" Then
                    MsgBox("Oggetto :" & fld.SCampoForm.ToString & vbCr & _
                           "non presente nella maschera", MsgBoxStyle.Critical, "Leggi Maschera")
                    Exit Function
                Else
                    campo = GetCtrl(str.Scheda, fld.SCampoForm.ToString)
                End If
                If campo Is Nothing Then
                    Continue For
                End If
                If fld.SOpzione <> "KEYMASTER" Then
                    Select Case fld.STipo
                        Case TipoCampo.TCur
                            'dr(fld.SNome) = Convert.ToDecimal(CTran(campo, 0))
                            campo.text = CTran(dr(fld.SNome), 0)
                        Case TipoCampo.TData
                            If Not Convert.IsDBNull(dr(fld.SNome)) Then
                                w_data = dr(fld.SNome)
                                'dt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("it-IT"))
                                campo.text = w_data.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("it-IT"))
                                'campo.text = String.Format("{0:dd-MM-yyyy}", CTran(dr(fld.SNome), 0))
                            End If
                        Case TipoCampo.TInt
                            campo.text = CTran(dr(fld.SNome), 0)
                        Case TipoCampo.TLong
                            If Trim(fld.STabellaJoin) <> "" Then
                                campo.text = CTran(Me.LookTab(fld.STabellaJoin, fld.SChiaveJoin, "NUMREC", dr(fld.SNome)).Pop, " ")
                                Me.popolaCampiCollegati(Struttura, campo)
                            Else
                                campo.text = CTran(dr(fld.SNome), 0)
                            End If
                        Case TipoCampo.TBool
                            campo.text = CTran(dr(fld.SNome), False)
                        Case Else
                            '                            If Trim(fld.STabellaJoin) <> "" Then
                            '                            campo.text = CTran(Me.LookTab(fld.STabellaJoin, fld.SChiaveJoin, "NUMREC", dr(fld.SNome)).Pop, " ")
                            '                            Me.popolaCampiCollegati(Struttura, campo)
                            '                            Else
                            campo.text = Trim(CTran(dr(fld.SNome), " "))
                            '                            End If
                            'If Trim(fld.STabellaJoin) <> "" Then ' Cerca la descrizione e riempie il campo
                            'campo_d = str.Scheda.controls(fld.SCampoForm.ToString & "_d")
                            'If campo_d.linkdati.linkcampi.trim = "" Then
                            ' Throw New System.Exception("Errore interno :proprietà linkcampi per il campo" & vbCr & campo_d.name & " non impostata!")
                            ' Else
                            ' campo_d.text = CTran(Me.LookTab(fld.STabellaJoin, campo_d.linkdati.linkcampi, fld.SChiaveJoin, dr(fld.SNome)).Pop, " ")
                            ' End If
                            ' End If
                    End Select
                End If
            Next
        Catch ex As Oracle.DataAccess.Client.OracleException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Exclamation, "LeggiMaschera")
        Catch ex As System.ArgumentException
            MsgBox(ex.Message)
        Catch ex As System.SystemException
            MsgBox(ex.Message)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
    Function leggi_maschera_mysql(ByVal Struttura As Object) As Boolean
        Dim str As New Schede
        str = Struttura
        Dim campo As New Object
        Dim campo_d As New Object
        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim w_data As New Date
        Dim strsql As String = ""
        Try
            str.Tabella = getTablename(str.Tabella)
            If str.ChiaveRicerca Is Nothing Then
                Throw New Exception("Chiave di ricerca vuota!" & vbCr & "verificare i tag : <FIELDPRIMARYKEY> e <FIELDTBLKEY>" & vbCr & "nel file struttura")
            End If
            If str.ChiaveRicerca.Trim = "" Then
                Throw New Exception("Chiave di ricerca vuota!" & vbCr & "verificare i tag : <FIELDPRIMARYKEY> e <FIELDTBLKEY>" & vbCr & "nel file struttura")
            End If
            strsql = "SELECT * FROM " & str.Tabella
            Dim StrWhere As String = " WHERE " & str.ChiaveRicerca & " = " & CTran(str.ValChiaveRicerca, 0)
            'For i = 0 To UBound(cp.tab_key) - 1
            'If Trim$(StrWhere) = "WHERE" Then
            'StrWhere = StrWhere & cp.tab_key(i)
            'Else
            'StrWhere = StrWhere & " and " & cp.tab_key(i)
            'End If
            'Next i
            strsql = strsql & StrWhere
            Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(strsql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
            Dim custCB As MySql.Data.MySqlClient.MySqlCommandBuilder = New MySql.Data.MySqlClient.MySqlCommandBuilder(da1)
            da1.Fill(ds)
            dt = ds.Tables.Item(0)
            If dt.Rows.Count = 0 Then
                Return False
                Exit Function
            End If
            dr = ds.Tables(0).Rows(0)
            ' se si vuole testare il tipo di campo usare : dt.Columns(0).DataType.ToString
            'Riempie il campo contenente il numrec
            'campo = str.Scheda.controls(str.ChiavePrimaria)
            campo = GetCtrl(str.mainContainer, str.ChiavePrimaria)
            If campo Is Nothing Then
                Throw New Exception(str.ChiavePrimaria & ", non presente nel form!")
            End If
            campo.text = CTran(dr(0), 0)
            For Each fld As StructCampi In str.StrutturaTabella
                If fld.SCampoForm.Trim = "" Then
                    Continue For
                End If
                If GetCtrl(str.Scheda, fld.SCampoForm.ToString) Is Nothing And fld.SOpzione <> "KEYMASTER" Then
                    MsgBox("Oggetto :" & fld.SCampoForm.ToString & vbCr & _
                           "non presente nella maschera", MsgBoxStyle.Critical, "Leggi Maschera")
                    Exit Function
                Else
                    campo = GetCtrl(str.Scheda, fld.SCampoForm.ToString)
                End If
                If campo Is Nothing Then
                    Continue For
                End If
                If fld.SOpzione <> "KEYMASTER" Then
                    Select Case fld.STipo
                        Case TipoCampo.TCur
                            'dr(fld.SNome) = Convert.ToDecimal(CTran(campo, 0))
                            campo.text = CTran(dr(fld.SNome), 0)
                        Case TipoCampo.TData
                            If Not Convert.IsDBNull(dr(fld.SNome)) Then
                                w_data = dr(fld.SNome)
                                'dt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("it-IT"))
                                campo.text = w_data.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("it-IT"))
                                'campo.text = String.Format("{0:dd-MM-yyyy}", CTran(dr(fld.SNome), 0))
                            End If
                        Case TipoCampo.TInt
                            campo.text = CTran(dr(fld.SNome), 0)
                        Case TipoCampo.TLong
                            If Trim(fld.STabellaJoin) <> "" Then
                                campo.text = CTran(Me.LookTab(fld.STabellaJoin, fld.SChiaveJoin, "NUMREC", dr(fld.SNome)).Pop, " ")
                                Me.popolaCampiCollegati(Struttura, campo)
                            ElseIf Not campo.linkdati.tabellacollegata Is Nothing Then
                                campo.text = CTran(dr(fld.SNome), 0)
                                Me.popolaCampiCollegati(Struttura, campo)
                            Else
                                campo.text = CTran(dr(fld.SNome), 0)
                            End If
                        Case TipoCampo.TBool
                            campo.text = CTran(dr(fld.SNome), False)
                        Case Else
                            '                            If Trim(fld.STabellaJoin) <> "" Then
                            '                            campo.text = CTran(Me.LookTab(fld.STabellaJoin, fld.SChiaveJoin, "NUMREC", dr(fld.SNome)).Pop, " ")
                            '                            Me.popolaCampiCollegati(Struttura, campo)
                            '                            Else
                            campo.text = Trim(CTran(dr(fld.SNome), " "))
                            '                            End If
                            'If Trim(fld.STabellaJoin) <> "" Then ' Cerca la descrizione e riempie il campo
                            'campo_d = str.Scheda.controls(fld.SCampoForm.ToString & "_d")
                            'If campo_d.linkdati.linkcampi.trim = "" Then
                            ' Throw New System.Exception("Errore interno :proprietà linkcampi per il campo" & vbCr & campo_d.name & " non impostata!")
                            ' Else
                            ' campo_d.text = CTran(Me.LookTab(fld.STabellaJoin, campo_d.linkdati.linkcampi, fld.SChiaveJoin, dr(fld.SNome)).Pop, " ")
                            ' End If
                            ' End If
                    End Select
                End If
            Next
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "LeggiMaschera")
        Catch ex As System.ArgumentException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "LeggiMaschera")
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "LeggiMaschera")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "LeggiMaschera")
        End Try
    End Function

    Function leggi_maschera_sqlserver(ByVal Struttura As Object) As Boolean
        Dim str As New Schede
        str = Struttura
        Dim campo As New Object
        Dim campo_d As New Object
        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim w_data As New Date

        Try
            str.Tabella = getTablename(str.Tabella)
            If str.ChiaveRicerca.Trim = "" Then
                Throw New Exception("Chiave di ricerca vuota!")
            End If
            Dim strsql As String = "SELECT * FROM " & str.Tabella
            'Sono arrivato qui
            Dim StrWhere As String = " WHERE " & str.ChiaveRicerca & " = " & CTran(str.ValChiaveRicerca, 0)
            'For i = 0 To UBound(cp.tab_key) - 1
            'If Trim$(StrWhere) = "WHERE" Then
            'StrWhere = StrWhere & cp.tab_key(i)
            'Else
            'StrWhere = StrWhere & " and " & cp.tab_key(i)
            'End If
            'Next i
            strsql = strsql & StrWhere
            Dim da1 As New SqlClient.SqlDataAdapter(strsql, CType(Globale.cn, SqlClient.SqlConnection))
            Dim custCB As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(da1)
            da1.Fill(ds)
            dt = ds.Tables.Item(0)
            If dt.Rows.Count = 0 Then
                Return False
                Exit Function
            End If
            dr = ds.Tables(0).Rows(0)
            ' se si vuole testare il tipo di campo usare : dt.Columns(0).DataType.ToString
            'Riempie il campo contenente il numrec
            campo = GetCtrl(str.mainContainer, str.ChiavePrimaria)
            If campo Is Nothing Then
                Throw New Exception(str.ChiavePrimaria & ", non presente nel form!")
            End If
            campo.text = CTran(dr(0), 0)
            For Each fld As StructCampi In str.StrutturaTabella
                If fld.SCampoForm.Trim = "" Then
                    Continue For
                End If
                If GetCtrl(str.Scheda, fld.SCampoForm.ToString) Is Nothing And fld.SOpzione <> "KEYMASTER" Then
                    MsgBox("Oggetto :" & fld.SCampoForm.ToString & vbCr & _
                           "non presente nella maschera", MsgBoxStyle.Critical, "Leggi Maschera")
                    Exit Function
                Else
                    campo = GetCtrl(str.Scheda, fld.SCampoForm.ToString)
                End If
                If campo Is Nothing Then
                    Continue For
                End If
                If fld.SOpzione <> "KEYMASTER" Then
                    Select Case fld.STipo
                        Case TipoCampo.TCur
                            'dr(fld.SNome) = Convert.ToDecimal(CTran(campo, 0))
                            campo.text = CTran(dr(fld.SNome), 0)
                        Case TipoCampo.TData
                            If Not Convert.IsDBNull(dr(fld.SNome)) Then
                                w_data = dr(fld.SNome)
                                'dt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("it-IT"))
                                campo.text = w_data.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("it-IT"))
                                'campo.text = String.Format("{0:dd-MM-yyyy}", CTran(dr(fld.SNome), 0))
                            End If
                        Case TipoCampo.TInt
                            campo.text = CTran(dr(fld.SNome), 0)
                        Case TipoCampo.TLong
                            If Trim(fld.STabellaJoin) <> "" Then
                                campo.text = CTran(Me.LookTab(fld.STabellaJoin, fld.SChiaveJoin, "NUMREC", dr(fld.SNome)).Pop, " ")
                                Me.popolaCampiCollegati(Struttura, campo)
                            Else
                                campo.text = CTran(dr(fld.SNome), 0)
                            End If
                        Case TipoCampo.TBool
                            campo.text = CTran(dr(fld.SNome), False)
                        Case Else
                            '                            If Trim(fld.STabellaJoin) <> "" Then
                            '                            campo.text = CTran(Me.LookTab(fld.STabellaJoin, fld.SChiaveJoin, "NUMREC", dr(fld.SNome)).Pop, " ")
                            '                            Me.popolaCampiCollegati(Struttura, campo)
                            '                            Else
                            campo.text = Trim(CTran(dr(fld.SNome), " "))
                            '                            End If
                            'If Trim(fld.STabellaJoin) <> "" Then ' Cerca la descrizione e riempie il campo
                            'campo_d = str.Scheda.controls(fld.SCampoForm.ToString & "_d")
                            'If campo_d.linkdati.linkcampi.trim = "" Then
                            ' Throw New System.Exception("Errore interno :proprietà linkcampi per il campo" & vbCr & campo_d.name & " non impostata!")
                            ' Else
                            ' campo_d.text = CTran(Me.LookTab(fld.STabellaJoin, campo_d.linkdati.linkcampi, fld.SChiaveJoin, dr(fld.SNome)).Pop, " ")
                            ' End If
                            ' End If
                    End Select
                End If
            Next
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Exclamation, "LeggiMaschera")
        Catch ex As System.ArgumentException
            MsgBox(ex.Message)
        Catch ex As System.SystemException
            MsgBox(ex.Message)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Overloads Function LookTab(ByVal TabellaDest As String, ByVal CampoDest As String, ByVal Key1 As String, ByVal ValKey1 As Object) As CStack

        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet

        Dim _stack As New CStack
        _stack.Push(" ")
        Dim ValoreChiave As String = IIf(IsDBNull(ValKey1), "0", ValKey1)
        If ValoreChiave.Trim = "" Then
            LookTab = _stack
            Exit Function
        End If
        Try
            TabellaDest = getTablename(TabellaDest)
            Dim strsql As String = "SELECT " & Trim(CampoDest) & " FROM " & TabellaDest.ToLower
            Dim StrWhere As String = " WHERE " & Key1 & " = '" & Convert.ToString(ValoreChiave) & "'"
            strsql = strsql & StrWhere
            Select Case Globale.TipoDb
                Case "SQLSERVER"
                    Dim myreader As SqlClient.SqlDataReader
                    Dim cmd As New SqlClient.SqlCommand(strsql, Globale.cn)
                    '       Dim numrec As Long = cmd.ExecuteNonQuery()
                    myreader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If myreader.FieldCount <> 0 Then
                        While myreader.Read()
                            For ii As Integer = 0 To myreader.FieldCount - 1
                                _stack.Push(myreader.GetValue(ii))
                            Next ii
                        End While
                    End If
                    myreader.Close()
                Case "ORACLE"
                    Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(strsql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
                    Dim custCB As Oracle.DataAccess.Client.OracleCommandBuilder = New Oracle.DataAccess.Client.OracleCommandBuilder(da1)
                    da1.Fill(ds)
                    dt = ds.Tables.Item(0)
                    If dt.Columns.Count <> 0 Then
                        For Each tRow As DataRow In dt.Rows
                            For ii As Integer = 0 To dt.Columns.Count - 1
                                _stack.Push(tRow.Item(ii))
                            Next ii
                        Next
                    End If
                Case "MYSQL"
                    Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(strsql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
                    Dim custCB As MySql.Data.MySqlClient.MySqlCommandBuilder = New MySql.Data.MySqlClient.MySqlCommandBuilder(da1)
                    da1.Fill(ds)
                    dt = ds.Tables.Item(0)
                    If dt.Columns.Count <> 0 Then
                        For Each tRow As DataRow In dt.Rows
                            For ii As Integer = 0 To dt.Columns.Count - 1
                                _stack.Push(tRow.Item(ii))
                            Next ii
                        Next
                    End If
            End Select
            LookTab = _stack
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As Oracle.DataAccess.Client.OracleException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As SqlClient.SqlException
            '            myreader.Close()
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As System.ArgumentException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        End Try
    End Function
    Overloads Function LookTab(ByVal TabellaDest As String, ByVal CampoDest As String, ByVal Chiavefissa As String) As CStack

        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim _stack As New CStack
        _stack.Push(" ")
        Try
            TabellaDest = getTablename(TabellaDest)
            Dim strsql As String = "SELECT " & Trim(CampoDest) & " FROM " & TabellaDest.ToLower
            Dim StrWhere As String = " WHERE " & Chiavefissa
            strsql = strsql & StrWhere

            Select Case Globale.TipoDb
                Case "SQLSERVER"
                    Dim myreader As SqlClient.SqlDataReader
                    Dim cmd As New SqlClient.SqlCommand(strsql, Globale.cn)
                    '       Dim numrec As Long = cmd.ExecuteNonQuery()
                    myreader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If myreader.FieldCount <> 0 Then
                        While myreader.Read()
                            For ii As Integer = 0 To myreader.FieldCount - 1
                                _stack.Push(myreader.GetValue(ii))
                            Next ii
                        End While
                    End If
                    myreader.Close()
                Case "ORACLE"
                    Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(strsql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
                    Dim custCB As Oracle.DataAccess.Client.OracleCommandBuilder = New Oracle.DataAccess.Client.OracleCommandBuilder(da1)
                    da1.Fill(ds)
                    dt = ds.Tables.Item(0)
                    If dt.Columns.Count <> 0 Then
                        For Each tRow As DataRow In dt.Rows
                            For ii As Integer = 0 To dt.Columns.Count - 1
                                _stack.Push(tRow.Item(ii))
                            Next ii
                        Next
                    End If
                Case "MYSQL"
                    Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(strsql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
                    Dim custCB As MySql.Data.MySqlClient.MySqlCommandBuilder = New MySql.Data.MySqlClient.MySqlCommandBuilder(da1)
                    da1.Fill(ds)
                    dt = ds.Tables.Item(0)
                    If dt.Columns.Count <> 0 Then
                        For Each tRow As DataRow In dt.Rows
                            For ii As Integer = 0 To dt.Columns.Count - 1
                                _stack.Push(tRow.Item(ii))
                            Next ii
                        Next
                    End If
            End Select
            LookTab = _stack
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As Oracle.DataAccess.Client.OracleException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As System.ArgumentException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        End Try
    End Function
    Overloads Function LookTabH(ByVal TabellaDest As String, ByVal CampoDest As String, ByVal CampiForm As String, ByVal Chiavefissa As String) As Hashtable

        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim tempHash As New Hashtable
        Try
            TabellaDest = getTablename(TabellaDest)
            'Dim Arrayfld() As String = CampoDest.Split(",") 'Creo un' array contenente i campi da riempire
            Dim ArrayfldForm() As String = CampiForm.Split(",") 'Creo un' array contenente i campi della form da riempire
            Dim strsql As String = "SELECT " & Trim(CampoDest) & " FROM " & TabellaDest.ToLower
            Dim StrWhere As String = " WHERE " & Chiavefissa
            strsql = strsql & StrWhere

            Select Case Globale.TipoDb
                Case "SQLSERVER"
                    Dim myreader As SqlClient.SqlDataReader
                    Dim cmd As New SqlClient.SqlCommand(strsql, Globale.cn)
                    '       Dim numrec As Long = cmd.ExecuteNonQuery()
                    myreader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If myreader.FieldCount <> 0 Then
                        While myreader.Read()
                            For ii As Integer = 0 To myreader.FieldCount - 1
                                tempHash.Add(ArrayfldForm(ii).ToUpper, myreader.GetValue(ii))
                            Next ii
                        End While
                    End If
                    myreader.Close()
                Case "ORACLE"
                    Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(strsql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
                    Dim custCB As Oracle.DataAccess.Client.OracleCommandBuilder = New Oracle.DataAccess.Client.OracleCommandBuilder(da1)
                    da1.Fill(ds)
                    dt = ds.Tables.Item(0)
                    If dt.Columns.Count <> 0 Then
                        For Each tRow As DataRow In dt.Rows
                            For ii As Integer = 0 To dt.Columns.Count - 1
                                tempHash.Add(ArrayfldForm(ii).ToUpper, tRow.Item(ii))
                            Next ii
                        Next
                    End If
                Case "MYSQL"
                    Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(strsql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
                    Dim custCB As MySql.Data.MySqlClient.MySqlCommandBuilder = New MySql.Data.MySqlClient.MySqlCommandBuilder(da1)
                    da1.Fill(ds)
                    dt = ds.Tables.Item(0)
                    If dt.Columns.Count <> 0 Then
                        For Each tRow As DataRow In dt.Rows
                            For ii As Integer = 0 To dt.Columns.Count - 1
                                tempHash.Add(ArrayfldForm(ii).ToUpper, tRow.Item(ii))
                            Next ii
                        Next
                    End If
            End Select
            Return tempHash
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As Oracle.DataAccess.Client.OracleException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As System.ArgumentException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        End Try
    End Function
    Overloads Function LookTabH(ByVal TabellaDest As String, ByVal CampoDest As String, ByVal CampiForm As String, ByVal Key1 As String, ByVal ValKey1 As Object) As Hashtable

        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet


        Dim tempHash As New Hashtable
        Dim ValoreChiave As String = IIf(IsDBNull(ValKey1), "0", ValKey1)
        If ValoreChiave.Trim = "" Then
            Return Nothing
        End If
        Try
            TabellaDest = getTablename(TabellaDest)
            'Dim Arrayfld() As String = CampoDest.Split(",") 'Creo un' array contenente i campi del database da leggere
            Dim ArrayfldForm() As String = CampiForm.Split(",") 'Creo un' array contenente i campi della form da riempire

            Dim strsql As String = "SELECT " & Trim(CampoDest) & " FROM " & TabellaDest.ToLower
            Dim StrWhere As String = " WHERE " & Key1 & " = '" & Convert.ToString(ValoreChiave) & "'"
            strsql = strsql & StrWhere



            Select Case Globale.TipoDb
                Case "SQLSERVER"
                    Dim myreader As SqlClient.SqlDataReader
                    Dim cmd As New SqlClient.SqlCommand(strsql, Globale.cn)
                    '       Dim numrec As Long = cmd.ExecuteNonQuery()
                    myreader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If myreader.FieldCount <> 0 Then
                        While myreader.Read()
                            For ii As Integer = 0 To myreader.FieldCount - 1
                                tempHash.Add(ArrayfldForm(ii).ToUpper, myreader.GetValue(ii))
                            Next ii
                        End While
                    End If
                    myreader.Close()
                Case "ORACLE"
                    Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(strsql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
                    Dim custCB As Oracle.DataAccess.Client.OracleCommandBuilder = New Oracle.DataAccess.Client.OracleCommandBuilder(da1)
                    da1.Fill(ds)
                    dt = ds.Tables.Item(0)
                    If dt.Columns.Count <> 0 Then
                        For Each tRow As DataRow In dt.Rows
                            For ii As Integer = 0 To dt.Columns.Count - 1
                                tempHash.Add(ArrayfldForm(ii).ToUpper, tRow.Item(ii))
                            Next ii
                        Next
                    End If
                Case "MYSQL"
                    Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(strsql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
                    Dim custCB As MySql.Data.MySqlClient.MySqlCommandBuilder = New MySql.Data.MySqlClient.MySqlCommandBuilder(da1)
                    da1.Fill(ds)
                    dt = ds.Tables.Item(0)
                    If dt.Columns.Count <> 0 Then
                        For Each tRow As DataRow In dt.Rows
                            For ii As Integer = 0 To dt.Columns.Count - 1
                                tempHash.Add(ArrayfldForm(ii).ToUpper, tRow.Item(ii))
                            Next ii
                        Next
                    End If
            End Select
            Return tempHash
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As Oracle.DataAccess.Client.OracleException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Errors(0).Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As System.ArgumentException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "LookTab")
        End Try
    End Function

    Private Function popolaCampi(ByVal Struttura As Schede, ByVal oCampo As Object) As Boolean

        Dim hlp As New frmhlp

        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim sch As Object = Struttura.Scheda
        Dim sf As New StructCampi
        Dim vTipoCampo As String = ""
        Dim strSubwhere As String = " ("

        Try
            Dim l_campo As ClSLib.CTextBoxEx = oCampo
            'Dim myreader As SqlClient.SqlDataReader
            If String.IsNullOrEmpty(l_campo.LinkDati.ChiaveSelezione) Then
                Return False
            End If
            '
            Dim l_Chiavi() As String
            Dim l_campiDB() As String
            Dim l_campiForm() As String
            'Cerca chiave fissa 
            Dim l_chiaveFissa As String = ""
            If Not IsNothing(l_campo.LinkDati.Criteriofisso) Then
                l_chiaveFissa = normalizeQuery(l_campo.LinkDati.Criteriofisso, Struttura.mainContainer)
            End If
            '
            If Not IsNothing(l_campo.LinkDati.ChiaveSelezione.Split(",")) Then
                l_Chiavi = l_campo.LinkDati.ChiaveSelezione.Split(",")
            End If
            If Not IsNothing(l_campo.LinkDati.campiDb.Split(",")) Then
                l_campiDB = l_campo.LinkDati.campiDb.Split(",")
            End If
            If Not IsNothing(l_campo.LinkDati.campiForm) Then
                l_campiForm = l_campo.LinkDati.campiForm.Split(",")
            End If
            '
            If IsNothing(l_campiForm) Then
                MsgBox("Immettere un valore in <campiform> per il campo : " & l_campo.Name, MsgBoxStyle.Exclamation, "popolaCampi")
                Exit Function
            End If
            If IsNothing(l_campiDB) Then
                MsgBox("Immettere un valore in <campiDb> per il campo  : " & l_campo.Name, MsgBoxStyle.Exclamation, "popolaCampi")
                Exit Function
            End If
            If IsNothing(l_Chiavi) Then
                MsgBox("Immettere un valore in <ChiaveSelezione> per il campo : " & l_campo.Name, MsgBoxStyle.Exclamation, "popolaCampi")
                Exit Function
            End If

            '
            Dim tRecJoin As New recJoin
            Dim tStrcampo As New StructCampi
            Dim hTabColl As New Hashtable
            Dim hNumrec As New Hashtable 'Contiene l' elenco dei campi collegati con il relativo numrec
            Dim sNumrec As String = "" 'Contiene la lista di campi collegati
            '
            Dim strSelect As String = "SELECT " & l_campo.LinkDati.TabellaCollegata.ToLower & "." & "NUMREC"
            'Crea la query Sql iterando in l_campo.LinkDati.campiDb
            Dim tCampoDb As String = ""
            For iIndex As Integer = 0 To l_campo.LinkDati.campiDb.Split(",").Length - 1
                tCampoDb = l_campo.LinkDati.campiDb.Split(",")(iIndex).TrimStart.TrimEnd
                tStrcampo = Globale.gMappaCampi(getTablename(l_campo.LinkDati.TabellaCollegata.ToLower) & "." & tCampoDb)
                If Not String.IsNullOrEmpty(tStrcampo.STabellaJoin) Then
                    If Not hTabColl.Contains(tStrcampo.STabellaJoin) Then
                        tRecJoin.Tabella = tStrcampo.STabellaJoin.ToLower
                        tRecJoin.campoSx = tCampoDb
                        tRecJoin.campoDx = tStrcampo.SChiaveJoin.ToLower
                        hNumrec.Add(tCampoDb, l_campiForm(iIndex))
                        sNumrec = sNumrec & ", " & tCampoDb
                        hTabColl.Add(tStrcampo.STabellaJoin, tRecJoin)
                    End If
                    strSelect = strSelect & ", " & tStrcampo.STabellaJoin.ToLower & "." & tStrcampo.SChiaveJoin
                Else
                    strSelect = strSelect & ", " & l_campo.LinkDati.TabellaCollegata.ToLower & "." & tStrcampo.SNome
                End If
            Next
            '          
            strSelect = strSelect & sNumrec & " FROM "
            For ii As Integer = 0 To hTabColl.Count - 1
                strSelect = strSelect & "("
            Next
            strSelect = strSelect & getTablename(l_campo.LinkDati.TabellaCollegata.ToLower) & " " & l_campo.LinkDati.TabellaCollegata.ToLower
            'Aggiunge le Join
            For Each chiave As String In hTabColl.Keys
                tRecJoin = hTabColl(chiave)
                strSelect = strSelect & " LEFT JOIN " & getTablename(tRecJoin.Tabella.ToLower) & " " & tRecJoin.Tabella.ToLower & " ON " & _
                                                        l_campo.LinkDati.TabellaCollegata.ToLower & "." & tRecJoin.campoSx & " = " & _
                                                        tRecJoin.Tabella.ToLower & ".NUMREC)"
            Next
            '
            strSelect = strSelect & " WHERE 1=1 AND "
            Dim wSaveQry As String = strSelect
            If l_chiaveFissa.Trim <> "" Then
                strSelect = strSelect & l_chiaveFissa & " AND "
            End If
            strSubwhere = " ("
            '
            'Se la chiave è vuota non fa nessuna lettura
            '
            If String.IsNullOrEmpty(l_campo.Text.Trim) Then
                'Azzera i campi collegati 
                For Each wCampo As String In l_campiForm
                    Dim frmCampo As ClSLib.CTextBoxEx  'Rappresenta il campo del form
                    Dim ii As Integer = 0
                    frmCampo = GetCtrl(Struttura.mainContainer, wCampo)
                    If frmCampo Is Nothing Then
                        Throw New Exception("Campo '" & wCampo & "' non trovato!" & vbCr & _
                                  "Verificare la proprietà LinkDati.CampiForm del campo " & l_campo.Name)
                    End If
                    If UBound(l_campiDB) >= ii Then
                        If frmCampo.TipoCampo = CTextBoxEx.Tipi.Numerico Then
                            frmCampo.Text = ""
                        Else
                            frmCampo.Text = ""
                        End If
                        ii = ii + 1
                    End If
                Next
                '
                popolaCampi = False
                Exit Function
            End If
            '
            'Prima cerca se c'è un record che corrisponde esattamente 
            '
            Dim firstkey As Boolean = True
            For Each wChiavi As String In l_Chiavi
                '
                vTipoCampo = CType(Globale.gMappaCampi(getTablename(l_campo.LinkDati.TabellaCollegata.ToLower) & "." & wChiavi.Trim), StructCampi).STipo
                If firstkey Then
                    Select Case vTipoCampo
                        Case TipoCampo.TChar, TipoCampo.TData
                            strSubwhere = strSubwhere & wChiavi & " = '" & l_campo.Text.Trim & "'"
                            firstkey = False
                        Case Else 'rimangono da valutare i tipi numerici
                            If IsNumeric(l_campo.Text.Trim) Then
                                strSubwhere = strSubwhere & wChiavi & " = " & l_campo.Text.Trim
                                firstkey = False
                            End If
                    End Select
                Else
                    Select Case vTipoCampo
                        Case TipoCampo.TChar, TipoCampo.TData
                            strSubwhere = strSubwhere & " OR " & wChiavi & " = '" & l_campo.Text.Trim & "'"
                        Case Else 'rimangono da valutare i tipi numerici
                            If IsNumeric(l_campo.Text.Trim) Then
                                strSubwhere = strSubwhere & " OR " & wChiavi & " = " & l_campo.Text.Trim
                            End If
                    End Select
                End If
                '
            Next
            '
            If strSubwhere.Substring(0, 2) = " (" Then
                strSubwhere = strSubwhere & " )"
            End If
            Dim strsql As String = strSelect & strSubwhere

            Select Case Globale.TipoDb
                Case "SQLSERVER"
                    Dim da1 As New SqlClient.SqlDataAdapter(strsql, CType(Globale.cn, SqlClient.SqlConnection))
                    Dim custCB As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(da1)
                    da1.Fill(ds)
                Case "ORACLE"
                    Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(strsql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
                    Dim custCB As Oracle.DataAccess.Client.OracleCommandBuilder = New Oracle.DataAccess.Client.OracleCommandBuilder(da1)
                    da1.Fill(ds)
                Case "MYSQL"
                    Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(strsql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
                    Dim custCB As MySql.Data.MySqlClient.MySqlCommandBuilder = New MySql.Data.MySqlClient.MySqlCommandBuilder(da1)
                    da1.Fill(ds)
            End Select
            dt = ds.Tables.Item(0)
            If dt.Rows.Count = 0 Then
                '
                'Se non c'è nessun record che corisponde esattamente allora cerco per radice
                '
                strSubwhere = " ("
                If Not String.IsNullOrEmpty(l_campo.Text.Trim) Then
                    firstkey = True
                    For Each wChiavi As String In l_Chiavi
                        If firstkey Then
                            strSubwhere = strSubwhere & wChiavi & " like '" & l_campo.Text.Trim & "%'"
                            firstkey = False
                        Else
                            strSubwhere = strSubwhere & " OR " & wChiavi & " like '" & l_campo.Text.Trim & "%'"
                        End If
                        '                        strWhere = strWhere & " OR " & wChiavi & " like '" & l_campo.Text.Trim & "%'"
                    Next
                    If strSubwhere.Substring(0, 2) = " (" Then
                        strSubwhere = strSubwhere & " )"
                    End If
                End If
                strsql = strSelect.ToLower & strSubwhere.ToLower
                Select Case Globale.TipoDb
                    Case "SQLSERVER"
                        Dim da1 As New SqlClient.SqlDataAdapter(strsql, CType(Globale.cn, SqlClient.SqlConnection))
                        Dim custCB As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(da1)
                        da1.Fill(ds)
                    Case "ORACLE"
                        Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(strsql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
                        Dim custCB As Oracle.DataAccess.Client.OracleCommandBuilder = New Oracle.DataAccess.Client.OracleCommandBuilder(da1)
                        da1.Fill(ds)
                    Case "MYSQL"
                        Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(strsql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
                        Dim custCB As MySql.Data.MySqlClient.MySqlCommandBuilder = New MySql.Data.MySqlClient.MySqlCommandBuilder(da1)
                        da1.Fill(ds)
                End Select
                dt = ds.Tables.Item(0)
            End If
            '
            If dt.Rows.Count = 0 Then
                'Azzera i campi collegati 
                For Each wCampo As String In l_campiForm
                    Dim frmCampo As ClSLib.CTextBoxEx  'Rappresenta il campo del form
                    Dim ii As Integer = 0
                    frmCampo = GetCtrl(Struttura.mainContainer, wCampo)
                    If frmCampo Is Nothing Then
                        Throw New Exception("Campo '" & wCampo & "' non trovato!" & vbCr & _
                                  "Verificare la proprietà LinkDati.CampiForm del campo " & l_campo.Name)
                    End If
                    If UBound(l_campiDB) >= ii Then
                        If frmCampo.TipoCampo = CTextBoxEx.Tipi.Numerico Then
                            frmCampo.Text = 0
                        Else
                            frmCampo.Text = ""
                        End If
                        If Not frmCampo.LinkDati.TabellaCollegata Is Nothing Then
                            frmCampo.LinkDati.NUMREC = 0
                        End If
                        ii = ii + 1
                    End If
                Next
                '
                popolaCampi = False
                Exit Function
            End If
            If dt.Rows.Count = 1 Then
                dr = ds.Tables(0).Rows(0)
                If CType(oCampo, ClSLib.CTextBoxEx).LinkDati.NUMREC = dr.Item("NUMREC") Then
                    Exit Function
                End If
                CType(oCampo, ClSLib.CTextBoxEx).LinkDati.NUMREC = dr.Item("NUMREC")
                Dim frmCampo As ClSLib.CTextBoxEx  'Rappresenta il campo del form
                Dim ii As Integer = 0
                For Each wCampo As String In l_campiForm
                    Try
                        frmCampo = GetCtrl(Struttura.mainContainer, wCampo.Trim)
                        If frmCampo Is Nothing Then
                            Throw New Exception("Campo '" & wCampo & "' non trovato!" & vbCr & _
                                      "Verificare la proprietà LinkDati.CampiForm del campo " & l_campo.Name)
                        End If
                        If UBound(l_campiDB) >= ii Then
                            frmCampo.Text = CTran(dr.Item(ii + 1), "") ' Perchè davanti c'è sempre il numrec che comunque non è presente nella prop. elenco campi
                            ii = ii + 1
                        End If
        Catch ex1 As System.NullReferenceException
            MsgBox("Oggetto :" & wCampo & " non trovato nel contenitore : " & sch.name.ToString.Trim, MsgBoxStyle.Critical, "popolaCampi")
        End Try
                    '                    frmCampo.LinkDati.NUMREC = dr.Item("NUMREC")
                Next
                For Each chiave As String In hNumrec.Keys
                    frmCampo = GetCtrl(Struttura.mainContainer, hNumrec(chiave))
                    frmCampo.LinkDati.NUMREC = CTran(dr.Item(chiave), 0)
                Next
            End If
            If dt.Rows.Count > 1 Then
                'Dim w_campo As New StructCampi
                'w_campo = Struttura.ElencoCampi(l_campo.TabIndex.ToString)
                hlp.l_campo = Struttura.ElencoCampi(l_campo.TabIndex.ToString)
                'Se non ho una struttura valida prendo dalla proprietà linkdati del campo
                If hlp.l_campo.STabellaJoin Is Nothing Then
                    If l_campo.LinkDati.TabellaCollegata.Trim <> "" Then
                        hlp.l_campo.STabellaJoin = l_campo.LinkDati.TabellaCollegata.Trim
                        hlp.l_campo.SCampoForm = l_campo.Name
                    End If
                End If
                '
                'hlp.l_campo = sch.controls(l_campo.Name.ToString)
                If l_chiaveFissa.Trim() <> "" Then
                    hlp.FixedKey = strSubwhere & " AND " & l_chiaveFissa
                Else
                    hlp.FixedKey = strSubwhere
                End If
                hlp.ShowDialog()
                If hlp.DialogResult = Windows.Forms.DialogResult.OK Then
                    Dim numrec As String = hlp.ValRet.valore
                    'CType(oCampo, ClSLib.CTextBoxEx).LinkDati.NUMREC = numrec

                    '
                    wSaveQry = wSaveQry & l_campo.LinkDati.TabellaCollegata & ".NUMREC = " & numrec
                    Dim ds1 As New DataSet
                    Select Case Globale.TipoDb
                        Case "SQLSERVER"
                            Dim da1 As New SqlClient.SqlDataAdapter(wSaveQry, CType(Globale.cn, SqlClient.SqlConnection))
                            Dim custCB As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(da1)
                            da1.Fill(ds1)
                        Case "ORACLE"
                            Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(wSaveQry, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
                            Dim custCB As Oracle.DataAccess.Client.OracleCommandBuilder = New Oracle.DataAccess.Client.OracleCommandBuilder(da1)
                            da1.Fill(ds1)
                        Case "MYSQL"
                            Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(wSaveQry, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
                            Dim custCB As MySql.Data.MySqlClient.MySqlCommandBuilder = New MySql.Data.MySqlClient.MySqlCommandBuilder(da1)
                            da1.Fill(ds1)
                    End Select
                    dt = ds1.Tables.Item(0)
                    If dt.Rows.Count = 1 Then
                        dr = ds1.Tables(0).Rows(0)
                        If CType(oCampo, ClSLib.CTextBoxEx).LinkDati.NUMREC = dr.Item("NUMREC") Then
                            Exit Function
                        End If
                        CType(oCampo, ClSLib.CTextBoxEx).LinkDati.NUMREC = dr.Item("NUMREC")
                        Dim frmCampo1 As ClSLib.CTextBoxEx  'Rappresenta il campo del form
                        Dim ii As Integer = 0
                        For Each wCampo As String In l_campiForm
                            Try
                                frmCampo1 = GetCtrl(Struttura.mainContainer, wCampo.Trim)
                                If frmCampo1 Is Nothing Then
                                    Throw New Exception("Campo '" & wCampo & "' non trovato!" & vbCr & _
                                              "Verificare la proprietà LinkDati.CampiForm del campo " & l_campo.Name)
                                End If
                                If UBound(l_campiDB) >= ii Then
                                    frmCampo1.Text = CTran(dr.Item(ii + 1), "") ' Perchè davanti c'è sempre il numrec che comunque non è presente nella prop. elenco campi
                                    ii = ii + 1
                                End If
                            Catch ex1 As System.NullReferenceException
                                MsgBox("Oggetto :" & wCampo & " non trovato nel contenitore : " & sch.name.ToString.Trim, MsgBoxStyle.Critical, "popolaCampi")
                            End Try
                            '                    frmCampo.LinkDati.NUMREC = dr.Item("NUMREC")
                        Next
                        For Each chiave As String In hNumrec.Keys
                            frmCampo1 = GetCtrl(Struttura.mainContainer, hNumrec(chiave))
                            frmCampo1.LinkDati.NUMREC = CTran(dr.Item(chiave), 0)
                        Next
                    End If
                    '
                End If
            End If
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MsgBox(ex.Message & vbCr & "Verificare la proprietà Linkdati del campo : " & oCampo.name, MsgBoxStyle.Critical, "popolacampi")
        Catch ex As Oracle.DataAccess.Client.OracleException
            MsgBox(ex.Errors(0).Message & vbCr & "Verificare la proprietà Linkdati del campo : " & oCampo.name, MsgBoxStyle.Critical, "popolacampi")
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Errors(0).Message & vbCr & "Verificare la proprietà Linkdati del campo : " & oCampo.name, MsgBoxStyle.Critical, "popolaCampi")
        Catch ex As System.ArgumentException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "popolaCampi")
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "popolaCampi")
        End Try
    End Function
    ''' <summary>
    ''' This method cut comma and replace with '=' and replace left operand with own value
    ''' </summary>
    Public Function normalizeQuery(ByVal pString As String, ByVal pScheda As Object) As String
        Try
            Dim rReturn As String = " 1=1"
            Dim stringhe As String() = pString.Split(",")
            For Each elemento As String In stringhe
                Dim substringhe As String() = elemento.ToString.Split("=")
                Dim wCampo As Object = GetCtrl(pScheda, substringhe(1))
                If wCampo.LinkDati.UsaNumrec = 1 Then 'Yes
                    rReturn = rReturn & " AND " & substringhe(0) & "=" & wCampo.linkdati.numrec
                Else
                    rReturn = rReturn & " AND " & substringhe(0) & "='" & wCampo.text & "'"
                End If
            Next
            Return rReturn
        Catch ex As Exception
            MsgBox(ex.Message & " criterio : " & pString & " Pannello : " & pScheda.Name, MsgBoxStyle.Information, "normalizeQuery")
        End Try

    End Function
    Overloads Function leggi_tabella(ByVal tabella As String, ByVal cp As CCampi) As Integer
        Dim i As Integer
        '       Dim a As String = tab_key(0)
        If IsNothing(cp.tab_campi) Then
            MsgBox("CCampi senza dimensione" & vbCr & _
                   "utilizzare DimensionaCampi()", MsgBoxStyle.Critical, "Leggi Tabella")
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT * FROM " & tabella
            Dim StrWhere As String = " WHERE "
            For i = 0 To UBound(cp.tab_key) - 1
                If Trim$(StrWhere) = "WHERE" Then
                    StrWhere = StrWhere & cp.tab_key(i)
                Else
                    StrWhere = StrWhere & " and " & cp.tab_key(i)
                End If
            Next i
            strsql = strsql & StrWhere
            Dim cmd As New SqlClient.SqlCommand(strsql, Globale.cn)
            '            Dim myreader As SqlClient.SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
            Dim myreader As SqlClient.SqlDataReader = cmd.ExecuteReader()
            leggi_tabella = 1
            If myreader.FieldCount <> 0 Then
                While myreader.Read()
                    leggi_tabella = 0
                    cp.tab_numrec = myreader.GetValue(0)
                    For i = LBound(cp.tab_campi) + 1 To UBound(cp.tab_campi) - 1
                        If myreader.Item(i).GetType.Name() = "SqlDecimal" Then
                            cp.tab_campi(i - 1) = myreader.GetSqlDecimal(i).Value
                        Else
                            cp.tab_campi(i - 1) = myreader.GetValue(i)
                        End If
                    Next
                End While
            End If
            cmd = Nothing
            myreader.Close()
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Errors(0).Message)
        Catch ex As System.ArgumentException
            MsgBox(ex.Message)
        Catch ex As System.SystemException
            MsgBox(ex.Message)
        End Try
    End Function
    Overloads Function leggi_tabella(ByVal tabella As String, ByRef cpListFld As Object, ByVal cp As CCampi) As Integer
        Dim i As Integer
        Dim ds As New DataSet
        Dim lSf() As CListField = cpListFld
        Dim valRet As Integer
        Dim strsql As String = ""
        Dim wTabella As String = getTablename(tabella.ToLower)
        '       Dim a As String = tab_key(0)
        If IsNothing(lSf) Then
            MsgBox("Lista Campi non inizializzata" & vbCr & _
                   "utilizzare DimensionaCampi_n()", MsgBoxStyle.Critical, "Leggi Tabella")
            Exit Function
        End If
        Try
            Dim tStr() As String
            strsql = "SELECT * FROM " & wTabella
            Dim StrWhere As String = " WHERE "
            For i = 0 To UBound(cp.tab_key) - 1
                Select Case Globale.TipoDb
                    Case "ORACLE"
                        tStr = cp.tab_key(i).ToString.Split("=")
                        If tStr.Length > 0 Then
                            If tStr(1).TrimStart.TrimEnd = "''" Then
                                cp.tab_key(i) = tStr(0) & "is null"
                            End If
                        End If
                End Select
                If Trim$(StrWhere) = "WHERE" Then
                    StrWhere = StrWhere & cp.tab_key(i)
                Else
                    StrWhere = StrWhere & " and " & cp.tab_key(i)
                End If
            Next i
            strsql = strsql & StrWhere
            Select Case Globale.TipoDb
                Case "SQLSERVER"
                    Dim da1 As New SqlClient.SqlDataAdapter(strsql, CType(Globale.cn, SqlClient.SqlConnection))
                    Dim custCB As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(da1)
                    da1.Fill(ds)
                    valRet = 1
                    If ds.Tables(0).Rows.Count > 0 Then
                        valRet = 0
                        cp.tab_numrec = ds.Tables(0).Rows(0).Item("NUMREC")
                        For Each element As CListField In lSf
                            lSf(element.ColumnId).Value = ds.Tables(0).Rows(0).Item(element.FieldName)
                        Next
                    End If
                    da1 = Nothing
                    custCB = Nothing
                    Return valRet
                Case "ORACLE"
                    'Dim cmd As New Oracle.DataAccess.Client.OracleCommand(strsql, Globale.cn)
                    '            Dim myreader As SqlClient.SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    'Dim myreader As Oracle.DataAccess.Client.OracleDataReader = cmd.ExecuteReader()
                    '
                    Dim da1 As New Oracle.DataAccess.Client.OracleDataAdapter(strsql, CType(Globale.cn, Oracle.DataAccess.Client.OracleConnection))
                    Dim custCB As Oracle.DataAccess.Client.OracleCommandBuilder = New Oracle.DataAccess.Client.OracleCommandBuilder(da1)
                    da1.Fill(ds)
                    valRet = 1
                    If ds.Tables(0).Rows.Count > 0 Then
                        valRet = 0
                        cp.tab_numrec = ds.Tables(0).Rows(0).Item("NUMREC")
                        For Each element As CListField In lSf
                            lSf(element.ColumnId).Value = CTran(ds.Tables(0).Rows(0).Item(element.FieldName), "")
                        Next
                    End If
                    da1 = Nothing
                    custCB = Nothing
                    Return valRet
                Case "MYSQL"
                    Dim da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(strsql, CType(Globale.cn, MySql.Data.MySqlClient.MySqlConnection))
                    Dim custCB As MySql.Data.MySqlClient.MySqlCommandBuilder = New MySql.Data.MySqlClient.MySqlCommandBuilder(da1)
                    da1.Fill(ds)
                    valRet = 1
                    If ds.Tables(0).Rows.Count > 0 Then
                        valRet = 0
                        cp.tab_numrec = ds.Tables(0).Rows(0).Item("NUMREC")
                        For Each element As CListField In lSf
                            lSf(element.ColumnId).Value = CTran(ds.Tables(0).Rows(0).Item(element.FieldName), "")
                        Next
                    End If
                    da1 = Nothing
                    custCB = Nothing
                    Return valRet
            End Select
        Catch ex As Oracle.DataAccess.Client.OracleException
            MsgBox(ex.Errors(0).Message & strsql)
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Errors(0).Message & strsql)
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MsgBox(ex.Message & strsql)
        Catch ex As System.ArgumentException
            MsgBox(ex.Message)
        Catch ex As System.SystemException
            MsgBox(ex.Message)
        End Try
    End Function
    ''' <Summary>
    ''' Execute query
    ''' </Summary>
    Overloads Function esegui_query(ByVal stringa_sql As String) As DataSet
        Dim cmd As New Object
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                da = New SqlClient.SqlDataAdapter
                cmd = New SqlClient.SqlCommand
            Case "ORACLE"
                da = New Oracle.DataAccess.Client.OracleDataAdapter
                cmd = New Oracle.DataAccess.Client.OracleCommand
            Case "MYSQL"
                da = New MySqlDataAdapter
                cmd = New MySqlCommand
                stringa_sql = stringa_sql.Replace("[", "`")
                stringa_sql = stringa_sql.Replace("]", "`")
        End Select
        cmd.CommandText = stringa_sql
        cmd.Connection = Globale.cn
        cmd.CommandTimeout = 120
        ' Set command properties
        ' Set data adapter command
        da.SelectCommand = cmd
        ' Populate dataset with datatable from adapter
        Dim ds As New DataSet
        da.Fill(ds)
        'Try
        'da.Fill(ds)
        'Catch ex As SqlClient.SqlException

        'If ex.Number <> 1913 Then 'Indice già esistente
        'MsgBox(ex.Errors(0).Message & "-" & stringa_sql)
        'End If
        'Catch ex As System.ArgumentException
        'MsgBox(ex.Message)
        'Catch ex As System.SystemException
        'MsgBox(ex.Message & ":" & stringa_sql)
        'Finally
        'End Try
        esegui_query = ds
    End Function
    ''' <Summary>
    ''' Build and excute a insert query getting fields/value pairs from hashtable specified as parameter.
    ''' </Summary>
    Overloads Function insert_query(ByVal pTablename As String, ByVal pFieldsPair As Hashtable) As DataSet
        Dim cmd As New Object
        Dim stringa_sql As String

        'Build query
        Dim _strFld As String = ""
        Dim _strVal As String = ""
        Dim strsql As String = "insert into " & pTablename & " ( "

        For Each _element As String In pFieldsPair.Keys
            _strFld = _strFld & _element & ","
            _strVal = _strVal & pFieldsPair(_element) & ","
        Next
        strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
        stringa_sql = strsql
        '
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                da = New SqlClient.SqlDataAdapter
                cmd = New SqlClient.SqlCommand
            Case "ORACLE"
                da = New Oracle.DataAccess.Client.OracleDataAdapter
                cmd = New Oracle.DataAccess.Client.OracleCommand
            Case "MYSQL"
                da = New MySqlDataAdapter
                cmd = New MySqlCommand
                stringa_sql = stringa_sql.Replace("[", "`")
                stringa_sql = stringa_sql.Replace("]", "`")
        End Select
        cmd.CommandText = stringa_sql
        cmd.Connection = Globale.cn
        da.SelectCommand = cmd
        ' Populate dataset with datatable from adapter
        Dim ds As New DataSet
        da.Fill(ds)
        Return ds
    End Function
    Overloads Function update_query(ByVal pTablename As String, ByVal pFieldsPair As Hashtable, ByVal pPrimaryKeys As Hashtable) As DataSet
        Dim cmd As New Object
        Dim stringa_sql As String

        'Build query
        Dim _strFld As String = ""
        Dim _strVal As String = ""
        Dim strsql As String = "update " & pTablename & " set "

        For Each _element As String In pFieldsPair.Keys
            _strVal = pFieldsPair(_element)
            _strFld = _strFld & _element & "=" & _strVal & ","
        Next
        strsql = strsql & _strFld.TrimEnd(",")
        '
        'Build the query's criterta
        '
        Dim _select_criteria As String = " WHERE "
        For Each element As String In pPrimaryKeys.Keys
            _select_criteria = _select_criteria & element & " = " & pPrimaryKeys(element) & " and "
        Next
        '
        _select_criteria = _select_criteria.Substring(0, _select_criteria.Length - 5)
        stringa_sql = strsql & " " & _select_criteria.Trim
        '
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                da = New SqlClient.SqlDataAdapter
                cmd = New SqlClient.SqlCommand
            Case "ORACLE"
                da = New Oracle.DataAccess.Client.OracleDataAdapter
                cmd = New Oracle.DataAccess.Client.OracleCommand
            Case "MYSQL"
                da = New MySqlDataAdapter
                cmd = New MySqlCommand
                stringa_sql = stringa_sql.Replace("[", "`")
                stringa_sql = stringa_sql.Replace("]", "`")
        End Select
        cmd.CommandText = stringa_sql
        cmd.Connection = Globale.cn
        da.SelectCommand = cmd
        ' Populate dataset with datatable from adapter
        Dim ds As New DataSet
        da.Fill(ds)
        Return ds
    End Function
    ''' <Summary>
    ''' Build and excute a insert query getting fields/value pairs from hashtable specified as parameter.
    ''' </Summary>
    Overloads Function CreateInsertQuery(ByVal pTablename As String, ByVal pFieldsPair As Hashtable, ByVal pConnection As Object, ByVal pTipoDbExt As String) As String
        Dim cmd As New Object
        Dim stringa_sql As String

        'Build query
        Dim _strFld As String = ""
        Dim _strVal As String = ""
        Dim strsql As String = "insert into " & pTablename & " ( "

        For Each _element As String In pFieldsPair.Keys
            _strFld = _strFld & _element & ","
            _strVal = _strVal & pFieldsPair(_element) & ","
        Next
        strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
        '
        stringa_sql = strsql
        Return stringa_sql
    End Function

    ''' <Summary>
    ''' Build and excute a insert query getting fields/value pairs from hashtable specified as parameter.
    ''' </Summary>
    Overloads Function insert_query(ByVal pTablename As String, ByVal pFieldsPair As Hashtable, ByVal pConnection As Object, ByVal pTipoDbExt As String) As DataSet
        Dim cmd As New Object
        Dim stringa_sql As String

        'Build query
        Dim _strFld As String = ""
        Dim _strVal As String = ""
        Dim strsql As String = "insert into " & pTablename & " ( "

        For Each _element As String In pFieldsPair.Keys
            _strFld = _strFld & _element & ","
            _strVal = _strVal & pFieldsPair(_element) & ","
        Next
        strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
        '
        stringa_sql = strsql
        Select Case pTipoDbExt
            Case "SQLSERVER"
                da = New SqlClient.SqlDataAdapter
                cmd = New SqlClient.SqlCommand
            Case "ORACLE"
                da = New Oracle.DataAccess.Client.OracleDataAdapter
                cmd = New Oracle.DataAccess.Client.OracleCommand
            Case "MYSQL"
                da = New MySqlDataAdapter
                cmd = New MySqlCommand
                stringa_sql = stringa_sql.Replace("[", "`")
                stringa_sql = stringa_sql.Replace("]", "`")
        End Select
        cmd.CommandText = stringa_sql
        cmd.Connection = pConnection
        da.SelectCommand = cmd
        ' Populate dataset with datatable from adapter
        Dim ds As New DataSet
        da.Fill(ds)
        Return ds
    End Function
    ''' <Summary>
    ''' Build and excute a insert query getting fields/value pairs from hashtable specified as parameter.
    ''' </Summary>
    Overloads Function update_query(ByVal pTablename As String, ByVal pFieldsPair As Hashtable, ByVal pConnection As Object, ByVal pTipoDbExt As String, ByVal pPrimaryKeys As Hashtable) As DataSet

        Try
            Dim cmd As New Object
            Dim stringa_sql As String

            'Build query
            Dim _strFld As String = ""
            Dim _strVal As String = ""
            Dim strsql As String = "update " & pTablename & " set "

            For Each _element As String In pFieldsPair.Keys
                _strFld = _strFld & _element & "=" & pFieldsPair(_element) & ","
            Next
            strsql = strsql & _strFld.TrimEnd(",")
            stringa_sql = strsql
            '
            'Build the query's criterta
            '
            Dim _select_criteria As String = " WHERE "
            For Each element As String In pPrimaryKeys.Keys
                _select_criteria = _select_criteria & element.ToString.Split(".")(1) & " = " & pPrimaryKeys(element) & " and "
            Next
            '
            _select_criteria = _select_criteria.Substring(0, _select_criteria.Length - 5)
            stringa_sql = strsql & " " & _select_criteria.Trim
            '
            Select Case pTipoDbExt
                Case "SQLSERVER"
                    da = New SqlClient.SqlDataAdapter
                    cmd = New SqlClient.SqlCommand
                Case "ORACLE"
                    da = New Oracle.DataAccess.Client.OracleDataAdapter
                    cmd = New Oracle.DataAccess.Client.OracleCommand
                Case "MYSQL"
                    da = New MySqlDataAdapter
                    cmd = New MySqlCommand
                    stringa_sql = stringa_sql.Replace("[", "`")
                    stringa_sql = stringa_sql.Replace("]", "`")
            End Select
            cmd.CommandText = stringa_sql
            cmd.Connection = pConnection
            da.SelectCommand = cmd
            ' Populate dataset with datatable from adapter
            Dim ds As New DataSet
            da.Fill(ds)
            Return ds
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Updatequery")
        End Try
    End Function
    Overloads Function esegui_query(ByVal stringa_sql As String, ByVal pConnection As Object, ByVal pTipoDbExt As String) As DataSet
        Dim cmd As New Object
        Select Case pTipoDbExt
            Case "SQLSERVER"
                da = New SqlClient.SqlDataAdapter
                cmd = New SqlClient.SqlCommand
            Case "ORACLE"
                da = New Oracle.DataAccess.Client.OracleDataAdapter
                cmd = New Oracle.DataAccess.Client.OracleCommand
            Case "MYSQL"
                da = New MySqlDataAdapter
                cmd = New MySqlCommand
        End Select
        cmd.CommandText = stringa_sql
        cmd.Connection = pConnection
        cmd.CommandTimeout = 120
        ' Set command properties
        ' Set data adapter command
        da.SelectCommand = cmd
        ' Populate dataset with datatable from adapter
        Dim ds As New DataSet
        da.Fill(ds)
        'Try
        'da.Fill(ds)
        'Catch ex As SqlClient.SqlException
        'If ex.Number <> 1913 Then 'Indice già esistente
        'MsgBox(ex.Errors(0).Message & "-" & stringa_sql)
        'End If
        'Catch ex As System.ArgumentException
        'MsgBox(ex.Message)
        'Catch ex As System.SystemException
        'MsgBox(ex.Message & ":" & stringa_sql)
        'Finally
        'End Try
        esegui_query = ds
    End Function

    Overloads Function esegui_query(ByVal stringa_sql As String, ByVal pCn As OleDb.OleDbConnection) As OleDb.OleDbDataReader
        Dim oledbcmd As New OleDb.OleDbCommand
        Dim oledbda As New OleDb.OleDbDataAdapter
        oledbcmd.CommandText = stringa_sql
        oledbcmd.Connection = pCn
        ' Set command properties
        ' Set data adapter command

        ' Populate dataset with datatable from adapter
        esegui_query = oledbcmd.ExecuteReader()
        'Try
        'esegui_query = oledbcmd.ExecuteReader()
        'Catch ex As OleDb.OleDbException
        'MsgBox(ex.Errors(0).Message & "-" & stringa_sql)
        'Catch ex As System.ArgumentException
        'MsgBox(ex.Message)
        'Catch ex As System.SystemException
        'MsgBox(ex.Message)
        'Finally
        'End Try
    End Function
    Overloads Function esegui_query(ByVal stringa_sql As String, ByVal pCn As SqlClient.SqlConnection) As SqlClient.SqlDataReader
        Dim cmd As New SqlClient.SqlCommand
        '    Dim da As New SqlClient.SqlDataAdapter
        cmd.CommandText = stringa_sql
        cmd.Connection = pCn
        cmd.CommandTimeout = 120
        ' Set command properties
        ' Set data adapter command

        ' Populate dataset with datatable from adapter
        esegui_query = cmd.ExecuteReader
        'Try
        'esegui_query = cmd.ExecuteReader
        'Catch ex As SqlClient.SqlException
        'MsgBox(ex.Errors(0).Message & "-" & stringa_sql)
        'Catch ex As System.ArgumentException
        'MsgBox(ex.Message)
        'Catch ex As System.SystemException
        'MsgBox(ex.Message)
        'Finally

        'End Try
    End Function
    Function esegui_sp(ByVal pStoredProcedure As String, ByVal pCn As SqlClient.SqlConnection) As DataSet


        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        cmd.CommandText = pStoredProcedure
        cmd.Connection = pCn
        cmd.CommandTimeout = 120
        cmd.CommandType = CommandType.StoredProcedure
        'cmd.Parameters.Add(New SqlParameter("@OE", YourValue)
        da.SelectCommand = cmd
        ' Populate dataset with datatable from adapter
        Dim ds As New DataSet
        da.Fill(ds)

        esegui_sp = ds
    End Function

    Overloads Function scrivi_tabella(ByVal Tabella As String, ByVal campi As CCampi) As Boolean
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                Dim dt As DataTable
                Dim dr As DataRow
                Dim ds As New DataSet
                Dim dsc As New DataSet


                If IsNothing(campi.tab_campi) Then
                    MsgBox("CCampi senza dimensione" & vbCr &
                           "utilizzare DimensionaCampi()", MsgBoxStyle.Critical, "Scrivi Tabella")
                    Return False
                End If
                Dim stringa_sql = "select * from " & Trim(Tabella) & " where 1=0"
                Dim da1 As New SqlClient.SqlDataAdapter(stringa_sql, CType(cn, SqlClient.SqlConnection))
                '
                AddHandler da1.RowUpdated, New SqlRowUpdatedEventHandler(AddressOf OnRowUpdated)
                '
                Dim custCB As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(da1)
                Try
                    da1.Fill(ds)
                    Dim i As Integer
                    dt = ds.Tables.Item(0)
                    dr = dt.NewRow
                    ' se si vuole testare il tipo di campo usare : dt.Columns(0).DataType.ToString
                    For i = LBound(campi.tab_campi) + 1 To UBound(campi.tab_campi) - 1
                        If Not IsNothing(campi.tab_campi(i - 1)) Then
                            Select Case dt.Columns(i).DataType.ToString
                                Case "System.Decimal"
                                    dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), 0, Convert.ToDecimal(campi.tab_campi(i - 1)))
                                    '                        Case "System.Double"
                                    '                           dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), 0, Convert.ToDouble(campi.tab_campi(i - 1)))
                                Case "System.String"
                                    dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), " ", campi.tab_campi(i - 1))
                                Case "System.DateTime"
                                    dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), System.DBNull.Value, campi.tab_campi(i - 1))
                                Case Else
                                    dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), 0, campi.tab_campi(i - 1))
                            End Select
                        Else
                            Select Case dt.Columns(i).DataType.ToString
                                Case "System.Decimal"
                                    dr(i) = 0
                                    '                        Case "System.Double"
                                    '                           dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), 0, Convert.ToDouble(campi.tab_campi(i - 1)))
                                Case "System.String"
                                    dr(i) = " "
                                Case "System.DateTime"
                                    dr(i) = System.DBNull.Value
                                Case Else
                                    dr(i) = 0
                            End Select
                        End If
                    Next
                    dt.Rows.Add(dr)
                    Dim myDataRowArray() As DataRow = ds.Tables(0).Select(Nothing, Nothing, DataViewRowState.Added)
                    da1.Update(myDataRowArray)
                    Return True
                Catch ex As SystemException
                    MsgBox(ex.Message, MsgBoxStyle.Critical, "Scrivi Tabella")
                    Return False
                End Try
        End Select
    End Function
    Overloads Function scrivi_tabella(ByVal Tabella As String, ByVal cpListFld As Object, ByVal campi As CCampi) As Boolean

        Dim dt As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim dsc As New DataSet
        Dim wTabella As String = getTablename(Tabella.ToLower)
        Dim lSf() As CListField = cpListFld
        Dim wQryinsert As String = "INSERT INTO " & wTabella & " "
        Dim wQryinsfield As String = ""
        Dim wQryinsvalue As String = ""

        '
        Try
            If IsNothing(campi.tab_campi) Then
                MsgBox("CCampi senza dimensione" & vbCr & _
                       "utilizzare DimensionaCampi()", MsgBoxStyle.Critical, "Scrivi Tabella")
                Return False
            End If
            For Each elemento As CListField In lSf
                ' 
                wQryinsfield += elemento.FieldName & ","
                wQryinsvalue += Me.ValAdapter(elemento.Value, elemento.DataType) & ","
                '
            Next
            wQryinsert += "(" & wQryinsfield.TrimEnd(",") & ") VALUES (" & wQryinsvalue.TrimEnd(",") & ")"
            Me.esegui_query(wQryinsert)
            Return True
        Catch ex As System.SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Scrivi Tabella")
            Return False
        End Try
        '
    End Function
    Overloads Function riscrivi_tabella(ByVal Tabella As String, ByVal cpListFld As Object, ByVal campi As CCampi) As Boolean

        Dim wQryupdfldval As String = ""
        Dim wExecqry As String = ""
        Dim lSf() As CListField = cpListFld
        Dim wTabella As String = getTablename(Tabella.ToLower)
        Dim wQryUpdate As String = "UPDATE " & wTabella & " SET "

        If IsNothing(campi.tab_campi) Then
            MsgBox("CCampi senza dimensione" & vbCr & _
                   "utilizzare DimensionaCampi()", MsgBoxStyle.Critical, "Riscrivi Tabella")
            Return False
        End If
        '
        Try
            For Each elemento As CListField In lSf
                '
                wQryupdfldval += elemento.FieldName & "=" & Me.ValAdapter(elemento.Value, elemento.DataType) & ","
                '
            Next
            wQryUpdate += wQryupdfldval.TrimEnd(",") & " WHERE NUMREC = " & campi.tab_numrec
            Me.esegui_query(wQryUpdate)
            Return True
        Catch ex As SystemException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Riscrivi Tabella")
            Return False
        End Try
    End Function
    Overloads Function riscrivi_tabella(ByVal Tabella As String, ByVal campi As CCampi)
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                Dim dt As DataTable
                Dim dr As DataRow
                Dim ds As New DataSet
                If IsNothing(campi.tab_campi) Then
                    MsgBox("CCampi senza dimensione" & vbCr & _
                           "utilizzare DimensionaCampi()", MsgBoxStyle.Critical, "Riscrivi Tabella")
                    Exit Function
                End If
                Dim da1 As New SqlClient.SqlDataAdapter
                '
                AddHandler da1.RowUpdated, New SqlRowUpdatedEventHandler(AddressOf OnRowUpdated)
                '
                Dim stringa_sql = "select * from " & Trim(Tabella) & " where numrec = " & campi.tab_numrec
                'Dim da1 As New SqlClient.SqlDataAdapter(stringa_sql, cn)
                da1.SelectCommand = New SqlClient.SqlCommand(stringa_sql, cn)
                Dim custCB As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(da1)
                Dim i As Integer
                Try
                    da1.Fill(ds, Tabella)
                    dt = ds.Tables.Item(0)
                    dr = ds.Tables(0).Rows(0)
                    For i = LBound(campi.tab_campi) + 1 To UBound(campi.tab_campi) - 1
                        If Not IsNothing(campi.tab_campi(i - 1)) Then
                            Select Case dt.Columns(i).DataType.ToString
                                Case "System.Decimal"
                                    dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), 0, Convert.ToDecimal(campi.tab_campi(i - 1)))
                                    '                        Case "System.Double"
                                    '                           dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), 0, Convert.ToDouble(campi.tab_campi(i - 1)))
                                Case "System.String"
                                    dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), " ", campi.tab_campi(i - 1))
                                Case "System.DateTime"
                                    dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), System.DBNull.Value, campi.tab_campi(i - 1))
                                    'dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)) Or CTran(campi.tab_campi(i - 1), "") = "", System.DBNull.Value, campi.tab_campi(i - 1))
                                Case Else
                                    dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), 0, campi.tab_campi(i - 1))
                            End Select
                        Else
                            Select Case dt.Columns(i).DataType.ToString
                                Case "System.Decimal"
                                    dr(i) = 0
                                    '                        Case "System.Double"
                                    '                           dr(i) = IIf(IsDBNull(campi.tab_campi(i - 1)), 0, Convert.ToDouble(campi.tab_campi(i - 1)))
                                Case "System.String"
                                    dr(i) = " "
                                Case "System.DateTime"
                                    dr(i) = System.DBNull.Value
                                Case Else
                                    dr(i) = 0
                            End Select
                        End If
                    Next
                    da1.Update(ds, Tabella)
                Catch ex As SystemException
                    MsgBox(ex.Message, MsgBoxStyle.Critical, "Scrivi Tabella")
                End Try
        End Select
    End Function
    Public Function op_ProgOp(ByVal CodAzi As String, ByVal Chiave As String, ByVal Operazione As TOpe, ByVal StartNum As Integer)
        '
        Try
            Dim cpRec As New CCampi
            Dim res As Integer
            Dim UltProg As Long = 0
            cpRec.tab_campi_n = cpRec.dimesiona_campi_n(Me, "ttabprg")

            '
            'Registra Lettura
            ReDim cpRec.tab_key(2)
            cpRec.tab_key(0) = "CODAZI = '" & Trim(CodAzi) & "'"
            cpRec.tab_key(1) = "CHIAVE = '" & Trim(Chiave) & "'"
            res = Me.leggi_tabella("ttabprg", cpRec.tab_campi_n, cpRec)
            If res = 0 Then
                UltProg = cpRec.tab_campi_n(2).Value
            End If
            Select Case Operazione
                Case TOpe.PG_ULTPROG
                    If CTran(StartNum, 0) <> 0 Then
                        Return StartNum
                    Else
                        Return UltProg
                    End If
                Case TOpe.PG_NXTPROG
                    If CTran(StartNum, 0) <> 0 Then
                        If StartNum <= UltProg Then
                            MsgBox("Impossibile partire dal progressivo impostato." & vbCr & _
                                   "Verrà ripresa la numerazione precedente!", MsgBoxStyle.Information, "Numerazione documenti")
                            UltProg = UltProg + 1
                        Else
                            UltProg = StartNum
                        End If
                    Else
                        UltProg = UltProg + 1
                    End If
                    cpRec.tab_campi_n(0).Value = CodAzi
                    cpRec.tab_campi_n(1).Value = Chiave
                    cpRec.tab_campi_n(2).Value = UltProg
                    If res = 0 Then
                        Me.riscrivi_tabella("TTABPRG", cpRec.tab_campi_n, cpRec)
                    Else
                        Me.scrivi_tabella("TTABPRG", cpRec.tab_campi_n, cpRec)
                    End If
                    Return UltProg
            End Select
        Catch ex As System.Exception
            op_ProgOp = False
            MsgBox(ex.Message, MsgBoxStyle.Information, "Numerazione documenti")
        End Try
    End Function
    Private Function AzzeraMask_old(ByVal Struttura As Schede, ByVal pstart As Integer) As Boolean

        Try
            Dim w_ElencoTabindex As New ArrayList
            Dim SettaFuoco As Boolean = False
            Dim campo As Object
            Dim Nomecampo As String
            Dim str As New Schede
            str = Struttura
            Dim sch As New Object
            sch = str.Scheda
            sch.select()

            For Each fld As StructCampi In str.StrutturaTabella
                If fld.SCampoForm.Trim <> "" Then
                    Try
                        'campo = sch.controls(fld.SCampoForm.ToString)
                        campo = GetCtrl(sch, fld.SCampoForm.ToString)
                        w_ElencoTabindex.Add(campo.tabindex)
                    Catch ex As System.Exception
                        MsgBox("Campo " & fld.SCampoForm.Trim & " non trovato!", MsgBoxStyle.Critical, "Azzera mask")
                    End Try
                End If
            Next
            'Ordino in campi per il loro tabindex
            w_ElencoTabindex.Sort()
            '
            For Each ii As Integer In w_ElencoTabindex
                If ii >= pstart Then ' punto di partenza
                    Nomecampo = str.ElencoCampi.Item(ii.ToString).scampoform
                    campo = GetCtrl(sch, Nomecampo)
                    If TypeOf campo Is CTextBoxEx Then
                        '
                        If campo.LinkDati.TabellaCollegata <> "" Then
                            campo.BackColor = Color.LightCyan
                        End If
                        '
                        If Not SettaFuoco Then
                            campo.Select()
                            campo.Focus()
                            SettaFuoco = True
                        End If
                        Try
                            campo.text = ""
                        Catch
                        End Try
                        campo.linkdati.numrec = 0
                        If CTran(campo.linkdati.campiform, "").ToString.Trim <> "" Then
                            azzeracampicoll(campo.linkdati.campiform.trim(), Struttura)
                        End If
                    End If
                    If TypeOf campo Is CComboBoxEx Then
                        campo.text = ""
                    End If
                End If
            Next
            '
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Azzera Mask")
            Return False
        End Try
    End Function
    Public Overloads Function AzzeraMask(ByVal Struttura As Schede, ByVal pstart As Integer) As Boolean

        Try
            Dim w_ElencoTabindex As New ArrayList
            Dim SettaFuoco As Boolean = False
            Dim str As New Schede
            Dim fldTabZero As Object
            str = Struttura
            Dim sch As New Object
            sch = str.Scheda
            sch.select()

            For Each fld As Control In str.Scheda.controls
                If TypeOf fld Is ClSLib.CTextBoxEx Or TypeOf fld Is ClSLib.CComboBoxEx Then
                    Try
                        If fld.TabIndex = 0 Then ' Se il campo è il primo della lista allora lo salvo per impostargli il fuoco succesivamente
                            fldTabZero = fld
                        End If
                        '
                        If fld.TabIndex >= pstart And fld.TabStop = True Then
                            If TypeOf fld Is CTextBoxEx Then
                                '
                                If CType(fld, ClSLib.CTextBoxEx).LinkDati.TabellaCollegata <> "" Then
                                    fld.BackColor = Color.LightCyan
                                End If
                                '
                                CType(fld, ClSLib.CTextBoxEx).LinkDati.NUMREC = 0
                                If CTran(CType(fld, ClSLib.CTextBoxEx).LinkDati.campiForm, "").ToString.Trim <> "" Then
                                    azzeracampicoll(CType(fld, ClSLib.CTextBoxEx).LinkDati.campiForm.Trim(), Struttura)
                                End If
                            End If
                            Try
                                fld.Text = ""
                            Catch
                            End Try
                        End If
                        '

                    Catch ex As System.Exception
                    End Try
                End If
            Next
            If fldTabZero Is Nothing Then
                Throw New Exception("Impostare almeno un campo con TabIndex=0")
            Else
                fldTabZero.select()
                fldTabZero.Focus()
            End If
            '
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Azzera Mask")
            Return False
        End Try
    End Function
    Public Overloads Function AzzeraMask(ByVal Struttura As Schede, ByVal pstart As Integer, ByVal pEmptyCombo As Boolean) As Boolean

        Try
            Dim w_ElencoTabindex As New ArrayList
            Dim SettaFuoco As Boolean = False
            Dim str As New Schede
            Dim fldTabZero As Object
            str = Struttura
            Dim sch As New Object
            sch = str.Scheda
            sch.select()

            For Each fld As Control In str.Scheda.controls
                If TypeOf fld Is ClSLib.CTextBoxEx Or TypeOf fld Is ClSLib.CComboBoxEx Then
                    Try
                        If fld.TabIndex = 0 Then ' Se il campo è il primo della lista allora lo salvo per impostargli il fuoco succesivamente
                            fldTabZero = fld
                        End If
                        '
                        If TypeOf fld Is CTextBoxEx Then
                            '
                            If CType(fld, ClSLib.CTextBoxEx).LinkDati.TabellaCollegata <> "" Then
                                fld.BackColor = Color.LightCyan
                            End If
                            '
                            CType(fld, ClSLib.CTextBoxEx).LinkDati.NUMREC = 0
                            If CTran(CType(fld, ClSLib.CTextBoxEx).LinkDati.campiForm, "").ToString.Trim <> "" Then
                                azzeracampicoll(CType(fld, ClSLib.CTextBoxEx).LinkDati.campiForm.Trim(), Struttura)
                            End If
                        End If
                        Try
                            If fld.TabIndex >= pstart Then
                                fld.Text = ""
                            End If
                        Catch
                        End Try
                        '

                    Catch ex As System.Exception
                    End Try
                End If
            Next
            If fldTabZero Is Nothing Then
                Throw New Exception("Impostare almeno un campo con TabIndex=0")
            Else
                CType(fldTabZero, ClSLib.CTextBoxEx).Select()
                fldTabZero.Focus()
            End If
            '
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Azzera Mask")
            Return False
        End Try

    End Function
    Private Function AzzeraMask_old_1(ByVal Struttura As Schede, ByVal pstart As Integer, ByVal pEmptyCombo As Boolean) As Boolean

        Try
            Dim w_ElencoTabindex As New ArrayList
            Dim SettaFuoco As Boolean = False
            Dim campo As Object
            Dim Nomecampo As String
            Dim str As New Schede
            str = Struttura
            Dim sch As New Object
            sch = str.Scheda
            sch.select()

            For Each fld As StructCampi In str.StrutturaTabella
                If fld.SCampoForm.Trim <> "" Then
                    Try
                        'campo = sch.controls(fld.SCampoForm.ToString)
                        campo = GetCtrl(sch, fld.SCampoForm.ToString)
                        w_ElencoTabindex.Add(campo.tabindex)
                    Catch ex As System.Exception
                        MsgBox("Campo " & fld.SCampoForm.Trim & " non trovato!", MsgBoxStyle.Critical, "Azzera mask")
                    End Try
                End If
            Next
            'Ordino in campi per il loro tabindex
            w_ElencoTabindex.Sort()
            '
            For Each ii As Integer In w_ElencoTabindex
                If ii >= pstart Then ' punto di partenza
                    Nomecampo = str.ElencoCampi.Item(ii.ToString).scampoform
                    campo = GetCtrl(sch, Nomecampo)
                    If TypeOf campo Is CTextBoxEx Then
                        '
                        If campo.LinkDati.TabellaCollegata <> "" Then
                            campo.BackColor = Color.LightCyan
                        End If
                        '
                        If Not SettaFuoco Then
                            campo.Select()
                            campo.Focus()
                            SettaFuoco = True
                        End If
                        campo.text = ""
                        campo.linkdati.numrec = 0
                        If CTran(campo.linkdati.campiform, "").ToString.Trim <> "" Then
                            azzeracampicoll(campo.linkdati.campiform.trim(), Struttura)
                        End If
                    End If
                    If TypeOf campo Is CComboBoxEx Then
                        campo.text = ""
                        If pEmptyCombo = True Then
                            campo.clear()
                        End If
                    End If
                End If
            Next
            '
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Azzera Mask")
            Return False
        End Try
    End Function

    Private Sub azzeracampicoll(ByVal pPar As String, ByVal Struttura As Schede)
        'pPar contiene la lista di campi, separati da ",", da azzerare
        Try
            Dim oFld As New Object
            Dim lFld() = pPar.Split(",")
            For Each lElement As String In lFld
                Struttura.Scheda.controls(lElement).text = ""
            Next
        Catch
        End Try
    End Sub
    Public Sub popolaCampiCollegati(ByVal pStruttura As Schede, ByVal pCampo As CTextBoxEx)

        ' This stack stores the directories to process.
        Dim queue As New Queue(Of Object)

        ' Add the initial directory
        If Not String.IsNullOrEmpty(pCampo.LinkDati.TabellaCollegata) Then
            queue.Enqueue(pCampo)
            'stack.Push(pCampo)
        End If

        ' Continue processing for each stacked directory
        Do While (queue.Count > 0)
            ' Get top directory string
            Dim oCampo As CTextBoxEx = queue.Dequeue
            Try
                If Not String.IsNullOrEmpty(pCampo.LinkDati.TabellaCollegata) Then
                    popolaCampi(pStruttura, oCampo)
                End If
                ' Loop through all subfield and add them to the stack.
                Dim oSubField As String
                For Each oSubField In oCampo.LinkDati.campiForm.Split(",")
                    Dim oCtrl As CTextBoxEx = GetCtrl(pStruttura.mainContainer, oSubField)
                    If Not IsNothing(oCtrl.LinkDati.campiForm) And oCtrl.Name.ToLower <> oCampo.Name.ToLower Then
                        queue.Enqueue(oCtrl)
                        'stack.Push(oCtrl)
                    End If
                Next
                '
            Catch ex As Exception
            End Try
        Loop

    End Sub
    Public Sub popolaCampiCollegati1(ByVal pStruttura As Schede, ByVal pCampo As CTextBoxEx)
        Try
            Dim fldCol As String()
            If Not IsNothing(pCampo.LinkDati.campiForm) Then
                fldCol = pCampo.LinkDati.campiForm.Split(",")
            Else
                Exit Sub
            End If
            For Each fld As String In fldCol
                '   If fld.Trim <> pCampo.Name.Trim Then
                Dim tFld As CTextBoxEx = GetCtrl(pStruttura.mainContainer, fld)
                If tFld Is Nothing Then
                    Throw New Exception("Campo '" & fld.ToUpper & "' non trovato!" & vbCr & _
                              "Verificare la proprietà LinkDati.CampiForm del campo.")
                End If
                If Not String.IsNullOrEmpty(tFld.LinkDati.TabellaCollegata) Then
                    popolaCampi(pStruttura, tFld)
                End If
                'End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "PopolaCampiCollegati")
        End Try
    End Sub
    Public Sub CercaConversioni(ByVal pStato As Object)
        Dim asm As System.Reflection.Assembly
        Dim t As Type()
        Dim ty As Type
        Dim m As MethodInfo()
        Dim mm As MethodInfo
        Dim rigafile As String
        Dim lNome As String = ""
        Dim lDescri As String = ""
        Dim lpath As String = ""
        Dim lFullName As String = ""
        Dim lNomefileinf As String = ""
        Dim lFileOk As String = ""

        If Directory.Exists("Conv") = False Then
            Directory.CreateDirectory("Conv")
        End If
        Dim di As New DirectoryInfo("Conv")
        Dim fi As FileInfo
        For Each fi In di.GetFiles("*.inf")
            lNomefileinf = fi.Name
            Dim sr As System.IO.FileStream
            sr = New System.IO.FileStream("Conv" & "\" & lNomefileinf, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            While filereader.Peek > -1
                rigafile = filereader.ReadLine
                Select Case rigafile.Split("=")(0)
                    Case "[Nome programma]"
                        lNome = rigafile.Split("=")(1)
                    Case "[Full name module]"
                        lFullName = rigafile.Split("=")(1)
                    Case "[Descrizione]"
                        lDescri = rigafile.Split("=")(1)
                    Case "[Path]"
                        lpath = rigafile.Split("=")(1)
                End Select
            End While
            sr.Close()
            asm = System.Reflection.Assembly.LoadFrom(lpath & "\" & lNome)
            'asm = System.Reflection.Assembly.LoadFrom("C:\dotnetapp2005\Formulati\saldiLotti\bin\Debug\saldilotti.dll")
            t = asm.GetTypes()
            ty = asm.GetType(lFullName)
            m = ty.GetMethods()
            For Each mm In m
                If mm.Name = "executeConversion" Then
                    Dim o As Object
                    o = Activator.CreateInstance(ty)
                    Dim oo As Object() = {pStato, lDescri} 'Parametri separati da virgola
                    'If there is no parameter in method then on the place of oo pass the null
                    mm.Invoke(o, oo)
                End If
            Next
            lFileOk = lNomefileinf.Replace(".inf", ".ok")
            System.IO.File.Move("conv\" & lNomefileinf, "conv\" & lFileOk)
            System.IO.File.Delete("conv\" & lNomefileinf)
            MsgBox("Procedure di conversione terminate!", MsgBoxStyle.Information, "")
            pStato.text = ""
            pStato.refresh()
        Next
    End Sub
    Public Function reverseData(ByVal pData As String) As String

        Dim tokField() As String
        Dim tokData() As String
        Dim sReturn As String
        Dim pSeparator As String
        '
        If pData.Contains("-") Then
            pSeparator = "-"
        End If
        If pData.Contains("/") Then
            pSeparator = "/"
        End If
        '
        Try
            tokField = pData.Split(" ") 'Separa la data dall' ora
            tokData = tokField(0).Split(pSeparator) 'separa i pezzi della data
            sReturn = tokData(2) & pSeparator & tokData(1) & pSeparator & tokData(0)
            If tokField.Length > 1 Then
                Return sReturn & " " & tokField(1).Trim
            Else
                Return sReturn
            End If

        Catch ex As Exception

        End Try

    End Function
    Public Function reverseDataSqlserver(ByVal pData As String) As String

        Dim tokField() As String
        Dim tokData() As String
        Dim sReturn As String
        Dim pSeparator As String
        '
        If pData.Contains("-") Then
            pSeparator = "-"
        End If
        If pData.Contains("/") Then
            pSeparator = "/"
        End If
        '
        Try
            tokField = pData.Split(" ") 'Separa la data dall' ora
            tokData = tokField(0).Split(pSeparator) 'separa i pezzi della data
            sReturn = tokData(2) & tokData(1) & tokData(0)
            If tokField.Length > 1 Then
                Return sReturn '& " " & tokField(1).Trim
            Else
                Return sReturn
            End If

        Catch ex As Exception

        End Try

    End Function
    Function LoadStructure(ByVal pNomeScheda As String, ByVal pScheda As Object, ByVal pTabella As String, ByVal pMaincontainer As Object) As Object
        Dim schede As New Schede
        schede.NomeScheda = pNomeScheda
        schede.Tabella = pTabella.ToLower
        schede.Scheda = pScheda
        schede.mainContainer = pMaincontainer
        schede.StrutturaTabella = leggi_struttura(pTabella.Trim, schede)
        schede.ElencoCampi = leggi_ordine(schede.Scheda, schede).m_elenco
        schede.ElencoTabIndex = leggi_ordine(schede.Scheda, schede).m_tabindex
        Return schede
    End Function
    ''' <summary>
    ''' This method starts at the specified container, and traverses all subcontainers.
    ''' It returns a List of object contained.
    ''' </summary>
    Public Shared Function GetRecursiveFields(ByVal initial As Object) As List(Of Object)
        ' This list stores the results.
        Dim result As New List(Of Object)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of Object)

        ' Add the initial directory
        stack.Push(initial)

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string
            Dim oContainer As Object = stack.Pop
            Try
                ' Add all immediate file paths
                result.AddRange(GetFields(oContainer))
                ' Loop through all subdirectories and add them to the stack.
                Dim oSubContainer As Object
                For Each oSubContainer In GetContainers(oContainer)
                    stack.Push(oSubContainer)
                Next

            Catch ex As Exception
            End Try
        Loop

        ' Return the list
        Return result
    End Function
    Private Overloads Shared Function GetFields(ByVal oContainer As Object) As List(Of Object)
        Dim oResult As New List(Of Object)
        For Each ob As Control In oContainer.controls
            'Se l' oggetto ob non contiene controlli figli è un oggetto finale (un campo) in caso contrario è un contenitore
            If ob.Controls.Count = 0 Then
                oResult.Add(ob)
            End If
        Next
        Return oResult
    End Function
    Private Overloads Shared Function GetFields(ByVal oContainer As Object, ByVal findName As String) As Object
        Dim oResult As New List(Of Object)

        Try
            'If findName Is Nothing Then
            'Exit Function
            'End If
            'If TypeOf oContainer Is String Then
            'Throw New System.Exception(oContainer & " non è Contenitore o non esiste!")
            'End If
            For Each ob As Control In oContainer.controls
                If ob.Controls.Count = 0 And ob.Name.Trim.ToUpper = findName.Trim.ToUpper Then
                    Return ob
                End If
            Next
            Return Nothing
        Catch ex As System.Exception
            'MsgBox(ex.Message, MsgBoxStyle.Critical, "GetFields")
        End Try
    End Function
    Private Shared Function GetContainers(ByVal oContainer As Object) As List(Of Object)
        Dim oResult As New List(Of Object)
        Try
            For Each ob As Control In oContainer.controls
                If ob.Controls.Count > 0 Then
                    oResult.Add(ob)
                End If
            Next
            Return oResult
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "GetContainer")
        End Try

    End Function
    ''' <summary>
    ''' This method return the object searched.
    ''' </summary>
    Public Overloads Function GetCtrl(ByVal initial As Object, ByVal findName As String) As Object
        ' This list stores the results.
        Try
            If findName.Trim = "" Then
                Return Nothing
            End If
            Dim result As New Object

            ' This stack stores the directories to process.
            Dim stack As New Stack(Of Object)

            ' Add the initial directory
            stack.Push(initial)

            ' Continue processing for each stacked container
            Do While (stack.Count > 0)
                ' Get top directory string
                Dim oContainer As Object = stack.Pop
                Try
                    ' Add all immediate control
                    Dim oCtrl As Object = GetFields(oContainer, findName)
                    If Not oCtrl Is Nothing Then
                        Return oCtrl
                    End If
                    ' Loop through all subcontainer and add them to the stack.
                    Dim oSubContainer As Object
                    For Each oSubContainer In GetContainers(oContainer)
                        stack.Push(oSubContainer)
                    Next

                Catch ex As Exception
                End Try
            Loop
            ' Return the list

        Catch ex As Exception

        End Try
        Return Nothing
    End Function
    ''' <summary>
    ''' This method search and return the focused text field starting by specific object 
    ''' Return nothing if error occurred
    ''' </summary>
    Public Overloads Function GetActiveTextField(ByVal initial As Object) As ClSLib.CTextBoxEx
        ' This list stores the results.
        Try
            Dim result As New ClSLib.CTextBoxEx

            ' This stack stores the directories to process.
            Dim stack As New Stack(Of Object)

            ' Add the initial directory
            stack.Push(initial)

            ' Continue processing for each stacked container
            Do While (stack.Count > 0)
                ' Get top directory string
                Dim oContainer As Object = stack.Pop
                Try
                    ' Add all immediate control
                    For Each obTxtField As Control In oContainer.controls
                        If TypeOf obTxtField Is ClSLib.CTextBoxEx Then
                            If obTxtField.Focused = True Then
                                Return obTxtField
                            End If
                        End If
                    Next
                    ' Loop through all subcontainer and add them to the stack.
                    Dim oSubContainer As Object
                    For Each oSubContainer In GetContainers(oContainer)
                        stack.Push(oSubContainer)
                    Next

                Catch ex As Exception
                End Try
            Loop
            ' Return the list

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' This method search and return (by object array) all CTextbox starting by specific object 
    ''' Return nothing if error occurred
    ''' </summary>
    Public Overloads Function GetAllTextField(ByVal initial As Object) As ArrayList
        ' This list stores the results.
        Try
            Dim result As New ArrayList

            ' This stack stores the directories to process.
            Dim stack As New Stack(Of Object)

            ' Add the initial directory
            stack.Push(initial)

            ' Continue processing for each stacked container
            Do While (stack.Count > 0)
                ' Get top directory string
                Dim oContainer As Object = stack.Pop
                Try
                    ' Add all immediate control
                    For Each obTxtField As Control In oContainer.controls
                        If TypeOf obTxtField Is ClSLib.CTextBoxEx Then
                            result.Add(obTxtField)
                        End If
                    Next
                    ' Loop through all subcontainer and add them to the stack.
                    Dim oSubContainer As Object
                    For Each oSubContainer In GetContainers(oContainer)
                        stack.Push(oSubContainer)
                    Next

                Catch ex As Exception
                End Try
            Loop
            Return result

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Get the query contained into pNomequery and operates substitude @AZ with Company name
    ''' </summary>
    ''' <param name="pStruttura"></param>
    ''' <param name="pNomequery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function getQuery(ByVal pStruttura As Schede, ByVal pNomequery As String) As String
        Try
            If pNomequery = "" Then
                Return ""
            End If
            Dim sr As System.IO.FileStream
            Dim Tablelist As New ArrayList
            Dim stringaQuery As String = ""
            Dim queryPath As String = "query\"
            sr = New System.IO.FileStream(queryPath & pNomequery, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            While filereader.Peek > -1
                stringaQuery = stringaQuery & " " & filereader.ReadLine
            End While
            Dim startPos As Integer = 0

            Do
                Dim iniPos As Integer = stringaQuery.IndexOf("@AZ", startPos)
                If iniPos = -1 Then
                    Exit Do
                End If
                Dim finPos As Integer = stringaQuery.IndexOf(" ", iniPos)
                If finPos = -1 Then
                    finPos = stringaQuery.Length
                End If
                Tablelist.Add(stringaQuery.Substring(iniPos + 3, finPos - iniPos - 3))
                startPos = finPos
            Loop
            For Each element As String In Tablelist

                Dim stringToreplace As String = getTablename(element) & " as " & element.Trim
                stringaQuery = stringaQuery.Replace("@AZ" & element, stringToreplace)
            Next
            Return stringaQuery.TrimStart.TrimEnd
        Catch ex As System.Exception
            MsgBox("Problemi durante il caricamento della query : " & pNomequery, MsgBoxStyle.Critical, "getQuery")
        End Try

    End Function
    ''' <summary>
    ''' This version of function does not execute any replacing; it return the query as written in the file pNomequery file
    ''' </summary>
    ''' <param name="pNomequery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function getQuery(ByVal pNomequery As String) As String
        Try
            If pNomequery = "" Then
                Return ""
            End If
            Dim sr As System.IO.FileStream
            Dim Tablelist As New ArrayList
            Dim stringaQuery As String = ""
            Dim queryPath As String = "query\"
            sr = New System.IO.FileStream(queryPath & pNomequery, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            While filereader.Peek > -1
                stringaQuery = stringaQuery & " " & filereader.ReadLine
            End While
            Return stringaQuery.TrimStart.TrimEnd
        Catch ex As System.Exception
            MsgBox("Problemi durante il caricamento della query : " & pNomequery, MsgBoxStyle.Critical, "getQuery")
        End Try

    End Function
    ''' <summary>
    ''' Get the query contained into pNomequery and operates substitude @AZ with Company name
    ''' </summary>
    ''' <param name="pStruttura"></param>
    ''' <param name="pNomequery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function getQuery(ByVal pAziCode As String, ByVal pNomequery As String) As String
        Try
            If pNomequery = "" Then
                Return ""
            End If
            Dim sr As System.IO.FileStream
            Dim Tablelist As New ArrayList
            Dim stringaQuery As String = ""
            Dim queryPath As String = "query\"
            sr = New System.IO.FileStream(queryPath & pNomequery, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            While filereader.Peek > -1
                stringaQuery = stringaQuery & " " & filereader.ReadLine
            End While
            Dim startPos As Integer = 0

            Do
                Dim iniPos As Integer = stringaQuery.IndexOf("@AZ", startPos)
                If iniPos = -1 Then
                    Exit Do
                End If
                Dim finPos As Integer = stringaQuery.IndexOf(" ", iniPos)
                If finPos = -1 Then
                    finPos = stringaQuery.Length
                End If
                Tablelist.Add(stringaQuery.Substring(iniPos + 3, finPos - iniPos - 3))
                startPos = finPos
            Loop
            For Each element As String In Tablelist

                Dim stringToreplace As String = pAziCode.Trim & element.Trim & " AS " & element.Trim
                stringaQuery = stringaQuery.Replace("@AZ" & element, stringToreplace)
            Next
            Return stringaQuery.TrimStart.TrimEnd
        Catch ex As System.Exception
            MsgBox("Problemi durante il caricamento della query : " & pNomequery, MsgBoxStyle.Critical, "getQuery")
        End Try

    End Function
    ''' <summary>
    ''' Get the query contained into pNomequery and operates substitude @AZ with Company name
    ''' </summary>
    ''' <param name="pStruttura"></param>
    ''' <param name="pNomequery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function getQuery(ByVal pAziCode As String, ByVal pNomequery As String, ByVal pParameter As Hashtable) As String
        Try
            If pNomequery = "" Then
                Return ""
            End If
            Dim sr As System.IO.FileStream
            Dim Tablelist As New ArrayList
            Dim stringaQuery As String = ""
            Dim queryPath As String = "query\"
            sr = New System.IO.FileStream(queryPath & pNomequery, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            While filereader.Peek > -1
                stringaQuery = stringaQuery & " " & filereader.ReadLine
            End While
            Dim startPos As Integer = 0

            Do
                Dim iniPos As Integer = stringaQuery.IndexOf("@AZ", startPos)
                If iniPos = -1 Then
                    Exit Do
                End If
                Dim finPos As Integer = stringaQuery.IndexOf(" ", iniPos)
                If finPos = -1 Then
                    finPos = stringaQuery.Length
                End If
                Tablelist.Add(stringaQuery.Substring(iniPos + 3, finPos - iniPos - 3))
                startPos = finPos
            Loop
            For Each element As String In Tablelist

                Dim stringToreplace As String = pAziCode.Trim & element.Trim & " AS " & element.Trim
                stringaQuery = stringaQuery.Replace("@AZ" & element, stringToreplace)
            Next
            If Not pParameter Is Nothing Then
                For Each element As String In pParameter.Keys
                    Dim ppar1 As String = pParameter(element)
                    If element = "@AZ" Then
                        Throw New Exception("Il parametro non puo' essere uguale ad @AZ")
                    End If
                    stringaQuery = stringaQuery.Replace(element, ppar1)
                Next
            End If
            Return stringaQuery.TrimStart.TrimEnd
        Catch ex As System.Exception
            MsgBox("Problemi durante il caricamento della query : " & pNomequery, MsgBoxStyle.Critical, "getQuery")
        End Try

    End Function

    Public Function normalizeQuery(ByVal stringaSql As String) As String
        Try
            Dim startPos As Integer = 0
            Dim Tablelist As New ArrayList

            Do
                Dim iniPos As Integer = stringaSql.IndexOf("@AZ", startPos)
                If iniPos = -1 Then
                    Exit Do
                End If
                Dim finPos As Integer = stringaSql.IndexOf(" ", iniPos)
                If finPos = -1 Then
                    finPos = stringaSql.Length
                End If
                Tablelist.Add(stringaSql.Substring(iniPos + 3, finPos - iniPos - 3))
                startPos = finPos
            Loop
            For Each element As String In Tablelist

                Dim stringToreplace As String = getTablename(element) & " as " & element.Trim
                stringaSql = stringaSql.Replace("@AZ" & element, stringToreplace)
            Next
            Return stringaSql.TrimStart.TrimEnd

        Catch ex As Exception

        End Try
    End Function
    Private Function getTipo(ByVal pSchede As Object, ByVal pCampo As String) As Integer

        Try
            Dim lStr As StructCampi
            For Each element As String In pSchede.ElencoCampi.Keys
                lStr = pSchede.ElencoCampi(element)
                If lStr.SCampoForm.ToUpper = pCampo.ToUpper Then
                    Return lStr.STipo
                End If
            Next
            Return 0
        Catch ex As Exception
        End Try

    End Function
    Public Function getMessaggi(ByVal pTipodoc As String, ByVal pCliente As Long, ByVal pFornitore As Long) As String
        Dim vMessaggio As String = ""
        Dim strsql As String = ""

        Try
            'Cerca i messaggi dedicati al cliente e/o al fornitore
            strsql = "SELECT MSCODMSG, MSMESSAG FROM " & Me.getTablename("messaggi") & " WHERE MSDATINI <= " & ValAdapter(Now.Date.ToString, TipoCampo.TData) & _
                                   " AND MSDATFIN >= " & ValAdapter(Now.Date.ToString, TipoCampo.TData) & _
                                   " AND MSTIPDOC = " & ValAdapter(pTipodoc.ToUpper, TipoCampo.TChar)
            If pCliente <> 0 Then
                strsql = strsql & " AND MSCODCLI = " & ValAdapter(pCliente.ToString, TipoCampo.TLong)
            End If
            If pCliente <> 0 Then
                strsql = strsql & " AND MSCODFOR = " & ValAdapter(pFornitore.ToString, TipoCampo.TLong)
            End If
            strsql = strsql & " LIMIT 1 "
            'Cerca messaggi generici
            strsql = strsql & "UNION ALL SELECT MSCODMSG, MSMESSAG FROM " & Me.getTablename("messaggi") & " WHERE MSDATINI <= " & ValAdapter(Now.Date.ToString, TipoCampo.TData) & _
                                   " AND MSDATFIN >= " & ValAdapter(Now.Date.ToString, TipoCampo.TData) & _
                                   " AND MSTIPDOC = " & ValAdapter(pTipodoc.ToUpper, TipoCampo.TChar) & _
                                   " AND MSCODCLI = 0 AND MSCODCLI = 0"
            strsql = strsql & " ORDER BY MSCODMSG LIMIT 1"
            Dim dsM As DataSet = esegui_query(strsql)
            If dsM.Tables(0).Rows.Count > 0 Then
                vMessaggio = dsM.Tables(0).Rows(0).Item("MSMESSAG")
            End If
        Catch ex As Exception

        Finally
        End Try
        Return vMessaggio


    End Function
    Public Function getDatiAzienda(ByVal Codice As String) As Boolean

        Dim strsql As String = ""

        Try

            strsql = "Select azienda.AZCODICE, azienda.AZRAGSOC, azienda.AZINDIRI, azienda.AZ___LOC, " & _
                     "azienda.AZ__PROV, azienda.AZTELEFO, azienda.AZTELFAX, azienda.AZ___CAP, azienda.AZPARIVA, " & _
                     "azienda.AZCODFIS, azienda.AZRAGSO1, azienda.AZRAGSO2 From azienda where AZCODICE = '" & Codice & "'"
            Dim dsM As DataSet = esegui_query(strsql)
            If dsM.Tables(0).Rows.Count > 0 Then
                Globale.gDatiAz.CodAzi = dsM.Tables(0).Rows(0).Item("AZCODICE")
                Globale.gDatiAz.RagSoc1 = dsM.Tables(0).Rows(0).Item("AZRAGSOC")
                Globale.gDatiAz.RagSoc2 = dsM.Tables(0).Rows(0).Item("AZRAGSO1")
                Globale.gDatiAz.RagSoc3 = dsM.Tables(0).Rows(0).Item("AZRAGSO2")
                Globale.gDatiAz.Indiri = dsM.Tables(0).Rows(0).Item("AZINDIRI")
                Globale.gDatiAz.Cap = dsM.Tables(0).Rows(0).Item("AZ___CAP")
                Globale.gDatiAz.Locali = dsM.Tables(0).Rows(0).Item("AZ___LOC")
                Globale.gDatiAz.Prov = dsM.Tables(0).Rows(0).Item("AZ__PROV")
                Globale.gDatiAz.Piva = dsM.Tables(0).Rows(0).Item("AZPARIVA")
                Globale.gDatiAz.Codfisc = dsM.Tables(0).Rows(0).Item("AZCODFIS")
                Globale.gDatiAz.Tel = dsM.Tables(0).Rows(0).Item("AZTELEFO")
                Globale.gDatiAz.Fax = dsM.Tables(0).Rows(0).Item("AZTELFAX")
            End If
            Return True
        Catch ex As Exception
            MsgBox("Errore in lettura AZIENDA", MsgBoxStyle.Information, "LETTURA DATI AZIENDA")
            Return False
        Finally
        End Try

    End Function
    Public Function getTablename(ByVal tablename As String) As String
        Try
            If Globale.gSysTable.Contains(tablename) Then
                Return tablename.ToLower
            End If
            Return Globale.CodAzi.Trim.ToLower & tablename.ToLower
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "getTablename")
        End Try
    End Function
    Public Function fillCombo(ByVal oCombobox As ClSLib.CComboBoxEx, ByVal pTablename As String, ByVal pTablefield As String) As Boolean

        Try

            Dim ds As DataSet
            Dim dr As DataRow
            Dim res As Int32
            Dim strsql As String = "SELECT " & pTablefield & " FROM " & getTablename(pTablename) & " ORDER BY " & pTablefield
            ds = esegui_query(strsql)
            For Each dr In ds.Tables(0).Rows
                oCombobox.Items.Add(dr.Item(pTablefield).ToString().Trim)
            Next
            ds = Nothing
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

End Class
