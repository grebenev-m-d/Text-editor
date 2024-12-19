using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;

namespace Text_editor.Models
{
    internal class FileOperation
    {
        static public void SaveFile_RichTextBox(RichTextBox rtbEditField)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text Format (.rtf)|*.rtf|All files (.)|*.*";
            if (dlg.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create))
                {
                    TextRange range = new TextRange(rtbEditField.Document.ContentStart, rtbEditField.Document.ContentEnd);
                    range.Save(fileStream, DataFormats.Rtf);
                }
            }
        }
        static public void OpenFile_RichTextBox(RichTextBox rtbEditField)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text Format (.rtf)|*.rtf|All files (.)|*.*";
            if (dlg.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open))
                {
                    TextRange range = new TextRange(rtbEditField.Document.ContentStart, rtbEditField.Document.ContentEnd);
                    range.Load(fileStream, DataFormats.Rtf);
                }
            }

        }

    }
}
