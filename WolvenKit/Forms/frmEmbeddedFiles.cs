﻿using System.IO;
using BrightIdeasSoftware;
using WeifenLuo.WinFormsUI.Docking;
using WolvenKit.CR2W;

namespace WolvenKit
{
    public partial class frmEmbeddedFiles : DockContent
    {
        private CR2WFile file;

        public frmEmbeddedFiles()
        {
            InitializeComponent();
            UpdateList();
        }

        public CR2WFile File
        {
            get { return file; }
            set
            {
                file = value;
                UpdateList();
            }
        }

        private void UpdateList()
        {
            if (File == null)
                return;

            listView.Objects = File.embedded;
        }

        private void listView_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Column == null || e.Item == null)
                return;

            if (e.ClickCount == 2)
            {
                var mem = new MemoryStream(((CR2WEmbeddedWrapper) e.Model).Data);

                var doc = MainController.Get().LoadDocument("Embedded file", mem);
                if (doc != null)
                {
                    doc.OnFileSaved += OnFileSaved;
                    doc.SaveTarget = (CR2WEmbeddedWrapper) e.Model;
                }
            }
        }

        private void OnFileSaved(object sender, FileSavedEventArgs e)
        {
            var doc = (frmCR2WDocument) sender;
            var editvar = (CR2WEmbeddedWrapper) doc.SaveTarget;
            editvar.Data = ((MemoryStream) e.Stream).ToArray();
        }
    }
}