<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Config))
        Dim LinkDati1 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati2 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati3 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati4 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati5 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati6 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati7 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati8 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati9 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati10 As ClSLib.LinkDati = New ClSLib.LinkDati
        Me.FoldDialog = New System.Windows.Forms.FolderBrowserDialog
        Me.ImgList = New System.Windows.Forms.ImageList(Me.components)
        Me.BtnSave = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtNotaAcc = New ClSLib.CTextBoxEx
        Me.lbCauinc = New System.Windows.Forms.Label
        Me.txtCauinc = New ClSLib.CTextBoxEx
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtCauEmi = New ClSLib.CTextBoxEx
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtEmifile = New ClSLib.CTextBoxEx
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtIncFile = New ClSLib.CTextBoxEx
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtCliFile = New ClSLib.CTextBoxEx
        Me.Label8 = New System.Windows.Forms.Label
        Me.btnFolder = New System.Windows.Forms.Button
        Me.txtFolder = New ClSLib.CTextBoxEx
        Me.FBD1 = New System.Windows.Forms.FolderBrowserDialog
        Me.txtCodAzi = New ClSLib.CTextBoxEx
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtMastroCli = New ClSLib.CTextBoxEx
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtCodpag = New ClSLib.CTextBoxEx
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImgList
        '
        Me.ImgList.ImageStream = CType(resources.GetObject("ImgList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImgList.TransparentColor = System.Drawing.Color.Transparent
        Me.ImgList.Images.SetKeyName(0, "Trash - Empty.ico")
        Me.ImgList.Images.SetKeyName(1, "Customize.ico")
        Me.ImgList.Images.SetKeyName(2, "Floppy.ico")
        Me.ImgList.Images.SetKeyName(3, "MAIL03.ICO")
        '
        'BtnSave
        '
        Me.BtnSave.Image = CType(resources.GetObject("BtnSave.Image"), System.Drawing.Image)
        Me.BtnSave.Location = New System.Drawing.Point(431, 207)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(48, 48)
        Me.BtnSave.TabIndex = 31
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.txtNotaAcc)
        Me.GroupBox2.Controls.Add(Me.lbCauinc)
        Me.GroupBox2.Controls.Add(Me.txtCauinc)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.txtCauEmi)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 17)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(244, 119)
        Me.GroupBox2.TabIndex = 42
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Conti destinatari"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 83)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 13)
        Me.Label1.TabIndex = 46
        Me.Label1.Text = "Cau.nota acc. :"
        '
        'txtNotaAcc
        '
        Me.txtNotaAcc.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtNotaAcc.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati1.campiDb = Nothing
        LinkDati1.campiForm = Nothing
        LinkDati1.ChiaveSelezione = Nothing
        LinkDati1.Criteriofisso = Nothing
        LinkDati1.elencoCampi = Nothing
        LinkDati1.LinkCampi = Nothing
        LinkDati1.NUMREC = CType(0, Long)
        LinkDati1.puntatoreDb = Nothing
        LinkDati1.TabellaCollegata = Nothing
        LinkDati1.UsaNumrec = ClSLib.SiNO.NO
        LinkDati1.ZoomOnZoom = Nothing
        Me.txtNotaAcc.LinkDati = LinkDati1
        Me.txtNotaAcc.Location = New System.Drawing.Point(103, 80)
        Me.txtNotaAcc.MaxLength = 50
        Me.txtNotaAcc.Messaggio = Nothing
        Me.txtNotaAcc.Name = "txtNotaAcc"
        Me.txtNotaAcc.NumeroDecimali = 0
        Me.txtNotaAcc.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtNotaAcc.Size = New System.Drawing.Size(113, 20)
        Me.txtNotaAcc.Tabella = Nothing
        Me.txtNotaAcc.TabIndex = 45
        Me.txtNotaAcc.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'lbCauinc
        '
        Me.lbCauinc.AutoSize = True
        Me.lbCauinc.Location = New System.Drawing.Point(28, 57)
        Me.lbCauinc.Name = "lbCauinc"
        Me.lbCauinc.Size = New System.Drawing.Size(74, 13)
        Me.lbCauinc.TabIndex = 44
        Me.lbCauinc.Text = "Cau. incasso :"
        '
        'txtCauinc
        '
        Me.txtCauinc.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCauinc.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati2.campiDb = Nothing
        LinkDati2.campiForm = Nothing
        LinkDati2.ChiaveSelezione = Nothing
        LinkDati2.Criteriofisso = Nothing
        LinkDati2.elencoCampi = Nothing
        LinkDati2.LinkCampi = Nothing
        LinkDati2.NUMREC = CType(0, Long)
        LinkDati2.puntatoreDb = Nothing
        LinkDati2.TabellaCollegata = Nothing
        LinkDati2.UsaNumrec = ClSLib.SiNO.NO
        LinkDati2.ZoomOnZoom = Nothing
        Me.txtCauinc.LinkDati = LinkDati2
        Me.txtCauinc.Location = New System.Drawing.Point(103, 54)
        Me.txtCauinc.MaxLength = 50
        Me.txtCauinc.Messaggio = Nothing
        Me.txtCauinc.Name = "txtCauinc"
        Me.txtCauinc.NumeroDecimali = 0
        Me.txtCauinc.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCauinc.Size = New System.Drawing.Size(113, 20)
        Me.txtCauinc.Tabella = Nothing
        Me.txtCauinc.TabIndex = 43
        Me.txtCauinc.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 31)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 13)
        Me.Label4.TabIndex = 42
        Me.Label4.Text = "Cau. emissione :"
        '
        'txtCauEmi
        '
        Me.txtCauEmi.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCauEmi.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati3.campiDb = Nothing
        LinkDati3.campiForm = Nothing
        LinkDati3.ChiaveSelezione = Nothing
        LinkDati3.Criteriofisso = Nothing
        LinkDati3.elencoCampi = Nothing
        LinkDati3.LinkCampi = Nothing
        LinkDati3.NUMREC = CType(0, Long)
        LinkDati3.puntatoreDb = Nothing
        LinkDati3.TabellaCollegata = Nothing
        LinkDati3.UsaNumrec = ClSLib.SiNO.NO
        LinkDati3.ZoomOnZoom = Nothing
        Me.txtCauEmi.LinkDati = LinkDati3
        Me.txtCauEmi.Location = New System.Drawing.Point(103, 28)
        Me.txtCauEmi.MaxLength = 50
        Me.txtCauEmi.Messaggio = Nothing
        Me.txtCauEmi.Name = "txtCauEmi"
        Me.txtCauEmi.NumeroDecimali = 0
        Me.txtCauEmi.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCauEmi.Size = New System.Drawing.Size(113, 20)
        Me.txtCauEmi.Tabella = Nothing
        Me.txtCauEmi.TabIndex = 41
        Me.txtCauEmi.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 183)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(139, 13)
        Me.Label5.TabIndex = 44
        Me.Label5.Text = "Nome file emissione fattura :"
        '
        'txtEmifile
        '
        Me.txtEmifile.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtEmifile.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati4.campiDb = Nothing
        LinkDati4.campiForm = Nothing
        LinkDati4.ChiaveSelezione = Nothing
        LinkDati4.Criteriofisso = Nothing
        LinkDati4.elencoCampi = Nothing
        LinkDati4.LinkCampi = Nothing
        LinkDati4.NUMREC = CType(0, Long)
        LinkDati4.puntatoreDb = Nothing
        LinkDati4.TabellaCollegata = Nothing
        LinkDati4.UsaNumrec = ClSLib.SiNO.NO
        LinkDati4.ZoomOnZoom = Nothing
        Me.txtEmifile.LinkDati = LinkDati4
        Me.txtEmifile.Location = New System.Drawing.Point(157, 180)
        Me.txtEmifile.MaxLength = 50
        Me.txtEmifile.Messaggio = Nothing
        Me.txtEmifile.Name = "txtEmifile"
        Me.txtEmifile.NumeroDecimali = 0
        Me.txtEmifile.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtEmifile.Size = New System.Drawing.Size(227, 20)
        Me.txtEmifile.Tabella = Nothing
        Me.txtEmifile.TabIndex = 43
        Me.txtEmifile.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(64, 235)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(87, 13)
        Me.Label6.TabIndex = 46
        Me.Label6.Text = "Nome file clienti :"
        '
        'txtIncFile
        '
        Me.txtIncFile.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtIncFile.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati5.campiDb = Nothing
        LinkDati5.campiForm = Nothing
        LinkDati5.ChiaveSelezione = Nothing
        LinkDati5.Criteriofisso = Nothing
        LinkDati5.elencoCampi = Nothing
        LinkDati5.LinkCampi = Nothing
        LinkDati5.NUMREC = CType(0, Long)
        LinkDati5.puntatoreDb = Nothing
        LinkDati5.TabellaCollegata = Nothing
        LinkDati5.UsaNumrec = ClSLib.SiNO.NO
        LinkDati5.ZoomOnZoom = Nothing
        Me.txtIncFile.LinkDati = LinkDati5
        Me.txtIncFile.Location = New System.Drawing.Point(157, 206)
        Me.txtIncFile.MaxLength = 50
        Me.txtIncFile.Messaggio = Nothing
        Me.txtIncFile.Name = "txtIncFile"
        Me.txtIncFile.NumeroDecimali = 0
        Me.txtIncFile.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtIncFile.Size = New System.Drawing.Size(227, 20)
        Me.txtIncFile.Tabella = Nothing
        Me.txtIncFile.TabIndex = 45
        Me.txtIncFile.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(22, 209)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(129, 13)
        Me.Label7.TabIndex = 48
        Me.Label7.Text = "Nome file incasso fattura :"
        '
        'txtCliFile
        '
        Me.txtCliFile.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCliFile.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati6.campiDb = Nothing
        LinkDati6.campiForm = Nothing
        LinkDati6.ChiaveSelezione = Nothing
        LinkDati6.Criteriofisso = Nothing
        LinkDati6.elencoCampi = Nothing
        LinkDati6.LinkCampi = Nothing
        LinkDati6.NUMREC = CType(0, Long)
        LinkDati6.puntatoreDb = Nothing
        LinkDati6.TabellaCollegata = Nothing
        LinkDati6.UsaNumrec = ClSLib.SiNO.NO
        LinkDati6.ZoomOnZoom = Nothing
        Me.txtCliFile.LinkDati = LinkDati6
        Me.txtCliFile.Location = New System.Drawing.Point(157, 232)
        Me.txtCliFile.MaxLength = 50
        Me.txtCliFile.Messaggio = Nothing
        Me.txtCliFile.Name = "txtCliFile"
        Me.txtCliFile.NumeroDecimali = 0
        Me.txtCliFile.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCliFile.Size = New System.Drawing.Size(227, 20)
        Me.txtCliFile.Tabella = Nothing
        Me.txtCliFile.TabIndex = 47
        Me.txtCliFile.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(75, 157)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(76, 13)
        Me.Label8.TabIndex = 49
        Me.Label8.Text = "Percorso files :"
        '
        'btnFolder
        '
        Me.btnFolder.Location = New System.Drawing.Point(451, 152)
        Me.btnFolder.Name = "btnFolder"
        Me.btnFolder.Size = New System.Drawing.Size(24, 23)
        Me.btnFolder.TabIndex = 50
        Me.btnFolder.Text = "?"
        Me.btnFolder.UseVisualStyleBackColor = True
        '
        'txtFolder
        '
        Me.txtFolder.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtFolder.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati7.campiDb = Nothing
        LinkDati7.campiForm = Nothing
        LinkDati7.ChiaveSelezione = Nothing
        LinkDati7.Criteriofisso = Nothing
        LinkDati7.elencoCampi = Nothing
        LinkDati7.LinkCampi = Nothing
        LinkDati7.NUMREC = CType(0, Long)
        LinkDati7.puntatoreDb = Nothing
        LinkDati7.TabellaCollegata = Nothing
        LinkDati7.UsaNumrec = ClSLib.SiNO.NO
        LinkDati7.ZoomOnZoom = Nothing
        Me.txtFolder.LinkDati = LinkDati7
        Me.txtFolder.Location = New System.Drawing.Point(157, 154)
        Me.txtFolder.MaxLength = 50
        Me.txtFolder.Messaggio = Nothing
        Me.txtFolder.Name = "txtFolder"
        Me.txtFolder.NumeroDecimali = 0
        Me.txtFolder.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtFolder.Size = New System.Drawing.Size(294, 20)
        Me.txtFolder.Tabella = Nothing
        Me.txtFolder.TabIndex = 51
        Me.txtFolder.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'txtCodAzi
        '
        Me.txtCodAzi.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCodAzi.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati8.campiDb = Nothing
        LinkDati8.campiForm = Nothing
        LinkDati8.ChiaveSelezione = Nothing
        LinkDati8.Criteriofisso = Nothing
        LinkDati8.elencoCampi = Nothing
        LinkDati8.LinkCampi = Nothing
        LinkDati8.NUMREC = CType(0, Long)
        LinkDati8.puntatoreDb = Nothing
        LinkDati8.TabellaCollegata = Nothing
        LinkDati8.UsaNumrec = ClSLib.SiNO.NO
        LinkDati8.ZoomOnZoom = Nothing
        Me.txtCodAzi.LinkDati = LinkDati8
        Me.txtCodAzi.Location = New System.Drawing.Point(285, 72)
        Me.txtCodAzi.MaxLength = 50
        Me.txtCodAzi.Messaggio = Nothing
        Me.txtCodAzi.Name = "txtCodAzi"
        Me.txtCodAzi.NumeroDecimali = 0
        Me.txtCodAzi.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCodAzi.Size = New System.Drawing.Size(80, 20)
        Me.txtCodAzi.Tabella = Nothing
        Me.txtCodAzi.TabIndex = 53
        Me.txtCodAzi.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(284, 56)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(81, 13)
        Me.Label9.TabIndex = 52
        Me.Label9.Text = "Codice Azienda"
        '
        'txtMastroCli
        '
        Me.txtMastroCli.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtMastroCli.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati9.campiDb = Nothing
        LinkDati9.campiForm = Nothing
        LinkDati9.ChiaveSelezione = Nothing
        LinkDati9.Criteriofisso = Nothing
        LinkDati9.elencoCampi = Nothing
        LinkDati9.LinkCampi = Nothing
        LinkDati9.NUMREC = CType(0, Long)
        LinkDati9.puntatoreDb = Nothing
        LinkDati9.TabellaCollegata = Nothing
        LinkDati9.UsaNumrec = ClSLib.SiNO.NO
        LinkDati9.ZoomOnZoom = Nothing
        Me.txtMastroCli.LinkDati = LinkDati9
        Me.txtMastroCli.Location = New System.Drawing.Point(383, 72)
        Me.txtMastroCli.MaxLength = 50
        Me.txtMastroCli.Messaggio = Nothing
        Me.txtMastroCli.Name = "txtMastroCli"
        Me.txtMastroCli.NumeroDecimali = 0
        Me.txtMastroCli.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtMastroCli.Size = New System.Drawing.Size(92, 20)
        Me.txtMastroCli.Tabella = Nothing
        Me.txtMastroCli.TabIndex = 55
        Me.txtMastroCli.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(392, 56)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 13)
        Me.Label10.TabIndex = 54
        Me.Label10.Text = "Mastro Clienti"
        '
        'txtCodpag
        '
        Me.txtCodpag.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCodpag.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati10.campiDb = Nothing
        LinkDati10.campiForm = Nothing
        LinkDati10.ChiaveSelezione = Nothing
        LinkDati10.Criteriofisso = Nothing
        LinkDati10.elencoCampi = Nothing
        LinkDati10.LinkCampi = Nothing
        LinkDati10.NUMREC = CType(0, Long)
        LinkDati10.puntatoreDb = Nothing
        LinkDati10.TabellaCollegata = Nothing
        LinkDati10.UsaNumrec = ClSLib.SiNO.NO
        LinkDati10.ZoomOnZoom = Nothing
        Me.txtCodpag.LinkDati = LinkDati10
        Me.txtCodpag.Location = New System.Drawing.Point(285, 116)
        Me.txtCodpag.MaxLength = 50
        Me.txtCodpag.Messaggio = Nothing
        Me.txtCodpag.Name = "txtCodpag"
        Me.txtCodpag.NumeroDecimali = 0
        Me.txtCodpag.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCodpag.Size = New System.Drawing.Size(80, 20)
        Me.txtCodpag.Tabella = Nothing
        Me.txtCodpag.TabIndex = 57
        Me.txtCodpag.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(284, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 13)
        Me.Label2.TabIndex = 56
        Me.Label2.Text = "Codice pagamento fisso"
        '
        'Config
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(493, 274)
        Me.Controls.Add(Me.txtCodpag)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtMastroCli)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtCodAzi)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtFolder)
        Me.Controls.Add(Me.btnFolder)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtCliFile)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtIncFile)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtEmifile)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.BtnSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "Config"
        Me.Text = "Parametri di configurazione"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents FoldDialog As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ImgList As System.Windows.Forms.ImageList
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lbCauinc As System.Windows.Forms.Label
    Friend WithEvents txtCauinc As ClSLib.CTextBoxEx
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtCauEmi As ClSLib.CTextBoxEx
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtEmifile As ClSLib.CTextBoxEx
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtIncFile As ClSLib.CTextBoxEx
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtCliFile As ClSLib.CTextBoxEx
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnFolder As System.Windows.Forms.Button
    Friend WithEvents txtFolder As ClSLib.CTextBoxEx
    Friend WithEvents FBD1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents txtCodAzi As ClSLib.CTextBoxEx
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtMastroCli As ClSLib.CTextBoxEx
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNotaAcc As ClSLib.CTextBoxEx
    Friend WithEvents txtCodpag As ClSLib.CTextBoxEx
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
