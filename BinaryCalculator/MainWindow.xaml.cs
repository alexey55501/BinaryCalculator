using BinaryCalculator.Classes.BL;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace BinaryCalculator
{
    public partial class MainWindow : Window
    {
        private char _actionState = ' ';

        public MainWindow()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            int minNumberSystemVal = 2;
            int maxNumberSystemVal = 16;

            for(int i = minNumberSystemVal; i <= maxNumberSystemVal; i++)
            {
                cbFirstParameterNumSystem.Items.Add(i);
                cbSecondParameterNumSystem.Items.Add(i);
                cbResultNumSystem.Items.Add(i);
            }
        }
        private void SelectAction(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button)
                {
                    var btn = sender as Button;
                    _actionState = (btn.Tag as string)[0];
                    foreach(Button item in (btn.Parent as Grid).Children)
                    {
                        if (item == btn)
                            item.Background = new SolidColorBrush(Colors.LimeGreen);
                        else item.Background = new SolidColorBrush(Colors.DarkBlue);
                    }
                    Parameter_TextChanged(null, null);
                }
            }
            catch (Exception ex) { }
        }

        private void Parameter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbFirstParameter.Text != string.Empty & tbSecondParameter.Text != string.Empty && _actionState != ' ')
            {
                try
                {
                    long par1_10sys = long.Parse(BaseConverter.Convert(int.Parse(cbFirstParameterNumSystem.Text), 10, tbFirstParameter.Text));
                    long par2_10sys = long.Parse(BaseConverter.Convert(int.Parse(cbSecondParameterNumSystem.Text), 10, tbSecondParameter.Text));
                    long result = 0;

                    switch (_actionState)
                    {
                        case '+': result = par1_10sys + par2_10sys; break;
                        case '-': result = par1_10sys - par2_10sys; break;
                        case '*': result = par1_10sys * par2_10sys; break;
                        case '/': if (par2_10sys == 0) MessageBox.Show("На 0 делить нельзя!"); else result = par1_10sys / par2_10sys; break;
                    }

                    tbResult.Text = BaseConverter.Convert(10, int.Parse(cbResultNumSystem.Text), result.ToString());
                }
                catch { }
            }
        }

        private void cbNumSystem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Parameter_TextChanged(null, null);
        }
    }
}
