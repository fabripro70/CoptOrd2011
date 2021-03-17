<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFilter
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.itemGrid = New System.Windows.Forms.DataGridView()
        Me.Tipo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Codice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Descrizione = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Index = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtCodice = New System.Windows.Forms.TextBox()
        Me.txtDescrizione = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.btnSalva = New System.Windows.Forms.Button()
        Me.btnElimina = New System.Windows.Forms.Button()
        Me.btnAggiorna = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbSito = New System.Windows.Forms.Label()
        Me.txtIndex = New System.Windows.Forms.TextBox()
        Me.TxtIndexSite = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.itemGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'itemGrid
        '
        Me.itemGrid.AllowUserToAddRows = False
        Me.itemGrid.AllowUserToDeleteRows = False
        Me.itemGrid.AllowUserToOrderColumns = True
        Me.itemGrid.AllowUserToResizeColumns = False
        Me.itemGrid.AllowUserToResizeRows = False
        Me.itemGrid.BackgroundColor = System.Drawing.Color.SteelBlue
        Me.itemGrid.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.itemGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightCyan
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.itemGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.itemGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.itemGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Tipo, Me.Codice, Me.Descrizione, Me.Index})
        Me.itemGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke
        Me.itemGrid.Location = New System.Drawing.Point(12, 92)
        Me.itemGrid.Name = "itemGrid"
        Me.itemGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.itemGrid.RowHeadersWidth = 4
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.itemGrid.RowsDefaultCellStyle = DataGridViewCellStyle4
        Me.itemGrid.Size = New System.Drawing.Size(647, 313)
        Me.itemGrid.TabIndex = 39
        Me.itemGrid.TabStop = False
        '
        'Tipo
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tipo.DefaultCellStyle = DataGridViewCellStyle2
        Me.Tipo.HeaderText = "Tipo"
        Me.Tipo.Name = "Tipo"
        Me.Tipo.Width = 150
        '
        'Codice
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Codice.DefaultCellStyle = DataGridViewCellStyle3
        Me.Codice.HeaderText = "Codice"
        Me.Codice.Name = "Codice"
        Me.Codice.Width = 120
        '
        'Descrizione
        '
        Me.Descrizione.HeaderText = "Descrizione"
        Me.Descrizione.Name = "Descrizione"
        Me.Descrizione.Width = 380
        '
        'Index
        '
        Me.Index.HeaderText = "Index"
        Me.Index.Name = "Index"
        Me.Index.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Yellow
        Me.Label5.Location = New System.Drawing.Point(177, 47)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 15)
        Me.Label5.TabIndex = 38
        Me.Label5.Text = "Codice"
        '
        'txtCodice
        '
        Me.txtCodice.BackColor = System.Drawing.Color.SteelBlue
        Me.txtCodice.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodice.ForeColor = System.Drawing.Color.White
        Me.txtCodice.Location = New System.Drawing.Point(179, 65)
        Me.txtCodice.Name = "txtCodice"
        Me.txtCodice.Size = New System.Drawing.Size(129, 21)
        Me.txtCodice.TabIndex = 37
        '
        'txtDescrizione
        '
        Me.txtDescrizione.BackColor = System.Drawing.Color.SteelBlue
        Me.txtDescrizione.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescrizione.ForeColor = System.Drawing.Color.White
        Me.txtDescrizione.Location = New System.Drawing.Point(312, 65)
        Me.txtDescrizione.Name = "txtDescrizione"
        Me.txtDescrizione.Size = New System.Drawing.Size(347, 21)
        Me.txtDescrizione.TabIndex = 40
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Yellow
        Me.Label1.Location = New System.Drawing.Point(312, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 15)
        Me.Label1.TabIndex = 41
        Me.Label1.Text = "Descrizione"
        '
        'cmbTipo
        '
        Me.cmbTipo.BackColor = System.Drawing.Color.SteelBlue
        Me.cmbTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTipo.ForeColor = System.Drawing.Color.White
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Items.AddRange(New Object() {"Articolo", "Gruppo merceologico", "Famiglia", "Tabella"})
        Me.cmbTipo.Location = New System.Drawing.Point(12, 65)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(164, 23)
        Me.cmbTipo.TabIndex = 42
        '
        'btnSalva
        '
        Me.btnSalva.Location = New System.Drawing.Point(177, 419)
        Me.btnSalva.Name = "btnSalva"
        Me.btnSalva.Size = New System.Drawing.Size(75, 23)
        Me.btnSalva.TabIndex = 45
        Me.btnSalva.TabStop = False
        Me.btnSalva.Text = "Salva"
        Me.btnSalva.UseVisualStyleBackColor = True
        '
        'btnElimina
        '
        Me.btnElimina.Location = New System.Drawing.Point(96, 419)
        Me.btnElimina.Name = "btnElimina"
        Me.btnElimina.Size = New System.Drawing.Size(75, 23)
        Me.btnElimina.TabIndex = 44
        Me.btnElimina.TabStop = False
        Me.btnElimina.Text = "Elimina"
        Me.btnElimina.UseVisualStyleBackColor = True
        '
        'btnAggiorna
        '
        Me.btnAggiorna.Location = New System.Drawing.Point(15, 419)
        Me.btnAggiorna.Name = "btnAggiorna"
        Me.btnAggiorna.Size = New System.Drawing.Size(75, 23)
        Me.btnAggiorna.TabIndex = 43
        Me.btnAggiorna.TabStop = False
        Me.btnAggiorna.Text = "Aggiorna"
        Me.btnAggiorna.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Yellow
        Me.Label2.Location = New System.Drawing.Point(12, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 15)
        Me.Label2.TabIndex = 46
        Me.Label2.Text = "Tipo"
        '
        'lbSito
        '
        Me.lbSito.AutoSize = True
        Me.lbSito.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbSito.ForeColor = System.Drawing.Color.Yellow
        Me.lbSito.Location = New System.Drawing.Point(18, 12)
        Me.lbSito.Name = "lbSito"
        Me.lbSito.Size = New System.Drawing.Size(41, 24)
        Me.lbSito.TabIndex = 47
        Me.lbSito.Text = "Sito"
        '
        'txtIndex
        '
        Me.txtIndex.Location = New System.Drawing.Point(443, 16)
        Me.txtIndex.Name = "txtIndex"
        Me.txtIndex.Size = New System.Drawing.Size(54, 20)
        Me.txtIndex.TabIndex = 48
        Me.txtIndex.Visible = False
        '
        'TxtIndexSite
        '
        Me.TxtIndexSite.Location = New System.Drawing.Point(626, 17)
        Me.TxtIndexSite.Name = "TxtIndexSite"
        Me.TxtIndexSite.ReadOnly = True
        Me.TxtIndexSite.Size = New System.Drawing.Size(33, 20)
        Me.TxtIndexSite.TabIndex = 49
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Yellow
        Me.Label3.Location = New System.Drawing.Point(553, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 15)
        Me.Label3.TabIndex = 50
        Me.Label3.Text = "Sequenza :"
        '
        'frmFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SteelBlue
        Me.ClientSize = New System.Drawing.Size(672, 446)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtIndexSite)
        Me.Controls.Add(Me.txtIndex)
        Me.Controls.Add(Me.lbSito)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnSalva)
        Me.Controls.Add(Me.btnElimina)
        Me.Controls.Add(Me.btnAggiorna)
        Me.Controls.Add(Me.cmbTipo)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtDescrizione)
        Me.Controls.Add(Me.itemGrid)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtCodice)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmFilter"
        Me.Text = "Filtro Articoli"
        CType(Me.itemGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents itemGrid As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtCodice As System.Windows.Forms.TextBox
    Friend WithEvents txtDescrizione As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
    Friend WithEvents btnSalva As System.Windows.Forms.Button
    Friend WithEvents btnElimina As System.Windows.Forms.Button
    Friend WithEvents btnAggiorna As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbSito As System.Windows.Forms.Label
    Friend WithEvents txtIndex As System.Windows.Forms.TextBox
    Friend WithEvents Tipo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Codice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Descrizione As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Index As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TxtIndexSite As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
