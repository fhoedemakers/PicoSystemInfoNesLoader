namespace PicoNesLoader
{
    partial class PicoLoader
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.growLabel1 = new PicoNesLoader.GrowLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.button_AddRoms = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SizeinKBytes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeleteRow = new System.Windows.Forms.DataGridViewButtonColumn();
            this.nesRomBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.openFileDialogNES = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.buttonCreateTar = new System.Windows.Forms.Button();
            this.timerCheckPico = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCheckPico = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelLinkToDriver = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.labelTotalSize = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nesRomBindingSource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.growLabel1);
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 398);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // growLabel1
            // 
            this.growLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.growLabel1.Location = new System.Drawing.Point(6, 19);
            this.growLabel1.Name = "growLabel1";
            this.growLabel1.Size = new System.Drawing.Size(764, 15);
            this.growLabel1.TabIndex = 2;
            this.growLabel1.Text = "growLabel1";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(1, 80);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(301, 15);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://github.com/fhoedemakers/PicoSystem_InfoNes";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // button_AddRoms
            // 
            this.button_AddRoms.Location = new System.Drawing.Point(12, 3);
            this.button_AddRoms.Name = "button_AddRoms";
            this.button_AddRoms.Size = new System.Drawing.Size(142, 23);
            this.button_AddRoms.TabIndex = 2;
            this.button_AddRoms.Text = "Add Rom(s)";
            this.button_AddRoms.UseVisualStyleBackColor = true;
            this.button_AddRoms.Click += new System.EventHandler(this.button_AddRoms_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(12, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 305);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add NES roms to the list below:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumnName,
            this.SizeinKBytes,
            this.DeleteRow});
            this.dataGridView1.DataSource = this.nesRomBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(770, 283);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column1.DataPropertyName = "Mapper";
            this.Column1.HeaderText = "Mapper";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 73;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ValidRom";
            this.dataGridViewTextBoxColumn1.HeaderText = "Valid?";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 43;
            // 
            // dataGridViewTextBoxColumnName
            // 
            this.dataGridViewTextBoxColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnName.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumnName.HeaderText = "Name";
            this.dataGridViewTextBoxColumnName.Name = "dataGridViewTextBoxColumnName";
            this.dataGridViewTextBoxColumnName.ReadOnly = true;
            this.dataGridViewTextBoxColumnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SizeinKBytes
            // 
            this.SizeinKBytes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SizeinKBytes.DataPropertyName = "SizeInBytes";
            this.SizeinKBytes.HeaderText = "KB";
            this.SizeinKBytes.Name = "SizeinKBytes";
            this.SizeinKBytes.ReadOnly = true;
            this.SizeinKBytes.Width = 46;
            // 
            // DeleteRow
            // 
            this.DeleteRow.HeaderText = "";
            this.DeleteRow.Name = "DeleteRow";
            this.DeleteRow.ReadOnly = true;
            this.DeleteRow.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DeleteRow.Text = "Delete";
            this.DeleteRow.UseColumnTextForButtonValue = true;
            // 
            // nesRomBindingSource
            // 
            this.nesRomBindingSource.AllowNew = false;
            this.nesRomBindingSource.DataSource = typeof(PicoNesLoader.NesRom);
            // 
            // openFileDialogNES
            // 
            this.openFileDialogNES.DefaultExt = "nes";
            this.openFileDialogNES.Filter = "nes roms|*.nes";
            this.openFileDialogNES.Multiselect = true;
            this.openFileDialogNES.Title = "Browse for .nes files";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.saveToolStripMenuItem.Text = "Exit";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(161, 3);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(143, 23);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "Delete Selected Rows";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Location = new System.Drawing.Point(310, 3);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(127, 23);
            this.buttonClearAll.TabIndex = 3;
            this.buttonClearAll.Text = "Clear List";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // buttonCreateTar
            // 
            this.buttonCreateTar.Enabled = false;
            this.buttonCreateTar.Location = new System.Drawing.Point(12, 31);
            this.buttonCreateTar.Name = "buttonCreateTar";
            this.buttonCreateTar.Size = new System.Drawing.Size(142, 23);
            this.buttonCreateTar.TabIndex = 4;
            this.buttonCreateTar.Text = "Upload To PicoSystem";
            this.buttonCreateTar.UseVisualStyleBackColor = true;
            this.buttonCreateTar.Click += new System.EventHandler(this.buttonCreateTar_Click);
            // 
            // timerCheckPico
            // 
            this.timerCheckPico.Interval = 1000;
            this.timerCheckPico.Tick += new System.EventHandler(this.timerCheckPico_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCheckPico,
            this.toolStripStatusLabelLinkToDriver,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 504);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelCheckPico
            // 
            this.toolStripStatusLabelCheckPico.Name = "toolStripStatusLabelCheckPico";
            this.toolStripStatusLabelCheckPico.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabelCheckPico.Text = "Checking Pico...";
            // 
            // toolStripStatusLabelLinkToDriver
            // 
            this.toolStripStatusLabelLinkToDriver.IsLink = true;
            this.toolStripStatusLabelLinkToDriver.Name = "toolStripStatusLabelLinkToDriver";
            this.toolStripStatusLabelLinkToDriver.Size = new System.Drawing.Size(167, 17);
            this.toolStripStatusLabelLinkToDriver.Text = "Click here to install USB driver.";
            this.toolStripStatusLabelLinkToDriver.Click += new System.EventHandler(this.toolStripStatusLabelLinkToDriver_Click);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.labelTotalSize);
            this.panelButtons.Controls.Add(this.label1);
            this.panelButtons.Controls.Add(this.buttonClearAll);
            this.panelButtons.Controls.Add(this.buttonDelete);
            this.panelButtons.Controls.Add(this.buttonCreateTar);
            this.panelButtons.Controls.Add(this.button_AddRoms);
            this.panelButtons.Location = new System.Drawing.Point(0, 335);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(462, 57);
            this.panelButtons.TabIndex = 6;
            // 
            // labelTotalSize
            // 
            this.labelTotalSize.AutoSize = true;
            this.labelTotalSize.Location = new System.Drawing.Point(279, 35);
            this.labelTotalSize.Name = "labelTotalSize";
            this.labelTotalSize.Size = new System.Drawing.Size(13, 15);
            this.labelTotalSize.TabIndex = 7;
            this.labelTotalSize.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(161, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Total size of archive:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(478, 339);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonCreateTar_Click);
            // 
            // PicoLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 526);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PicoLoader";
            this.Text = "PicoSystem_InfoNes - NES Rom uploader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PicoLoader_FormClosing);
            this.Load += new System.EventHandler(this.PicoLoader_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nesRomBindingSource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button button1;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn validRomDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private BindingSource nesRomBindingSource;
        private Button button3;
        private Button button_AddRoms;
        private OpenFileDialog openFileDialogNES;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private Button buttonDelete;
        private Button buttonClearAll;
        private Button buttonCreateTar;
        private System.Windows.Forms.Timer timerCheckPico;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelCheckPico;
        private ToolStripStatusLabel toolStripStatusLabelLinkToDriver;
        private ToolStripProgressBar toolStripProgressBar1;
        private Panel panelButtons;
        private Label labelTotalSize;
        private Label label1;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumnName;
        private DataGridViewTextBoxColumn SizeinKBytes;
        private DataGridViewButtonColumn DeleteRow;
        private LinkLabel linkLabel1;
        private PicoNesLoader.GrowLabel growLabel1;
        private Button button2;
    }
}