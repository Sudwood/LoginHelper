using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        IntPtr thisWindow;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            thisWindow = FindWindow(null, "Form1");

            
            if(Properties.Settings.Default.HotkeyModifier != 0x001 || Properties.Settings.Default.HotkeyModifier != 0x002 || Properties.Settings.Default.HotkeyModifier != 0x004 || Properties.Settings.Default.HotkeyModifier != 0x008)
            {
                RegisterHotKey(thisWindow, 1, (uint)fsModifiers.Control, (uint)Keys.F1);
                Properties.Settings.Default.HotkeyModifier = (uint)fsModifiers.Control;
                Properties.Settings.Default.HotKeyLetter = (uint)Keys.F1;
            }
            else if (Properties.Settings.Default.HotKeyLetter != 0 && Properties.Settings.Default.HotkeyModifier == 0)
            {
                RegisterHotKey(thisWindow, 1, (uint)fsModifiers.Control, (uint)Keys.F1);
                Properties.Settings.Default.HotkeyModifier = (uint)fsModifiers.Control;
                Properties.Settings.Default.HotKeyLetter = (uint)Keys.F1;
            }
            else
            {
                RegisterHotKey(thisWindow, 1, Properties.Settings.Default.HotkeyModifier, Properties.Settings.Default.HotKeyLetter);
            }

            label1.Text = "HotKey: "+ getModifierString((uint)fsModifiers.Control)+ " "+ Keys.F1;
        }

        public enum fsModifiers
        {
            Alt = 0x0001,
            Control = 0x002,
            Shift = 0x0004,
            Window = 0x008
        }

        private String getModifierString(uint num)
        {
            if(num == 0x001)
            {
                return "Alt";
            }
            if (num == 0x002)
            {
                return "Control";
            }
            if (num == 0x004)
            {
                return "Shift";
            }
            if (num == 0x008)
            {
                return "Windows";
            }
            return "";
        }

        private void Form1_Closed(object sender, FormClosedEventArgs e)
        {
            UnregisterHotKey(thisWindow, 1);
        }

        protected override void WndProc(ref Message keyPressed)
        {
             if(keyPressed.Msg == 0x0312)
            {
                System.Threading.Thread.Sleep(2000);
                SendKeys.Send(Clipboard.GetText());
                Console.WriteLine(Clipboard.GetText());
            }
            base.WndProc(ref keyPressed);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Press Modifier");
        }
    }
}
