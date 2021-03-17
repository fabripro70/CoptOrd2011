Imports System
Imports System.io
Imports System.Net
Imports System.Net.Sockets
Imports System.Globalization
Imports System.Text
Imports Microsoft.VisualBasic
Imports iTextSharp
Imports iTextSharp.text.pdf


Public Class formDisoc

    'For moving form by dragging
    Dim loca As New Point(0, 0)
    Dim curs As New Point(System.Windows.Forms.Cursor.Position)
    Dim curpos As New Point(0, 0)
    '
    Private Structure Ive
        Dim _impIva As Decimal
        Dim _imponi As Decimal
        Dim _codcon As String
    End Structure
    '
    Private Structure RifPar
        Dim _pnserial As String
        Dim _cproword As Integer
        Dim _cprownum As Integer
    End Structure
    Private Structure pnt_dett
        Dim _Numdoc As String
        Dim _alfdoc As String
        Dim _DatDoc As String
        Dim _Codese As String
        Dim _DatReg As String
        Dim _tipcon As String
        Dim _Codcli As String
        Dim _Segno As String
        Dim _Importo As Decimal
        Dim _Conto As String
        Dim _Codiva As String
        Dim _impIva As Decimal
        Dim _Imponi As Decimal
        Dim _Codpag As String
    End Structure

    Dim cf As New CConfig
    Dim op As New COpeFile
    '
    Dim _hspn As New Hashtable
    Dim _h_hspn As New CHashGest(_hspn)
    '
    Dim _aKey_pn As New ArrayList
    '
    Dim _hCli As New Hashtable ' Contains the traslation from GECIM customer code to ADHOC customer code.
    Dim _h_hCli As New CHashGest(_hCli)
    '
    Dim importedFolder As String = ""
    Private Sub formEva_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        cn_dbf.Close()
    End Sub

    Private Sub FormExport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If String.IsNullOrEmpty(Globale.CodAzi) Then
            Config.ShowDialog()
            Exit Sub
        End If
        Me.txtBarcode.Text = ""
        Me.txtCodAzi.Text = Globale.CodAzi
        '

        op.imposta_connessione(Globale.ConnectionString)
        'cn_ext.ConnectionString = Globale.ConnectionString
        'cn_ext.Open()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Config.Show()
    End Sub

    Private Sub shpMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        curpos = Cursor.Position
    End Sub
    Private Sub Position()
        loca = Me.Location
        curs = System.Windows.Forms.Cursor.Position
    End Sub

    Private Sub shpMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Timer1.Start()
        Position()
    End Sub

    Private Sub shpmain_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Timer1.Stop()
        Position()
    End Sub


    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Location = loca - curs + System.Windows.Forms.Cursor.Position
    End Sub
    Private Sub btnUscita_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Application.Exit()
    End Sub
    Private Sub btnRiduci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        'Questa funzione serve per intercettare il tasto F10 e fare in modo che quando si preme non si abiliti il menu

        Select Case keyData
            Case Keys.Escape 'Uscita
                Application.Exit()
            Case Else
                Return MyBase.ProcessCmdKey(msg, keyData)
        End Select
        Return True
        'Return MyBase.ProcessCmdKey(msg, keyData)
    End Function


    Private Sub btnFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim savePath As String = System.IO.Directory.GetCurrentDirectory
        With OFD1
            .Filter = "All files|*.*"
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                For Each oFile As String In .FileNames
                    txtBarcode.Text = oFile.Trim
                    '
                Next
            End If
        End With
        'Me.btnSave_Click(Nothing, Nothing)
        System.IO.Directory.SetCurrentDirectory(savePath)
    End Sub

    Private Sub btnChangeFold_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim savePath As String = System.IO.Directory.GetCurrentDirectory
        With FBD1
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtBarcode.Text = .SelectedPath
            End If
        End With
        'With OFD1
        '.Filter = "Tutti i files|*.*"
        'If .ShowDialog = Windows.Forms.DialogResult.OK Then
        'For Each oFile As String In .FileNames
        'lstSpedire.Items.Add(oFile)
        'Next
        'End If
        'End With
        'Me.btnSave_Click(Nothing, Nothing)
        System.IO.Directory.SetCurrentDirectory(savePath)
    End Sub

    Private Sub btnImporta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImporta.Click
        _hCli.Clear()
        Me.sostituisci()
        MsgBox("Operazione eseguita", MsgBoxStyle.Exclamation)
    End Sub
    Private Sub sostituisci()

        Try
            Dim _codart As String = ""
            Dim _desart As String = ""
            Dim _dessup As String = ""
            Dim _unimis As String = ""

            Dim strsql As String = "select cacodice from " & Me.GetTableNameLocal("KEY_ARTI") & _
                                   " where cacodart = " & op.ValAdapter(Me.txtAttuale.Text.Trim, TipoCampo.TChar) & _
                                   " and cacodice = " & op.ValAdapter(Me.txtBarcode.Text.Trim, TipoCampo.TChar)
            Dim ds As DataSet = op.esegui_query(strsql)
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New Exception("Associazione non trovata")
            End If
            strsql = "select ardesart, ardessup, arunmis1 from " & Me.GetTableNameLocal("ART_ICOL") & _
                     " where arcodart = " & op.ValAdapter(Me.txtNuovo.Text.Trim, TipoCampo.TChar)
            ds = op.esegui_query(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                _codart = Me.txtNuovo.Text.Trim
                _desart = ds.Tables(0).Rows(0).Item("ardesart").ToString
                _dessup = ds.Tables(0).Rows(0).Item("ardessup").ToString
                _unimis = ds.Tables(0).Rows(0).Item("arunmis1").ToString
            Else
                Throw New Exception("Articolo nuovo inesistente !")
            End If
            strsql = "update " & Me.GetTableNameLocal("KEY_ARTI") & _
                   " SET CACODART = " & op.ValAdapter(_codart, TipoCampo.TChar) & _
                   ", CADESART = " & op.ValAdapter(_desart, TipoCampo.TChar) & _
                   ", CADESSUP = " & op.ValAdapter(_dessup, TipoCampo.TChar) & _
                   ", CAUNIMIS = " & op.ValAdapter(_unimis, TipoCampo.TChar) & _
                   " WHERE CACODICE = " & op.ValAdapter(Me.txtBarcode.Text.Trim, TipoCampo.TChar) & _
                   " AND CACODART = " & op.ValAdapter(Me.txtAttuale.Text.Trim, TipoCampo.TChar)
            op.esegui_query(strsql)
            '
            '
        Catch ex As IO.IOException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "sostituisci articoli")
            op.RollbackTrans()
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "sostituisci articoli")
            op.RollbackTrans()
        End Try
    End Sub
    Private Sub ImportaListini()

        Try
            Dim _pnt_dett As New pnt_dett
            Dim _a_key As String = ""
            Dim _prog As String = ""
            Dim rigafile As String = ""
            Dim _flNC As Boolean = False
            Dim wFileName As String = "*" & Me.txtBarcode.Text.Trim
            Dim _PairFldVal As New Hashtable

            'Lettura file

            Dim sr As System.IO.FileStream
            sr = New System.IO.FileStream(Me.txtBarcode.Text.Trim, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            Dim _Index As Integer = 1
            Dim _campi
            Dim __key_lis_tini As String = ""
            Dim __key_lis_scag As String = ""
            Dim __key_listini As String = ""
            Dim _key_lis_tini As String = ""
            Dim _key_lis_scag As String = ""
            Dim _key_listini As String = ""
            op.BeginTrans()
            While filereader.Peek > -1
                rigafile = filereader.ReadLine
                _campi = rigafile.Split(";")
                '
                Dim _Codlis As String = _campi(0)
                Dim _Deslis As String = _campi(1)
                Dim _ivalis As String = _campi(2)
                Dim _vallis As String = _campi(3)
                Dim _datatt As String = _campi(5)
                Dim _datdis As String = _campi(6)
                Dim _unimis As String = _campi(7)
                Dim _codart As String = _campi(8)
                Dim _rownum As String = _campi(9)
                Dim _quanti As String = _campi(10)
                Dim _prezzo As String = _campi(11)
                Dim _scont1 As String = _campi(12)
                Dim _scont2 As String = _campi(13)
                Dim _scont3 As String = _campi(14)
                Dim _scont4 As String = _campi(15)
                _key_listini = _Codlis
                _key_lis_tini = _Codlis.Trim & _codart.Trim & _rownum.ToString.Trim
                _key_lis_scag = _codart.Trim & _rownum.ToString.Trim & _quanti.ToString.Trim
                '
                'LISTINI
                '
                If _key_listini <> __key_listini Then
                    '
                    __key_listini = _Codlis
                    '
                    _PairFldVal.Clear()
                    _PairFldVal.Add("LSCODLIS", op.ValAdapter(_Codlis, TipoCampo.TChar))
                    _PairFldVal.Add("LSDESLIS", op.ValAdapter(_Deslis, TipoCampo.TChar))
                    _PairFldVal.Add("LSIVALIS", op.ValAdapter(_ivalis, TipoCampo.TChar))
                    _PairFldVal.Add("LSVALLIS", op.ValAdapter(_vallis, TipoCampo.TChar))
                    _PairFldVal.Add("LSQUANTI", op.ValAdapter("S", TipoCampo.TData))
                    _PairFldVal.Add("UTDC", op.ValAdapter(Globale.g_SysData, TipoCampo.TChar))
                    _PairFldVal.Add("UTDV", op.ValAdapter(Globale.g_SysData, TipoCampo.TChar))
                    '
                    Dim _strFld As String = ""
                    Dim _strVal As String = ""
                    Dim strsql As String = "insert into " & Me.GetTableNameLocal("LISTINI") & " ( "
                    '
                    For Each _element As String In _PairFldVal.Keys
                        _strFld = _strFld & _element & ","
                        _strVal = _strVal & _PairFldVal(_element) & ","
                    Next
                    strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                    '
                    op.esegui_query(strsql)
                End If
                '
                'LIS_TINI
                '
                If _key_lis_tini <> __key_lis_tini Then
                    '
                    _key_lis_tini = _Codlis.Trim & _codart.Trim & _rownum.ToString.Trim
                    '
                    _PairFldVal.Clear()
                    _PairFldVal.Add("LICODART", op.ValAdapter(_codart, TipoCampo.TChar))
                    _PairFldVal.Add("CPROWNUM", op.ValAdapter(_rownum, TipoCampo.TChar))
                    _PairFldVal.Add("LICODLIS", op.ValAdapter(_Codlis, TipoCampo.TChar))
                    _PairFldVal.Add("LIDATATT", op.ValAdapter(_datatt, TipoCampo.TData))
                    _PairFldVal.Add("LIDATDIS", op.ValAdapter(_datdis, TipoCampo.TData))
                    '
                    Dim _strFld As String = ""
                    Dim _strVal As String = ""
                    Dim strsql As String = "insert into " & Me.GetTableNameLocal("LIS_TINI") & " ( "
                    '
                    For Each _element As String In _PairFldVal.Keys
                        _strFld = _strFld & _element & ","
                        _strVal = _strVal & _PairFldVal(_element) & ","
                    Next
                    strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                    '
                    op.esegui_query(strsql)
                End If
                '
                'LIS_SCAG
                '
                If _key_lis_scag <> __key_lis_scag Then
                    '
                    _key_lis_scag = _codart.Trim & _rownum.ToString.Trim & _quanti.ToString.Trim
                    '
                    _PairFldVal.Clear()
                    _PairFldVal.Add("LICODART", op.ValAdapter(_codart, TipoCampo.TChar))
                    _PairFldVal.Add("LIROWNUM", op.ValAdapter(_rownum, TipoCampo.TChar))
                    _PairFldVal.Add("LIQUANTI", op.ValAdapter(_quanti, TipoCampo.TChar))
                    _PairFldVal.Add("LIPREZZO", op.ValAdapter(_prezzo, TipoCampo.TCur))
                    _PairFldVal.Add("LISCONT1", op.ValAdapter(_scont1, TipoCampo.TCur))
                    _PairFldVal.Add("LISCONT2", op.ValAdapter(_scont2, TipoCampo.TCur))
                    _PairFldVal.Add("LISCONT3", op.ValAdapter(_scont3, TipoCampo.TCur))
                    _PairFldVal.Add("LISCONT4", op.ValAdapter(_scont4, TipoCampo.TCur))
                    '
                    Dim _strFld As String = ""
                    Dim _strVal As String = ""
                    Dim strsql As String = "insert into " & Me.GetTableNameLocal("LIS_SCAG") & " ( "
                    '
                    For Each _element As String In _PairFldVal.Keys
                        _strFld = _strFld & _element & ","
                        _strVal = _strVal & _PairFldVal(_element) & ","
                    Next
                    strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                    '
                    op.esegui_query(strsql)
                End If

            End While
            op.CommitTrans()
            sr.Close()
            '
        Catch ex As IO.IOException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import listini")
            op.RollbackTrans()
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import listini")
            op.RollbackTrans()
        End Try
    End Sub
    Private Function GetTableNameLocal(ByVal pTablename) As String
        Return Globale.CodAzi.Trim & pTablename
    End Function

    Private Sub btnConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfig.Click
        Config.ShowDialog()
        '
        Me.txtBarcode.Text = Globale.PathFile
        Me.txtCodAzi.Text = Globale.CodAzi
        '
    End Sub
    Private Sub fileLaunch()
        Dim pInf As New System.Diagnostics.ProcessStartInfo
        pInf.FileName = "notepad.exe"
        pInf.Arguments = Environment.CurrentDirectory & "\" & "errori.txt"
        Process.Start(pInf)
    End Sub
    Private Function pnSerial() As Long
        '
        Dim _Autonum As Long
        Dim ds As DataSet
        Dim _key As String = "prog\SEPNT\'" & Globale.CodAzi.Trim & "'"
        Dim strsql As String = "select autonum from cpwarn where tablecode = " & op.ValAdapter(_key, TipoCampo.TChar)
        ds = op.esegui_query(strsql)
        If ds.Tables(0).Rows.Count > 0 Then
            _Autonum = ds.Tables(0).Rows(0).Item("autonum") + 1
        End If
        strsql = "update cpwarn set autonum = " & op.ValAdapter(_Autonum, TipoCampo.TLong) & " where tablecode = " & op.ValAdapter(_key, TipoCampo.TChar)
        ds = op.esegui_query(strsql)
        Return _Autonum
    End Function
    Private Function pnProg(ByVal pCodese As String) As Integer
        '
        Dim _Prog As Integer
        Dim ds As DataSet
        Dim _key As String = "prog\PRPNT\'" & Globale.CodAzi.Trim & "'\'" & pCodese & "'\0\'        ' "
        Dim strsql As String = "select autonum from cpwarn where tablecode = " & op.ValAdapter(_key, TipoCampo.TChar)
        ds = op.esegui_query(strsql)
        If ds.Tables(0).Rows.Count > 0 Then
            _Prog = ds.Tables(0).Rows(0).Item("autonum") + 1
        End If
        strsql = "update cpwarn set autonum = " & op.ValAdapter(_Prog, TipoCampo.TLong) & " where tablecode = " & op.ValAdapter(_key, TipoCampo.TChar)
        ds = op.esegui_query(strsql)
        Return _Prog
    End Function

End Class