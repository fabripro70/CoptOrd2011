<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class formDisoc
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(formDisoc))
        Dim LinkDati1 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati2 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati3 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati4 As ClSLib.LinkDati = New ClSLib.LinkDati
        Me.btnImporta = New System.Windows.Forms.Button
        Me.ImgList = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnConfig = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.txtBarcode = New ClSLib.CTextBoxEx
        Me.FBD1 = New System.Windows.Forms.FolderBrowserDialog
        Me.txtCodAzi = New ClSLib.CTextBoxEx
        Me.OFD1 = New System.Windows.Forms.OpenFileDialog
        Me.txtAttuale = New ClSLib.CTextBoxEx
        Me.txtNuovo = New ClSLib.CTextBoxEx
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnImporta
        '
        Me.btnImporta.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnImporta.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImporta.Image = CType(resources.GetObject("btnImporta.Image"), System.Drawing.Image)
        Me.btnImporta.Location = New System.Drawing.Point(209, 12)
        Me.btnImporta.Name = "btnImporta"
        Me.btnImporta.Size = New System.Drawing.Size(48, 48)
        Me.btnImporta.TabIndex = 23
        Me.btnImporta.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnImporta, "Inizia Importazione")
        Me.btnImporta.UseVisualStyleBackColor = False
        '
        'ImgList
        '
        Me.ImgList.ImageStream = CType(resources.GetObject("ImgList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImgList.TransparentColor = System.Drawing.Color.Transparent
        Me.ImgList.Images.SetKeyName(0, "Trash - Empty.ico")
        Me.ImgList.Images.SetKeyName(1, "Customize.ico")
        Me.ImgList.Images.SetKeyName(2, "Floppy.ico")
        Me.ImgList.Images.SetKeyName(3, "MAIL03.ICO")
        Me.ImgList.Images.SetKeyName(4, "Eject.ico")
        Me.ImgList.Images.SetKeyName(5, "Connexions.png")
        Me.ImgList.Images.SetKeyName(6, "cache.ico")
        Me.ImgList.Images.SetKeyName(7, "kate.ico")
        '
        'btnConfig
        '
        Me.btnConfig.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfig.Image = CType(resources.GetObject("btnConfig.Image"), System.Drawing.Image)
        Me.btnConfig.Location = New System.Drawing.Point(268, 12)
        Me.btnConfig.Name = "btnConfig"
        Me.btnConfig.Size = New System.Drawing.Size(48, 48)
        Me.btnConfig.TabIndex = 85
        Me.btnConfig.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnConfig, "Configurazione")
        Me.btnConfig.UseVisualStyleBackColor = False
        '
        'Timer1
        '
        '
        'txtBarcode
        '
        Me.txtBarcode.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtBarcode.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
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
        Me.txtBarcode.LinkDati = LinkDati1
        Me.txtBarcode.Location = New System.Drawing.Point(126, 92)
        Me.txtBarcode.MaxLength = 25
        Me.txtBarcode.Messaggio = Nothing
        Me.txtBarcode.Name = "txtBarcode"
        Me.txtBarcode.NumeroDecimali = 0
        Me.txtBarcode.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtBarcode.Size = New System.Drawing.Size(190, 20)
        Me.txtBarcode.Tabella = Nothing
        Me.txtBarcode.TabIndex = 0
        Me.txtBarcode.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'txtCodAzi
        '
        Me.txtCodAzi.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCodAzi.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
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
        Me.txtCodAzi.LinkDati = LinkDati2
        Me.txtCodAzi.Location = New System.Drawing.Point(238, 173)
        Me.txtCodAzi.MaxLength = 50
        Me.txtCodAzi.Messaggio = Nothing
        Me.txtCodAzi.Name = "txtCodAzi"
        Me.txtCodAzi.NumeroDecimali = 0
        Me.txtCodAzi.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCodAzi.ReadOnly = True
        Me.txtCodAzi.Size = New System.Drawing.Size(78, 20)
        Me.txtCodAzi.Tabella = Nothing
        Me.txtCodAzi.TabIndex = 84
        Me.txtCodAzi.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'OFD1
        '
        Me.OFD1.FileName = "OpenFileDialog1"
        Me.OFD1.Multiselect = True
        '
        'txtAttuale
        '
        Me.txtAttuale.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtAttuale.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
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
        Me.txtAttuale.LinkDati = LinkDati3
        Me.txtAttuale.Location = New System.Drawing.Point(126, 118)
        Me.txtAttuale.MaxLength = 25
        Me.txtAttuale.Messaggio = Nothing
        Me.txtAttuale.Name = "txtAttuale"
        Me.txtAttuale.NumeroDecimali = 0
        Me.txtAttuale.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtAttuale.Size = New System.Drawing.Size(190, 20)
        Me.txtAttuale.Tabella = Nothing
        Me.txtAttuale.TabIndex = 1
        Me.txtAttuale.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'txtNuovo
        '
        Me.txtNuovo.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtNuovo.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
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
        Me.txtNuovo.LinkDati = LinkDati4
        Me.txtNuovo.Location = New System.Drawing.Point(126, 144)
        Me.txtNuovo.MaxLength = 25
        Me.txtNuovo.Messaggio = Nothing
        Me.txtNuovo.Name = "txtNuovo"
        Me.txtNuovo.NumeroDecimali = 0
        Me.txtNuovo.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtNuovo.Size = New System.Drawing.Size(190, 20)
        Me.txtNuovo.Tabella = Nothing
        Me.txtNuovo.TabIndex = 2
        Me.txtNuovo.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(25, 95)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 88
        Me.Label1.Text = "Codice a barre :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(22, 121)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 13)
        Me.Label2.TabIndex = 89
        Me.Label2.Text = "Articolo attuale :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(26, 147)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(97, 13)
        Me.Label3.TabIndex = 90
        Me.Label3.Text = "Articolo nuovo :"
        '
        'formDisoc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Orange
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(342, 205)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNuovo)
        Me.Controls.Add(Me.txtAttuale)
        Me.Controls.Add(Me.btnConfig)
        Me.Controls.Add(Me.txtCodAzi)
        Me.Controls.Add(Me.txtBarcode)
        Me.Controls.Add(Me.btnImporta)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "formDisoc"
        Me.Text = "Sostituzione Articoli su Codice a barre"
        Me.TransparencyKey = System.Drawing.SystemColors.MenuHighlight
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ImgList As System.Windows.Forms.ImageList
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnImporta As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents txtBarcode As ClSLib.CTextBoxEx
    Friend WithEvents FBD1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnConfig As System.Windows.Forms.Button
    Friend WithEvents txtCodAzi As ClSLib.CTextBoxEx
    Friend WithEvents OFD1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txtAttuale As ClSLib.CTextBoxEx
    Friend WithEvents txtNuovo As ClSLib.CTextBoxEx
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
