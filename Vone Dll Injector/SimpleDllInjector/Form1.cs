using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Sebboys_DLL_Injector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();

            Process[] PC = Process.GetProcesses().Where(p => (long)p.MainWindowHandle != 0).ToArray();
            comboBox1.Items.Clear();
            foreach (Process p in PC)
            {
                comboBox1.Items.Add(p.ProcessName);
            }
        }


        static readonly IntPtr INTPTR_ZERO = (IntPtr)0;
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        public static int Inject(string PN, string DLLP)
        {
            //1 = file does not exist
            //2 = Process not active
            //3 = injection failed
            //4 = injection succeeded

            if (!File.Exists(DLLP)) { return 1; }

            uint _procId = 0;
            Process[] _procs = Process.GetProcesses();
            for (int i = 0; i < _procs.Length; i++)
            {
                if (_procs[i].ProcessName == PN)
                {
                    _procId = (uint)_procs[i].Id;
                }
            }

            if (_procId == 0) { return 2; }

            if (!SI(_procId, DLLP))
            {
                return 3;
            }

            return 4;
        }

        public static bool SI(uint P, string DDLP)
        {
            IntPtr hndProc = OpenProcess((0x2 | 0x8 | 0x10 | 0x20 | 0x400), 1, P);

            if (hndProc == INTPTR_ZERO) { return false; }


            IntPtr lpAddress = VirtualAllocEx(hndProc, (IntPtr)null, (IntPtr)DLLP.Length, (0x1000 | 0x2000), 0x40);

            if (lpAddress == INTPTR_ZERO)
            {
                return false;
            }

            byte[] bytes = Encoding.ASCII.GetBytes(DLLP);

            if (WriteProcessMemory(hndProc, lpAddress, bytes, (uint)bytes.Length, 0) == 0)
            {
                return false;
            }

            CloseHandle(hndProc);

            return true;

        }

        private static string DLLP { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.InitialDirectory = @"C:\";
                OFD.Title = "Please Choose a DLL File";
                OFD.DefaultExt = "dll";
                OFD.Filter = "DLL Files (*.dll)|*.dll";
                OFD.CheckFileExists = true;
                OFD.CheckPathExists = true;
                OFD.ShowDialog();

                textBox1.Text = OFD.FileName;
                DLLP = OFD.FileName;
            }
            catch (Exception ed)
            {
                MessageBox.Show(ed.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DLLP = textBox1.Text;
            textBox1.ReadOnly = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=dQw4w9WgXcQ&ab_channel=RickAstley");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Process[] PC = Process.GetProcesses().Where(p => (long)p.MainWindowHandle != 0).ToArray();
            comboBox1.Items.Clear();
            foreach (Process p in PC)
            {
                comboBox1.Items.Add(p.ProcessName);
            }
            MessageBox.Show("Flushed the Process List!");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int Result = Inject(comboBox1.Text, DLLP);

            if (Result != 1)
            {
                if (Result == 2)
                {
                    MessageBox.Show("Process Does Not Exist");

                }
                else if (Result == 3)
                {
                    MessageBox.Show("Injection Failed");
                }
                else if (Result == 4)
                {
                    MessageBox.Show("Injection Succeeded");

                }
            }
            else
            {
                MessageBox.Show("Please Pick a DLL file!");
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("I mean this!");
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Current Version: 1.0\nShould change once it is updated!");
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
