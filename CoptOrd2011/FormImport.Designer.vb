<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class formImport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(formImport))
        Dim LinkDati1 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati2 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati3 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati4 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati5 As ClSLib.LinkDati = New ClSLib.LinkDati
        Me.btnImporta = New System.Windows.Forms.Button
        Me.ImgList = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnUscita = New System.Windows.Forms.Button
        Me.btnChangeFold = New System.Windows.Forms.Button
        Me.btnRiduci = New System.Windows.Forms.Button
        Me.btnConfig = New System.Windows.Forms.Button
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer
        Me.RectangleShape4 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
        Me.RectangleShape3 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
        Me.RectangleShape2 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
        Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape
        Me.shpMain = New Microsoft.VisualBasic.PowerPacks.RectangleShape
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.txtFolder = New ClSLib.CTextBoxEx
        Me.btnFolder = New System.Windows.Forms.Button
        Me.txtCliFile = New ClSLib.CTextBoxEx
        Me.txtIncFile = New ClSLib.CTextBoxEx
        Me.txtEmifile = New ClSLib.CTextBoxEx
        Me.FBD1 = New System.Windows.Forms.FolderBrowserDialog
        Me.txtCodAzi = New ClSLib.CTextBoxEx
        Me.SuspendLayout()
        '
        'btnImporta
        '
        Me.btnImporta.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnImporta.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImporta.Image = CType(resources.GetObject("btnImporta.Image"), System.Drawing.Image)
        Me.btnImporta.Location = New System.Drawing.Point(94, 12)
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
        'btnUscita
        '
        Me.btnUscita.BackColor = System.Drawing.Color.Gainsboro
        Me.btnUscita.BackgroundImage = CType(resources.GetObject("btnUscita.BackgroundImage"), System.Drawing.Image)
        Me.btnUscita.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnUscita.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUscita.ImageIndex = 0
        Me.btnUscita.Location = New System.Drawing.Point(452, 20)
        Me.btnUscita.Name = "btnUscita"
        Me.btnUscita.Size = New System.Drawing.Size(32, 32)
        Me.btnUscita.TabIndex = 64
        Me.btnUscita.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnUscita, "Uscita")
        Me.btnUscita.UseVisualStyleBackColor = False
        '
        'btnChangeFold
        '
        Me.btnChangeFold.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnChangeFold.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnChangeFold.Image = CType(resources.GetObject("btnChangeFold.Image"), System.Drawing.Image)
        Me.btnChangeFold.Location = New System.Drawing.Point(147, 12)
        Me.btnChangeFold.Name = "btnChangeFold"
        Me.btnChangeFold.Size = New System.Drawing.Size(48, 48)
        Me.btnChangeFold.TabIndex = 70
        Me.btnChangeFold.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnChangeFold, "Cambia cartella")
        Me.btnChangeFold.UseVisualStyleBackColor = False
        '
        'btnRiduci
        '
        Me.btnRiduci.BackColor = System.Drawing.Color.Gainsboro
        Me.btnRiduci.BackgroundImage = CType(resources.GetObject("btnRiduci.BackgroundImage"), System.Drawing.Image)
        Me.btnRiduci.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnRiduci.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRiduci.ImageIndex = 0
        Me.btnRiduci.Location = New System.Drawing.Point(414, 20)
        Me.btnRiduci.Name = "btnRiduci"
        Me.btnRiduci.Size = New System.Drawing.Size(32, 32)
        Me.btnRiduci.TabIndex = 74
        Me.btnRiduci.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnRiduci, "Riduci ad icona")
        Me.btnRiduci.UseVisualStyleBackColor = False
        '
        'btnConfig
        '
        Me.btnConfig.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfig.Image = CType(resources.GetObject("btnConfig.Image"), System.Drawing.Image)
        Me.btnConfig.Location = New System.Drawing.Point(201, 12)
        Me.btnConfig.Name = "btnConfig"
        Me.btnConfig.Size = New System.Drawing.Size(48, 48)
        Me.btnConfig.TabIndex = 85
        Me.btnConfig.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnConfig, "Configurazione")
        Me.btnConfig.UseVisualStyleBackColor = False
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape4, Me.RectangleShape3, Me.RectangleShape2, Me.RectangleShape1, Me.shpMain})
        Me.ShapeContainer1.Size = New System.Drawing.Size(532, 262)
        Me.ShapeContainer1.TabIndex = 1
        Me.ShapeContainer1.TabStop = False
        '
        'RectangleShape4
        '
        Me.RectangleShape4.BackgroundImage = CType(resources.GetObject("RectangleShape4.BackgroundImage"), System.Drawing.Image)
        Me.RectangleShape4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.RectangleShape4.BorderColor = System.Drawing.Color.Transparent
        Me.RectangleShape4.Location = New System.Drawing.Point(22, 195)
        Me.RectangleShape4.Name = "RectangleShape4"
        Me.RectangleShape4.Size = New System.Drawing.Size(181, 19)
        '
        'RectangleShape3
        '
        Me.RectangleShape3.BackgroundImage = CType(resources.GetObject("RectangleShape3.BackgroundImage"), System.Drawing.Image)
        Me.RectangleShape3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.RectangleShape3.BorderColor = System.Drawing.Color.Transparent
        Me.RectangleShape3.Location = New System.Drawing.Point(71, 158)
        Me.RectangleShape3.Name = "RectangleShape3"
        Me.RectangleShape3.Size = New System.Drawing.Size(189, 19)
        '
        'RectangleShape2
        '
        Me.RectangleShape2.BackgroundImage = CType(resources.GetObject("RectangleShape2.BackgroundImage"), System.Drawing.Image)
        Me.RectangleShape2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.RectangleShape2.BorderColor = System.Drawing.Color.Transparent
        Me.RectangleShape2.Location = New System.Drawing.Point(87, 117)
        Me.RectangleShape2.Name = "RectangleShape2"
        Me.RectangleShape2.Size = New System.Drawing.Size(189, 19)
        '
        'RectangleShape1
        '
        Me.RectangleShape1.BackgroundImage = CType(resources.GetObject("RectangleShape1.BackgroundImage"), System.Drawing.Image)
        Me.RectangleShape1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.RectangleShape1.BorderColor = System.Drawing.Color.Transparent
        Me.RectangleShape1.Location = New System.Drawing.Point(-1, 71)
        Me.RectangleShape1.Name = "RectangleShape1"
        Me.RectangleShape1.Size = New System.Drawing.Size(188, 19)
        '
        'shpMain
        '
        Me.shpMain.BackgroundImage = CType(resources.GetObject("shpMain.BackgroundImage"), System.Drawing.Image)
        Me.shpMain.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque
        Me.shpMain.BorderColor = System.Drawing.Color.Transparent
        Me.shpMain.CornerRadius = 25
        Me.shpMain.Location = New System.Drawing.Point(6, 5)
        Me.shpMain.Name = "shpMain"
        Me.shpMain.SelectionColor = System.Drawing.Color.Transparent
        Me.shpMain.Size = New System.Drawing.Size(520, 253)
        '
        'Timer1
        '
        '
        'txtFolder
        '
        Me.txtFolder.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtFolder.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
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
        Me.txtFolder.LinkDati = LinkDati1
        Me.txtFolder.Location = New System.Drawing.Point(91, 92)
        Me.txtFolder.MaxLength = 50
        Me.txtFolder.Messaggio = Nothing
        Me.txtFolder.Name = "txtFolder"
        Me.txtFolder.NumeroDecimali = 0
        Me.txtFolder.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtFolder.Size = New System.Drawing.Size(294, 20)
        Me.txtFolder.Tabella = Nothing
        Me.txtFolder.TabIndex = 83
        Me.txtFolder.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'btnFolder
        '
        Me.btnFolder.Location = New System.Drawing.Point(385, 90)
        Me.btnFolder.Name = "btnFolder"
        Me.btnFolder.Size = New System.Drawing.Size(24, 23)
        Me.btnFolder.TabIndex = 82
        Me.btnFolder.Text = "?"
        Me.btnFolder.UseVisualStyleBackColor = True
        '
        'txtCliFile
        '
        Me.txtCliFile.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCliFile.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
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
        Me.txtCliFile.LinkDati = LinkDati2
        Me.txtCliFile.Location = New System.Drawing.Point(91, 214)
        Me.txtCliFile.MaxLength = 50
        Me.txtCliFile.Messaggio = Nothing
        Me.txtCliFile.Name = "txtCliFile"
        Me.txtCliFile.NumeroDecimali = 0
        Me.txtCliFile.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCliFile.ReadOnly = True
        Me.txtCliFile.Size = New System.Drawing.Size(227, 20)
        Me.txtCliFile.Tabella = Nothing
        Me.txtCliFile.TabIndex = 79
        Me.txtCliFile.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'txtIncFile
        '
        Me.txtIncFile.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtIncFile.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
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
        Me.txtIncFile.LinkDati = LinkDati3
        Me.txtIncFile.Location = New System.Drawing.Point(91, 176)
        Me.txtIncFile.MaxLength = 50
        Me.txtIncFile.Messaggio = Nothing
        Me.txtIncFile.Name = "txtIncFile"
        Me.txtIncFile.NumeroDecimali = 0
        Me.txtIncFile.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtIncFile.ReadOnly = True
        Me.txtIncFile.Size = New System.Drawing.Size(227, 20)
        Me.txtIncFile.Tabella = Nothing
        Me.txtIncFile.TabIndex = 77
        Me.txtIncFile.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
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
        Me.txtEmifile.Location = New System.Drawing.Point(91, 134)
        Me.txtEmifile.MaxLength = 50
        Me.txtEmifile.Messaggio = Nothing
        Me.txtEmifile.Name = "txtEmifile"
        Me.txtEmifile.NumeroDecimali = 0
        Me.txtEmifile.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtEmifile.ReadOnly = True
        Me.txtEmifile.Size = New System.Drawing.Size(227, 20)
        Me.txtEmifile.Tabella = Nothing
        Me.txtEmifile.TabIndex = 75
        Me.txtEmifile.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'txtCodAzi
        '
        Me.txtCodAzi.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtCodAzi.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
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
        Me.txtCodAzi.LinkDati = LinkDati5
        Me.txtCodAzi.Location = New System.Drawing.Point(368, 214)
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
        'formImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(532, 262)
        Me.Controls.Add(Me.btnConfig)
        Me.Controls.Add(Me.txtCodAzi)
        Me.Controls.Add(Me.txtFolder)
        Me.Controls.Add(Me.btnFolder)
        Me.Controls.Add(Me.txtCliFile)
        Me.Controls.Add(Me.txtIncFile)
        Me.Controls.Add(Me.txtEmifile)
        Me.Controls.Add(Me.btnRiduci)
        Me.Controls.Add(Me.btnChangeFold)
        Me.Controls.Add(Me.btnUscita)
        Me.Controls.Add(Me.btnImporta)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "formImport"
        Me.Text = "Importazione fatture"
        Me.TransparencyKey = System.Drawing.SystemColors.ControlDarkDark
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ImgList As System.Windows.Forms.ImageList
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnImporta As System.Windows.Forms.Button
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents shpMain As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents btnUscita As System.Windows.Forms.Button
    Friend WithEvents btnChangeFold As System.Windows.Forms.Button
    Friend WithEvents btnRiduci As System.Windows.Forms.Button
    Friend WithEvents txtFolder As ClSLib.CTextBoxEx
    Friend WithEvents btnFolder As System.Windows.Forms.Button
    Friend WithEvents txtCliFile As ClSLib.CTextBoxEx
    Friend WithEvents txtIncFile As ClSLib.CTextBoxEx
    Friend WithEvents txtEmifile As ClSLib.CTextBoxEx
    Friend WithEvents RectangleShape2 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents RectangleShape1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents RectangleShape4 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents RectangleShape3 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents FBD1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents txtCodAzi As ClSLib.CTextBoxEx
    Friend WithEvents btnConfig As System.Windows.Forms.Button
End Class
