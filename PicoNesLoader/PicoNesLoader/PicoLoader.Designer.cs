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
            this.labelAppVersion = new System.Windows.Forms.Label();
            this.growLabel1 = new PicoNesLoader.GrowLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.button_AddRoms = new System.Windows.Forms.Button();
            this.groupBoxList = new System.Windows.Forms.GroupBox();
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.linkLabelUpdate = new System.Windows.Forms.LinkLabel();
            this.labelFlashBinaryEnd = new System.Windows.Forms.Label();
            this.labelFlashBinaryStart = new System.Windows.Forms.Label();
            this.labelFlashSize = new System.Windows.Forms.Label();
            this.labelProgramVersion = new System.Windows.Forms.Label();
            this.labelProgramName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBoxList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nesRomBindingSource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.labelAppVersion);
            this.groupBox1.Controls.Add(this.growLabel1);
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 430);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 71);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // labelAppVersion
            // 
            this.labelAppVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAppVersion.AutoSize = true;
            this.labelAppVersion.Location = new System.Drawing.Point(704, 53);
            this.labelAppVersion.Name = "labelAppVersion";
            this.labelAppVersion.Size = new System.Drawing.Size(70, 15);
            this.labelAppVersion.TabIndex = 3;
            this.labelAppVersion.Text = "labelVersion";
            // 
            // growLabel1
            // 
            this.growLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.growLabel1.Location = new System.Drawing.Point(6, 14);
            this.growLabel1.Name = "growLabel1";
            this.growLabel1.Size = new System.Drawing.Size(764, 15);
            this.growLabel1.TabIndex = 2;
            this.growLabel1.Text = "growLabel1";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(3, 42);
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
            this.button_AddRoms.Text = "Add Rom(s) to list";
            this.button_AddRoms.UseVisualStyleBackColor = true;
            this.button_AddRoms.Click += new System.EventHandler(this.button_AddRoms_Click);
            // 
            // groupBoxList
            // 
            this.groupBoxList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxList.Controls.Add(this.dataGridView1);
            this.groupBoxList.Location = new System.Drawing.Point(12, 27);
            this.groupBoxList.Name = "groupBoxList";
            this.groupBoxList.Size = new System.Drawing.Size(776, 233);
            this.groupBoxList.TabIndex = 1;
            this.groupBoxList.TabStop = false;
            this.groupBoxList.Text = "Add NES roms to the list below:";
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
            this.dataGridView1.Size = new System.Drawing.Size(770, 211);
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
            this.SizeinKBytes.DataPropertyName = "SizeinKBytes";
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
            this.buttonCreateTar.Text = "Flash To PicoSystem";
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
            this.panelButtons.Location = new System.Drawing.Point(0, 377);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(462, 57);
            this.panelButtons.TabIndex = 6;
            // 
            // labelTotalSize
            // 
            this.labelTotalSize.AutoSize = true;
            this.labelTotalSize.Location = new System.Drawing.Point(305, 35);
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
            this.label1.Size = new System.Drawing.Size(143, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Total size of roms to flash:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.linkLabelUpdate);
            this.groupBox3.Controls.Add(this.labelFlashBinaryEnd);
            this.groupBox3.Controls.Add(this.labelFlashBinaryStart);
            this.groupBox3.Controls.Add(this.labelFlashSize);
            this.groupBox3.Controls.Add(this.labelProgramVersion);
            this.groupBox3.Controls.Add(this.labelProgramName);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(15, 266);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(770, 100);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PicoSystem Info";
            // 
            // linkLabelUpdate
            // 
            this.linkLabelUpdate.AutoSize = true;
            this.linkLabelUpdate.Location = new System.Drawing.Point(242, 27);
            this.linkLabelUpdate.Name = "linkLabelUpdate";
            this.linkLabelUpdate.Size = new System.Drawing.Size(99, 15);
            this.linkLabelUpdate.TabIndex = 10;
            this.linkLabelUpdate.TabStop = true;
            this.linkLabelUpdate.Text = "Update Available!";
            this.linkLabelUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUpdate_LinkClicked);
            // 
            // labelFlashBinaryEnd
            // 
            this.labelFlashBinaryEnd.AutoSize = true;
            this.labelFlashBinaryEnd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelFlashBinaryEnd.Location = new System.Drawing.Point(530, 50);
            this.labelFlashBinaryEnd.Name = "labelFlashBinaryEnd";
            this.labelFlashBinaryEnd.Size = new System.Drawing.Size(40, 15);
            this.labelFlashBinaryEnd.TabIndex = 9;
            this.labelFlashBinaryEnd.Text = "label7";
            // 
            // labelFlashBinaryStart
            // 
            this.labelFlashBinaryStart.AutoSize = true;
            this.labelFlashBinaryStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelFlashBinaryStart.Location = new System.Drawing.Point(530, 27);
            this.labelFlashBinaryStart.Name = "labelFlashBinaryStart";
            this.labelFlashBinaryStart.Size = new System.Drawing.Size(40, 15);
            this.labelFlashBinaryStart.TabIndex = 8;
            this.labelFlashBinaryStart.Text = "label7";
            // 
            // labelFlashSize
            // 
            this.labelFlashSize.AutoSize = true;
            this.labelFlashSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelFlashSize.Location = new System.Drawing.Point(120, 73);
            this.labelFlashSize.Name = "labelFlashSize";
            this.labelFlashSize.Size = new System.Drawing.Size(40, 15);
            this.labelFlashSize.TabIndex = 7;
            this.labelFlashSize.Text = "label7";
            // 
            // labelProgramVersion
            // 
            this.labelProgramVersion.AutoSize = true;
            this.labelProgramVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelProgramVersion.Location = new System.Drawing.Point(120, 50);
            this.labelProgramVersion.Name = "labelProgramVersion";
            this.labelProgramVersion.Size = new System.Drawing.Size(40, 15);
            this.labelProgramVersion.TabIndex = 6;
            this.labelProgramVersion.Text = "label7";
            // 
            // labelProgramName
            // 
            this.labelProgramName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelProgramName.Location = new System.Drawing.Point(120, 27);
            this.labelProgramName.Name = "labelProgramName";
            this.labelProgramName.Size = new System.Drawing.Size(126, 15);
            this.labelProgramName.TabIndex = 5;
            this.labelProgramName.Text = "PicoSystem_InfoNes";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(425, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "Flash Binary End:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(425, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "Flash Binary Start:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Flash Size:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Program Version:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Program Name:";
            // 
            // PicoLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 526);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBoxList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PicoLoader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PicoSystem_InfoNes - NES Rom uploader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PicoLoader_FormClosing);
            this.Load += new System.EventHandler(this.PicoLoader_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nesRomBindingSource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBoxList;
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
        private LinkLabel linkLabel1;
        private PicoNesLoader.GrowLabel growLabel1;
        private GroupBox groupBox3;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label labelFlashBinaryEnd;
        private Label labelFlashBinaryStart;
        private Label labelFlashSize;
        private Label labelProgramVersion;
        private Label labelProgramName;
        private LinkLabel linkLabelUpdate;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumnName;
        private DataGridViewTextBoxColumn SizeinKBytes;
        private DataGridViewButtonColumn DeleteRow;
        private Label labelAppVersion;
    }
}