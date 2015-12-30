namespace Exam
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_browse = new System.Windows.Forms.Button();
            this.f_path = new System.Windows.Forms.TextBox();
            this.txt_keyword = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.lbl_folder = new System.Windows.Forms.Label();
            this.lbl_keyword = new System.Windows.Forms.Label();
            this.data_results = new System.Windows.Forms.DataGridView();
            this.btn_exp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.data_results)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_browse
            // 
            this.btn_browse.Location = new System.Drawing.Point(486, 33);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(80, 20);
            this.btn_browse.TabIndex = 0;
            this.btn_browse.Text = "Browse";
            this.btn_browse.UseVisualStyleBackColor = true;
            this.btn_browse.Click += new System.EventHandler(this.btn_result_Click);
            // 
            // f_path
            // 
            this.f_path.Location = new System.Drawing.Point(12, 33);
            this.f_path.Name = "f_path";
            this.f_path.Size = new System.Drawing.Size(443, 20);
            this.f_path.TabIndex = 1;
            // 
            // txt_keyword
            // 
            this.txt_keyword.Location = new System.Drawing.Point(12, 85);
            this.txt_keyword.Name = "txt_keyword";
            this.txt_keyword.Size = new System.Drawing.Size(443, 20);
            this.txt_keyword.TabIndex = 2;
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(486, 85);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(80, 20);
            this.btn_search.TabIndex = 3;
            this.btn_search.Text = "Search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // lbl_folder
            // 
            this.lbl_folder.AutoSize = true;
            this.lbl_folder.Location = new System.Drawing.Point(12, 17);
            this.lbl_folder.Name = "lbl_folder";
            this.lbl_folder.Size = new System.Drawing.Size(29, 13);
            this.lbl_folder.TabIndex = 4;
            this.lbl_folder.Text = "Path";
            // 
            // lbl_keyword
            // 
            this.lbl_keyword.AutoSize = true;
            this.lbl_keyword.Location = new System.Drawing.Point(12, 69);
            this.lbl_keyword.Name = "lbl_keyword";
            this.lbl_keyword.Size = new System.Drawing.Size(48, 13);
            this.lbl_keyword.TabIndex = 5;
            this.lbl_keyword.Text = "Keyword";
            // 
            // data_results
            // 
            this.data_results.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_results.Location = new System.Drawing.Point(12, 130);
            this.data_results.Name = "data_results";
            this.data_results.ReadOnly = true;
            this.data_results.Size = new System.Drawing.Size(554, 216);
            this.data_results.TabIndex = 6;
            this.data_results.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_results_CellClick);
            // 
            // btn_exp
            // 
            this.btn_exp.Location = new System.Drawing.Point(486, 358);
            this.btn_exp.Name = "btn_exp";
            this.btn_exp.Size = new System.Drawing.Size(75, 23);
            this.btn_exp.TabIndex = 7;
            this.btn_exp.Text = "Export";
            this.btn_exp.UseVisualStyleBackColor = true;
            this.btn_exp.Click += new System.EventHandler(this.btn_exp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 393);
            this.Controls.Add(this.btn_exp);
            this.Controls.Add(this.data_results);
            this.Controls.Add(this.lbl_keyword);
            this.Controls.Add(this.lbl_folder);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.txt_keyword);
            this.Controls.Add(this.f_path);
            this.Controls.Add(this.btn_browse);
            this.Name = "Form1";
            this.Text = "Search";
            ((System.ComponentModel.ISupportInitialize)(this.data_results)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.TextBox f_path;
        private System.Windows.Forms.TextBox txt_keyword;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Label lbl_folder;
        private System.Windows.Forms.Label lbl_keyword;
        private System.Windows.Forms.DataGridView data_results;
        private System.Windows.Forms.Button btn_exp;
    }
}

