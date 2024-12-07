using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Reflection;
using Text_editor.Models;


namespace Text_editor
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool activ_btnBold = true;
        bool activ_btnItalic = true;
        bool activ_btnUnderline = true;

        Brush ColorText = Brushes.Black;
        public MainWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
           
        }
    
        
        public void ButtonSwitch(ref bool buttonState, Button name)
        {
            if (buttonState)
            {
                buttonState = false;
                name.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#485158"));
               
            }
            else 
            {
                buttonState = true;
                name.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99A2A4"));

            }
        }
        public void SaveFile_RichTextBox()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditField.Document.ContentStart, rtbEditField.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
        }
        public void OpenFile_RichTextBox()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text Format (*.rtf)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditField.Document.ContentStart, rtbEditField.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }
  
        private void rtbEditField_SelectionChanged(object sender, RoutedEventArgs e)
        {
            activ_btnBold = rtbEditField.Selection.GetPropertyValue(Bold.FontWeightProperty).Equals(FontWeights.Bold);
            ButtonSwitch(ref activ_btnBold, btnBold);

            activ_btnItalic = rtbEditField.Selection.GetPropertyValue(Inline.FontStyleProperty).Equals(FontStyles.Italic);
            ButtonSwitch(ref activ_btnItalic, btnItalic);
            activ_btnUnderline = rtbEditField.Selection.GetPropertyValue(Underline.TextDecorationsProperty).Equals(TextDecorations.Underline);
            ButtonSwitch(ref activ_btnUnderline, btnUnderline);
        }

        private void btnBold_Click(object sender, RoutedEventArgs e)//Кнопка для жирного текста
        {

            if (activ_btnBold)
            {
                rtbEditField.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                rtbEditField.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
            ButtonSwitch(ref activ_btnBold, btnBold);


        }

        private void btnItalic_Click(object sender, RoutedEventArgs e) //Кнопка для курсива
        {

            if (activ_btnItalic)
            {
                rtbEditField.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                rtbEditField.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            }
            ButtonSwitch(ref activ_btnItalic, btnItalic);
        }

        private void btnUnderline_Click(object sender, RoutedEventArgs e) //Кнопка для нижнего подчеркивания текста
        {

            if (activ_btnUnderline)
            {
                rtbEditField.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                rtbEditField.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
            ButtonSwitch(ref activ_btnUnderline, btnUnderline);
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontName = Convert.ToString(cmbFontFamily.SelectedItem);

            if (fontName != null)
            {
                rtbEditField.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, fontName); //Изменения шрифта
                rtbEditField.Focus();
            }
        }

        private void cmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontSize = Convert.ToString(cmbFontSize.SelectedItem);
            if (fontSize != null)
            {
                rtbEditField.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSize); // Изменения размера шрифта
                rtbEditField.Focus();
            }
          
        }

        private void btnHide_Click(object sender, RoutedEventArgs e) // Скрыть окно
        {
            WindowState = WindowState.Minimized;
        }

        private void btnCollapseExpand_Click(object sender, RoutedEventArgs e) // свернуть, развернуть окно
        {
            if (WindowState == WindowState.Normal) WindowState = WindowState.Maximized;
            else if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;

        }

        private void btnClose_Click(object sender, RoutedEventArgs e) // закрыть окно
        {
            Close();
        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e) // Сохранение файла
        {
            FileOperation.SaveFile_RichTextBox(rtbEditField);
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e) // Открытие файла  
        {
            FileOperation.OpenFile_RichTextBox(rtbEditField);
        
        }

        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            ColorText = new SolidColorBrush((Color)ColorConverter.ConvertFromString(((Button)sender).Background.ToString()));
    
            rtbEditField.Selection.ApplyPropertyValue(ForegroundProperty, ColorText);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            byte.TryParse(txtRed.Text, out byte colorRed);
            byte.TryParse(txtGreen.Text, out byte colorGreen);
            byte.TryParse(txtBlue.Text, out byte colorBlue);
            byte.TryParse(txtAlphaChannel.Text, out byte colorAlphaChannel);

            rtbEditField.Selection.ApplyPropertyValue(ForegroundProperty, new SolidColorBrush(Color.FromArgb(colorAlphaChannel,/*R*/ colorRed, /*G*/colorGreen, /*B*/colorBlue)));
            PaletteIdicator.Background = new SolidColorBrush(Color.FromArgb(colorAlphaChannel,/*R*/ colorRed, /*G*/colorGreen, /*B*/colorBlue));

        }
    }
}
