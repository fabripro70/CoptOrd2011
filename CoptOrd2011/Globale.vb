Imports System.Globalization
Public Structure HlpStruct
    Dim IndiceColonna As Integer
    Dim NomeColonna As String
    Dim TitoloColonna As String
    Dim LunghezzaColonna As String
    Dim visibile As Boolean
End Structure
Public Structure Schede
    Dim NomeScheda As String
    Dim Scheda As Object
    Dim SchedeLinked() As Object
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
    Dim OrderBy As String               ' criterio di ordinamento di MainQuery
    Dim mainContainer As Object         ' Contenitore principale del form, di solito è Tab
End Structure
Public Structure datiAzienda
    Dim CodAzi As String
    Dim RagSoc1 As String
    Dim RagSoc2 As String
    Dim RagSoc3 As String
    Dim Indiri As String
    Dim Cap As String
    Dim Locali As String
    Dim Prov As String
    Dim Tel As String
    Dim Fax As String
    Dim Piva As String
    Dim Codfisc As String
End Structure
Module Globale

    Public Declare Function ReleaseCapture Lib "user32" () As Long
    Public Declare Function CreateRectRgn Lib "gdi32" (ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long) As Long
    Public Declare Function CreateRoundRectRgn Lib "gdi32" (ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long, ByVal X3 As Long, ByVal Y3 As Long) As Long
    Public Declare Function SetWindowRgn Lib "user32" (ByVal hwnd As Long, ByVal hRgn As Long, ByVal bRedraw As Long) As Long
    Public Declare Function BeginPath Lib "gdi32" (ByVal hdc As Long) As Long
    Public Declare Function TextOut Lib "gdi32" Alias "TextOutA" (ByVal hdc As Long, ByVal X As Long, ByVal Y As Long, ByVal lpString As String, ByVal NCount As Long) As Long
    Public Declare Function EndPath Lib "gdi32" (ByVal hdc As Long) As Long
    Public Declare Function PathToRegion Lib "gdi32" (ByVal hdc As Long) As Long
    Public Declare Function GetRgnBox Lib "gdi32" (ByVal hRgn As Long, ByVal lpRect As RECT) As Long
    Public Declare Function CreateRectRgnIndirect Lib "gdi32" (ByVal lpRect As RECT) As Long
    Public Declare Function CombineRgn Lib "gdi32" (ByVal hDestRgn As Long, ByVal hSrcRgn1 As Long, ByVal hSrcRgn2 As Long, ByVal nCombineMode As Long) As Long
    Public Declare Function DeleteObject Lib "gdi32" (ByVal hObject As Long) As Long
    Public Declare Function SelectClipPath Lib "gdi32" (ByVal hdc As Long, ByVal iMode As Long) As Long
    Public Declare Function SelectClipRgn Lib "gdi32" (ByVal hdc As Long, ByVal hRgn As Long) As Long
    Public Declare Function CreateBrushIndirect Lib "gdi32" (ByVal lpLogBrush As LOGBRUSH) As Long
    Public Declare Function CreateEllipticRgn Lib "gdi32" (ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long) As Long
    Public Declare Function CreatePen Lib "gdi32" (ByVal nPenStyle As Long, ByVal nWidth As Long, ByVal crColor As Long) As Long
    Public Declare Function CreatePolygonRgn Lib "gdi32" (ByVal lpPoint As POINTAPI, ByVal NCount As Long, ByVal nPolyFillMode As Long) As Long
    Public Declare Function PaintRgn Lib "gdi32" (ByVal hdc As Long, ByVal hRgn As Long) As Long
    Public Declare Function PtInRegion Lib "gdi32" (ByVal hRgn As Long, ByVal X As Long, ByVal Y As Long) As Long
    Public Declare Function SelectObject Lib "gdi32" (ByVal hdc As Long, ByVal hObject As Long) As Long
    Public Declare Function FrameRgn Lib "gdi32" (ByVal hdc As Long, ByVal hRgn As Long, ByVal hBrush As Long, ByVal nWidth As Long, ByVal nHeight As Long) As Long
    Public Declare Function CreateSolidBrush Lib "gdi32" (ByVal crColor As Long) As Long
    Public Declare Function GetCurrentObject Lib "gdi32" (ByVal hdc As Long, ByVal uObjectType As Long) As Long
    Public Declare Function StrokeAndFillPath Lib "gdi32" (ByVal hdc As Long) As Long
    Public Declare Function StrokePath Lib "gdi32" (ByVal hdc As Long) As Long
    Public Declare Function FillRect Lib "user32" (ByVal hdc As Long, ByVal lpRect As RECT, ByVal hBrush As Long) As Long
    Public Declare Function SaveDC Lib "gdi32" (ByVal hdc As Long) As Long
    Public Declare Function RestoreDC Lib "gdi32" (ByVal hdc As Long, ByVal nSavedDC As Long) As Long
    Public Declare Function GetDriveType Lib "kernel32" Alias "GetDriveTypeA" (ByVal nDrive As String) As Long
    Public Declare Function GetLogicalDriveStrings Lib "kernel32" Alias "GetLogicalDriveStringsA" (ByVal nBufferLength As Long, ByVal lpBuffer As String) As Long
    '
    Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long

    Public Structure RECT
        Dim Left As Long
        Dim Top As Long
        Dim Right As Long
        Dim dimBottom As Long
    End Structure

    Public Structure LOGBRUSH
        Dim lbStyle As Long
        Dim lbColor As Long
        Dim lbHatch As Long
    End Structure

    Public Structure POINTAPI
        Dim X As Long
        Dim Y As Long
    End Structure
    Public gDatiAz As New datiAzienda
    Public gElencoProg As New Hashtable ' chiave : nome modulo come lo si vede nel menu, dati : Item-name del menu
    Public gElencoProgk As New ArrayList 'questo array contiene solo le voci di menu ordinate
    Public gMappaCampi As New Hashtable  'Chiave : nome tabella.nome campo, dati : hashtable dei campi
    Public gSysTable As New Hashtable    'Contiene i nomi delle tabelle in comume con tutte le aziende
    Public gTable As New Hashtable       'Contiene i nomi delle tabelle che hanno in testa al nome il codice azienda
    Public xp As Long
    Public yp As Long
    Public cn_ext As New SqlClient.SqlConnection
    Public cn_dbf As New OleDb.OleDbConnection
    Public cn_dbext As New Object
    Public cn As New Object
    'Public cn As New SqlClient.SqlConnection
    'Public cnO As New Oracle.DataAccess.Client.OracleConnection
    Public opConn As New COpeFile
    Public Server As String
    Public TipoDb As String
    Public FonteODBC As String
    Public User As String
    Public Password As String
    Public DataBase As String
    Public percorsoApp As String
    Public FileExport As String
    Public NomeProgramma As String
    Public TitoloProgramma As String
    Public FonteExt_1 As String
    Public FonteExt_2 As String
    Public OkCtrl As Boolean
    Public g_SysData As Date = Today
    Public ConnessoG1 As Boolean = False
    Public FtpServer As String
    Public RemoteFolder As String
    Public cartellaAggLocale As String
    Public FtpUser As String
    Public FtpPwd As String
    Public CauEmi As String
    Public CauInc As String
    Public CauNC As String
    Public FileEmiFat As String
    Public FileIncFat As String
    Public FileCli As String
    Public ConnectionString As String
    Public CodAzi As String
    Public MastroCli As String
    Public codPag As String
    Public PathFile As String
    Public CodAge As String
    Public Utente As String
    Public StampaDatiAz As Boolean
    Public MultiAzi As Boolean
    Public doAccess As Boolean = False
    Public g_logo As String = ""
    Public ImportFolder As String
    Public DocType As String
    Public DocType_P As String
    Public Store As String
    Public CatCon As String
    Public ListDef As String
    Public gCompres As String
    Public gFasce As String
    Public gExtendLog As String
    Public gUTF8 As String
    Public gTableAgg As String
    Public gExpOrdStatus As String
    Public gDefPayCode As String
    Public gCodNaz As String
    Public gLenCodCli As String
    Public gCauCorrisp As String
    'Parametri gestore di e-mail
    Public g_Mittente As String
    Public g_SMTP As String
    Public g_Auth As String
    Public g_Utente As String
    Public g_Password As String
    Public g_PathFileImport As String
    Public g_PercorsoApp As String
    Public g_AutoSend As String
    Public g_PathMail As String
    Public g_PathMailSent As String
    Public g_Ssl As String
    Public g_Port As String
    Public g_emailTo As String
    Public g_emailCC As String
    Public g_CatCom As String
    '
    Public Enum GStato As Integer
        Inserimento = 0
        Modifica = 1
    End Enum
    Enum TlbBtn As Integer
        Conferma = 0
        Annulla = 1
        Aiuto = 2
        Altro = 4
        Cancella = 5
        Stampa = 6
        Uscita = 7
    End Enum
    Enum TlbOnOff As Integer
        Abilitato = 0
        Disabilitato = 1
    End Enum
    Public Function getFormByName(ByVal pFormname As String, ByVal pMainform As Object) As Form
        Try
            Dim frm As Form
            frm = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(pMainform.GetType.Assembly.GetName.Name, System.String.Concat(pMainform.GetType.Assembly.GetName.Name, ".", pFormname))
            Return frm
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "GetFormByName")
        End Try
    End Function
    Public Function CTran(ByVal valore As Object, ByVal ValRetIfNull As Object) As Object

        Try
            If valore Is Nothing Then
                Return ValRetIfNull
            End If
            If IsDBNull(valore) Then
                CTran = ValRetIfNull
                Exit Function
            End If
            If Trim(valore) = "" Then
                CTran = ValRetIfNull
            Else
                CTran = valore
            End If
        Catch
            Return ValRetIfNull
        End Try
    End Function
    Public Function IsNumeric(ByVal anyString As String) As Boolean
        If anyString Is Nothing Then
            anyString = ""
        End If
        If anyString.Length > 0 Then
            Dim dummyOut As Double = New Double()
            Dim cultureInfo As System.Globalization.CultureInfo = _
                New System.Globalization.CultureInfo("it-IT", True)

            Return Double.TryParse(anyString, System.Globalization.NumberStyles.Any, _
                cultureInfo.NumberFormat, dummyOut)
        Else
            Return False
        End If
    End Function
    Public Sub setToolbarState(ByVal pMainform As Object, ByVal pMode As Boolean, ByVal pTbs As Hashtable)
        Dim tlb As Object = pMainform.controls("tls")
        If pMode Then
            For Each element As ToolStripButton In tlb.items
                pTbs.Add(element.Name, element.Enabled)
            Next
        Else
            For Each element As String In pTbs.Keys
                CType(tlb, ToolStrip).Items(element).Enabled = pTbs(element)
            Next
        End If
    End Sub
    Public Function TlbTastoOnOff(ByVal maschera As Object, ByVal pulsante As TlbBtn, ByVal stato As TlbOnOff) As Boolean
        Dim Btn As Object
        '        For Each ob As Object In maschera
        'If ob.ToString = "tls" Then
        ' tlb = maschera.control
        'End If
        'Next
        Try
            Dim tlb As Object = maschera.controls("tls")
            Select Case pulsante
                Case TlbBtn.Conferma
                    Btn = tlb.items("tlsConferma") 'Conbferma
                Case TlbBtn.Annulla
                    Btn = tlb.items("tlsAnnulla") 'Annulla
                Case TlbBtn.Cancella
                    Btn = tlb.items("tlsCancella") 'Cancella
                Case TlbBtn.Altro
                    Btn = tlb.items("tlsAltro") 'Cancella
                Case TlbBtn.Aiuto
                    Btn = tlb.items("tlsAiuto") 'Aiuto
                Case TlbBtn.Stampa
                    Btn = tlb.items("tlsStampa") 'Stampa
                Case TlbBtn.Uscita
                    Btn = tlb.items("tlsUscita") 'Uscita
            End Select
            Select Case stato
                Case TlbOnOff.Abilitato
                    Btn.enabled = True
                Case TlbOnOff.Disabilitato
                    Btn.enabled = False
            End Select
        Catch
        End Try
    End Function
    Function trim_x(ByVal stringa As Object) As Object
        trim_x = IIf(IsDBNull(stringa), "", Trim(stringa))
    End Function
    Function DoControllo(ByVal form As System.Object) As Boolean
        Dim ii As Integer
        For ii = 0 To form.Controls.Count - 1
            If TypeOf form.controls(ii) Is TextBox Or TypeOf form.controls(ii) Is ComboBox Then
                If form.Controls(ii).Focus Then
                    DoControllo = True
                    Exit Function
                End If
            End If
        Next ii
        DoControllo = False
    End Function
    Function trasforma_stringa(ByVal strsql As String) As String

        Dim pString As String = ""
        Try
            pString = strsql.Replace("'", "''")
        Catch ex As Exception

        End Try
        Return pString
    End Function
    Function trasforma_stringa_old(ByVal strsql)
        Dim k1, k2 As Integer
        Dim stringa As String

        If IsDBNull(strsql) Then
            Return ""
        End If
        k1 = InStr(Trim$(strsql), "'")
        If k1 = 0 Then
            trasforma_stringa_old = strsql
        Else
            k2 = InStr(Mid$(strsql, k1 + 1), "'")
            If k2 = 0 Then
                trasforma_stringa_old = Left$(Trim$(strsql), k1) & Right$(strsql, (Len(strsql) - k1 + 1))
            Else
                stringa = Left$(Trim$(strsql), k1)
                Do Until k2 = 0
                    stringa = stringa & Mid$(strsql, k1, k2 + 1)
                    k1 = k2 + k1
                    k2 = InStr(Mid$(strsql, k1 + 1), "'")
                Loop
                trasforma_stringa_old = stringa & Right$(strsql, (Len(strsql) - k1 + 1))
            End If
        End If
        Exit Function
    End Function
    Public Function RovesciaData(ByVal data As String) As String
        Dim giorno As String
        Dim mese As String
        Dim anno As String
        If Trim$(data) = "" Then
            Exit Function
        End If
        Select Case Len(Trim$(data))
            Case 8
                giorno = data.Substring(0, 2)
                mese = data.Substring(3, 2)
                anno = data.Substring(6, 2)
            Case 10
                giorno = data.Substring(0, 2)
                mese = data.Substring(3, 2)
                anno = data.Substring(6, 4)
            Case Else
                MsgBox("Formato data Errato.", vbOKOnly, "ATTENZIONE!")
                RovesciaData = "ERR"
                Exit Function
        End Select
        If Not IsDate(giorno & "/" & mese & "/" & anno) Then
            MsgBox("Data Errata.", vbOKOnly, "ATTENZIONE!")
            RovesciaData = "ERR"
            Exit Function
        End If
        RovesciaData = anno & mese & giorno
    End Function
    Public Function ConverteDataSql(ByVal DataDaConvertire As String) As String
        Dim dt As Date

        dt = DataDaConvertire
        ConverteDataSql = dt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("it-IT"))

    End Function
    ''' <summary>
    ''' Questo metodo formatta pNumber in base al numero di decimali pNumdec voluto 
    ''' pSepar : se true visualizza il separatore di migliaia
    ''' ritorna il numero formattato
    ''' </summary>
    Public Function FormatNumber(ByVal pNumber As Double, ByVal pNumdec As Integer, ByVal pSepar As Boolean) As Object
        Dim nfi As NumberFormatInfo = New CultureInfo("it-IT", True).NumberFormat
        Try
            nfi.NumberDecimalDigits = pNumdec
            Dim myfloat As Double = pNumber
            If pSepar Then
                Return myfloat.ToString("N", nfi)
            Else
                nfi.CurrencyGroupSeparator = String.Empty
                Return myfloat.ToString("N", nfi)
            End If
        Catch
        End Try
    End Function
    ''' <summary>
    ''' Questo restituisce il pannello abilitato nella tabpage selezionata 
    ''' </summary>
    Public Function FindActivePan(ByVal pTabPage As TabPage) As Object
        For Each elPan As Object In pTabPage.Controls
            If TypeOf elPan Is Panel Then
                If elPan.Enabled Then
                    Return elPan
                End If
            End If
        Next
        Return Nothing
    End Function
    ''' <summary>
    ''' Questo restituisce il pannello abilitato nella tabpage selezionata 
    ''' </summary>
    Public Function DeactivateAllPan(ByVal pTabPage As TabPage)

        Try
            For Each elPan As Panel In pTabPage.Controls
                elPan.Enabled = False
            Next
        Catch
        End Try
    End Function
End Module
